using System;
using YunHu.Lib.Data.Abstractions.Attributes;

namespace YunHu.Module.Admin.Domain.Button
{
    public partial class ButtonEntity
    {
        [Ignore]
        public Guid RoleId { get; set; }
    }
}
