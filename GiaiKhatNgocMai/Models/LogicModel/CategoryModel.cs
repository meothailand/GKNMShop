using GiaiKhatNgocMai.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.LogicModel
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            SubCategories = new List<CategoryModel>();
            Items = new List<ProductModel>();
        }

        public CategoryModel(Category cate, bool passbyItem = true):this()
        {
            ModelObjectHelper.CopyObject(cate, this);
            foreach (var subcate in cate.Category1)
            {
                SubCategories.Add(new CategoryModel(subcate, passbyItem));
            }

            if (!passbyItem)
            {
                foreach (var item in cate.Items)
                {
                    Items.Add(new ProductModel(item));
                }
            }
        }
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ParentCategory { get; set; }
        public string CategoryDesciption { get; set; }
        public string UrlName { get; set; }

        public List<CategoryModel> SubCategories { get; set; }
        public List<ProductModel> Items { get; set; }
        public Category ToEntity()
        {
            UrlName = StringHelper.VNSignedToLowerUnsigned(CategoryName);
            var cate = new Category();
            ModelObjectHelper.CopyObject(this, cate);
            return cate;
        }
    }
}