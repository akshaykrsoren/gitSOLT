using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaregiverLiteWCF.Class
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