using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class ShipmentFeeModel
    {
        public ShipmentFeeModel()
        {
        }
        public ShipmentFeeModel(ShipmentFee fee):this()
        {
            ModelObjectHelper.CopyObject(fee, this);
            FreeThreshold = (fee.FreeThreshold.HasValue) ? fee.FreeThreshold.Value : 0;
        }
        public int Id { get; set; }
        public string District { get; set; }
        public decimal Fee { get; set; }
        public decimal FreeThreshold { get; set; }
        public ShipmentFee ToEntity()
        {
            var fee = new ShipmentFee();
            ModelObjectHelper.CopyObject(this, fee);
            return fee;
        }
    }
}