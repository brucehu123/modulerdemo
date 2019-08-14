using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Role.Application.RoleService.ResultModels;
using YunHu.Module.Role.Domain.Role.Models;

namespace YunHu.Module.Role.Application.RoleService
{
    public class RoleService : IRoleService
    {
        public Task<IResultModel> Add(RoleAddModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Edit(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Query(RoleQueryModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IResultModel> Update(RoleUpdateModel model)
        {
            throw new NotImplementedException();
        }
    }
}
