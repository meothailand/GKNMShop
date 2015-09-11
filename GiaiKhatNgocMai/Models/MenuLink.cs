using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models
{
    public class MenuLinkContainer
    {
        public List<MenuLink> Links { get; set; }
        public MenuLink Menu;
        public MenuLinkContainer()
        {
            Menu = new MenuLink();
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

        public void SetCurrentLinkByLink(string link)
        {
            var menuLink = Links.SingleOrDefault(i => i.Link == link);
            foreach (var item in Links)
            {
                item.IsCurrent = false;
            }
            if (menuLink != null)
            {
                menuLink.IsCurrent = true;
            }
        }

        public bool ClearCurrentStatus()
        {
            Menu.IsCurrent = false;
            if (Links.Exists(i => i.IsCurrent == true))
            {
                foreach (var link in Links)
                {
                    link.IsCurrent = false;
                }
            }
            return true;
        }
    }

    public class MenuLink
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
        public bool IsCurrent { get; set; }
    }
}