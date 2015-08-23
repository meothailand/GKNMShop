namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class PagingHelper
    {
        public int Current { get; private set; }
        public int TotalPage { get; private set; }
        public int TotalItems { get; private set; }
        public int StartIndex { get; private set; }
        public int Count { get; private set; }

        public PagingHelper GetPageInfo(int totalItem, int page = 1, int itemsPerPage = 10)
        {
            if (totalItem == 0) return this;
            TotalItems = totalItem;
            TotalPage = (totalItem % itemsPerPage > 0) ? (totalItem / itemsPerPage) + 1 : (totalItem / itemsPerPage);
            Current = (page < 1) ? 1 : (page > TotalPage) ? TotalPage : page;
            StartIndex = (Current - 1) * itemsPerPage;
            Count = (Current == TotalPage) ? totalItem - StartIndex : itemsPerPage;
            return this;
        }
    }
}