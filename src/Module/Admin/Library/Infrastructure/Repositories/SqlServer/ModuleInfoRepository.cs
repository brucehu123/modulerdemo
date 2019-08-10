using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Lib.Data.Query;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.ModuleInfo;
using YunHu.Module.Admin.Domain.ModuleInfo.Models;

namespace YunHu.Module.Admin.Infrastructure.Repositories.SqlServer
{
    public class ModuleInfoRepository : RepositoryAbstract<ModuleInfoEntity>, IModuleInfoRepository
    {
        public ModuleInfoRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IList<ModuleInfoEntity>> Query(ModuleInfoQueryModel model)
        {
            var paging = model.Paging();
            var query = Db.Find();
            query.WhereIf(model.Name.NotNull(), m => m.Name.Contains(model.Name));
            query.WhereIf(model.Code.NotNull(), m => m.Code.Contains(model.Code));

            if (!paging.OrderBy.Any())
                query.OrderByDescending(m => m.Id);

            var list = await query.LeftJoin<AccountEntity>((x, y) => x.CreatedBy == y.Id)
                .Select((x, y) => new { x, Creator = y.Name })
                .PaginationAsync(paging);
            model.TotalCount = paging.TotalCount;
            return list;
        }

        public Task<bool> Exists(string code, Guid? id = null)
        {
            var query = Db.Find(m => m.Code == code);
            query.WhereIf(id != null, m => m.Id != id);
            return query.ExistsAsync();
        }

        public Task<bool> UpdateByCode(ModuleInfoEntity entity)
        {
            return Db.Find().Where(m => m.Code == entity.Code).UpdateAsync(m => new ModuleInfoEntity
            {
                Name = entity.Name,
                Version = entity.Version,
                Remarks = entity.Remarks
            });
        }
    }
}
