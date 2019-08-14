using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;
using YunHu.Module.Admin.Domain.Account;
using YunHu.Module.Admin.Domain.AccountRole;

namespace YunHu.Module.Admin.Infrastructure.Repositories.SqlServer
{
    public class AccountRoleRepository : RepositoryAbstract<AccountRoleEntity>, IAccountRoleRepository
    {
        public AccountRoleRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> Delete(Guid accountId, Guid roleId)
        {
            return Db.Find(m => m.AccountId == accountId && m.RoleId == roleId).DeleteAsync();
        }

        public Task<bool> DeleteByAccount(Guid accountId)
        {
            return Db.Find(m => m.AccountId == accountId).DeleteAsync();
        }

        public Task<bool> Exists(Guid accountId, Guid roleId)
        {
            return Db.Find(m => m.AccountId == accountId && m.RoleId == roleId).ExistsAsync();
        }

       

        public Task<IList<AccountRoleEntity>> QueryByRole(Guid roleId)
        {
            return Db.Find(m => m.RoleId == roleId).ToListAsync();
        }

     

        public Task<bool> ExistsByRole(Guid roleId)
        {
            return Db.Find(m => m.RoleId == roleId).InnerJoin<AccountEntity>((x, y) => x.AccountId == y.Id && y.Deleted == false).ExistsAsync();
        }

        public Task<IList<AccountRoleEntity>> QueryByMenu(Guid menuId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AccountRoleEntity>> QueryByButton(Guid buttonId)
        {
            throw new NotImplementedException();
        }
    }
}
