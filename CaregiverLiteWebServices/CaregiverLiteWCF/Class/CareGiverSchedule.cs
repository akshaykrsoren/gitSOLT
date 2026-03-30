using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class CareGiverSchedule
    {
        private int m_ScheduleId;
        private int m_TimeSlotId;
        private int m_NurseId;
        private string m_UserId;
        private string m_Date;
        private bool m_IsAppointed;
        private string m_PartialFromTime;
        private string m_PartialToTime;
        private bool m_IsPartial;
        private string m_FromTime;
        private string m_ToTime;
        private string m_SlotFromTime;
        private string m_SlotToTime;
        private string m_PatientName;
        private string m_DisplayDate;
        private int m_ServiceId;
        private bool m_IsAvailable;
        


        [DataMember]
        public int ScheduleId
        {
            get { return m_ScheduleId; }
            set { m_ScheduleId = value; }
        }

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
        public string PatientName
        {
            get { return m_PatientName; }
            set { m_PatientName = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        [DataMember]
        public bool IsAppointed
        {
            get { return m_IsAppointed; }
            set { m_IsAppointed = value; }
        }


        [DataMember]
        public bool IsAvailable
        {
            get { return m_IsAvailable; }
            set { m_IsAvailable = value; }
        }

        [DataMember]
        public string PartialFromTime
        {
            get { return m_PartialFromTime; }
            set { m_PartialFromTime = value; }
        }
        [DataMember]
        public string PartialToTime
        {
            get { return m_PartialToTime; }
            set { m_PartialToTime = value; }
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
        [DataMember]
        public string DisplayDate
        {
            get { return m_DisplayDate; }
            set { m_DisplayDate = value; }
        }

        [DataMember]
        public string SlotFromTime
        {
            get { return m_SlotFromTime; }
            set { m_SlotFromTime = value; }
        }
        [DataMember]
        public string SlotToTime
        {
            get { return m_SlotToTime; }
            set { m_SlotToTime = value; }
        }

        [DataMember]
        public bool IsPartial
        {
            get { return m_IsPartial; }
            set { m_IsPartial = value; }
        }

        [DataMember]
        public int ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }
    }

    [DataContract]
    public class UnavailabilityRequest
    {
        private int m_NurseId;
        private string m_PatientName;        
        private string m_SlotFromTime;
        private string m_SlotToTime;
        private string m_Date;
        private string m_FromTime;
        private string m_ToTime;
        private string m_DisplayDate;
        private string m_ServiceId; 
        private string m_UserId;
        private string m_CancelComment;
        private int m_TimeSlotId;

        [DataMember]
        public string FromTime
        {
            get { return m_FromTime; }
            set { m_FromTime = value; }
        }
        [DataMember]
        public string CancelComment
        {
            get { return m_CancelComment; }
            set { m_CancelComment = value; }
        }
        [DataMember]
        public string ToTime
        {
            get { return m_ToTime; }
            set { m_ToTime = value; }
        }
        [DataMember]
        public string DisplayDate
        {
            get { return m_DisplayDate; }
            set { m_DisplayDate = value; }
        }

        [DataMember]
        public string SlotFromTime
        {
            get { return m_SlotFromTime; }
            set { m_SlotFromTime = value; }
        }
        [DataMember]
        public string SlotToTime
        {
            get { return m_SlotToTime; }
            set { m_SlotToTime = value; }
        }

        [DataMember]
        public string ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string PatientName
        {
            get { return m_PatientName; }
            set { m_PatientName = value; }
        }

        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        [DataMember]
        public int TimeSlotId
        {
            get { return m_TimeSlotId; }
            set { m_TimeSlotId = value; }
        }
        
    }


}