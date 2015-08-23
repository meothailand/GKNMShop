using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Settings
{
    public class SiteSettings
    {
        public const string TokenKey = "meothailand-accesskey";
        public const string MainBanner = "Main banner";
        public const string SubBanner = "Sub banner";
        public static string GetProductImagePath()
        {
            return ConfigurationManager.AppSettings["ProductPhotoLocation"];
        }

        public static string GetThumbnailPath()
        {
            return ConfigurationManager.AppSettings["ThumbPhotoLocation"];
        }

        public static string GetNewsBannerPath()
        {
            return ConfigurationManager.AppSettings["NewsPhotoLocation"];
        }
        public static CultureInfo GetCulture
        {
            get { return System.Globalization.CultureInfo.GetCultureInfo("vi-VN");}
        }
    }
}