using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Module.Admin.Domain.Menu;
using YunHu.Module.Admin.Domain.RoleMenu;

namespace YunHu.Module.Admin.Infrastructure.Repositories.SqlServer
{
    public class RoleMenuRepository : RepositoryAbstract<RoleMenuEntity>, IRoleMenuRepository
    {
        public RoleMenuRepository(IDbContext context) : base(context)
        {
        }
        public Task<IList<RoleMenuEntity>> GetByRoleId(Guid roleId)
        {
            return Db.Find(e => e.RoleId == roleId)
                .LeftJoin<MenuEntity>((x, y) => x.MenuId == y.Id)
                .Select((x, y) => new { x, MenuType = y.Type })
                .ToListAsync();
        }

        public Task<bool> DeleteByMenuId(Guid menuId)
        {
            return Db.Find(e => e.MenuId == menuId).DeleteAsync();
        }

        public Task<bool> ExistsWidthMenuId(Guid menuId)
        {
            return Db.Find(e => e.MenuId == menuId).ExistsAsync();
        }

        public Task<bool> DeleteByRoleId(Guid roleId)
        {
            return Db.Find(e => e.RoleId == roleId).DeleteAsync();
        }
    }
}
