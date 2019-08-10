using YunHu.Lib.Data.Abstractions.Attributes;
using YunHu.Module.Admin.Domain.Menu;

namespace YunHu.Module.Admin.Domain.RoleMenu
{
    public partial class RoleMenuEntity
    {
        [Ignore]
        public MenuType MenuType { get; set; }
    }
}
