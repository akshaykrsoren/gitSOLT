using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CaregiverLite.Models
{
    public class DataTableProperty
    {
        public int PageNo { get; set; }

        public int RecordPerPage { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }

        public string Filter { get; set; }
    }




}