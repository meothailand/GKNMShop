using System.Web.Mvc;

namespace GiaiKhatNgocMai.Areas.BackendSite
{
    public class BackendSiteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BackendSite";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BackendSite_default",
                "BackendSite/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}