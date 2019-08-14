using System;
using System.Collections.Generic;
using System.Text;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;

namespace YunHu.Module.Role.Infrastructure
{
    public class RoleDbContext : DbContext
    {
        public RoleDbContext(IDbContextOptions options) : base(options)
        {
        }
    }
}
