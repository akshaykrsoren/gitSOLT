using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CaregiverLite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );



            //routes.MapRoute(
            //    name: "OpenMapFromExcel",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "AttendanceManagement", action = "OpenMapFromExcel", id = UrlParameter.Optional }
            //);

           
            routes.MapRoute(
                name: "SessionTimeOut",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "SessionTimeout", id = UrlParameter.Optional }
            );



        }
    }
}
