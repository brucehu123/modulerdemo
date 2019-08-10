using YunHu.Lib.Data.Abstractions;
using YunHu.Lib.Data.Core;

namespace YunHu.Module.Admin.Infrastructure.Repositories
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(IDbContextOptions options) : base(options)
        {
        }
    }
}
