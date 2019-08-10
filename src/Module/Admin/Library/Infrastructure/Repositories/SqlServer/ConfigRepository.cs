using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Lib.Data.Query;
using YunHu.Lib.Utils.Core.Extensions;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.Config;
using YunHu.Module.Admin.Domain.Config.Models;

namespace YunHu.Module.Admin.Infrastructure.Repositories.SqlServer
{
    public class ConfigRepository : RepositoryAbstract<ConfigEntity>, IConfigRepository
    {
        public ConfigRepository(IDbContext context) : base(context)
        {
        }

        public Task<bool> Exists(string key)
        {
            return ExistsAsync(m => m.Key.Equals(key));
        }

        public Task<bool> Exists(ConfigEntity entity)
        {
            var query = Db.Find(m => m.Key == entity.Key);
            query.WhereIf(entity.Id > 0, m => m.Id != entity.Id);

            return query.ExistsAsync();
        }

        public Task<IList<ConfigEntity>> QueryByPrefix(string prefix)
        {
            return Db.Find(m => m.Key.StartsWith(prefix)).ToListAsync();
        }

        public async Task<IList<ConfigEntity>> Query(ConfigQueryModel model)
        {
            var paging = model.Paging();
            var query = Db.Find();
            query.WhereIf(model.Key.NotNull(), m => m.Key.Contains(model.Key) || m.Value.Contains(model.Key));
            if (!paging.OrderBy.Any())
            {
                query.OrderByDescending(m => m.Id);
            }

            query.LeftJoin<AccountEntity>((x, y) => x.CreatedBy == y.Id).Select((x, y) => new { x, Creator = y.Name });
            var list = await query.PaginationAsync(paging);
            model.TotalCount = paging.TotalCount;
            return list;
        }

        public Task<ConfigEntity> GetByKey(string key)
        {
            return Db.Find(m => m.Key == key).FirstAsync();
        }


        public override async Task<bool> UpdateAsync(ConfigEntity entity)
        {
            if (await Exists(entity.Key))
                return await Db.Find(m => m.Key == entity.Key).UpdateAsync(m => new ConfigEntity { Value = entity.Value, Remarks = entity.Remarks });

            return await AddAsync(entity);
        }
    }
}
