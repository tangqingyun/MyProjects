using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCustomViewEngine.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            string sss = Request.ServerVariables["Server_Name"];
            
            return View();
        }


    }
}
