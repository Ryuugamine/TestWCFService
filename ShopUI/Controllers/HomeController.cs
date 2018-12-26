using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopUI.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string UserID)
        //{
        //    ViewBag.Resp = "1312";
        //    return RedirectToAction("Index");
        //}
    }
}