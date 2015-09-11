using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Areas.BackendSite.Models
{
    public class PageHeaderViewModel
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageCalendar { get; private set; }
        public Dictionary<string,string> PageNavigation { get; set; }
        public PageHeaderViewModel()
        {
            PageNavigation = new Dictionary<string, string>();
            var firstdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var lastdate = firstdate.AddMonths(1).AddDays(-1);
            PageCalendar = string.Format("{0: MMMM, dd, yyyy} - {1: MMMM, dd, yyyy}", firstdate, lastdate);
        }
        public PageHeaderViewModel SetCalendar(DateTime calendar)
        {
            PageCalendar = string.Format("{0: MMMM, dd, yyyy}", calendar);
            return this;
        }
    }
}