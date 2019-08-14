using System;
using System.Collections.Generic;
using System.Text;
using YunHu.Lib.Data.Abstractions.Attributes;
using YunHu.Lib.Data.Core.Entities.Extend;

namespace YunHu.Module.Role.Domain.Role
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("Role")]
    public partial class RoleEntity : EntityBaseWithSoftDelete
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
