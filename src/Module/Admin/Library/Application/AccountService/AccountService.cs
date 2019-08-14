using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YunHu.Lib.Auth.Abstractions;
using YunHu.Lib.Cache.Abstractions;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Utils.Core.Encrypt;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Lib.Utils.Core.Helpers;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Admin.Application.AccountService.ResultModels;
using YunHu.Module.Admin.Application.AccountService.ViewModels;
using YunHu.Module.Admin.Application.SystemService;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.Account.Models;
using YunHu.Module.Admin.Domain.AccountRole;

using YunHu.Module.Admin.Infrastructure.Repositories;

namespace YunHu.Module.Admin.Application.AccountService
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// 验证码缓存Key
        /// </summary>
        public const string VerifyCodeKey = "ADMIN_VERIFY_CODE_";

        /// <summary>
        /// 账户权限列表缓存Key
        /// </summary>
        public const string AccountPermissionListKey = "ADMIN_ACCOUNT_PERMISSION_LIST_";

        //默认密码
        public const string DefaultPassword = "123456";
        private readonly LoginInfo _loginInfo;
        private readonly ICacheHandler _cache;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IAccountRepository _accountRepository;

      
        private readonly DrawingHelper _drawingHelper;
        private readonly ISystemService _systemService;

        public AccountService(LoginInfo loginInfo, ICacheHandler cache, IMapper mapper, IUnitOfWork<AdminDbContext> uow, IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, DrawingHelper drawingHelper, ILogger<AccountService> logger, ISystemService systemService)
        {
            _loginInfo = loginInfo;
            _cache = cache;
            _mapper = mapper;
            _uow = uow;
            _accountRepository = accountRepository;

           
            _drawingHelper = drawingHelper;
            _systemService = systemService;
        }

        public IResultModel CreateVerifyCode(int length = 6)
        {
            var verifyCodeModel = new VerifyCodeResultModel
            {
                Id = Guid.NewGuid().ToString("N"),
                Base64String = _drawingHelper.DrawVerifyCodeBase64String(out string code, length)
            };

            //把验证码放到内存缓存中，有效期10分钟
            _cache.SetAsync(VerifyCodeKey + verifyCodeModel.Id, code, 10);

            return ResultModel.Success(verifyCodeModel);
        }

        public async Task<ResultModel<AccountEntity>> Login(LoginModel model)
        {
            var result = new ResultModel<AccountEntity>();

            var verifyCodeKey = VerifyCodeKey + model.PictureId;
            var systemConfig = (await _systemService.GetConfig()).Data;
            if (systemConfig.LoginVerifyCode)
            {
                if (model.Code.IsNull())
                    return result.Failed("请输入验证码");

                var code = await _cache.GetAsync(verifyCodeKey);
                if (model.PictureId.IsNull() || !model.Code.Equals(code))
                    return result.Failed("验证码有误");
            }

            var account = await _accountRepository.GetByUserName(model.UserName);
            if (!CheckAccount(account, out string msg))
            {
                return result.Failed(msg);
            }

            var password = EncryptPassword(account.UserName.ToLower(), model.Password);
            if (!account.Password.Equals(password))
                return result.Failed("密码错误");

            #region ==修改登录信息==

            //是否激活
            var status = account.Status == AccountStatus.Inactive ? AccountStatus.Enabled : AccountStatus.UnKnown;
            await _accountRepository.UpdateLoginInfo(account.Id, _loginInfo.IPv4, status);

            #endregion

            //删除验证码缓存
            await _cache.RemoveAsync(verifyCodeKey);

            return result.Success(account);
        }

        public async Task<IResultModel> LoginInfo()
        {
            var account = await _accountRepository.GetAsync(_loginInfo.AccountId);
            if (!CheckAccount(account, out string msg))
            {
                return ResultModel.Failed(msg);
            }

            var model = new LoginResultModel
            {
                Id = account.Id,
                Name = account.Name,
                Skin = new SkinConfigModel
                {
                    Name = "pretty",
                    Theme = "",
                    FontSize = ""
                }
            };



            return ResultModel.Success(model);
        }

        /// <summary>
        /// 检测账户
        /// </summary>
        /// <returns></returns>
        private bool CheckAccount(AccountEntity account, out string msg)
        {
            msg = "";
            if (account == null || account.Deleted)
            {
                msg = "账户不存在";
                return false;
            }
            if (account.Status == AccountStatus.Closed)
            {
                msg = "该账户已注销，请联系管理员~";
                return false;
            }

            if (account.Status == AccountStatus.Disabled)
            {
                msg = "该账户已禁用，请联系管理员~";
                return false;
            }

            return true;
        }

        public async Task<IResultModel> UpdatePassword(UpdatePasswordModel model)
        {
            var account = await _accountRepository.GetAsync(_loginInfo.AccountId);
            if (account == null || account.Deleted)
                return ResultModel.Failed("账户不存在");

            var oldPassword = EncryptPassword(account.UserName, model.OldPassword);
            if (!account.Password.Equals(oldPassword))
                return ResultModel.Failed("原密码错误");

            var newPassword = EncryptPassword(account.UserName, model.NewPassword);
            var result = await _accountRepository.UpdatePassword(_loginInfo.AccountId, newPassword);

            return ResultModel.Result(result);
        }



        public async Task<IResultModel> Query(AccountQueryModel model)
        {
            var result = new QueryResultModel<AccountEntity>
            {
                Rows = await _accountRepository.Query(model),
                Total = model.TotalCount
            };

            //foreach (var item in result.Rows)
            //{
            //    var roles = await _accountRoleRepository.QueryRole(item.Id);
            //    item.Roles = roles.Select(r => new OptionResultModel { Label = r.Name, Value = r.Id }).ToList();
            //}

            return ResultModel.Success(result);
        }

        public async Task<IResultModel<Guid>> Add(AccountAddModel model)
        {
            var result = new ResultModel<Guid>();

            var account = _mapper.Map<AccountEntity>(model);

            var exists = await Exists(account);
            if (!exists.Successful)
                return exists;

            //默认未激活状态，用户首次登录激活
            account.Status = AccountStatus.Inactive;

            //设置默认密码
            if (account.Password.IsNull())
                account.Password = DefaultPassword;

            account.Password = EncryptPassword(account.UserName.ToLower(), account.Password);

            _uow.BeginTransaction();
            if (await _accountRepository.AddAsync(account))
            {

                _uow.Commit();
                return result.Success(account.Id);

            }

            return result.Failed();
        }

        public async Task<IResultModel> Edit(Guid id)
        {
            var entity = await _accountRepository.GetAsync(id);
            if (entity == null)
                return ResultModel.Failed("账户不存在");

            var model = _mapper.Map<AccountUpdateModel>(entity);
            //var roles = await _accountRoleRepository.QueryRole(id);
            //model.Roles = roles.Select(m => m.Id).ToList();
            return ResultModel.Success(model);
        }

        public async Task<IResultModel> Update(AccountUpdateModel model)
        {
            var entity = await _accountRepository.GetAsync(model.Id);
            if (entity == null)
                return ResultModel.Failed("账户不存在！");

            var account = _mapper.Map(model, entity);

            var exists = await Exists(account);
            if (!exists.Successful)
                return exists;

            _uow.BeginTransaction();
            var result = await _accountRepository.UpdateAsync(account);

            _uow.Commit();
            ClearPermissionListCache(account.Id);

            return ResultModel.Success();

           
        }

        public async Task<IResultModel> Delete(Guid id)
        {
            var entity = await _accountRepository.GetAsync(id);
            if (entity == null)
                return ResultModel.NotExists;
            if (entity.Id == _loginInfo.AccountId)
                return ResultModel.Failed("不允许删除自己的账户");

            var result = await _accountRepository.SoftDeleteAsync(id);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> ResetPassword(Guid id)
        {
            var account = await _accountRepository.GetAsync(id);
            if (account == null || account.Deleted)
                return ResultModel.Failed("账户不存在");

            var newPassword = EncryptPassword(account.UserName, DefaultPassword);
            var result = await _accountRepository.UpdatePassword(id, newPassword);

            return ResultModel.Result(result);
        }

        

        public void ClearPermissionListCache(Guid id)
        {
            _cache.RemoveAsync(AccountPermissionListKey + id).Wait();
        }

        #region ==获取账户的菜单树==

     

       

        #endregion

        /// <summary>
        /// 判断账户是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private async Task<IResultModel<Guid>> Exists(AccountEntity entity)
        {
            var result = new ResultModel<Guid>();

            if (await _accountRepository.ExistsUserName(entity.UserName, entity.Id, entity.Type))
                return result.Failed("用户名已存在");
            if (entity.Phone.NotNull() && await _accountRepository.ExistsPhone(entity.Phone, entity.Id, entity.Type))
                return result.Failed("手机号已存在");
            if (entity.Email.NotNull() && await _accountRepository.ExistsEmail(entity.Email, entity.Id, entity.Type))
                return result.Failed("邮箱已存在");

            return result.Success(Guid.Empty);
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <returns></returns>
        public string EncryptPassword(string userName, string password)
        {
            return Md5Encrypt.Encrypt($"{userName}_{password}");
        }
    }

   
}