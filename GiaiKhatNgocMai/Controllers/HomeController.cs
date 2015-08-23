using GiaiKhatNgocMai.Infrastructure.Exceptions;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.ViewModel;
using GiaiKhatNgocMai.Settings;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace GiaiKhatNgocMai.Controllers
{
    public class HomeController : WebController
    {
        string checkOutUrlPrefix = "/dat-hang/buoc/";
        string defaultErrorMessage = "Có lỗi trong quá trình xử lý. Vui lòng kiểm tra lại thông tin nhập. " +
                                    "Reload lại trang và thử lại. Nếu lỗi vẫn xảy ra hãy liên hệ với chúng tôi về tình trạng này." +
                                    "Cảm ơn quý khách.";

        [Route(template: "", Name = "Home")]
        public ActionResult Index()
        {
            var items = Context.GetItems(i => i.IsFeatured == true);
            var topNine = items.Count() > 9 ? items.ToList().GetRange(0, 9) : items;
            var model = new HomepageViewModel().SetFeatureProducts(topNine)
                                               .SetHotNews(Context.GetConditionalNews(i => i.IsHotNew == true && i.IsPublic == true));
            model.SiteLinks.SetCurrentLink("trang chủ");
            
            return View(model);
        }

        [Route(template:"San-pham", Name="CategoryDefault")]
        [Route(template:"San-pham/{id:int}/{category}", Name="Category")]
        [Route("San-pham/{id:int}/{category}/page/{page:int}")]
        public ActionResult MenuByCategory(int id, string category, int page = 1)
        {
            var model = new CategoryViewModel().SetCategory(Context.GetCategory(id), page);
            model.CurrentCateId = model.Category.Id;
            model.Navigations.Add(model.Category.CategoryName, "");
            return View(model);
        }

        [Route(template:"San-pham/{category}/{id:int}/{productName}", Name="Product")]
        public ActionResult Product(string category, int id, string productName)
        {
            var model = new ProductViewModel().SetProduct(Context.GetItem(id));
            var similars = Context.GetItems(i => i.CategoryId == model.Product.CategoryId 
                                                        && i.Id != model.Product.Id).ToList();
            model.SetSimilarProducts(similars.GetRange(0, similars.Count > 6 ? 6 : similars.Count));
            model.CurrentCateId = model.Product.CategoryId;
            model.Navigations.Add(model.Product.CategoryName, string.Format("/San-pham/{0}/{1}", model.Product.CategoryId, category));
            model.Navigations.Add(model.Product.ItemName, "");
            return View(model);
        }


        [Route(template:"Dat-hang/buoc/{step:int}", Name="Order")]
        public ActionResult Checkout(int step = 1)
        {
            var model = new CheckoutViewModel(checkOutUrlPrefix);
            model.Message = GetMessageTempData();
            model.IsError = !string.IsNullOrWhiteSpace(model.Message);
            model.Navigations.Add("Giỏ hàng", "");
            return View(model.SetCurrentStep(step));
        }

        [Route("Dat-hang/buoc/1/luu")]
        [HttpPost]
        public ActionResult CheckoutStep1(string orderNote)
        {
            var model = new CheckoutViewModel(checkOutUrlPrefix);
            model.Cart.Note = orderNote.Length <= 200 ? orderNote : orderNote.Substring(0, 200);
            return RedirectToAction("Checkout", new { step = 2 });
        }

        [Route("Dat-hang/buoc/2/luu")]
        [HttpPost]
        public ActionResult CheckoutStep2(string customer_name, string customer_phone, string email_address, string submit_button)
        {
            var model = new CheckoutViewModel(checkOutUrlPrefix);
            try
            {
                model.Cart.SetCustomerInfo(customer_name, customer_phone);
                if(!string.IsNullOrWhiteSpace(email_address))
                    model.Cart.SetCustomerEmail(email_address);
            }
            catch(Exception ex)
            {
                if(ex.GetType() == typeof(ArgumentNullException) || ex.GetType() == typeof(InvalidDataException))
                {
                    SetMessageTempData(ex.Message);
                }else
                {
                    SetMessageTempData(defaultErrorMessage);
                }
                return RedirectToAction("Checkout", new { step = 2 });
            }
            return RedirectToAction("Checkout", new { step = 3 });
        }

        [Route("dat-hang/buoc/2/dang-nhap")]
        [HttpPost]
        public ActionResult CheckoutStep2(string customer_email, string customer_password)
        {
            var onSuccessUrl = Url.RouteUrl("Order", new { step = 3 });
            var onErrorUrl = Url.RouteUrl("Order", new { step = 2 });
            try
            {
                Login(customer_email, customer_password);
                return RedirectToAction("Checkout", new { step = 3 });
            }
            catch
            {
                SetMessageTempData("Email đăng nhập hay mật khẩu không đúng");
                return RedirectToAction("Checkout", new { step = 2 });
            }
        }

        [Route("dat-hang/buoc/3/giaohang")]
        [HttpPost]
        public ActionResult CheckoutStep3(string shipment_address, int shipment_district, string shipment_date, int shipment_time)
        {
            var model = new CheckoutViewModel(checkOutUrlPrefix);           
            try
            {
                model.Cart.SetShipmentAddressAndFee(model.ShipmentFees.SingleOrDefault(i => i.Id == shipment_district),
                                                        shipment_address);
                var convertedDate = StringHelper.StringToDateTime(shipment_date, '/');
                var sTime = new DateTime(convertedDate.Year, convertedDate.Month, convertedDate.Day, shipment_time, 0, 0);
                model.Cart.SetShipmentTime(sTime);               
                
            }
            catch(Exception ex)
            {
                SetMessageTempData((ex.GetType() == typeof(ArgumentNullException) || ex.GetType() == typeof(ArgumentOutOfRangeException)) ?
                                ex.Message : defaultErrorMessage);
                return RedirectToAction("Checkout", new { step = 3 });
            }
            return RedirectToAction("Checkout", new { step = 4 });
        }

        [Route("dat-hang/buoc/3/nhanhang")]
        [HttpPost]
        public ActionResult CheckoutStep3(string shipment_date1, int shipment_time1)
        {
            var model = new CheckoutViewModel("/dat-hang/buoc");
            try
            {
                model.Cart.SetNoDelivery();
                var convertedDate = StringHelper.StringToDateTime(shipment_date1, '/');
                var sTime = new DateTime(convertedDate.Year, convertedDate.Month, convertedDate.Day, shipment_time1, 0, 0);
                model.Cart.SetShipmentTime(sTime);                
            }
            catch (Exception ex)
            {
                SetMessageTempData((ex.GetType() == typeof(ArgumentOutOfRangeException)) ?
                                ex.Message : defaultErrorMessage);
                return RedirectToAction("Checkout", new { step = 3 });
            }
            return RedirectToAction("Checkout", new { step = 4 });
        }

        [Route("dat-hang/luu-don-hang")]
        public ActionResult CheckoutStep5()
        {
            var model = new CheckoutViewModel(checkOutUrlPrefix);
            if (model.Cart.OrderDetails.Count == 0) return RedirectToAction("Checkout", new { step = 1 });
            try
            {
                if (!model.Cart.Delivery) model.Cart.SetNoDelivery();
                model.Cart.OrderedDate = DateTime.Now;
                string orderNo = Context.CreateOrder(model.Cart.ToEntity());
                model.Cart.OrderNumber = orderNo;
            }
            catch
            {
                SetMessageTempData(defaultErrorMessage);
                return RedirectToAction("Checkout", new { step = 4 });
            }
            return RedirectToAction("Checkout", new { step = 5 });
        }

        [Route("Chinh-sach")]
        public ViewResult SitePolicy()
        {
            var model = new ViewModelBase();
            model.SiteLinks.SetCurrentLink("chính sách giao hàng");
            model.Navigations.Add("Chính sách giao hàng", "");
            return View(model);
        }

        [Route("Tin-tuc")]
        [Route("Tin-tuc/{id:int}/{title}")]
        public ViewResult SiteNews(int? id, string title)
        {
            var model = new NewsViewModel();
            if (id.HasValue)
            {
                 
            }
            else
            {

            }
            model.SiteLinks.SetCurrentLink("tin tức");
            return View(model);
        }
        [Route("lien-he")]
        public ViewResult ContactUs()
        {
            var model = new ContactViewModel();
            model.SiteLinks.SetCurrentLink("liên hệ");
            return View(model);
        }
        
    }
}
