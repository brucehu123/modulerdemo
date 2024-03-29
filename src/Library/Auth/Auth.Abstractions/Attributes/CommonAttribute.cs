﻿using System;

namespace YunHu.Lib.Auth.Abstractions.Attributes
{
    /// <summary>
    /// 通用权限(只要登录即可访问，不需要授权)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CommonAttribute : Attribute
    {

    }
}
