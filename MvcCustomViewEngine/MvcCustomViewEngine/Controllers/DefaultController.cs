using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCustomViewEngine.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 5000)]
        public ActionResult PageCache()
        {
            return View(DateTime.Now.ToString());
        }
    }
}
