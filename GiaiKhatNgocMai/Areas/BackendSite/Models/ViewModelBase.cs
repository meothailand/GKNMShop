using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Areas.BackendSite.Models
{
    public class ViewModelBase
    {
        public PageHeaderViewModel HeaderModel { get; set; }
        public ViewModelBase()
        {
            HeaderModel = new PageHeaderViewModel();
        }
    }
}