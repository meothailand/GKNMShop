using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Infrastructure.Utils;
using System.ComponentModel;
using Newtonsoft.Json;
using GiaiKhatNgocMai.Infrastructure.Exceptions;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class OrderModel
    {
        public OrderModel() 
        {
            _orderDetails = new List<OrderDetailModel>();
        }
        public OrderModel(Order order, bool passbyDetail = true): this()
        {
            if (order != null)
            {
                ModelObjectHelper.CopyObject(order, this);
                this.StatusText = Status.HasValue ? Status.Value ? "Hoàn thành" : "Đã xác nhận" : "Đang chờ xác nhận";
                if (!passbyDetail)
                {
                    foreach (var dt in order.OrderDetails)
                    {
                        this._orderDetails.Add(new OrderDetailModel(dt));
                    }
                }                
            }            
        }
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> OrderedBy { get; set; }
        public string CustomerName { get; private set; }
        public string ContactNumber { get; private set; }
        public string EmailAddress { get; private set; }
        public DateTime OrderedDate { get; set; }
        public bool Delivery { get; private set; }
        public string ShipmentAddress { get; private set; }
        public string ShipmentDistrict { get; private set; }
        public DateTime ShipmentDate { get; private set; }
        public decimal ShipmentFee { get; private set; }
        public decimal Total { get; private set; }
        public string Note { get; set; }
        public Nullable<bool> Status { get; set; }
        public string StatusText { get; private set; }
        public decimal FeeThreshold { get; private set; }

        [JsonProperty(PropertyName = "items")]
        private List<OrderDetailModel> _orderDetails;
        public List<OrderDetailModel> OrderDetails
        {
            get { return _orderDetails; }
        }

        public Order ToEntity()
        {
            var entity = new Order();
            this.ShipmentFee = Total < FeeThreshold ? this.ShipmentFee : 0;
            if (!Delivery)
            {
                ShipmentAddress = "";
                ShipmentDistrict = "";
            }
            ModelObjectHelper.CopyObject(this, entity);
            foreach (var dt in this._orderDetails)
            {
                entity.OrderDetails.Add(ModelObjectHelper.CopyObject(dt, new OrderDetail()));
            }
            return entity;
        }
        public void AddItems(IEnumerable<OrderDetailModel> items)
        {
            foreach (var i in items)
            {
                i.Quantity = i.Quantity <= 1 ? 1 : i.Quantity;
                if (OrderDetails.Exists(dt => dt.ItemId == i.ItemId))
                {
                    var toUpdate = OrderDetails.SingleOrDefault(dt => dt.ItemId == i.ItemId);
                    toUpdate.Quantity = i.Quantity;
                }
                else
                {
                    OrderDetails.Add(i);                    
                }
            }
            CalculateTotal();
        }

        public void AddItem(OrderDetailModel item)
        {
            item.Quantity = item.Quantity <= 0 ? 1 : item.Quantity;
            if (OrderDetails.Exists(dt => dt.ItemId == item.ItemId))
            {
                var toUpdate = OrderDetails.SingleOrDefault(dt => dt.ItemId == item.ItemId);
                toUpdate.Quantity = item.Quantity;
            }
            else
            {
                OrderDetails.Add(item);
            }
            CalculateTotal();
        }

        public void RemoveItems(int[] ids)
        {
            foreach (var i in ids)
            {
                if (OrderDetails.Exists(dt => dt.ItemId == i))
                {
                    OrderDetails.Remove(OrderDetails.SingleOrDefault(dt => dt.ItemId == i));
                }
            }
            CalculateTotal();
        }

        public void RemoveItem(int id)
        {            
            if (OrderDetails.Exists(dt => dt.ItemId == id))
            {
                OrderDetails.Remove(OrderDetails.SingleOrDefault(dt => dt.ItemId == id));
            }
            CalculateTotal();
        }
        
        /// <summary>
        /// Set shipment address and district then calculate the ship fee, update order total and set Delivery to true
        /// </summary>
        /// <param name="fee"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public OrderModel SetShipmentAddressAndFee(ShipmentFeeModel fee, string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException("Yêu cầu nhập địa chỉ giao hàng.");
            if (fee == null) throw new ArgumentNullException("Địa chỉ giao hàng (tên quận) không hợp lệ");

            if (string.IsNullOrWhiteSpace(ShipmentAddress) || address.ToLower() != ShipmentAddress.ToLower()) 
                this.ShipmentAddress = address;

            if (string.IsNullOrWhiteSpace(ShipmentDistrict) || fee.District != ShipmentDistrict)
                this.ShipmentDistrict = fee.District;

            this.Delivery = true;
            this.ShipmentFee = fee.Fee;
            FeeThreshold = fee.FreeThreshold;            
            CalculateTotal();
            return this;
        }
        /// <summary>
        /// Set Delivery to false then re-calculate order total and clear ShipmentAddress, ShipmentDistrict if clearData set to true
        /// </summary>
        /// <param name="clearData"></param>
        /// <returns></returns>
        public OrderModel SetNoDelivery()
        {
            this.Delivery = false;
            ShipmentFee = 0;
            CalculateTotal();
            return this;
        }
        /// <summary>
        /// Set ship date and time - ship time must later than the current time at least 40 minutes
        /// </summary>
        /// <param name="shipDateTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException" value="hello"></exception>
        public OrderModel SetShipmentTime(DateTime shipDateTime)
        {
            if (shipDateTime.Day > DateTime.Now.Day || (shipDateTime - DateTime.Now).TotalMinutes >= 40)
            {
                this.ShipmentDate = shipDateTime;
                return this;
            }
            else { throw new ArgumentOutOfRangeException("Thời gian giao hàng phải trễ hơn thời gian đặt hàng ít nhất 40 phút."); }
        }
        /// <summary>
        /// Set customer name and phone
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="GiaiKhatNgocMai.Infrastructure.Exceptions.InvalidDataException"></exception>
        public OrderModel SetCustomerInfo(string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
                throw new ArgumentNullException("Yêu cầu nhập tên và số điện thoại liên lạc");
            if (!AccountHelper.ValidatePhoneNumber(phone))
                throw new InvalidDataException("Số điện thoại không đúng.");
            CustomerName = name;
            ContactNumber = phone;
            return this;
        }
        /// <summary>
        /// Set customer email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="GiaiKhatNgocMai.Infrastructure.Exceptions.InvalidDataException"></exception>
        public OrderModel SetCustomerEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("Địa chỉ email không hợp lệ");
            if (!AccountHelper.ValidateEmail(email)) 
                throw new InvalidDataException("Địa chỉ email không hợp lệ");

            EmailAddress = email;
            return this;
        }

        private void CalculateTotal()
        {
            this.Total = this.OrderDetails.Sum(i => i.Quantity * i.Price);
            if (Total == 0) ShipmentFee = 0;
            Total = !Delivery || (Total >= FeeThreshold) ? Total : (Total + ShipmentFee);
        }
    }
}