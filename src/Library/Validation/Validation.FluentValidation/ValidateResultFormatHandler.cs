using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YunHu.Lib.Utils.Core.Result;
using YunHu.Lib.Validation.Abstractions;

namespace YunHu.Lib.Validation.FluentValidation
{
    public class ValidateResultFormatHandler : IValidateResultFormatHandler
    {
        public void Format(ResultExecutingContext context)
        {
            //只返回第一条错误信息
            context.Result = new JsonResult(ResultModel.Failed(context.ModelState.Values.First().Errors[0].ErrorMessage));
        }
    }
}
