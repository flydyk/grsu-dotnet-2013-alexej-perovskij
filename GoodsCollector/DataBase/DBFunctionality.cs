using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodsCollectorService.DataBase
{
    public class DBFunctionality : IDisposable
    {
        ProductionEntities db = new ProductionEntities();
        public void AddSaveItems<T>(IEnumerable<T> items) where T : class
        {
            try
            {
                db.Set<T>().AddRange(items);
                db.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public T Find<T>(int id) where T : class
        {
            return db.Set<T>().Find(id);
        }
        public IEnumerable<T> FindAll<T>(Func<T, bool> p) where T : class
        {
            try
            {
                return db.Set<T>().Where(p);
            }
            catch (Exception e) { throw e; }
        }
        /*
        public static IEnumerable<Manager> FindAllManagers(Func<Manager, bool> p)
        {
            IEnumerable<Manager> managers;

            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {
                    managers = db.Managers.Where(p).ToList();
                }
                return managers;
            }
            catch (Exception e) { throw e; }
        }
        public static IEnumerable<Stock> FindAllStocks(Func<Stock, bool> p)
        {
            IEnumerable<Stock> managers;

            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {
                    managers = db.Stocks.Where(p).ToList();
                }
                return managers;
            }
            catch (Exception e) { throw e; }
        }
        public static IEnumerable<Good> FindAllGoods(Func<Good, bool> p)
        {
            IEnumerable<Good> managers;

            try
            {
                using (ProductionEntities db = new ProductionEntities())
                {
                    managers = db.Goods.Where(p).ToList();
                }
                return managers;
            }
            catch (Exception e) { throw e; }
        }

        */


        public void Dispose()
        {
            db.Dispose();
        }
    }
}
