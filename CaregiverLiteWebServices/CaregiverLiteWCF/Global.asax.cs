using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CaregiverLiteWCF
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();         
        }



        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = new Exception();
            ex = Server.GetLastError().GetBaseException();

            System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("~/errorLOG.txt"), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
            System.IO.StreamWriter s = new System.IO.StreamWriter(fs);
            s.BaseStream.Seek(0, System.IO.SeekOrigin.End);
            s.WriteLine("ERROR DATE: " + System.DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture) + " \nERROR MESSAGE: " + ex.Message + "\nSOURCE: " + ex.Source + "\nFORM NAME: " + System.Web.HttpContext.Current.Request.Url.ToString() + "\nQUERYSTRING: " + Request.QueryString.ToString() + "\nTARGETSITE: " + ex.TargetSite.ToString() + "\nSTACKTRACE: " + ex.StackTrace + System.Diagnostics.EventLogEntryType.Error);
            s.WriteLine("-------------------------------------------------------------------------------------------------------------");
            s.Close();
        }



    }
}
