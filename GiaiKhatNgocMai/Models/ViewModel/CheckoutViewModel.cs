using GiaiKhatNgocMai.Infrastructure.Utils;
using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class CheckoutViewModel: ViewModelBase
    {
        public StepModel[] Steps { get; private set; }
        public string StepTitle { get; private set; }
        public int CurrentStep { get; private set; }
        public int LastestStep { get; private set; }
        public CartView CartReview { get; private set; }
        public List<ShipmentFeeModel> ShipmentFees { get; private set; }
        public CheckoutViewModel (string baseLink)
	    {
            if (baseLink.LastIndexOf('/') + 1 < baseLink.Length) baseLink = baseLink + "/";
            Steps = new StepModel[]{    new StepModel(1, "Xem giỏ hàng", baseLink + "1"),
                                        new StepModel(2, "Khách hàng", baseLink + "2"),
                                        new StepModel(3, "Giao hàng", baseLink + "3"),
                                        new StepModel(4, "Xác nhận", baseLink + "4"),
                                        new StepModel(5, "Hoàn tất", baseLink + "5")                                   
                                   };
            CurrentStep = 1;
            LastestStep = CacheHelper._CacheHelper.GetLastestCheckoutStep();
            ShipmentFees = CacheHelper._CacheHelper.LoadShipmentFee().ToList();
	    }

        public Dictionary<int, string> DeliveryTime
        {
            get
            {
                return new Dictionary<int, string>(){   {8, "8 giờ sáng"},
                                                        {9, "9 giờ sáng"}, 
                                                        {10, "10 giờ sáng"},
                                                        {11, "11 giờ sáng"},
                                                        {12, "12 giờ trưa"},
                                                        {13, "1 giờ chiều"},
                                                        {14, "2 giờ chiều"},
                                                        {15, "3 giờ chiều"},
                                                        {16, "4 giờ chiều"},
                                                        {17, "5 giờ chiều"},
                                                        {18, "6 giờ chiều"}};
            }
        }
        public CheckoutViewModel SetCurrentStep(int step)
        {
            if (Cart.OrderDetails.Count == 0 || step < 1) step = 1;
            if (step - LastestStep > 1 || step > Steps.Length) step = LastestStep;            
            switch (step)
            {
                case 1: 
                    StepTitle = "Thống kê giỏ hàng"; 
                    break;
                case 2:
                    if (CacheHelper._CacheHelper.GetCustomer().IsLoggedIn)
                    {
                        step = 3;
                        break;
                    }
                    StepTitle = "Thông tin khách hàng"; 
                    break;
                case 3:
                    StepTitle = "Thông tin giao hàng";
                    break;
                case 4: 
                    StepTitle = "Xác nhận đơn hàng"; 
                    break;
                case 5: 
                    StepTitle = "Hoàn tất đặt hàng";
                    CartReview = new CartView()
                    {
                        ContactNumber = Cart.ContactNumber,
                        CustomerName = Cart.CustomerName,
                        Delivery = Cart.Delivery,
                        OrderNumber = Cart.OrderNumber,
                        ShipmentAddress = Cart.Delivery ? Cart.ShipmentAddress + " " + Cart.ShipmentDistrict : 
                                          "Khách nhận hàng tại Giải khát Ngọc Mai",
                        ShipmentDate = Cart.ShipmentDate,
                        Total = Cart.Total
                    };
                    ClearCart();
                    break;
            }
            CurrentStep = step;
            if(CurrentStep == Steps.Length)
            {
                CacheHelper._CacheHelper.SetLastestCheckoutStep(1);
            }
            else if (CurrentStep > LastestStep)
            {
                CacheHelper._CacheHelper.SetLastestCheckoutStep(CurrentStep);
            }            
            return this;
        }

        public void ClearCart()
        {
            Cart.RemoveItems(Cart.OrderDetails.Select(i => i.ItemId).ToArray());
            Cart.OrderNumber = null;
            Cart.SetNoDelivery();            
        }

        public class StepModel
        {
            public StepModel(int step, string text, string url)
            {
                Step = step;
                DisplayText = text;
                Url = url;
            }
            public int Step { get; set; }
            public string DisplayText { get; set; }
            public string Url { get; set; }
        }

        public class CartView
        {
            public string OrderNumber { get; set; }
            public DateTime ShipmentDate { get; set; }
            public string ContactNumber { get; set; }
            public string CustomerName { get; set; }
            public string ShipmentAddress { get; set; }
            public decimal Total { get; set; }
            public bool Delivery { get; set; }
        }
    }
}