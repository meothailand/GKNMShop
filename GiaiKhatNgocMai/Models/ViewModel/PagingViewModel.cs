using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Infrastructure.Utils;
namespace GiaiKhatNgocMai.Models.ViewModel
{
    public class PagingViewModel<TItem> where TItem : class
    {
        public PagingHelper Paging { get; private set; }
        public List<TItem> Items { get; private set; }

        public PagingViewModel(PagingHelper paging, List<TItem> items)
        {
            Paging = paging;
            Items = items;
        }
    }
}