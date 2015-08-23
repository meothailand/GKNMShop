using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Models.LogicModel;
using GiaiKhatNgocMai.Settings;
namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        public ProductModel Product { get; private set; }
        public IEnumerable<ProductModel> SameCateProducts { get; private set; }

        public ProductViewModel SetProduct(Item value)
        {
            Product = new ProductModel(value, passbyPhoto:false);
            return this;
        }
        public ProductViewModel SetSimilarProducts(IEnumerable<Item> value)
        {
            SameCateProducts = value.Select(i => new ProductModel(i));
            return this;
        }
    }
}