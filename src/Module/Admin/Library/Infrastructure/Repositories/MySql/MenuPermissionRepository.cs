﻿using YunHu.Lib.Data.Abstractions;

namespace YunHu.Module.Admin.Infrastructure.Repositories.MySql
{
    public class MenuPermissionRepository : SqlServer.MenuPermissionRepository
    {
        public MenuPermissionRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
