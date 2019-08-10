﻿using System.Threading.Tasks;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Module.Admin.Application.SystemService.ViewModels;

namespace YunHu.Module.Admin.Application.SystemService
{
    /// <summary>
    /// 系统相关服务
    /// </summary>
    public interface ISystemService
    {
        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <returns></returns>
        Task<IResultModel<SystemConfigModel>> GetConfig(string host = null);

        /// <summary>
        /// 更改系统配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IResultModel UpdateConfig(SystemConfigModel model);

        /// <summary>
        /// 系统安装
        /// </summary>
        /// <returns></returns>
        Task<IResultModel> Install(SystemInstallModel model);
    }
}
