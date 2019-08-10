using System.Collections.Generic;
using YunHu.Lib.Utils.Core.Result;

namespace YunHu.Module.Admin.Domain.Account
{
    public partial class AccountEntity
    {
        /// <summary>
        /// 关联角色
        /// </summary>
        public List<OptionResultModel> Roles { get; set; }
    }
}
