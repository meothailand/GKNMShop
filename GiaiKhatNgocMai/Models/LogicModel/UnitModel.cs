using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class UnitModel
    {
        public UnitModel() { }
        public UnitModel(Unit unit)
        {
            Id = unit.Id;
            UnitName = unit.UnitName;
        }
        public int Id { get; set; }
        public string UnitName { get; set; }

        public Unit ToEntity()
        {
            var unit = new Unit();
            unit.Id = Id;
            unit.UnitName = UnitName;
            return unit;
        }
    }
}