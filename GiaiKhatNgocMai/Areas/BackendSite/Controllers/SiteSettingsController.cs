using GiaiKhatNgocMai.Areas.BackendSite.Models;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Areas.BackendSite.Controllers
{
    [BackendRoleBased(new RoleType[]{RoleType.Admin, RoleType.Editor})]
    public class SiteSettingsController : BaseController
    {
        // GET: BackendSite/SiteSettings
        public ActionResult Banners()
        {
            var model = new BannerViewModel();
            model.Banners = Context.GetBanners(i => i.Id > 0).OrderBy(i => i.Type).Select(i => new BannerModel(i)).ToList();
            return View(model);
        }

        public ActionResult NewBanner()
        {
            return View();
        }

        public ActionResult News()
        {
            return View();
        }
    }
}