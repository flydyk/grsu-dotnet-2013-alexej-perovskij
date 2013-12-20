using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCompanySellInfo.Helpers
{
    public enum SearchFilters
    {
        None, Manager, Date, Goods, Client
    }
    public enum OrderFilters
    {
        None, Manager, Date, Goods, Client,
        ManagerDesc, DateDesc, GoodsDesc, ClientDesc
    }
}