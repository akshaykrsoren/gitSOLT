using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Holiday
    {

        int m_ID;
        string m_Name;
        string m_Day;
        string m_Month;
        string m_InsertDateTime;
        string m_UpdateDateTime;

        [DataMember]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        [DataMember]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        [DataMember]
        public string Day
        {
            get { return m_Day; }
            set { m_Day = value; }
        }
        [DataMember]
        public string Month
        {
            get { return m_Month; }
            set { m_Month = value; }
        }
        [DataMember]
        public string InsertDateTime
        {
            get { return m_InsertDateTime; }
            set { m_InsertDateTime = value; }
        }
        [DataMember]
        public string UpdateDateTime
        {
            get { return m_UpdateDateTime; }
            set { m_UpdateDateTime = value; }
        }


    }

    [DataContract]
    public class HolidaysList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Holiday> HolidayList { get; set; }
    }
}