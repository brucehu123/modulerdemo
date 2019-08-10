using YunHu.Lib.Data.Abstractions;

namespace YunHu.Module.Admin.Infrastructure.Repositories.MySql
{
    public class RoleRepository : SqlServer.RoleRepository
    {
        public RoleRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
