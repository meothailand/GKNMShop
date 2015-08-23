using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class SiteNewsModel
    {
        public SiteNewsModel()
        {
            DatePosted = DateTime.Now;
        }
        public SiteNewsModel(SiteNew news): this()
        {
            ModelObjectHelper.CopyObject(news, this);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; private set; }
        public DateTime DateUpdated { get; private set; }
        public string UrlTitle { get; private set; }
        public SiteNew ToEntity()
        {
            var news = new SiteNew();
            news.UrlTitle = StringHelper.VNSignedToLowerUnsigned(Title);
            ModelObjectHelper.CopyObject(this, news);
            news.DateUpdated = DateTime.Now;
            return news;
        }
    }
}