using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class UnavailabilityRequests
    {
        private int m_UnavailabilityRequestId;
        private int m_NurseId;
        private string m_UserId;
        private string m_NurseName;
        private int m_ServiceId;
        private string m_ServiceName;
        private string m_Date;
        private string m_TimeSlot;
        private bool m_IsApproved;
        private bool m_IsReject;
        private int m_NoOfAppointment;
        private string m_RejectComment;

        [DataMember]
        public int UnavailabilityRequestId
        {
            get { return m_UnavailabilityRequestId; }
            set { m_UnavailabilityRequestId = value; }
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
        public string NurseName
        {
            get { return m_NurseName; }
            set { m_NurseName = value; }
        }

        [DataMember]
        public int ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        [DataMember]
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; }
        }

        [DataMember]
        public string Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        [DataMember]
        public string TimeSlot
        {
            get { return m_TimeSlot; }
            set { m_TimeSlot = value; }
        }

        [DataMember]
        public bool IsApproved
        {
            get { return m_IsApproved; }
            set { m_IsApproved = value; }
        }

        [DataMember]
        public bool IsReject
        {
            get { return m_IsReject; }
            set { m_IsReject = value; }
        }

        [DataMember]
        public int NoOfAppointment
        {
            get { return m_NoOfAppointment; }
            set { m_NoOfAppointment = value; }
        }

        [DataMember]
        public string RejectComment
        {
            get { return m_RejectComment; }
            set { m_RejectComment = value; }
        }
    }

    [DataContract]
    public class UnavailabilityRequestsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<UnavailabilityRequests> UnavailabilityRequestList { get; set; }
    }
}