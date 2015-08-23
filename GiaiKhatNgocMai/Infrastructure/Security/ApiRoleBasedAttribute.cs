using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using GiaiKhatNgocMai.Settings;
using System.Web.Http.Controllers;

namespace GiaiKhatNgocMai.Infrastructure.Security
{
    public class ApiRoleBasedAttribute : AuthorizeAttribute
    {
        public RoleType SiteRole {get; set;}
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null) return false;
            if (actionContext.Request.Headers.Any(h => h.Key == SiteSettings.TokenKey))
            {
                var header = actionContext.Request.Headers.FirstOrDefault(h => h.Key == SiteSettings.TokenKey);
                var content = header.Value.First();
                if (content == null) throw new Exception();
                //validate token here
                //check role here
                return true;
            }
            return false;
        }
    }
}