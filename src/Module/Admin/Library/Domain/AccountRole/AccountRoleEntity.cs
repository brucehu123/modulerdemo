using System;
using YunHu.Lib.Data.Abstractions.Attributes;
using YunHu.Lib.Data.Core.Entities;

namespace YunHu.Module.Admin.Domain.AccountRole
{
    /// <summary>
    /// 账户角色
    /// </summary>
    [Table("Account_Role")]
    public class AccountRoleEntity : Entity<int>
    {
        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
