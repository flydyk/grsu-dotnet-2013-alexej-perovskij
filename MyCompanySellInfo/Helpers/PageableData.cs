using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCompanySellInfo.Helpers
{
    public class PageableData<T> where T : class
    {
        protected static int ItemPerPageDefault = 50;

        public IEnumerable<T> List { get; set; }

        public IQueryable<T> QueryableSet { get; set; }

        public int PageNo { get; set; }

        public int CountPage { get; set; }

        public int ItemPerPage { get; set; }

        public T DefaultItem
        {
            get
            { return default(T); }
        }

        public PageableData(IQueryable<T> queryableSet, int itemPerPage = 0)
        {
            if (itemPerPage == 0)
            {
                itemPerPage = ItemPerPageDefault;
            }
            ItemPerPage = itemPerPage;
            QueryableSet = queryableSet;

            var count = queryableSet.Count();

            CountPage = (int)decimal.
                Remainder(count, itemPerPage) == 0 ?
                count / itemPerPage :
                count / itemPerPage + 1;
        }


        public void SelectPage(int page)
        {
            PageNo = page;
            List = QueryableSet.Skip((PageNo - 1) * ItemPerPage).Take(ItemPerPage);
        }
    }
}