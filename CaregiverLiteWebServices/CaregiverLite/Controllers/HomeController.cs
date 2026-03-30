using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string city = "ghatshila";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            string name = "ashaish";
            string age = "323223";
            string sss = "askhay";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            string address = "USA";

            string city = "ghatshila";
            string address = "delhi";


            string age = "323223";
            return View();
        }
    }
}