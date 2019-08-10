using YunHu.Lib.Data.Abstractions;

namespace YunHu.Module.Admin.Infrastructure.Repositories.MySql
{
    public class AccountRepository : SqlServer.AccountRepository
    {
        public AccountRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
