using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Models.LogicModel;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Infrastructure.Interfaces;
using GiaiKhatNgocMai.Infrastructure.Implementation;
using GiaiKhatNgocMai.Settings;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class ViewModelBase
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Navigations { get; set; }
        public string ThumbPath { get; private set; }
        public string PhotoPath { get; private set; }
        public PagingHelper PagingInfo { get; private set; }
        public int CurrentCateId { get; set; }
        public MenuLinkContainer SiteLinks { get; private set; }

        public ViewModelBase()
        {
            Navigations = new Dictionary<string, string>(){{"Trang chủ", "/"}};
            PagingInfo = new PagingHelper();
            CurrentCateId = 0;
            ThumbPath = SiteSettings.GetThumbnailPath();
            PhotoPath = SiteSettings.GetProductImagePath();
            GenerateLink();
        }
        public OrderModel Cart
        {
            get
            {
                return CacheHelper._CacheHelper.GetCartFromSession();
            }
        }
        public List<CategoryModel> Categories { 
            get{ return CacheHelper._CacheHelper.LoadCategory(); } 
        }
        public List<SiteNewsModel> Policies { get { return CacheHelper._CacheHelper.LoadSiteNews().ToList(); } }

        public CultureInfo Culture()
        {
            return SiteSettings.GetCulture;
        }

        private void GenerateLink()
        {
            SiteLinks = new MenuLinkContainer();
            SiteLinks.Links.AddRange(new List<MenuLink>()
            {
                new MenuLink(){ LinkText="trang chủ", IsCurrent= false, Link = "/"},
                new MenuLink(){ LinkText="hướng dẫn mua hàng", IsCurrent= false, Link = "/huong-dan"},
                new MenuLink(){ LinkText="chính sách giao hàng", IsCurrent= false, Link = "/chinh-sach"},
                new MenuLink(){ LinkText="tin tức", IsCurrent= false, Link = "/tin-tuc"},
                new MenuLink(){ LinkText="liên hệ", IsCurrent= false, Link = "/lien-he"}
            });
        }
    }
}