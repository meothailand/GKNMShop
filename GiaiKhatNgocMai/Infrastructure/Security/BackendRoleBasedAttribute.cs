using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GiaiKhatNgocMai.Settings;
using System.Web.Http.Controllers;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models;
using System.IO;
using System.Xml;

namespace GiaiKhatNgocMai.Infrastructure.Security
{
    public sealed class BackendRoleBasedAttribute : AuthorizeAttribute
    {
        public RoleType[] SiteRole {get; set;}
        public BackendRoleBasedAttribute(params RoleType[] siterole)
        {
            SiteRole = siterole;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) return false;
            var user = CacheHelper._CacheHelper.GetLoggedInUser();
            if (user != null && Array.IndexOf(SiteRole, user.Role) >= 0)
            {
                var routeInfo = httpContext.Request.RequestContext.RouteData;
                var controller = routeInfo.GetRequiredString("controller");
                var action = routeInfo.GetRequiredString("action");
                foreach (var m in user.UserMenu)
                {
                    m.ClearCurrentStatus();
                }
                var current = user.UserMenu.SingleOrDefault(i => i.Menu.Link == controller.ToLower());
                if (current != null)
                {
                    current.Menu.IsCurrent = true;
                    current.SetCurrentLinkByLink(action.ToLower());
                }
                return true;
            }
            return false;
            //if (actionContext.Request.Headers.Any(h => h.Key == SiteSettings.TokenKey))
            //{
            //    var header = actionContext.Request.Headers.FirstOrDefault(h => h.Key == SiteSettings.TokenKey);
            //    var content = header.Value.First();
            //    if (content == null) throw new Exception();
            //    //validate token here
            //    //check role here
            //    return true;
            //}
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/backendsite/login");
        }
    }
}