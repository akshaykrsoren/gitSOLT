using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CaregiverLite.Models
{
    public class QuickBlox
    {
    }
    public class QuickBloxSession
    {
        private int m_application_id;
        private string m_auth_key;
        private string m_timestamp;
        private string m_nonce;
        private string m_signature;
        private Userdata m_user;

        [DataMember]
        public int application_id
        {
            get { return m_application_id; }
            set { m_application_id = value; }
        }
        [DataMember]
        public string auth_key
        {
            get { return m_auth_key; }
            set { m_auth_key = value; }
        }

        [DataMember]
        public string timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
        }
        [DataMember]
        public string nonce
        {
            get { return m_nonce; }
            set { m_nonce = value; }
        }
        [DataMember]
        public string signature
        {
            get { return m_signature; }
            set { m_signature = value; }
        }
        [DataMember]
        public Userdata user
        {
            get { return m_user; }
            set { m_user = value; }
        }
    }

    public class Userdata
    {
        private string m_login;
        private string m_password;

        [DataMember]
        public string login
        {
            get { return m_login; }
            set { m_login = value; }
        }
        [DataMember]
        public string password
        {
            get { return m_password; }
            set { m_password = value; }
        }
    }

    public class QuickbloxReponseItem
    {
        public string _id { get; set; }
        public string created_at { get; set; }
        public string last_message { get; set; }
        public int? last_message_date_sent { get; set; }
        public int? last_message_user_id { get; set; }
        public string name { get; set; }
        public List<int> occupants_ids { get; set; }
        public string photo { get; set; }
        public int type { get; set; }
        public string updated_at { get; set; }
        public int user_id { get; set; }
        public string xmpp_room_jid { get; set; }
        public int unread_messages_count { get; set; }
    }

    public class QuickbloxReponse
    {
        public int total_entries { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
        public List<QuickbloxReponseItem> items { get; set; }
    }

    public class PullAll
    {
        public List<int> occupants_ids { get; set; }
    }

    public class AddDialog
    {
        public string name { get; set; }
        public PullAll push_all { get; set; }
    }

    public class occupants_id
    {
        public int occupants_ids { get; set; }
    }

    public class UpdateDialog
    {
        public string name { get; set; }
        public PullAll pull_all { get; set; }
    }


    public class UserdataQuickBloxReq
    {

        private string m_login;
        private string m_password;
        private string m_email;
        [DataMember]
        public string login
        {
            get { return m_login; }
            set { m_login = value; }
        }
        [DataMember]
        public string password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        [DataMember]
        public string email
        {
            get { return m_email; }
            set { m_email = value; }
        }

    }
    public class userReq
    {
        public UserdataQuickBloxReq user { get; set; }
    }
}