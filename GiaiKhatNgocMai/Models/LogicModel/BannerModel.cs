using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class BannerModel
    {
        public BannerModel() { }
        public BannerModel(Banner banner)
            : this()
        {
            ModelObjectHelper.CopyObject(banner, this);
            this.BannerImage = string.Format("{0}/{1}",SiteSettings.GetNewsBannerPath(), this.BannerImage);
        }
        public int Id { get; set; }
        public string BannerLink { get; set; }
        public string Caption1 { get; set; }
        public string Caption2 { get; set; }
        public string Caption3 { get; set; }
        public string BannerImage { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public Banner ToEntity()
        {
            var banner = new Banner();
            ModelObjectHelper.CopyObject(this, banner);
            return banner;
        }
    }
}