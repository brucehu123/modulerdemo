using System.Collections.Generic;
using YunHu.Module.Admin.Domain.Permission;

namespace YunHu.Module.Admin.Application.SystemService.ViewModels
{
    public class SystemInstallModel
    {
        public List<PermissionEntity> Permissions { get; set; }
    }
}
