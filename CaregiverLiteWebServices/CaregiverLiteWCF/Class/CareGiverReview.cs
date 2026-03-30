using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class CareGiverReview
    {
        int m_PatientId;
        int m_NurseId;
        int m_ServiceId;
        int m_AppointmentId;
        double m_Rating;
        string m_ReviewTitle;
        string m_ReviewContent;
        string m_InsertUserId;
        bool m_IsApproved;

        [DataMember]
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public int ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; }
        }

        [DataMember]
        public int AppointmentId
        {
            get { return m_AppointmentId; }
            set { m_AppointmentId = value; }
        }


        [DataMember]
        public double Rating
        {
            get { return m_Rating; }
            set { m_Rating = value; }
        }

        [DataMember]
        public string ReviewTitle
        {
            get { return m_ReviewTitle; }
            set { m_ReviewTitle = value; }
        }
        [DataMember]
        public string ReviewContent
        {
            get { return m_ReviewContent; }
            set { m_ReviewContent = value; }
        }

        [DataMember]
        public string InsertUserId
        {
            get { return m_InsertUserId; }
            set { m_InsertUserId = value; }
        }

        [DataMember]
        public bool IsApproved
        {
            get { return m_IsApproved; }
            set { m_IsApproved = value; }
        }
    }

    [DataContract]
    public class CareGiverReviewList
    {

        int m_TotalStar;
        double m_AvgStarRating;
        string m_Result;
        string m_TotalStar1;
        string m_TotalStar2;
        string m_TotalStar3;
        string m_TotalStar4;
        string m_TotalStar5;

        [DataMember]
        public int TotalStar
        {
            get { return m_TotalStar; }
            set { m_TotalStar = value; }
        }

        [DataMember]
        public string Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

        [DataMember]
        public double AverageStarRating
        {
            get { return m_AvgStarRating; }
            set { m_AvgStarRating = value; }
        }

        [DataMember]
        public string StarOne
        {
            get { return m_TotalStar1; }
            set { m_TotalStar1 = value; }
        }

        [DataMember]
        public string StarTwo
        {
            get { return m_TotalStar2; }
            set { m_TotalStar2 = value; }
        }
        [DataMember]
        public string StarThree
        {
            get { return m_TotalStar3; }
            set { m_TotalStar3 = value; }
        }
        [DataMember]
        public string StarFour
        {
            get { return m_TotalStar4; }
            set { m_TotalStar4 = value; }
        }
        [DataMember]
        public string StarFive
        {
            get { return m_TotalStar5; }
            set { m_TotalStar5 = value; }
        }

        [DataMember]
        public List<DisplayCareGiverReview> ListCareGiverReview { get; set; }
    }

    [DataContract]
    public class DisplayCareGiverReview
    {
        int m_PatientId;
        int m_NurseId;
        string m_PatientName;
        string m_CareGiverName;
        string m_ReviewTitle;
        string m_ReviewContent;
        double m_StarRating;
        string m_ReviewDate;

        [DataMember]
        public int PatientId
        {
            get { return m_PatientId; }
            set { m_PatientId = value; }
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
        public string CareGiverName
        {
            get { return m_CareGiverName; }
            set { m_CareGiverName = value; }
        }


        [DataMember]
        public string ReviewTitle
        {
            get { return m_ReviewTitle; }
            set { m_ReviewTitle = value; }
        }
        [DataMember]
        public string ReviewContent
        {
            get { return m_ReviewContent; }
            set { m_ReviewContent = value; }
        }

        [DataMember]
        public string ReviewDate
        {
            get { return m_ReviewDate; }
            set { m_ReviewDate = value; }
        }

        [DataMember]
        public double StarRating
        {
            get { return m_StarRating; }
            set { m_StarRating = value; }
        }
    }

    [DataContract]
    public class CareGiverUserNRate
    {
        int m_NoOfUser;
        double m_AvgStarRating;
        string m_Result;
        string m_TotalStar1;
        string m_TotalStar2;
        string m_TotalStar3;
        string m_TotalStar4;
        string m_TotalStar5;

        [DataMember]
        public int NoOfUser
        {
            get { return m_NoOfUser; }
            set { m_NoOfUser = value; }
        }

        [DataMember]
        public double AverageReview
        {
            get { return m_AvgStarRating; }
            set { m_AvgStarRating = value; }
        }


        [DataMember]
        public string StarOne
        {
            get { return m_TotalStar1; }
            set { m_TotalStar1 = value; }
        }
        [DataMember]
        public string StarTwo
        {
            get { return m_TotalStar2; }
            set { m_TotalStar2 = value; }
        }
        [DataMember]
        public string StarThree
        {
            get { return m_TotalStar3; }
            set { m_TotalStar3 = value; }
        }
        [DataMember]
        public string StarFour
        {
            get { return m_TotalStar4; }
            set { m_TotalStar4 = value; }
        }
        [DataMember]
        public string StarFive
        {
            get { return m_TotalStar5; }
            set { m_TotalStar5 = value; }
        }
        [DataMember]
        public string Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

    }

}