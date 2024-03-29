﻿using YunHu.Lib.Data.Abstractions.Attributes;
using YunHu.Lib.Data.Core.Entities.Extend;

namespace YunHu.Module.Admin.Domain.Config
{
    /// <summary>
    /// 配置项
    /// </summary>
    [Table("Config")]
    public class ConfigEntity : EntityBase<int>
    {
        /// <summary>
        /// 键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
