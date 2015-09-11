using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Areas.BackendSite.Models
{
    public class BannerViewModel : ViewModelBase
    {
        public List<BannerModel> Banners { get; set; }
        public PagingHelper PageInfo { get; set; }
        public BannerViewModel()
        {
            HeaderModel.PageTitle = "Banners";
            HeaderModel.PageDescription = "manage website banners";
            HeaderModel.SetCalendar(DateTime.Today);
            HeaderModel.PageNavigation.Add("Banners", "");
        }
    }
}