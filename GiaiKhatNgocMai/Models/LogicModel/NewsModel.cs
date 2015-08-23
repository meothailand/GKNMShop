using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class NewsModel
    {
        public NewsModel()
        {
            DatePosted = DateTime.Now;
        }
        public NewsModel(News news):this()
        {
            ModelObjectHelper.CopyObject(news, this);
            Author = news.User.UserName;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] NewsAvatar { get; set; }
        public bool IsHotNew { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DatePosted { get; set; }
        public int PostedBy { get; set; }
        public string UrlTitle { get; set; }
        public string Author { get; set; }
        public News ToEntity()
        {
            var news = new News();
            ModelObjectHelper.CopyObject(this,news);
            news.UrlTitle = StringHelper.VNSignedToLowerUnsigned(news.Title);
            return news;
        }
    }
}