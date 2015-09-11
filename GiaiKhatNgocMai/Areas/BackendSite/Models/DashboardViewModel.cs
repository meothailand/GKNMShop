using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Areas.BackendSite.Models
{
    public class DashboardViewModel : ViewModelBase
    {
        public List<OrderModel> MonthOrders { get; set; }

        public PagingHelper PageInfo { get; set; }

        public DashboardViewModel()
        {
            HeaderModel.PageTitle = "Dashboard";
            HeaderModel.PageDescription = "statistics and more";
            HeaderModel.PageNavigation.Add("Dashboard", "");
        }
    }
}