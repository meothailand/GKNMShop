using GiaiKhatNgocMai.Infrastructure.Exceptions;
using GiaiKhatNgocMai.Infrastructure.Security;
using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Orders = new List<OrderModel>();
        }
        public CustomerModel(Customer cus, bool passbyOrder = true): this()
        {
            ModelObjectHelper.CopyObject(cus, this);
            if (!passbyOrder)
            {
                foreach (var od in cus.Orders)
                {
                    Orders.Add(new OrderModel(od));
                }
            }           
        }
       
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPassword { get; set; }
        public string ShipAddress { get; set; }
        public string ShipDistrict { get; set; }
        public bool RecieveInfo { get; set; }
        public int CustomerPoint { get; set; }
        public virtual List<OrderModel> Orders { get; set; }

        public CustomerModel SetPassword(string password)
        {
            if (password.Length < 6) throw new InvalidDataException("Password phải dài ít nhất 6 ký tự.");
            CustomerPassword = password;
            return this;
        }

        public void ResetPassword(string newPass, string confirmPass, string currentPass, bool isReset)
        {
            AccountHelper.ResetPassword(newPass, confirmPass, currentPass, this.CustomerPassword, isReset);
        }
        /// <summary>
        /// Validate customer information
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerPhone"></param>
        /// <param name="customerEmail"></param>
        /// <param name="shipAddress"></param>
        /// <param name="shipDistrict"></param>
        /// <param name="customerPassword"></param>
        /// <exception cref="GiaiKhatNgocMai.Infrastructure.Exceptions.InvalidDataException"></exception>
        public static void ValidateCustomer(CustomerModel cus)
        {
            var isValid = (!string.IsNullOrWhiteSpace(cus.CustomerName) && cus.CustomerName.Length >= 2 && cus.CustomerName.Length <= 50) &&
                          (!string.IsNullOrWhiteSpace(cus.ShipAddress) && cus.ShipAddress.Length >= 10 && cus.ShipAddress.Length <= 50) &&
                          (AccountHelper.ValidatePhoneNumber(cus.CustomerPhone)) && (AccountHelper.ValidateEmail(cus.CustomerEmail));

            if (!isValid)
            {
                string[] message = new string[4];
                if (string.IsNullOrWhiteSpace(cus.CustomerName) || cus.CustomerName.Length < 2 || cus.CustomerName.Length > 50) message[0] = "Tên Khách Hàng";
                if (!AccountHelper.ValidatePhoneNumber(cus.CustomerPhone)) message[1] = "Số Điện Thoại";
                if (!AccountHelper.ValidateEmail(cus.CustomerEmail)) message[2] = "Email";
                if (string.IsNullOrWhiteSpace(cus.ShipAddress) || cus.ShipAddress.Length < 10 || cus.ShipAddress.Length > 50) message[3] = "Địa Chỉ";
                throw new InvalidDataException("Vui lòng kiểm tra lại các thông tin sau: " + string.Join(", ", message.Select(i => !string.IsNullOrWhiteSpace(i) ? i : "")));
            }
        }

        /// <summary>
        /// Validate customer information
        /// </summary>
        /// <param name="customerName"></param>
        /// <param name="customerPhone"></param>
        /// <param name="customerEmail"></param>
        /// <param name="shipAddress"></param>
        /// <param name="shipDistrict"></param>
        /// <param name="customerPassword"></param>
        /// <exception cref="GiaiKhatNgocMai.Infrastructure.Exceptions.InvalidDataException"></exception>
        public static void ValidateCustomer(Customer cus)
        {
            var isValid = (!string.IsNullOrWhiteSpace(cus.CustomerName) && cus.CustomerName.Length >= 2 && cus.CustomerName.Length <= 50) &&
                          (!string.IsNullOrWhiteSpace(cus.ShipAddress) && cus.ShipAddress.Length >= 10 && cus.ShipAddress.Length <= 50) &&
                          (AccountHelper.ValidatePhoneNumber(cus.CustomerPhone)) && (AccountHelper.ValidateEmail(cus.CustomerEmail));

            if (!isValid)
            {
                string[] message = new string[4];
                if (string.IsNullOrWhiteSpace(cus.CustomerName) || cus.CustomerName.Length < 2 || cus.CustomerName.Length > 50) message[0] = "Tên Khách Hàng";
                if (!AccountHelper.ValidatePhoneNumber(cus.CustomerPhone)) message[1] = "Số Điện Thoại";
                if (!AccountHelper.ValidateEmail(cus.CustomerEmail)) message[2] = "Email";
                if (string.IsNullOrWhiteSpace(cus.ShipAddress) || cus.ShipAddress.Length < 10 || cus.ShipAddress.Length > 50) message[3] = "Địa Chỉ";
                throw new InvalidDataException("Vui lòng kiểm tra lại các thông tin sau: " + string.Join(", ", message.Select(i => !string.IsNullOrWhiteSpace(i) ? i : "")));
            }
        }
        public Customer ToEntity()
        {
            var cus = new Customer();
            ValidateCustomer(this);
            ModelObjectHelper.CopyObject(this, cus);
            return cus;
        }
    }
}