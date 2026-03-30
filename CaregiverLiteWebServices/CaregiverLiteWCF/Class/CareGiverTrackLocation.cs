using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class CareGiverTrackLocation
    {
        private int m_PatientRequestId;
        private int m_NurseId;
        private string m_LocationDateTime;
        private string m_LocationLatitude;
        private string m_LocationLongitude;
        private string m_Status;
        private string m_TotalMiles;


        [DataMember]
        public int PatientRequestId
        {
            get { return m_PatientRequestId; }
            set { m_PatientRequestId = value; }
        }

        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
        }

        [DataMember]
        public string LocationDateTime
        {
            get { return m_LocationDateTime; }
            set { m_LocationDateTime = value; }
        }
        [DataMember]
        public string LocationLatitude
        {
            get { return m_LocationLatitude; }
            set { m_LocationLatitude = value; }
        }

        [DataMember]
        public string LocationLongitude
        {
            get { return m_LocationLongitude; }
            set { m_LocationLongitude = value; }
        }

        [DataMember]
        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        [DataMember]
        public string TotalMiles
        {
            get { return m_TotalMiles; }
            set { m_TotalMiles = value; }
        }
        [DataMember]
        public string CheckInLatLong { get;  set; }
        [DataMember]
        public string CheckoutLatLong { get; set; }
        
    }
}