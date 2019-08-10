﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using YunHu.Lib.Utils.Core.Extensions;

namespace YunHu.Lib.Auth.Abstractions.Attributes
{
    /// <summary>
    /// 权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionValidateAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loginInfo = context.HttpContext.RequestServices.GetService<LoginInfo>();
            //未登录
            if (loginInfo == null || loginInfo.AccountId.IsEmpty())
                return;

            //排除匿名访问
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            //排除通用接口
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(CommonAttribute)))
                return;

            var handler = context.HttpContext.RequestServices.GetService<IPermissionValidateHandler>();
            if (!handler.Validate(context))
            {
                //无权访问
                context.Result = new ForbidResult();
            }
        }
    }
}
