using GiaiKhatNgocMai.Areas.BackendSite.Models;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Areas.BackendSite.Controllers
{
    [BackendRoleBased(new RoleType[] { RoleType.Admin, RoleType.Editor })]
    public class DashboardController : BaseController
    {
        // GET: BackendSite/Dashboard        
        public ActionResult Index()
        {
            var model = new DashboardViewModel();
            var dateOfMonth = GetFirstLastDayOfMonth();
            var orders = Context.GetOrders(i => i.OrderedDate >= dateOfMonth[0] &&
                                                       i.OrderedDate <= dateOfMonth[1]);
            model.MonthOrders = orders.Select(i => new OrderModel(i)).ToList();
            return View(model);
        }

        public ActionResult GetUnConfirmedOrders()
        {
            var dateOfMonth = GetFirstLastDayOfMonth();
            var orders = Context.GetOrders(i => i.OrderedDate >= dateOfMonth[0] &&
                                                i.OrderedDate <= dateOfMonth[1] &&
                                                i.Status == null);
            return PartialView("_OrderTablePartial", orders);
        }

        public ActionResult GetTodayDeliverOrders()
        {
            var orders = Context.GetOrders(i => i.ShipmentDate >= DateTime.Today &&
                                                i.ShipmentDate < DateTime.Today.AddDays(1));
            return PartialView("_OrderTablePartial", orders);
        }

        public ActionResult GetUpcomingOrders()
        {
            var orders = Context.GetOrders(i => i.ShipmentDate >= DateTime.Today.AddDays(1));
            return PartialView("_OrderTablePartial", orders);
        }

        public JsonResult GetOrderCountInMonth()
        {
            var model = new DashboardViewModel();
            var dateOfMonth = GetFirstLastDayOfMonth();
            var orders = Context.GetOrders(i => i.OrderedDate >= dateOfMonth[0] &&
                                                       i.OrderedDate <= dateOfMonth[1]);
            return Json(new
            {
                toConfirmOrder = orders.Count(i => i.Status == null),
                todayDeliveryOrder = orders.Count(i => i.ShipmentDate >= DateTime.Today && i.ShipmentDate < DateTime.Today.AddDays(1)),
                upcomingOrder = orders.Count(i => i.ShipmentDate >= DateTime.Today.AddDays(1)),
                totalOrder = orders.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        private DateTime[] GetFirstLastDayOfMonth()
        {
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(1);
            return new DateTime[]{firstDayOfMonth, lastDayOfMonth};
        }
    }
}