using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace CaregiverLiteWCF
{
    [DataContract]
    public class Chatting
    {

        private string m_CareGiverName;
        private int m_NurseId;
        private string m_AppointmentDate;
        private string m_Time;
        private string m_Point;
        private string m_Rating;
        private string m_FromEmail;
        private string m_ToEmail;
        private string m_UserId;
        private string m_UserName;
        private string m_QuickBloxId;
        private string m_QuickBloxDialogId;
        private string m_CaregiverProfileImage;
        private int m_ChattingGroupId;
        private string m_GroupName;
        private string m_GroupSubject;
        private string m_DialogId;
        private int m_NurseCoordinatorId;
        private int m_NurseCoordinatorPermission;

        private int m_SchedulerId;
        private string m_SchedulerName;
        private string m_Role;

        private string m_SchedulerEmailId;
        private string m_SchedulerUserId;
        private int m_OfficeId;
        private string m_OfficeName;

        private string m_Result;

        private string m_GroupAdminUserId;
        private bool m_IsGroupAdmin;

        private int m_GroupTypeId;
        private bool m_IsOfficeGroup { get; set; }
        //            ChattingGroupId
        //GroupName
        //DialogId
            
        [DataMember]
        public int unreadcounttest { get; set; }


        [DataMember]
        public string CareGiverName
        {
            get { return m_CareGiverName; }
            set { m_CareGiverName = value; }
        }
        [DataMember]
        public int NurseId
        {
            get { return m_NurseId; }
            set { m_NurseId = value; }
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
        [DataMember]
        public string FromEmail
        {
            get { return m_FromEmail; }
            set { m_FromEmail = value; }
        }
        [DataMember]
        public string ToEmail
        {
            get { return m_ToEmail; }
            set { m_ToEmail = value; }
        }
        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        [DataMember]
        public string Role
        {
            get { return m_Role; }
            set { m_Role = value; }
        }
        [DataMember]
        public string QuickBloxId
        {
            get { return m_QuickBloxId; }
            set { m_QuickBloxId = value; }
        }
        [DataMember]
        public string QuickBloxDialogId
        {
            get { return m_QuickBloxDialogId; }
            set { m_QuickBloxDialogId = value; }
        }
        [DataMember]
        public string CaregiverProfileImage
        {
            get { return m_CaregiverProfileImage; }
            set { m_CaregiverProfileImage = value; }
        }
        //-------
        [DataMember]
        public int ChattingGroupId
        {
            get { return m_ChattingGroupId; }
            set { m_ChattingGroupId = value; }
        }
        [DataMember]
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        [DataMember]
        public string GroupSubject
        {
            get { return m_GroupSubject; }
            set { m_GroupSubject = value; }
        }


        [DataMember]
        public string DialogId
        {
            get { return m_DialogId; }
            set { m_DialogId = value; }
        }

        [DataMember]
        public int NurseCoordinatorId
        {
            get { return m_NurseCoordinatorId; }
            set { m_NurseCoordinatorId = value; }
        }
        [DataMember]
        public int NurseCoordinatorPermission
        {
            get { return m_NurseCoordinatorPermission; }
            set { m_NurseCoordinatorPermission = value; }
        }
        [DataMember]
        public int SchedulerId
        {
            get { return m_SchedulerId; }
            set { m_SchedulerId = value; }
        }
        [DataMember]
        public string SchedulerName
        {
            get { return m_SchedulerName; }
            set { m_SchedulerName = value; }
        }
        [DataMember]
        public string SchedulerEmailId
        {
            get { return m_SchedulerEmailId; }
            set { m_SchedulerEmailId = value; }
        }
        [DataMember]
        public string SchedulerUserId
        {
            get { return m_SchedulerUserId; }
            set { m_SchedulerUserId = value; }
        }

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }
        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

        [DataMember]
        public string Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

        [DataMember]
        public string GroupAdminUserId
        {
            get { return m_GroupAdminUserId; }
            set { m_GroupAdminUserId = value; }
        }

        [DataMember]
        public bool IsGroupAdmin
        {
            get { return m_IsGroupAdmin; }
            set { m_IsGroupAdmin = value; }
        }


        [DataMember]
        public int GroupTypeId
        {
            get { return m_GroupTypeId; }
            set { m_GroupTypeId = value; }
        }
        [DataMember]
        public bool IsOfficeGroup
        {
            get { return m_IsOfficeGroup; }
            set { m_IsOfficeGroup = value; }
        }

        //-------
    }

    [DataContract]
    public class ChattingsList
    {
        [DataMember]
        public int TotalNumberofRecord { get; set; }

        [DataMember]
        public int FilteredRecord { get; set; }

        [DataMember]
        public List<Chatting> objChattingsList { get; set; }


    }

    [DataContract]
    public class ChattingGroupMember
    {

        private int m_ChattingGroupMemberId;
        private int m_ChattingGroupId;
        private string m_UserId;
        private string m_Type;
        private string m_QuickBloxId;
        private string m_Email;
        private string m_MemberName;
        private string m_MemberId;

        private int m_OfficeId;
        private string m_OfficeName;

        private bool m_IsGroupAdmin;
        private int m_Permission;
        private string m_PermissionType;

        [DataMember]
        public int ChattingGroupMemberId
        {
            get { return m_ChattingGroupMemberId; }
            set { m_ChattingGroupMemberId = value; }
        }
        [DataMember]
        public int ChattingGroupId
        {
            get { return m_ChattingGroupId; }
            set { m_ChattingGroupId = value; }
        }
        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        [DataMember]
        public string QuickBloxId
        {
            get { return m_QuickBloxId; }
            set { m_QuickBloxId = value; }
        }
        [DataMember]
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        [DataMember]
        public string MemberName
        {
            get { return m_MemberName; }
            set { m_MemberName = value; }
        }
        [DataMember]
        public string MemberId
        {
            get { return m_MemberId; }
            set { m_MemberId = value; }
        }

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }
        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

        [DataMember]
        public bool IsGroupAdmin
        {
            get { return m_IsGroupAdmin; }
            set { m_IsGroupAdmin = value; }
        }
        [DataMember]
        public int Permission
        {
            get { return m_Permission; }
            set { m_Permission = value; }
        }
        [DataMember]
        public string PermissionType
        {
            get { return m_PermissionType; }
            set { m_PermissionType = value; }
        }
        //-------
    }


    [DataContract]
    public class GroupChat
    {
        private int m_OfficeId;
        private int m_GroupTypeID;
        private string m_UserId;
        private string m_GroupDialogId;
        private string m_GroupName;
        private string m_GroupSubject;
        private string m_LogInUserId;


        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }

        [DataMember]
        public int GroupTypeID
        {
            get { return m_GroupTypeID; }
            set { m_GroupTypeID = value; }
        }


        [DataMember]
        public string OrgSuperAdminName { get; set; }

        [DataMember]
        public string OrgSuperAdminQBId { get; set; }


        [DataMember]
        public string OrgSuperAdminEmail { get; set; }


        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string GroupDialogId
        {
            get { return m_GroupDialogId; }
            set { m_GroupDialogId = value; }
        }
        [DataMember]
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        [DataMember]
        public string GroupSubject
        {
            get { return m_GroupSubject; }
            set { m_GroupSubject = value; }
        }

        [DataMember]
        public int OrganisationId { get; set; }

        [DataMember]
        public string LogInUserId
        {
            get { return m_LogInUserId; }
            set { m_LogInUserId = value; }
        }
    }



    [DataContract]
    public class PatientChatModel
    {
        private int m_OfficeId;
        private int m_GroupTypeID;
        private string m_UserId;
        private string m_GroupDialogId;
        private string m_LogInUserId;
        private string m_QBGroupDialogIds;

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }

        [DataMember]
        public int GroupTypeID
        {
            get { return m_GroupTypeID; }
            set { m_GroupTypeID = value; }
        }


        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string GroupDialogId
        {
            get { return m_GroupDialogId; }
            set { m_GroupDialogId = value; }
        }

        [DataMember]
        public string LogInUserId
        {
            get { return m_LogInUserId; }
            set { m_LogInUserId = value; }
        }

        [DataMember]
        public string QBGroupDialogIds
        {
            get { return m_QBGroupDialogIds; }
            set { m_QBGroupDialogIds = value; }
        }

    }


    public class Filter1
    {

        public int type = 2;
        public int limit { get; set; }
        public string sort_desc { get; set; }


    }

    [DataContract]
    public class StatusListChattingGroup_V1
    {

        [DataMember]
        public List<ChattingGroup> Data { get; set; }

        //  public List<InitiateChattingList> MemberInitiateChattingList { get; set; }
        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool IsAllowToCreateGroupChat { get; set; }
    }







    public class ChattingGroup
    {



        private string m_UserId;
        private string m_GroupName;
        private int m_GroupId;
        private string m_GroupDialogId;
        private int m_Permission { get; set; }
        private int m_OfficeId { get; set; }
        private bool m_IsGroupAdmin { get; set; }
        private bool m_IsOfficeGroup { get; set; }
        private string m_GroupSubject { get; set; }
        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        [DataMember]
        public int GroupId
        {
            get { return m_GroupId; }
            set { m_GroupId = value; }
        }
        [DataMember]
        public string GroupDialogId
        {
            get { return m_GroupDialogId; }
            set { m_GroupDialogId = value; }
        }
        [DataMember]
        public int Permission
        {
            get { return m_Permission; }
            set { m_Permission = value; }
        }
        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }
        [DataMember]
        public bool IsOfficeGroup
        {
            get { return m_IsOfficeGroup; }
            set { m_IsOfficeGroup = value; }
        }
        [DataMember]
        public bool IsGroupAdmin
        {
            get { return m_IsGroupAdmin; }
            set { m_IsGroupAdmin = value; }
        }
        [DataMember]
        public string GroupSubject
        {
            get { return m_GroupSubject; }
            set { m_GroupSubject = value; }
        }
    }



    [DataContract]
    public class PatientChatList
    {
      
        private string m_UserId;
        private int m_ChattingGroupId;
        private string m_GroupName;
        private string m_GroupSubject;
        private string m_DialogId;
        private int m_OfficeId;
        private string m_OfficeName;

        private int m_GroupTypeId;
        private string m_Time;
        private bool m_IsOfficeGroup { get; set; }

        private string m_GroupAdminUserId;
        private bool m_IsGroupAdmin;


      


        [DataMember]
        public string Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }
       
        [DataMember]
        public string UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; }
        }

        [DataMember]
        public int ChattingGroupId
        {
            get { return m_ChattingGroupId; }
            set { m_ChattingGroupId = value; }
        }
        [DataMember]
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        [DataMember]
        public string GroupSubject
        {
            get { return m_GroupSubject; }
            set { m_GroupSubject = value; }
        }


        [DataMember]
        public string DialogId
        {
            get { return m_DialogId; }
            set { m_DialogId = value; }
        }

        [DataMember]
        public int OfficeId
        {
            get { return m_OfficeId; }
            set { m_OfficeId = value; }
        }
        [DataMember]
        public string OfficeName
        {
            get { return m_OfficeName; }
            set { m_OfficeName = value; }
        }

   

        [DataMember]
        public int GroupTypeId
        {
            get { return m_GroupTypeId; }
            set { m_GroupTypeId = value; }
        }
        [DataMember]
        public bool IsOfficeGroup
        {
            get { return m_IsOfficeGroup; }
            set { m_IsOfficeGroup = value; }
        }

        [DataMember]
        public string GroupAdminUserId
        {
            get { return m_GroupAdminUserId; }
            set { m_GroupAdminUserId = value; }
        }

        [DataMember]
        public bool IsGroupAdmin
        {
            get { return m_IsGroupAdmin; }
            set { m_IsGroupAdmin = value; }
        }

        //-------
    }
    //[DataContract]
    //public class PatientChatRoom
    //{


    //    [DataMember]
    //    public List<Chatting> objChattingsList { get; set; }


    //}

}