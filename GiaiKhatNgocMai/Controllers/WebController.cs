using GiaiKhatNgocMai.Infrastructure.Implementation;
using GiaiKhatNgocMai.Infrastructure.Interfaces;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Controllers
{
    public class WebController : Controller
    {
        internal string mesageTempData = "mesageTempData";
        public IGiaiKhatNgocMaiDBContext Context { get; set; }
        public ISerializer Serializer { get; private set; }
        public WebController()
        {
            Context = GKNMDBContext.Instance;
            Serializer = SerializerHelper.Get();
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        protected string GetMessageTempData()
        {
            return (TempData[mesageTempData] != null) ? (string)TempData[mesageTempData] : null;
        }

        protected void SetMessageTempData(string msg)
        {
            TempData[mesageTempData] = msg;
        }

        protected void Login(string loginEmail, string loginPassword)
        {
            var customer = Context.GetCustomer(loginEmail, loginPassword);
            if (customer != null)
            {
                var cus = new CustomerLoginModel();
                cus.SetLogin(customer.Id, customer.CustomerName, customer.CustomerEmail);
                CacheHelper._CacheHelper.SetCustomer(cus);
                var cart = CacheHelper._CacheHelper.GetCartFromSession();
                cart.CustomerId = cus.Id;
                cart.SetCustomerInfo(customer.CustomerName, customer.CustomerPhone);
                cart.SetCustomerEmail(customer.CustomerEmail);
                cart.SetShipmentAddressAndFee(CacheHelper._CacheHelper.LoadShipmentFee().SingleOrDefault(i => i.District == customer.ShipDistrict),
                                              customer.ShipAddress);
            }
            else
            {
                throw new Exception("Login fail");
            }
        }
    }
}