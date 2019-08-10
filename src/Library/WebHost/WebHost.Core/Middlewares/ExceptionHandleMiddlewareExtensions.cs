﻿

using Microsoft.AspNetCore.Builder;

namespace YunHu.Lib.WebHost.Core.Middlewares
{
    public static class ExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandleMiddleware>();

            return app;
        }
    }
}
