using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class OrderDetailModel
    {
        public OrderDetailModel() {  }
        public OrderDetailModel(Item item)
        {
            ItemId = item.Id;
            ItemName = item.ItemName;
            Quantity = 1;
            Price = item.Price;
            Unit = item.Unit;
            UrlName = StringHelper.VNSignedToLowerUnsigned(ItemName);
            ItemPhoto = item.Image;
        }
        public OrderDetailModel(OrderDetail detail):this()
        {
            if (detail != null)
                ModelObjectHelper.CopyObject(detail, this);
            UrlName = StringHelper.VNSignedToLowerUnsigned(ItemName);
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public Nullable<int> ToppingId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string UrlName { get; set; }
        public string ItemPhoto { get; set; }
        public OrderDetail ToEntity()
        {
            var orderdetail = new OrderDetail();
            ModelObjectHelper.CopyObject(this, orderdetail);
            return orderdetail;
        }
    }
}