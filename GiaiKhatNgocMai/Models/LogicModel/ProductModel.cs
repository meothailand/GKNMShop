using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class ProductModel
    {
        public ProductModel(){}

        public ProductModel(Item item, bool passbyPhoto = true):this()
        {
            ModelObjectHelper.CopyObject(item, this);
            CategoryUrlName = item.Category.UrlName;
            CategoryName = item.Category.CategoryName;
            if (!passbyPhoto)
            {
                ItemPhotoes = item.ItemPhotoes.Select(i => new ItemPhotoModel(i)).ToList();
            }
        }
        public int Id { get; set; }
        public string CategoryUrlName { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public string UrlName { get; private set; }
        public string ItemDescription { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public bool IsFeatured { get; set; }
        public string Image { get; set; }
        public bool IsTopping { get; set; }
        public List<ItemPhotoModel> ItemPhotoes { get; set; }
        public Item ToEntity()
        {
            this.UrlName = StringHelper.VNSignedToLowerUnsigned(this.ItemName);
            var item = new Item();
            ModelObjectHelper.CopyObject(this, item);
            return item;            
        }
    }
}