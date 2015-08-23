using GiaiKhatNgocMai.Models.LogicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class HomepageViewModel: ViewModelBase
    {
        public IEnumerable<ProductModel> FeaturedProducts { get; private set; }
        public IEnumerable<NewsModel> HotNews { get; private set; }

        public HomepageViewModel SetFeatureProducts(IEnumerable<Item> products)
        {
            FeaturedProducts = products.Select(i => new ProductModel(i)).ToList();
            return this;
        }

        public HomepageViewModel SetHotNews(IEnumerable<News> newsList)
        {
            HotNews = newsList.Select(i => new NewsModel(i)).ToList();
            return this;
        }
    }
}