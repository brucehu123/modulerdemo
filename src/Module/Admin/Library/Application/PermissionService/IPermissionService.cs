﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Admin.Domain.Permission;
using YunHu.Module.Admin.Domain.Permission.Models;

namespace YunHu.Module.Admin.Application.PermissionService
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResultModel> Query(PermissionQueryModel model);

        /// <summary>
        /// 同步
        /// </summary>
        /// <returns></returns>
        Task<IResultModel> Sync(List<PermissionEntity> permissions);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultModel> Delete(Guid id);
    }
}
