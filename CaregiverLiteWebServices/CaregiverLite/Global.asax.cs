using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CaregiverLite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

       

  
        protected void Application_BeginRequest()
        {

            //var routeCollection1 = new RouteCollection();
           
            //var route = new Route("{Account}/{SessionTimeout}", new MvcRouteHandler());

            //routeCollection1.Add("route1", route);

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }


        protected void Application_End()
        {
            SqlConnection.ClearAllPools();
        }

            //void Session_Start(object sender, EventArgs e)
            //{
            //    // Code that runs when a new user session is started

            //    Application.Lock();
            //    Application["UsersOnline"] = (int)Application["UsersOnline"] + 1;
            //    Application.UnLock();
            //}
            //void Session_End(object sender, EventArgs e)
            //{
            //    // Code that runs when an existing user session ends.

            //    Application.Lock();
            //    Application["UsersOnline"] = (int)Application["UsersOnline"] - 1;
            //    Application.UnLock();
            //}





        }
}
