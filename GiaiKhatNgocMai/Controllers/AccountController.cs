using GiaiKhatNgocMai.Infrastructure.Exceptions;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.LogicModel;
using GiaiKhatNgocMai.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiaiKhatNgocMai.Controllers
{
    [RoutePrefix("khach-hang")]
    public class AccountController : WebController
    {
        [Route("dang-nhap", Name="Login")]
        public ActionResult CustomerLogin()
        {
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn) return RedirectToRoute("Home");
            var model = new LoginViewModel();
            model.Navigations.Add("Đăng nhập", "");
            return View(model);
        }

        [Route("dang-nhap")]
        [HttpPost]
        public ActionResult CustomerLogin(string LoginEmail, string LoginPassword)
        {
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn) return RedirectToRoute("Home");
            try
            {
                Login(LoginEmail, LoginPassword);
            }
            catch
            {
                CacheHelper._CacheHelper.GetCustomer().SetError();
                var model = new LoginViewModel()
                {
                    LoginEmail = LoginEmail,
                    IsError = true,
                    Message = "Email đăng nhập hoặc mật khẩu không đúng"
                };
                model.Navigations.Add("Đăng nhập","");
                return View(model);
            }
            return RedirectToRoute("Home");
        }

        [Route("dang-ky")]
        public ActionResult CustomerRegister()
        {
            if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn) return RedirectToRoute("Home");
            var model = new CustomerAccountViewModel();
            model.Navigations.Add("Đăng ký tài khoản", "");
            return View(model);
        }

        [Route("dang-ky")]
        [HttpPost]
        public ActionResult CustomerRegister(string customerName, string customerPhone, string customerEmail,
                                             string shipAddress, string shipDistrict, string customerPassword, string confirmPassword)
        {
            var isInValid = false;
            var errMsg = "";
            if (!CacheHelper._CacheHelper.LoadShipmentFee().Exists(i => i.District == shipDistrict))
            {
                isInValid = true;
                errMsg += "Địa chỉ giao hàng [Quận] không tồn tại. ";
            }                
            if (Context.GetCustomers(i => i.CustomerEmail == customerEmail).Count() > 0)
            {
                isInValid = true;
                errMsg += "Địa chỉ email:" + customerEmail +" đã được sử dụng. Nếu bạn quên mật khẩu hãy vào [Đăng nhập] và chọn [Quên mật khẩu] để đặt lại mật khẩu mới. ";
            }
            if ((customerPassword != confirmPassword) || customerPassword.Length < 6)
            {
                isInValid = true;
                errMsg += (customerPassword != confirmPassword) ? "Mật khẩu và xác nhận mật khẩu khác nhau. " :
                    (customerPassword.Length < 6) ? "Mật khẩu ít nhất phải có 6 ký tự." : "";
            }
            if (isInValid)
            {
                var model = new CustomerAccountViewModel();
                model.CustomerAccount = new CustomerModel()
                {
                    CustomerName = customerName,
                    ShipAddress = shipAddress,
                    ShipDistrict = shipDistrict
                };
                model.IsError = isInValid;
                model.Message = errMsg;
                model.Navigations.Add("Đăng ký tài khoản", "");
                return View(model);
            }
            try
            {
                var customer = new CustomerModel().SetPassword(customerPassword);
                customer.CustomerEmail = customerEmail;
                customer.CustomerName = customerName;
                customer.CustomerPhone = customerPhone;
                customer.ShipAddress = shipAddress;
                customer.ShipDistrict = shipDistrict;
                customer.RecieveInfo = true;
                var id = Context.CreateCustomer(customer.ToEntity());
                var mailHelper = new MailHelper();
                var subj = "Giải khát Ngọc Mai - Welcome " + customerName;
                var body = RenderRazorViewToString("~/Views/Shared/NewAccountEmail.cshtml", customer);
                mailHelper.SendMailNoAttachment(customerEmail, subj, body, null, true);
                return View("CustomerLogin", new LoginViewModel() { IsAccountCreated = true, Message = "Tài khoản đã tạo thành công, bạn có thể đăng nhập vào shop." });
            }
            catch(Exception ex)
            {
                var exType = ex.GetType();
                var model = new CustomerAccountViewModel(); 
                model.CustomerAccount = new CustomerModel()
                {CustomerName = customerName,
                ShipAddress = shipAddress,
                ShipDistrict = shipDistrict};
                model.IsError = true;
                model.Navigations.Add("Đăng ký tài khoản", "");
                if(exType == typeof(InvalidDataException))
                {                        
                    model.Message = ex.Message;
                    return View(model);
                }
                else if(exType == typeof(System.Net.Mail.SmtpFailedRecipientsException) ||
                        exType == typeof(System.Net.Mail.SmtpException))
                {
                    model.Message = "Tài khoản của quý khách đã được tạo nhưng hệ thống không thể gửi mail cho quý khách theo địa chỉ mail quý khách cung cấp. " +
                                    "Xin quý khách vui lòng liên hệ chúng tôi để xác nhận địa chỉ email. Thành thật xin lỗi quý khách.";
                    return View(model);
                }
                else
                {
                    model.Message = "Không thể tạo được tài khoản. Vui lòng kiểm tra lại các dữ liệu nhập.";
                    return View(model);
                }
            }
        }

        [Route("mat-khau")]
        [HttpPost]
        public JsonResult ResetPassword(string email)
        {
            var cus = Context.GetCustomers(i => i.CustomerEmail == email).FirstOrDefault();
            if (cus != null)
            {
                var mailman = new MailHelper();
                var subj = "Giải khát Ngọc Mai - Lấy lại mật khẩu cho " + cus.CustomerName;
                var body = RenderRazorViewToString("~/Views/Shared/ResetPassword.cshtml", cus);
                try
                {
                    mailman.SendMailNoAttachment(cus.CustomerEmail, subj, body, null, true);
                    return Json(new
                    {
                        isfound = true,
                        issent = true,
                        message = string.Format("Chúng tôi đã gửi mail đến địa chỉ {0}. Bạn kiểm tra mail để lấy lại mật khẩu.", cus.CustomerEmail)
                    });
                }
                catch
                {
                    return Json(new 
                    { isfound = true, 
                      issent = false,
                      message = string.Format("Tài khoản tồn tại nhưng chúng tôi không thể gửi mail theo địa chỉ {0} cho bạn. Bạn vui lòng thử lại hoặc liên hệ với chúng tôi để được giúp đỡ.", cus.CustomerEmail)
                    });
                }
            }

            return Json(new
            {
                isfound = false,
                message = string.Format("Không có tài khoản nào cho địa chỉ email {0}", email)
            });
        }

        [Route("tai-khoan")]
        [CtrlRoleBased(SiteRole=RoleType.Customer)]
        public ActionResult ManageAccount()
        {
            var cus = Context.GetCustomer(i => i.Id == CacheHelper._CacheHelper.GetCustomer().Id);
            var model = new CustomerAccountViewModel(cus);
            model.Navigations.Add("Quản lý tài khoản", "");
            return View(model);
        }

        [Route("dang-xuat")]
        public ActionResult Logout()
        {
            CacheHelper._CacheHelper.ClearSession(CacheHelper.CustomerLoginSession);
            CacheHelper._CacheHelper.ClearSession(CacheHelper.CartSession);
            return Redirect("/");
        }
    }
}