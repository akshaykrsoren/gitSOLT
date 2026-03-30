using CaregiverLite.Action_Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Controllers
{
    [SessionExpire]
    public class UnAuthorisedController : Controller
    {
        // GET: UnAuthorised
        public ActionResult Index()
        {
            return View();
        }
    }
}