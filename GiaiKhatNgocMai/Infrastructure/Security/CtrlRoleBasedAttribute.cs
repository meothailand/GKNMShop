﻿using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Infrastructure.Security
{
    public class CtrlRoleBasedAttribute : AuthorizeAttribute
    {
        public RoleType SiteRole { get; set; }
        public string RedirectUrl { get; set; }

        public CtrlRoleBasedAttribute():base()
        {
            RedirectUrl = "/khach-hang/dang-nhap";
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn)
            {
                return true;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(RedirectUrl);
        }
    }
}