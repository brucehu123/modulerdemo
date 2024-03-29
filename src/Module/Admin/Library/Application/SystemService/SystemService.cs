﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using YunHu.Lib.Cache.Abstractions;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Admin.Application.ModuleInfoService;
using YunHu.Module.Admin.Application.SystemService.ViewModels;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.AccountRole;
using YunHu.Module.Admin.Domain.Config;
//using YunHu.Module.Admin.Domain.Role;
using YunHu.Module.Admin.Infrastructure.Repositories;

namespace YunHu.Module.Admin.Application.SystemService
{
    public class SystemService : ISystemService
    {
        /// <summary>
        /// 系统配置项前缀
        /// </summary>
        private const string SystemConfigPrefix = "sys_";

        private const string SystemConfigCacheKey = "ADMIN_SYSTEM_CONFIG";

        private readonly IUnitOfWork _uow;
        private readonly IConfigRepository _configRepository;
        private readonly IModuleInfoService _moduleInfoService;
        private readonly IAccountRepository _accountRepository;
        //private readonly IRoleRepository _roleRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly ICacheHandler _cache;

        public SystemService(IUnitOfWork<AdminDbContext> uow, IConfigRepository configRepository, IModuleInfoService moduleInfoService, ICacheHandler cache, IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository)
        {
            _uow = uow;
            _configRepository = configRepository;
            _moduleInfoService = moduleInfoService;
            //_roleRepository = roleRepository;
            _cache = cache;
            _accountRepository = accountRepository;
            _accountRoleRepository = accountRoleRepository;
        }

        public async Task<IResultModel<SystemConfigModel>> GetConfig(string host = null)
        {
            var result = new ResultModel<SystemConfigModel>();
            if (!_cache.TryGetValue(SystemConfigCacheKey, out SystemConfigModel model))
            {
                model = new SystemConfigModel();

                var configList = await _configRepository.QueryByPrefix(SystemConfigPrefix);

                foreach (var config in configList)
                {
                    switch (config.Key)
                    {
                        case SystemConfigKey.Title:
                            model.Title = config.Value;
                            break;
                        case SystemConfigKey.Logo:
                            model.Logo = config.Value;
                            break;
                        case SystemConfigKey.Home:
                            model.Home = config.Value;
                            break;
                        case SystemConfigKey.UserInfoPage:
                            model.UserInfoPage = config.Value;
                            break;
                        case SystemConfigKey.ButtonPermission:
                            model.ButtonPermission = config.Value.ToBool();
                            break;
                        case SystemConfigKey.Auditing:
                            model.Auditing = config.Value.ToBool();
                            break;
                        case SystemConfigKey.LoginVerifyCode:
                            model.LoginVerifyCode = config.Value.ToBool();
                            break;
                        case SystemConfigKey.ToolbarFullscreen:
                            model.Toolbar.Fullscreen = config.Value.ToBool();
                            break;
                        case SystemConfigKey.ToolbarSkin:
                            model.Toolbar.Skin = config.Value.ToBool();
                            break;
                        case SystemConfigKey.ToolbarLogout:
                            model.Toolbar.Logout = config.Value.ToBool();
                            break;
                        case SystemConfigKey.ToolbarUserInfo:
                            model.Toolbar.UserInfo = config.Value.ToBool();
                            break;
                        case SystemConfigKey.CustomCss:
                            model.CustomCss = config.Value;
                            break;
                    }
                }

                await _cache.SetAsync(SystemConfigCacheKey, model);
            }

            if (host.NotNull() && model.Logo.NotNull())
            {
                model.LogoUrl = new Uri($"{host}/upload/{model.Logo}").ToString().ToLower();
            }

            return result.Success(model);
        }

        public IResultModel UpdateConfig(SystemConfigModel model)
        {
            if (model == null)
                return ResultModel.Failed();

            var tasks = new List<Task>();

            _uow.BeginTransaction();

            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.Title,
                Value = model.Title,
                Remarks = "系统标题"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.Logo,
                Value = model.Logo,
                Remarks = "系统Logo"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.Home,
                Value = model.Home,
                Remarks = "系统首页"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.UserInfoPage,
                Value = model.UserInfoPage,
                Remarks = "个人信息页"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.ButtonPermission,
                Value = model.ButtonPermission.ToString(),
                Remarks = "启用按钮权限"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.Auditing,
                Value = model.Auditing.ToString(),
                Remarks = "启用审计日志"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.LoginVerifyCode,
                Value = model.LoginVerifyCode.ToString(),
                Remarks = "启用登录验证码功能"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.ToolbarFullscreen,
                Value = model.Toolbar.Fullscreen.ToString(),
                Remarks = "显示工具栏全屏按钮"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.ToolbarSkin,
                Value = model.Toolbar.Skin.ToString(),
                Remarks = "显示工具栏皮肤按钮"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.ToolbarLogout,
                Value = model.Toolbar.Logout.ToString(),
                Remarks = "显示工具栏退出按钮"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.ToolbarUserInfo,
                Value = model.Toolbar.UserInfo.ToString(),
                Remarks = "显示工具栏用户信息按钮"
            }));
            tasks.Add(_configRepository.UpdateAsync(new ConfigEntity
            {
                Key = SystemConfigKey.CustomCss,
                Value = model.CustomCss,
                Remarks = "自定义CSS样式"
            }));

            Task.WaitAll(tasks.ToArray());

            _uow.Commit();

            _cache.RemoveAsync(SystemConfigCacheKey).Wait();

            return ResultModel.Success();
        }

        public async Task<IResultModel> Install(SystemInstallModel model)
        {
            //await _moduleInfoService.Sync();
            //await _permissionService.Sync(model.Permissions);

            //var role = new RoleEntity
            //{
            //    Name = "系统管理员"
            //};

            //await _roleRepository.AddAsync(role);

            //var account = new AccountEntity
            //{
            //    UserName = "tdkj",
            //    Password = "FDFAEC6B4F80E739A50ADC802C5B537C",
            //    Name = "管理员"
            //};
            //await _accountRepository.AddAsync(account);

            //await _accountRoleRepository.AddAsync(new AccountRoleEntity { AccountId = account.Id, RoleId = role.Id });

            //UpdateConfig(new SystemConfigModel
            //{
            //    Title = "云呼权限管理系统",
            //    Auditing = false,
            //    Toolbar = new SystemToolbar
            //    {
            //        Fullscreen = true,
            //        Skin = true,
            //        Logout = true,
            //        UserInfo = true
            //    }
            //});

            return ResultModel.Success();
        }
    }
}
