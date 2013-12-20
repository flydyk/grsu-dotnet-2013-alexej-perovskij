using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MyCompanySellInfo.Helpers
{
    public static class DataProcessor
    {
        private static string _DATE_FORMAT = "dd-MM-yyyy";
        public static string DATE_FORMAT
        {
            get
            {
                return _DATE_FORMAT;
            }
            set { _DATE_FORMAT = value; }
        }
        public static void OrderStocks(ref IQueryable<MyCompanySellInfo.Models.Stock> stocks, OrderFilters order)
        {
            switch (order)
            {
                case OrderFilters.None:
                case OrderFilters.Date:
                    stocks = stocks.OrderBy(x => x.Date);
                    break;
                case OrderFilters.DateDesc:
                    stocks = stocks.OrderByDescending(x => x.Date);
                    break;
                case OrderFilters.Manager:
                    stocks = stocks.OrderBy(x => x.Manager.LastName);
                    break;
                case OrderFilters.ManagerDesc:
                    stocks = stocks.OrderByDescending(x => x.Manager.LastName);
                    break;
                case OrderFilters.Goods:
                    stocks = stocks.OrderBy(x => x.Good.Name);
                    break;
                case OrderFilters.GoodsDesc:
                    stocks = stocks.OrderByDescending(x => x.Good.Name);
                    break;
                case OrderFilters.Client:
                    stocks = stocks.OrderBy(x => x.Client);
                    break;
                case OrderFilters.ClientDesc:
                    stocks = stocks.OrderByDescending(x => x.Client);
                    break;
                default:
                    break;
            }
        }

        public static void FilterStocks(ref IQueryable<MyCompanySellInfo.Models.Stock> stocks, SearchFilters searchItem, string searchString)
        {
            switch (searchItem)
            {
                case SearchFilters.Client:
                    stocks = stocks.Where(s => s.Client.Contains(searchString));
                    break;
                case SearchFilters.Manager:
                    stocks = stocks.Where(s =>
                        s.Manager.FirstName.Contains(searchString) ||
                        s.Manager.LastName.Contains(searchString));
                    break;
                case SearchFilters.Date:
                    DateTime date;
                    bool success = DateTime.TryParseExact(searchString, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    if (success)
                    {
                        stocks = stocks.Where(s => s.Date.Equals(date));
                    }
                    break;
                case SearchFilters.Goods:
                    stocks = stocks.Where(s => s.Good.Name.Contains(searchString));
                    break;
                default:
                    break;
            }
        }
    }
}