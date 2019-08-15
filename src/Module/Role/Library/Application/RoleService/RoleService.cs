using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Role.Application.RoleService.ResultModels;
using YunHu.Module.Role.Domain.Role;
using YunHu.Module.Role.Domain.Role.Models;

namespace YunHu.Module.Role.Application.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        public Task<IResultModel> Add(RoleAddModel model)
        {
            return Task.FromResult(ResultModel.Success("新增"));
        }

        public Task<IResultModel> Delete(Guid id)
        {
            return Task.FromResult(ResultModel.Success("删除"));
        }

        public Task<IResultModel> Edit(Guid id)
        {
            return Task.FromResult(ResultModel.Success("编辑"));
        }

        public async Task<IResultModel> QueryAsync(RoleQueryModel model)
        {
            var result = new QueryResultModel<RoleEntity>
            {
                Rows = await _repository.Query(model),
                Total = model.TotalCount
            };
            return ResultModel.Success(result);
        }

        public Task<IResultModel> Update(RoleUpdateModel model)
        {
            return Task.FromResult(ResultModel.Success("修改"));
        }
    }
}
