using System.Security.Claims;
using YunHu.Lib.Utils.Core.Result;

namespace YunHu.Lib.Auth.Abstractions
{
    /// <summary>
    /// 登录处理
    /// </summary>
    public interface ILoginHandler
    {
        IResultModel Hand(Claim[] claims);
    }
}
