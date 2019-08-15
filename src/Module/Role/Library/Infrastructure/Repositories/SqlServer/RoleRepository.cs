using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Lib.Data.Query;
using YunHu.Lib.Utils.Core.Extensions;
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
            var paging = model.Paging();
            var query = Db.Find(m => m.Deleted == false);
         
            var list = await query.PaginationAsync(paging);
            model.TotalCount = paging.TotalCount;
            return list;
        }

        public override Task<IList<RoleEntity>> GetAllAsync()
        {
            return Db.Find(m => m.Deleted == false).ToListAsync();
        }
    }
}
