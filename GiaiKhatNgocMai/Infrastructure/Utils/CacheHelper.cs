using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using GiaiKhatNgocMai.Models;
using GiaiKhatNgocMai.Models.LogicModel;
using System.Web.Caching;
using GiaiKhatNgocMai.Infrastructure.Interfaces;
using GiaiKhatNgocMai.Infrastructure.Implementation;
using GiaiKhatNgocMai.Settings;
using GiaiKhatNgocMai.Areas.BackendSite.Models;
using System.Xml;
using GiaiKhatNgocMai.Infrastructure.Security;
using System.IO;

namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class CacheHelper
    {
        public const string CategoryCache = "CategoryMenuCache";
        public const string BannerCache = "BannerCache";
        public const string HotNewsCache = "HotNewsCache";
        public const string SiteNewsCache = "SiteNewsCache";
        public const string ShipmentFeeCache = "ShipmentFeeCache";
        public const string CartSession = "Shopping-cart-session";
        public const string CheckoutProgressSession = "CheckoutProgressSession";
        public const string CustomerLoginSession = "CustomerLoginSession";
        public const string UserLoginSession = "UserLoginSession";

        private static CacheHelper _cacheHelper;
        private IGiaiKhatNgocMaiDBContext Context = GKNMDBContext.Instance;
        public static CacheHelper _CacheHelper
        {
            get
            {
                if (_cacheHelper == null) _cacheHelper = new CacheHelper();
                return _cacheHelper;
            }           
        }

        public void ClearCache(string cacheName)
        {
            if (HttpContext.Current.Cache[cacheName] != null) HttpContext.Current.Cache.Remove(cacheName);
        }

        public void ClearSession(string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] != null) HttpContext.Current.Session.Remove(sessionName);
        }

        public List<SiteNewsModel> LoadSiteNews()
        {
            if (HttpContext.Current.Cache[SiteNewsCache] == null) 
            { 
                var value = Context.GetAllPolicyNews().Select(i => new SiteNewsModel(i)).ToList();;
                HttpContext.Current.Cache.Insert(SiteNewsCache, value, null, DateTime.Now.AddDays(2),Cache.NoSlidingExpiration);
                return value;
            }
            return (List<SiteNewsModel>)HttpContext.Current.Cache[SiteNewsCache];
        }

        public List<NewsModel> LoadHotNews()
        {
            if (HttpContext.Current.Cache[HotNewsCache] == null)
            {
                var newsList = Context.GetConditionalNews(i => i.IsHotNew == true).OrderByDescending(i => i.DatePosted);
                var value = newsList.Count() > 6 ? newsList.ToList().GetRange(0, 6).Select(i => new NewsModel(i)).ToList() : 
                                                   newsList.Select(i => new NewsModel(i)).ToList();
                HttpContext.Current.Cache.Insert(HotNewsCache, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                return value;
            }
            return (List<NewsModel>)HttpContext.Current.Cache[HotNewsCache];
        }

        public List<BannerModel> LoadBanner()
        {
            if (HttpContext.Current.Cache[BannerCache] == null)
            {
                var value = Context.GetBanners(i => i.IsActive == true)
                                   .OrderBy(b =>  b.Type).Select(i => new BannerModel(i)).ToList();
                if (value.Count(i => i.Type == SiteSettings.SubBanner) > 3)
                {
                    value.RemoveRange(value.IndexOf(value.First(i => i.Type == SiteSettings.SubBanner)) + 3, value.Count - 3);
                }
                
                HttpContext.Current.Cache.Insert(BannerCache, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                return value;
            }
            return (List<BannerModel>)HttpContext.Current.Cache[BannerCache];
        }

        public List<CategoryModel> LoadCategory()
        {
            if (HttpContext.Current.Cache[CategoryCache] == null)
            {
                var value = Context.GetCategories(i => i.Id > 0).Select(i => new CategoryModel(i)).ToList();
                HttpContext.Current.Cache.Insert(CategoryCache, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                return value;
            }
            return (List<CategoryModel>)HttpContext.Current.Cache[CategoryCache];
        }
        public List<ShipmentFeeModel> LoadShipmentFee()
        {
            if (HttpContext.Current.Cache[ShipmentFeeCache] == null)
            {
                var value = Context.GetShipmentFees().Select(i => new ShipmentFeeModel(i)).ToList();
                HttpContext.Current.Cache.Insert(ShipmentFeeCache, value, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                return value;
            }
            return (List<ShipmentFeeModel>)HttpContext.Current.Cache[ShipmentFeeCache];
        }
        public OrderModel GetCartFromSession()
        {
            OrderModel order;
            if (HttpContext.Current.Session[CartSession] == null)
            {
                order = new OrderModel();
                HttpContext.Current.Session[CheckoutProgressSession] = 1;
                HttpContext.Current.Session[CartSession] = order;
            }
            else
            {
                order = (OrderModel)HttpContext.Current.Session[CartSession];
            }
            return order;
        }

        public int GetLastestCheckoutStep()
        {
            if (HttpContext.Current.Session[CheckoutProgressSession] != null)
                return (int)HttpContext.Current.Session[CheckoutProgressSession];

            HttpContext.Current.Session[CheckoutProgressSession] = 1;
            return 1;
        }

        public void SetLastestCheckoutStep(int step)
        {
            HttpContext.Current.Session[CheckoutProgressSession] = step;
        }

        public CustomerLoginModel GetCustomer()
        {
            if (HttpContext.Current.Session[CustomerLoginSession] == null)
            {
                HttpContext.Current.Session[CustomerLoginSession] = new CustomerLoginModel();
            }
            return (CustomerLoginModel)HttpContext.Current.Session[CustomerLoginSession];
        }

        public void SetCustomer(CustomerLoginModel cus)
        {
            HttpContext.Current.Session[CustomerLoginSession] = cus;
        }

        public void SetLoggedInUser(LoginViewModel user)
        {
            var xmlDoc = new XmlDocument();
            var xmlFilePath = HttpContext.Current.Server.MapPath("~/Areas/BackendSite/Models");
            switch (user.Role)
            {
                case RoleType.Admin:
                    xmlFilePath = Path.Combine(xmlFilePath, "UserAdminMenu.xml");
                    break;
                case RoleType.Editor:
                    xmlFilePath = Path.Combine(xmlFilePath, "UserEditorMenu.xml");
                    break;
                case RoleType.Member:
                    xmlFilePath = Path.Combine(xmlFilePath, "UserMemberMenu.xml");
                    break;
                default:
                    break;
            }
            xmlDoc.Load(xmlFilePath);
            var menu = new List<MenuLinkContainer>();
            foreach (XmlElement node in xmlDoc.DocumentElement.ChildNodes)
            {
                var item = new MenuLinkContainer();
                item.Menu = new MenuLink()
                {
                    IsCurrent = node.GetAttribute("IsActive") == "true",
                    Link = node.GetAttribute("Link"),
                    LinkText = node.GetAttribute("MenuText")
                };
                if (node.HasChildNodes)
                {
                    item.Links = node.ChildNodes.Cast<XmlElement>().Select(child => new MenuLink()
                    {
                        IsCurrent = child.GetAttribute("IsActive") == "true",
                        LinkText = child.GetAttribute("MenuText"),
                        Link = child.GetAttribute("Link")
                    }).ToList();
                }
                menu.Add(item);
            }
            user.UserMenu = menu;
            HttpContext.Current.Session[UserLoginSession] = user;
        }

        public LoginViewModel GetLoggedInUser()
        {
            if (HttpContext.Current.Session[UserLoginSession] != null)
            {
                return (LoginViewModel)HttpContext.Current.Session[UserLoginSession];
            }
            return null;
        }
    }
}
