using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyCompanySellInfo.Models;
using System.Globalization;

namespace MyCompanySellInfo.Controllers
{
    public class StockController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: /Stock/
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Index(string searchItem, string searchString)
        {
            var searchItems = new List<string>() { "Manager", "Date", "Goods Name" };

            if (!searchItems.Contains(searchItem))
                // Generate error or something
                ;

            ViewBag.searchItem = new SelectList(searchItems, searchItem);
            ViewBag.searchString = searchString;
            var stocks = db.Stocks.Include(s => s.Good).Include(s => s.Manager);
            if (!String.IsNullOrEmpty(searchString) &&
                !String.IsNullOrEmpty(searchItem))
            {
                switch (searchItem)
                {
                    case "Manager":
                        stocks = stocks.Where(s =>
                            s.Manager.FirstName.Contains(searchString) ||
                            s.Manager.LastName.Contains(searchString));
                        break;
                    case "Date":
                        DateTime date;
                        bool success = DateTime.TryParseExact(searchString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                        if (success)
                        {
                            stocks = stocks.Where(s => s.Date.Equals(date));
                        }
                        break;
                    case "Goods Name":
                        stocks = stocks.Where(s => s.Good.Name.Contains(searchString));
                        break;
                    default:
                        break;
                }
            }

            return View(stocks.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChartData()
        {
            var groupedStocks = db.Stocks
                .GroupBy(stock => stock.Good.Name)
                .Select(Oxy => new
                {
                    GName = Oxy.Key,
                    GCount = Oxy.Count()
                });

            int i = 0;
            object[][] stockResult = new object[groupedStocks.Count() + 1][];
            stockResult[i++] = new object[] { "Goods", "Sold goods" };

            foreach (var item in groupedStocks)
            {
                stockResult[i++] = new object[] { item.GName, item.GCount };
            }
            object[] d = new object[2];
            d[0] = stockResult;

            var groupedManagers = db.Stocks
                .GroupBy(stock => stock.Manager)
                .Select(Oxy => new
                {
                    MName = Oxy.Key.LastName,
                    SumCost = Oxy.Sum(y => y.Cost ?? 0)
                });

            i = 0;
            object[][] managerResult = new object[groupedManagers.Count() + 1][];
            managerResult[i++] = new object[] { "Managers", "Managers activity statistic" };

            foreach (var item in groupedManagers)
            {
                managerResult[i++] = new object[] { item.MName, item.SumCost };
            }
            d[1] = managerResult;

            var jsonResult = Json(d, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }


        // GET: /Stock/Details/5
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: /Stock/Create
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Create()
        {
            ViewBag.GoodsID = new SelectList(db.Goods, "Id", "Name");
            ViewBag.ManagerID = new SelectList(db.Managers, "Id", "FirstName");
            return View();
        }

        // POST: /Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Create([Bind(Include = "Id,GoodsID,Client,ManagerID,Date,Cost")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GoodsID = new SelectList(db.Goods, "Id", "Name", stock.GoodsID);
            ViewBag.ManagerID = new SelectList(db.Managers, "Id", "FirstName", stock.ManagerID);
            return View(stock);
        }

        // GET: /Stock/Edit/5
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.GoodsID = new SelectList(db.Goods, "Id", "Name", stock.GoodsID);
            ViewBag.ManagerID = new SelectList(db.Managers, "Id", "FirstName", stock.ManagerID);
            return View(stock);
        }

        // POST: /Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Edit([Bind(Include = "Id,GoodsID,Client,ManagerID,Date,Cost")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GoodsID = new SelectList(db.Goods, "Id", "Name", stock.GoodsID);
            ViewBag.ManagerID = new SelectList(db.Managers, "Id", "FirstName", stock.ManagerID);
            return View(stock);
        }

        // GET: /Stock/Delete/5
        [Authorize(Roles = "canDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: /Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
