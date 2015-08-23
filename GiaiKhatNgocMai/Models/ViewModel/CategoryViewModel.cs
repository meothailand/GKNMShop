using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Infrastructure.Utils;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        public CategoryModel Category { get; private set; }
        public CategoryViewModel SetCategory(Category cate, int page)
        {
            PagingInfo.GetPageInfo(cate.Items.Count(), page, 9);
            var products = cate.Items.ToList().GetRange(PagingInfo.StartIndex, PagingInfo.Count);
            cate.Items = products;
            Category = new CategoryModel(cate, passbyItem:false);
            return this;
        }
    }
}