using YunHu.Lib.Data.Abstractions;

namespace YunHu.Module.Admin.Infrastructure.Repositories.MySql
{
    public class ModuleInfoRepository : SqlServer.ModuleInfoRepository
    {
        public ModuleInfoRepository(IDbContext context) : base(context)
        {
        }
    }
}
