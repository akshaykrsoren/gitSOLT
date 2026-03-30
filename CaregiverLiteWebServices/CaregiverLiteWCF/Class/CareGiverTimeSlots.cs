using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class CareGiverTimeSlots
    {
        private int m_TimeSlotId;
        private int m_NurseId;
        private string m_UserId;
        private int m_Week;
        private int m_Year;
        private string m_FromTime;
        private string m_ToTime;

        [DataMember]
        public int TimeSlotId
        {
            get { return m_TimeSlotId; }
            set { m_TimeSlotId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public int Week
        {
            get { return m_Week; }
            set { m_Week = value; }
        }

        [DataMember]
        public int Year
        {
            get { return m_Year; }
            set { m_Year = value; }
        }

        [DataMember]
        public string FromTime
        {
            get { return m_FromTime; }
            set { m_FromTime = value; }
        }

        [DataMember]
        public string ToTime
        {
            get { return m_ToTime; }
            set { m_ToTime = value; }
        }
    }
    [DataContract]
    public class CareGiverMultipleTimeSlots
    {
        private string m_TimeSlots;
        private string m_Schedules;
        private int m_NurseId;
        private string m_Week;
        private int m_Year;
        private string m_Repeat;
        private string m_UserId;

        [DataMember]
        public string TimeSlots
        {
            get { return m_TimeSlots; }
            set { m_TimeSlots = value; }
        }

        [DataMember]
        public string Schedules
        {
            get { return m_Schedules; }
            set { m_Schedules = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string Week
        {
            get { return m_Week; }
            set { m_Week = value; }
        }

        [DataMember]
        public int Year
        {
            get { return m_Year; }
            set { m_Year = value; }
        }

        [DataMember]
        public string Repeat
        {
            get { return m_Repeat; }
            set { m_Repeat = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }
    }
}