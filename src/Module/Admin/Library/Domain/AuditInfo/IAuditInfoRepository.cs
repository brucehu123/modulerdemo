using System.Collections.Generic;
using System.Threading.Tasks;
using YunHu.Lib.Data.Abstractions;
using YunHu.Module.Admin.Domain.AuditInfo.Models;

namespace YunHu.Module.Admin.Domain.AuditInfo
{
    /// <summary>
    /// 审计信息仓储
    /// </summary>
    public interface IAuditInfoRepository : IRepository<AuditInfoEntity>
    {
        Task<IList<AuditInfoEntity>> Query(AuditInfoQueryModel model);

        Task<AuditInfoEntity> Details(int id);
    }
}
