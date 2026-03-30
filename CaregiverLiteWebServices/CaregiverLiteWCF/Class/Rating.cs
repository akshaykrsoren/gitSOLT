using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class Rating
    {

        private string m_SchedulerName;
        private string m_AppointmentDate;
        private string m_Time;
        private string m_Point;
        private string m_Rating;


        [DataMember]
        public string SchedulerName
        {
            get { return m_SchedulerName; }
            set { m_SchedulerName = value; }
        }
        [DataMember]
        public string AppointmentDate
        {
            get { return m_AppointmentDate; }
            set { m_AppointmentDate = value; }
        }
        [DataMember]
        public string Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }
        [DataMember]
        public string Point
        {
            get { return m_Point; }
            set { m_Point = value; }
        }
        [DataMember]
        public string Rate
        {
            get { return m_Rating; }
            set { m_Rating = value; }
        }


    }
    [DataContract]
    public class RatingsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Rating> objRatingsList { get; set; }

     
    }
}