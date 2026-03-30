using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CaregiverLiteWCF
{
    [DataContract]
    public class StripeAccessToken
    {
        string m_AccessToken;
        string m_Livemode;
        string m_RefreshToken;
        string m_TokenType;
        string m_StripePublishableKey;
        string m_StripeUserID;
        string m_Scope;
        int m_NurseId;


        [DataMember]
        public string AccessToken { get { return m_AccessToken; } set { m_AccessToken = value; } }

        [DataMember]
        public string Livemode { get { return m_Livemode; } set { m_Livemode = value; } }

        [DataMember]
        public string RefreshToken { get { return m_RefreshToken; } set { m_RefreshToken = value; } }

        [DataMember]
        public string TokenType { get { return m_TokenType; } set { m_TokenType = value; } }

        [DataMember]
        public string StripePublishableKey { get { return m_StripePublishableKey; } set { m_StripePublishableKey = value; } }

        [DataMember]
        public string StripeUserID { get { return m_StripeUserID; } set { m_StripeUserID = value; } }

        [DataMember]
        public string Scope { get { return m_Scope; } set { m_Scope = value; } }

        [DataMember]
        public int NurseId { get { return m_NurseId; } set { m_NurseId = value; } }

        [DataMember]
        public string UserId { get; set; }
    }
}