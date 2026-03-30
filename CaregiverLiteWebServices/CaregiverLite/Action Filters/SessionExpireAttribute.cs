using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
            
namespace CaregiverLite.Action_Filters
{           
    public class SessionExpireAttribute : ActionFilterAttribute
    {       
            
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {   

            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if (HttpContext.Current.Session["UserId"] == null)
            {
              //  HttpContext.Current.Response.Redirect("~/Account/SessionTimeout");
                filterContext.Result = new RedirectResult("~/Account/SessionTimeout");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}