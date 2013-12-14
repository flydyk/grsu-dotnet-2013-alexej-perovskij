using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyCompanySellInfo.Models;

namespace MyCompanySellInfo.Controllers
{
    public class ManagerController : Controller
    {
        private ProductionEntities db = new ProductionEntities();

        // GET: /Manager/
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Index()
        {
            return View(db.Managers.ToList());
        }

        // GET: /Manager/Details/5
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChartData(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }

            var groupedStocks = manager.Stocks
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
            object[] d = new object[1];
            d[0] = stockResult;

            return Json(d,JsonRequestBehavior.AllowGet);
        }

        // GET: /Manager/Create
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Manager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Create([Bind(Include="Id,FirstName,LastName")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Managers.Add(manager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: /Manager/Edit/5
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: /Manager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canEdit,canDelete")]
        public ActionResult Edit([Bind(Include="Id,FirstName,LastName")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: /Manager/Delete/5
        [Authorize(Roles = "canDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = db.Managers.Find(id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: /Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "canDelete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = db.Managers.Find(id);
            db.Managers.Remove(manager);
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
