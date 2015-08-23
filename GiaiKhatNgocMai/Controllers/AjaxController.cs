using GiaiKhatNgocMai.Infrastructure.Exceptions;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models;
using GiaiKhatNgocMai.Models.LogicModel;
using GiaiKhatNgocMai.Models.ViewModel;
using GiaiKhatNgocMai.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Controllers
{
    public class AjaxController : WebController
    {
        // GET: Ajax
        public ActionResult GetCategorizedProductByPage(int cateId, int page)
        {
            var products = Context.GetItems(i => i.CategoryId == cateId);
            var paging = new PagingHelper().GetPageInfo(products.Count(), page, 9);
            var result = products.ToList().GetRange(paging.StartIndex, paging.Count).Select(i => new ProductModel(i));
            var model = new PagingViewModel<ProductModel>(paging, result.ToList());
            return View();
        }

        public ActionResult GetNewsByPage(int page)
        {
            var newsList = Context.GetConditionalNews(i => i.IsPublic == true);
            var paging = new PagingHelper().GetPageInfo(newsList.Count(), page, 5);
            var items = newsList.ToList().GetRange(paging.StartIndex, paging.Count).Select(i => new NewsModel(i));
            var model = new PagingViewModel<NewsModel>(paging, items.ToList());
            return View();
        }

        [Route("cart/add")]
        [HttpPost]
        public JsonResult AddCart(int itemId)
        {
            var order = CacheHelper._CacheHelper.GetCartFromSession();
            OrderDetailModel toUpdate;
            if(order.OrderDetails.Exists(i => i.ItemId == itemId)) {
                toUpdate = order.OrderDetails.SingleOrDefault(i => i.ItemId == itemId);
                toUpdate.Quantity += 1;
            }else{
                toUpdate = new OrderDetailModel(Context.GetItem(itemId));
            }
            
            order.AddItem(toUpdate);

            return Json(new
                        {
                            cartHtml = RenderRazorViewToString("_CartPartial", new CartViewModel()),
                            itemCount = order.OrderDetails.Sum(i => i.Quantity)
                        });
        }

        [Route("cart/update")]
        [HttpPost]
        public JsonResult UpdateCart(int itemId, int? toppingId, int quantity = 1, bool getOrderRow = false)
        {
            quantity = quantity <= 0 ? 1 : quantity;
            var order = CacheHelper._CacheHelper.GetCartFromSession();
            OrderDetailModel toUpdate = toUpdate = (order.OrderDetails.Exists(i => i.ItemId == itemId)) ?
                            order.OrderDetails.SingleOrDefault(i => i.ItemId == itemId) :
                            new OrderDetailModel(Context.GetItem(itemId));

            toUpdate.Quantity = quantity;
            order.AddItem(toUpdate);                 

            return getOrderRow ? Json(new
                                    {
                                        cartHtml = RenderRazorViewToString("_CartPartial", new CartViewModel()),
                                        itemCount = order.OrderDetails.Sum(i => i.Quantity),
                                        itemTotal = string.Format(SiteSettings.GetCulture, "{0:c0}", toUpdate.Quantity*toUpdate.Price),
                                        shipping = order.Total >= order.FeeThreshold ? "---" : string.Format(SiteSettings.GetCulture, "{0:c0}", order.ShipmentFee),
                                        productsTotal = string.Format(SiteSettings.GetCulture, "{0:c0}", order.OrderDetails.Sum(i => i.Quantity*i.Price)),
                                        totalOrder = string.Format(SiteSettings.GetCulture, "{0:c0}", order.Total),
                                    }) :
                                Json(new
                                {
                                    cartHtml = RenderRazorViewToString("_CartPartial", new CartViewModel()),
                                    itemCount = order.OrderDetails.Sum(i => i.Quantity)
                                });
        }

        [Route("cart/remove")]
        [HttpDelete]
        public JsonResult RemoveFromCart(int itemId)
        {
            var order = CacheHelper._CacheHelper.GetCartFromSession();
            order.RemoveItem(itemId);

            return Json(new
            {
                cartHtml = RenderRazorViewToString("_CartPartial", new CartViewModel()),
                itemCount = order.OrderDetails.Sum(i => i.Quantity),
                shipping = order.Total >= order.FeeThreshold ? "---" : string.Format(SiteSettings.GetCulture, "{0:c0}", order.ShipmentFee),
                productsTotal = string.Format(SiteSettings.GetCulture, "{0:c0}", order.OrderDetails.Sum(i => i.Quantity * i.Price)),
                totalOrder = string.Format(SiteSettings.GetCulture, "{0:c0}", order.Total),
            });
        }

        [Route("ajax/get-customer-orders/{page:int}")]
        public JsonResult GetCustomerOrders(int page)
        {
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn)
            {
                var orders = Context.GetOrders(i => i.CustomerId == CacheHelper._CacheHelper.GetCustomer().Id).ToList();
                var pageinfo = new PagingHelper();
                pageinfo.GetPageInfo(orders.Count, page, 5);
                var orderModels = orders.GetRange(pageinfo.StartIndex, pageinfo.Count).Select(i => new OrderModel(i)); ;
                return Json(new
                {
                    loggedIn = true,
                    order_table = RenderRazorViewToString("_CustomerOrderTablePartial", orderModels),
                    page_info = pageinfo

                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                loggedIn = false,
                link = Url.RouteUrl("Login")

            }, JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/get-order-for-customer/{id:int}/{num}")]
        public JsonResult GetCustomerOrders(int id, string num)
        {
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn)
            {
                var order = Context.GetOrder(id);
                if (order != null && order.CustomerId == CacheHelper._CacheHelper.GetCustomer().Id)
                    return Json(new
                    {
                        loggedIn = true,
                        found = true,
                        order_detail = RenderRazorViewToString("_OrderDetailPartial", new OrderModel(order, passbyDetail: false))
                    }, JsonRequestBehavior.AllowGet);

                return Json(new
                {
                    loggedIn = true,
                    found = false,
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                loggedIn = false,
                link = Url.RouteUrl("Login")

            }, JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/updatecustomer")]
        [HttpPost]
        public JsonResult UpdateCustomerInfo(string name, string phone, string address, int district, bool receiveInfo)
        {
            var isLoggedIn = CacheHelper._CacheHelper.GetCustomer().IsLoggedIn;
            var found = false;
            var updated = false;
            if (isLoggedIn)
            {
                var customer = Context.GetCustomer(i => i.Id == CacheHelper._CacheHelper.GetCustomer().Id);
                found = (customer != null);
                if (found)
                {
                    try
                    {
                        var fees = CacheHelper._CacheHelper.LoadShipmentFee();
                        customer.CustomerName = name;
                        customer.CustomerPhone = phone;
                        customer.ShipAddress = address;
                        customer.ShipDistrict = fees.SingleOrDefault(i => i.Id == district).District;
                        customer.RecieveInfo = receiveInfo;
                        CustomerModel.ValidateCustomer(customer);
                        Context.UpdateCustomer(customer);
                        updated = true;
                    }
                    catch (Exception e)
                    {
                        return Json(new
                        {
                            loggedIn = isLoggedIn,
                            found = found,
                            updated = false,
                            error = e.GetType() == typeof(InvalidDataException) ? e.Message :
                            "Không thể cập nhật thông tin. Xin refresh lại trang và thử lại hoặc liên hệ hotline của shop để được hỗ trợ."
                        });
                    }                    
                }
            }

            return Json(new
            {
                loggedIn = isLoggedIn,
                found = found,
                updated = updated,
                link = Url.RouteUrl("Login")
            });
        }
    
        [Route("ajax/resetpassword")]
        [HttpPost]
        public JsonResult ResetPassword(string email, string current, string newp, string cfmp)
        {
            var isLoggedIn = CacheHelper._CacheHelper.GetCustomer().IsLoggedIn;
            var found = false;
            var updated = false;            
           
            if (isLoggedIn)
            {
                var customer = Context.GetCustomer(email, current);
                found = (customer != null);
                if (found)
                {
                    if (newp != cfmp || newp.Length < 6)
                    {
                        return Json(new { 
                            isLoggedIn = isLoggedIn,
                            found = found,
                            updated = updated,
                            error = ((newp != cfmp) ? "Mật khẩu và xác nhận mật khẩu không trùng khớp. ": "") +
                                    ((newp.Length < 6) ? "Mật khẩu phải chứa ít nhất 6 kí tự." : "")
                        });
                    }

                    customer.CustomerPassword = newp;
                    Context.UpdateCustomer(customer);
                    updated = true;
                }
            }
            return Json(new
            {
                isLoggedIn = isLoggedIn,
                found = found,
                updated = updated,
                link = Url.RouteUrl("Login")
            });
        }

        [Route("ajax/sendmessage")]
        [HttpPost]
        public JsonResult SendMessage(string message, string name, string email)
        {
            var isValid = ((!string.IsNullOrWhiteSpace(message) || message.Length < 50)&&
                           (!string.IsNullOrWhiteSpace(name))&&
                           !string.IsNullOrWhiteSpace(email));
            isValid = isValid ? AccountHelper.ValidateEmail(email) : false;

            if (isValid)
            {
                try
                {
                    MailHelper mail = new MailHelper();
                    mail.SelfSendMailNoAttachment(string.Format("[GKNM] Message from: {0}", name), message, null);
                }
                catch
                {
                    return Json(new
                    {
                        sent = false,
                        error = "Không thể gửi tin nhắn cho hệ thống. Vui lòng refresh trang và thử lại hoặc liên hệ hotline với chúng tôi."
                    });
                }
            }

            return Json(new
            {
                sent = isValid,
                error = isValid ? "" : "Dữ liệu nhập không hợp lý. Vui lòng kiểm tra và thử lại."
            });
        }
    }
}