using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCompanySellInfo.Controllers
{
    public class DbProductionController : Controller
    {
        //
        // GET: /DbProduction/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
	}
}