using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CaregiverLiteWCF
{
    public class Settings
    {
        public static string CONTENTDATABASE()
        {
            return "CONTENT";
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection Con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString);
            return Con;
        }

        public static string CareGiverSuperAdminDatabase()
        {
            return ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
        }

        public static string CaregiverLiteDatabase()
        {
            return ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
        }
    }

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