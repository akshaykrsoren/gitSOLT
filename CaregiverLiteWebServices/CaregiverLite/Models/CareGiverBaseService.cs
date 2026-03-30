using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CaregiverLite.Models
{
    public class CaregiverLiteBaseService
    {
        public string hostUrl { get; set; }
        //Set the Service Name
        public string rootSuffix { get; set; }
        //Call the Service this Object
        public HttpClient client { get; set; }

        public CancellationTokenModelBinder cancellationToken { get; set; }

        public CaregiverLiteBaseService()
        {

            hostUrl = System.Configuration.ConfigurationManager.AppSettings["WebServiceUrl"].ToString();
          
            // initialize the HttpClient object which is the basis for all of our data requests
            client = new HttpClient();

            // the BaseAddress can only be the root url, 
            // so anything after 'http://domain:port/' goes in the rootSuffix property
            client.BaseAddress = new Uri(hostUrl);

            // this header is required to work with our service
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        }
    }

    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

    }
}

namespace CareGiver
{
    public class StripePayment
    {
        public static decimal GetStripeFee(decimal OriginalAmount, decimal StripePercentage, decimal minimamAmount)
        {
            decimal StripeFee = 0;
            decimal TotalAmount = 0;

            TotalAmount = Math.Round((100 * (OriginalAmount + minimamAmount)) / (100 - StripePercentage), 2);

            StripeFee = TotalAmount - OriginalAmount;

            return StripeFee;
        }

        public static decimal GetStripeTotalAmount(decimal OriginalAmount, decimal StripePercentage)
        {
            decimal TotalAmount = 0;

            TotalAmount = Math.Round(((OriginalAmount * StripePercentage) / 100) + OriginalAmount, 2);

            return TotalAmount;
        }

        public static decimal GetOriganlAmount(decimal Amount, decimal StripePercentage)
        {
            decimal OriginalAmount = 0;

            OriginalAmount = Math.Round((Amount * 100) / (StripePercentage + 100), 2);

            return OriginalAmount;
        }
    }
}