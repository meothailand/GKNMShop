using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Models.LogicModel;
using GiaiKhatNgocMai.Infrastructure.Utils;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class CustomerAccountViewModel : ViewModelBase
    {
        public CustomerModel CustomerAccount { get; set; }
        public List<ShipmentFeeModel> ShipmentFees
        {
            get { return CacheHelper._CacheHelper.LoadShipmentFee().ToList(); }
        }
        public CustomerAccountViewModel()
        {
            CustomerAccount = new CustomerModel();
        }

        public CustomerAccountViewModel(Customer cus)
        {
            PagingInfo.GetPageInfo(cus.Orders.Count, 1, 5);
            var orders = cus.Orders.ToList().GetRange(PagingInfo.StartIndex, PagingInfo.Count);
            CustomerAccount = new CustomerModel(cus);
            CustomerAccount.Orders = orders.Select(i => new OrderModel(i)).ToList();
        }
    }
}