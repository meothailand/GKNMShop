using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Models;
using GiaiKhatNgocMai.Infrastructure.Security;

namespace GiaiKhatNgocMai.Areas.BackendSite.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string UserName  { get; set; }
        public List<MenuLinkContainer> UserMenu { get; set; }
        public RoleType Role { get; set; }

        public void SetCurrentMenu(string controllerName, string actionName)
        {
            var firstLevel = UserMenu.SingleOrDefault(i => i.Menu.Link == controllerName);
            if (firstLevel != null)
            {
                firstLevel.Menu.IsCurrent = true;
                firstLevel.SetCurrentLinkByLink(actionName);
            }
        }
    }
}