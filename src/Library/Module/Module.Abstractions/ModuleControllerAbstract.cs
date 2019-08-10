using Microsoft.AspNetCore.Mvc;
using YunHu.Lib.Auth.Abstractions.Attributes;
using YunHu.Lib.Validation.Abstractions;

namespace YunHu.Lib.Module.Abstractions
{
    /// <summary>
    /// 模块通用控制器
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [PermissionValidate]
    [ValidateResultFormat]
    public abstract class ModuleControllerAbstract : ControllerBase
    {
       
    }
}
