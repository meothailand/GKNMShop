using GiaiKhatNgocMai.Areas.BackendSite.Models;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models;
using GiaiKhatNgocMai.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace GiaiKhatNgocMai.Areas.BackendSite.Controllers
{
    public class LoginController : BaseController
    {
        // GET: BackendSite/Login
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel login)
        {
            var user = Context.GetUser(login.Email, login.Password);
            if (user != null)
            {
                login.Role = SecurityHelper.GetRoleType(user.Role);
                login.Password = string.Empty;
                login.UserName = user.UserName;
                CacheHelper._CacheHelper.SetLoggedInUser(login);
                return RedirectToAction("Index", "Dashboard");
            }
            login.Password = string.Empty;
            login.ErrorMessage = "Sai email đăng nhập hay mật khẩu.";
            return View(login);
        }
    }
}