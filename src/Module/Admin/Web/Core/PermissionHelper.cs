using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using YunHu.Lib.Auth.Abstractions.Attributes;
using YunHu.Lib.Utils.Core.Attributes;
using YunHu.Lib.Utils.Mvc.Helpers;

namespace YunHu.Module.Admin.Web.Core
{
    [Singleton]
    public class PermissionHelper
    {
        private readonly MvcHelper _mvcHelper;

        public PermissionHelper(MvcHelper mvcHelper)
        {
            _mvcHelper = mvcHelper;
        }

   
      
    }
}
