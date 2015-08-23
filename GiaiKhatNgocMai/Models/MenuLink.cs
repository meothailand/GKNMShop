using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models
{
    public class MenuLinkContainer
    {
        public List<MenuLink> Links { get; set; }
        public MenuLinkContainer()
        {
            Links = new List<MenuLink>();
        }

        public void SetCurrentLink(string linktext)
        {
            var link = Links.SingleOrDefault(i => i.LinkText == linktext);
            Links.Select(l => l.IsCurrent = false);
            if (link != null)
            {
                link.IsCurrent = true;
            }
        }
    }

    public class MenuLink
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
        public bool IsCurrent { get; set; }
    }
}