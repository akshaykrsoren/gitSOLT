using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class RewardPoint
    {
        private int m_NurseId;
        private string m_Name;
        private string m_Photo;
        private string m_CompletedReqCount;
        private string m_TotalRewardPoint;
        private string m_Comment;
        //Model
        private string m_SchedulerName;
        private string m_AppointmentDate;
        private string m_Time;
        private string m_Point;
        private string m_Email;
        private string m_Office;
        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }
        [DataMember]
        public string Office
        {
            get { return m_Office; }
            set { m_Office = value; }
        }

        [DataMember]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [DataMember]
        public string Photo
        {
            get { return m_Photo; }
            set { m_Photo = value; }
        }


        [DataMember]
        public string CompletedReqCount
        {
            get { return m_CompletedReqCount; }
            set { m_CompletedReqCount = value; }
        }


        [DataMember]
        public string TotalRewardPoint
        {
            get { return m_TotalRewardPoint; }
            set { m_TotalRewardPoint = value; }
        }
        [DataMember]
        public string Comment
        {
            get { return m_Comment; }
            set { m_Comment = value; }
        }
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
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }


    }
    [DataContract]
    public class RewardPointsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<RewardPoint> objRewardPointList { get; set; }

        [DataMember]
        public RewardPoint objRewardPoint { get; set; }
    }
}