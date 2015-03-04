using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCacheTest.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration=5,VaryByParam="p")]
        public ActionResult PageCache() {

            ViewBag.DateNow = DateTime.Now.ToString();
            return View();
        
        }

    }
}
