using GiaiKhatNgocMai.Infrastructure.Implementation;
using GiaiKhatNgocMai.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Areas.BackendSite.Controllers
{
    public class BaseController : Controller
    {
        internal string tempData = "TempDataMessage";
        public IGiaiKhatNgocMaiDBContext Context { get; private set; }
        public BaseController()
        {
            Context = GKNMDBContext.Instance;
        }
        public void SetTempData<T>(T value) where T : class
        {
            TempData[tempData] = value;
        }
        public T GetTempData<T>() where T : class
        {
            try
            {
                return (T)TempData[tempData];
            }
            catch
            {
                return null;
            }
        }
    }
}