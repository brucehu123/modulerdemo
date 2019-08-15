using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Role.Application.RoleService.ResultModels;
using YunHu.Module.Role.Domain.Role.Models;

namespace YunHu.Module.Role.Application.RoleService
{
    public interface IRoleService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResultModel> QueryAsync(RoleQueryModel model);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResultModel> Add(RoleAddModel model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        Task<IResultModel> Delete(Guid id);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultModel> Edit(Guid id);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResultModel> Update(RoleUpdateModel model);
    }
}
