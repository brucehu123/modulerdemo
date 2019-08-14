using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Module.Role.Domain.Role;
using YunHu.Module.Role.Domain.Role.Models;

namespace YunHu.Module.Role.Infrastructure.Repositories.SqlServer
{
    public class RoleRepository : RepositoryAbstract<RoleEntity>, IRoleRepository
    {
        public RoleRepository(IDbContext dbContext) : base(dbContext)
        {

        }

        public Task<bool> Exists(string name, Guid? id = null)
        {
            var query = Db.Find(m => m.Deleted == false && m.Name.Equals(name));
            query.WhereIf(id != null, m => m.Id != id);
            return query.ExistsAsync();
        }

        public async Task<IList<RoleEntity>> Query(RoleQueryModel model)
        {
            //var paging = model.Paging();
            //var query = Db.Find(m => m.Deleted == false).LeftJoin<AccountEntity>((x, y) => x.CreatedBy == y.Id);
            //query.WhereIf(model.Name.NotNull(), (x, y) => x.Name.Contains(model.Name));
            //query.Select((x, y) => new { x, Creator = y.Name });
            //var list = await query.PaginationAsync(paging);
            //model.TotalCount = paging.TotalCount;
            //return list;
            return null;
        }

        public override Task<IList<RoleEntity>> GetAllAsync()
        {
            return Db.Find(m => m.Deleted == false).ToListAsync();
        }
    }
}
