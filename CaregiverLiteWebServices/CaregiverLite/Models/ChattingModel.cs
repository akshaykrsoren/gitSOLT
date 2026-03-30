using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace CaregiverLite.Models
{
    public class ChattingModel
    {
        public string m_CareGiverName;
        public string m_NurseId;
        public string m_AppointmentDate;
        public string m_Time;
        public string m_Point;
        public string m_Rating;

        public string m_FromID;
        public string m_ToID;
    }

    // Added By Vinod 
    public class ChatManagementModel
    {
        public string UserId { get; set; }
        public int OfficeId { get; set; }
        public string GroupType { get; set; }
    }

    public class ChattingModelV
    {
        public string CareGiverName;
        public string Role;
        public string OfficeName;
        public string NurseId;
        public string AppointmentDate;
        public string Time;
        public string Point;
        public string Rating;

        public string FromID;
        public string ToID;
    }


    public class CreateGroup
    {
        public int type = 2;
        public string name { get; set; }
        public List<int> occupants_ids { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {

        public string class_name { get; set; }
        public string ChatCategory { get; set; }
        public string OfficeID { get; set; }
    }

    public class DeleteGroup
    {
        public string dialog_id { get; set; }
        public int force { get; set; }
        //  public List<int> occupants_ids { get; set; }
    }




    public class GroupChatModel
    {
        public string GroupName { get; set; }
        public string GroupSubject { get; set; }
        public string GroupDialogId { get; set; }
        public string MembersUserId { get; set; } // multiple member's userid
        public int OfficeId { get; set; }
        public string QuickBloxIds { get; set; } // multiple member's quick bloxid
        public bool IsOfficeGroup { get; set; }
        public bool IsCustomGroup { get; set; }
        public string SelectedGroup { get; set; }

        public string OfficeIds { get; set; }
        public bool IsCheckOfficeGroup { get; set; }
    }
    public class ChattingServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public bool ResultInBool { get; set; }
        public ChattingsList ChattingsList { get; set; }
        public List<Chatting> ChattingList { get; set; }
        public List<PatientChatList> ChattingGroupList { get; set; }    
        public List<CareGivers> CaregiverList { get; set; }
        public string QuickBloxId { get; set; }
        public Chatting objDialogDetail { get; set; }
        public ChattingGroupMember objGroupMemberDetail { get; set; }
        public List<ChattingGroupMember> objGroupMemberDetailList { get; set; }
        public List<NurseCoordinator> NurseCoordinatorsList { get; set; }
        public List<ScheduleInfo> SchedulerList { get; set; }

        public ChattingServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }
        public async Task<ChattingsList> GetAllChattingList(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, string OfficeId)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllChattingList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + OfficeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ChattingsList;
        }

        public async Task<ChattingsList> GetChattingListPatientGroupWise(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, string GroupTypeId)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetChattingListPatientGroupWise/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + GroupTypeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ChattingsList;
        }

        public async Task<Chatting> GetQuickBloxIdByNurseId(int NurseId)
        {
            Chatting QuickBlox = new Chatting();

            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetQuickBloxIdByNurseId/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    QuickBlox = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return QuickBlox;
        }
        public async Task<string> GetQuickBloxIdBySchedulerUserId(string SchedulerUserId)
        {
            string QuickBloxId = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetQuickBloxIdBySchedulerUserId/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    QuickBloxId = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).QuickBloxId;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return QuickBloxId;
        }
        public async Task<string> SaveQBId(string UserId, string QuickBloxId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SaveQBId/" + UserId + "/" + QuickBloxId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
        public async Task<string> SaveDialogId(string NurseUserId, string SchedulerUserId, string DialogId, string UserType)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SaveDialogId/" + NurseUserId + "/" + SchedulerUserId + "/" + DialogId + "/" + UserType, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
        public async Task<string> GetDialogId(string NurseUserId, string SchedulerUserId)
        {

            string DialogId = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetDialogId/" + NurseUserId + "/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    DialogId = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return DialogId;
        }
        public async Task<Chatting> GetDialogDetail(string ID)
        {

            var objDialogDetail = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetDialogDetail/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objDialogDetail = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objDialogDetail;
        }

        public async Task<List<ChattingGroupMember>> GetGroupMemberDetail(string ID, string OrganisationId)
        {

            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetGroupMemberDetail/" + ID + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }

        //Get All Dialog Ids Releted To login User
        public async Task<ChattingsList> GetAllDialogId(string SchedulerUserId)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllDialogId/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ChattingsList;
        }

        public async Task<ChattingsList> GetAllGroupDialogId(string SchedulerUserId)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllGroupDialogId/" + SchedulerUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ChattingsList;
        }

        //public async Task<string> AddGroupDialogId(string DialogId, string GroupName, string UserId, string OfficeId)
        //{

        //    string result = "";
        //    try
        //    {
        //        var json = "";
        //        // Send request to server
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddGroupDialogId/" + DialogId + "/" + GroupName + "/" + UserId + "/" + OfficeId, this.cancellationToken).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse the response body. Blocking!
        //            json = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogError(ex);
        //    }
        //    return result;
        //}



        public async Task<string> NotifyUserForChatMessage(CareGivers CareGiver)
        {

            string result = "";
            var notifiableUserList = new List<CareGivers>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "NotifyUserForChatMessage", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!


                    //Send Push Notification For Chatting
                    Notification obj = new Notification();
                    IosResponse ios_response = new IosResponse();
                    AndroidResponse android_response = new AndroidResponse();
                    PushNotification objPushNotification = new PushNotification();

                    json = await response.Content.ReadAsStringAsync();
                    notifiableUserList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).CaregiverList;

                    //List<String> deviceids = new List<String>();
                    List<string> iosdevice_Permission1 = new List<String>();
                    List<string> iosdevice_Permission2 = new List<String>();
                    List<string> iosdevice_Permission3 = new List<String>();
                    //List<String> androiddeviceids = new List<String>();
                    List<string> androiddeviceids_Permission1 = new List<String>();
                    List<string> androiddeviceids_Permission2 = new List<String>();
                    List<string> androiddeviceids_Permission3 = new List<String>();

                    int cnt1 = 0, cnt2 = 0;

                    //for (int i = 0; i < notifiableUserList.Count; i++)
                    //{
                    //    if (notifiableUserList[i].DeviceType.ToLower() == "ios" && notifiableUserList[i].DeviceToken != "")
                    //    {

                    //        //deviceids.Add(notifiableUserList[i].DeviceToken);
                    //        //deviceids[cnt1] = result[i].DeviceToken;
                    //        //cnt1++;

                    //     //   notifiableUserList[i].BadgeCount = GetBadgeCountWithIncrement(result[i].UserId.ToString());


                    //        if (notifiableUserList[i].Permission == "1")
                    //            iosdevice_Permission1.Add(notifiableUserList[i].DeviceToken);
                    //        else if (notifiableUserList[i].Permission == "2")
                    //            iosdevice_Permission2.Add(notifiableUserList[i].DeviceToken);
                    //        else
                    //            iosdevice_Permission3.Add(notifiableUserList[i].DeviceToken);
                    //    }
                    //    else if (notifiableUserList[i].DeviceType.ToLower() == "android" && notifiableUserList[i].DeviceToken != "")
                    //    {
                    //        //androiddeviceids.Add(notifiableUserList[i].DeviceToken);
                    //        //androiddeviceids[cnt2] = result[i].DeviceToken;
                    //        //cnt2++;
                    //        if (notifiableUserList[i].Permission == "1")
                    //            androiddeviceids_Permission1.Add(notifiableUserList[i].DeviceToken);
                    //        else if (notifiableUserList[i].Permission == "2")
                    //            androiddeviceids_Permission2.Add(notifiableUserList[i].DeviceToken);
                    //        else
                    //            androiddeviceids_Permission3.Add(notifiableUserList[i].DeviceToken);
                    //    }
                    //}


                    string sMsg = "";
                    if (CareGiver.ChattingType == "1")
                    {
                        sMsg = CareGiver.UserName + " Sent Message : " + CareGiver.Msg;
                        CareGiver.Name = CareGiver.UserName;
                    }
                    else
                    {
                        sMsg = CareGiver.UserName + " @ " + CareGiver.Name + " : " + CareGiver.Msg;
                    }


                    for (int i = 0; i < notifiableUserList.Count; i++)
                    {
                        if (notifiableUserList[i].DeviceType.ToLower() == "ios" && notifiableUserList[i].DeviceToken != "")
                        {

                            //deviceids.Add(result[i].DeviceToken);
                            //deviceids[cnt1] = result[i].DeviceToken;
                            //cnt1++;
                            //   notifiableUserList[i].BadgeCount = GetBadgeCountWithIncrement(result[i].UserId.ToString());
                            //result[i].DeviceToken = "686fab61fe840980f83ada73f8a6fa77defb98c0002bd383e50e191516c64966";
                            if (notifiableUserList[i].Permission == "1")
                            {
                                iosdevice_Permission1.Add(notifiableUserList[i].DeviceToken);
                                if (iosdevice_Permission1.Count > 0)
                                    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission1.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, notifiableUserList[i].BadgeCount, "1");
                                iosdevice_Permission1.Clear();
                            }
                            else if (notifiableUserList[i].Permission == "2")
                            {
                                iosdevice_Permission2.Add(notifiableUserList[i].DeviceToken);
                                if (iosdevice_Permission2.Count > 0)
                                    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission2.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, notifiableUserList[i].BadgeCount, "2");
                                iosdevice_Permission2.Clear();
                            }
                            else
                            {
                                iosdevice_Permission3.Add(notifiableUserList[i].DeviceToken);
                                if (iosdevice_Permission3.Count > 0)
                                    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission3.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, notifiableUserList[i].BadgeCount, "");
                                iosdevice_Permission3.Clear();
                            }
                        }
                        else if (notifiableUserList[i].DeviceType.ToLower() == "android" && notifiableUserList[i].DeviceToken != "")
                        {

                            //androiddeviceids.Add(result[i].DeviceToken);
                            //androiddeviceids[cnt2] = result[i].DeviceToken;
                            //cnt2++;
                            //cnt1++;
                            // result[i].BadgeCount = GetBadgeCountWithIncrement(result[i].UserId.ToString());

                            if (notifiableUserList[i].Permission == "1")
                            {
                                androiddeviceids_Permission1.Add(notifiableUserList[i].DeviceToken);
                                if (androiddeviceids_Permission1.Count > 0)
                                    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission1.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "1", notifiableUserList[i].BadgeCount);
                                androiddeviceids_Permission1.Clear();

                            }
                            else if (notifiableUserList[i].Permission == "2")
                            {
                                androiddeviceids_Permission2.Add(notifiableUserList[i].DeviceToken);
                                if (androiddeviceids_Permission2.Count > 0)
                                    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission2.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "2", notifiableUserList[i].BadgeCount);
                                androiddeviceids_Permission2.Clear();

                            }
                            else
                            {
                                androiddeviceids_Permission3.Add(notifiableUserList[i].DeviceToken);
                                if (androiddeviceids_Permission3.Count > 0)
                                    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission3.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "", notifiableUserList[i].BadgeCount);
                                androiddeviceids_Permission3.Clear();

                            }
                        }
                    }
                    //string Msg = CareGiver.Msg;
                    //string sMsg = CareGiver.UserName + " Sent Message : " + CareGiver.Msg;
                    //ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(deviceids.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, 0);
                    //android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId);

                    //For IOS Devices
                    //if (iosdevice_Permission1.Count > 0)
                    //    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission1.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, CareGiver.BadgeCount, "1");
                    //if (iosdevice_Permission2.Count > 0)
                    //    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission2.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, CareGiver.BadgeCount, "2");
                    //if (iosdevice_Permission3.Count > 0)
                    //    ios_response = objPushNotification.Send_IphoneNotification_Multy_CareGiver_FORCHATTING(iosdevice_Permission3.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, CareGiver.BadgeCount, "");

                    ////For Android Devices
                    //if (androiddeviceids_Permission1.Count > 0)
                    //    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission1.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "1");
                    //if (androiddeviceids_Permission2.Count > 0)
                    //    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission2.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "2");
                    //if (androiddeviceids_Permission3.Count > 0)
                    //    android_response = objPushNotification.SendNotification_Android_Multy_FORCHATTING(androiddeviceids_Permission3.ToArray(), sMsg, CareGiver.ChattingType, CareGiver.Name, CareGiver.DialogId, "");


                    //return true;
                    //

                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        //Nurse Coordinator
        public async Task<List<NurseCoordinator>> GetUnAssignedNurseCoordinatorList(string ChattingGroupId)
        {
            NurseCoordinatorsList NurseCoordinatorsList = new NurseCoordinatorsList();
            List<NurseCoordinator> objNurseCoordinator = new List<NurseCoordinator>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetUnAssignedNurseCoordinatorList/" + ChattingGroupId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objNurseCoordinator = JsonConvert.DeserializeObject<NurseCoordinatorServiceProxy>(json).NurseCoordinatorList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objNurseCoordinator;
        }
        public async Task<string> SetNurseCoordinator(string ChattingGroupId, string NurseCoordinatorId, string Permission)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SetNurseCoordinator/" + ChattingGroupId + "/" + NurseCoordinatorId + "/" + Permission, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }




        public async Task<string> SetNurseCoordinatorAndOfficeManager(string ChattingGroupId, string NurseCoordinatorId, string Permission,string CaregiverQBId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SetNurseCoordinatorAndOfficeManager/" + ChattingGroupId + "/" + NurseCoordinatorId + "/" + Permission +"/"+ CaregiverQBId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }
























        public async Task<string> SaveQBIdOfNurseCoordinator(string Email, string QuickBloxId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SaveQBIdOfNurseCoordinator/" + Email + "/" + QuickBloxId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        public async Task<string> GetNurseCoordinatorPermissionGroupWise(string ChattingGroupId, string LoginUserUserId)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetNurseCoordinatorPermissionGroupWise/" + ChattingGroupId + "/" + LoginUserUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return Result;
        }

        public async Task<List<ChattingGroupMember>> GetNurseCoordinatorPermissionGroupWiseList(string ID)
        {

            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetNurseCoordinatorPermissionGroupWiseList/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }

        //Added By Pinki     
        //Chat for Caregiver    
        public async Task<List<ScheduleInfo>> GetAllSchedulerbyCaregiverId(CareGivers CareGiver)
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllSchedulerbyCaregiverId", new { CareGiver }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    SchedulerList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).SchedulerList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return SchedulerList;
        }
        public async Task<Chatting> GetQuickBloxDetByUserId(string UserId)
        {
            Chatting objChatting = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetQuickBloxDetByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objChatting = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objChatting;
        }
        public async Task<Chatting> GetQuickBloxDetById(int Id)
        {
            Chatting objChatting = new Chatting();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetQuickBloxDetById/" + Id.ToString(), this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    objChatting = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objChatting;
        }
        public async Task<List<Chatting>> GetPatientChattingGroupList(string UserId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientChattingGroupList/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattings;
        }

        //Assignement and Removal of Caregiver from Group Chat
        public async Task<List<CareGivers>> GetUnassignedCaregiverList(string ChattingGroupId)
        {
            List<CareGivers> careGivers = new List<CareGivers>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetUnassignedCaregiverList/" + ChattingGroupId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    careGivers = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).CaregiverList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return careGivers;
        }
        public async Task<List<ChattingGroupMember>> GetAssignedCaregiverListGroupWise(string ID)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignedCaregiverListGroupWise/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }
        public async Task<string> AddCaregiverIntoGroup(string ChattingGroupId, string NurseId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddCaregiverIntoGroup/" + ChattingGroupId + "/" + NurseId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }
        public async Task<string> RemoveMemberFromGroupChat(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "RemoveMemberFromGroupChat/" + ChattingGroupId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }


        // Added By Vinod on 17th Oct 2018

        public async Task<List<CareGivers>> GetMemberList(string UserId)
        {
            List<CareGivers> careGivers = new List<CareGivers>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllMemberList/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    careGivers = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).CaregiverList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return careGivers;
        }





        //Superadmin can assign/remove multiple scheduler/caregiver to/from  chat groups 
        public async Task<List<Chatting>> GetAllGroupExceptAssignedGroupByUserId(string UserId, string LoginUserId)
        {
            List<Chatting> chattingGroups = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllGroupExceptAssignedGroupByUserId/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattingGroups = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattingGroups;
        }
        public async Task<List<Chatting>> GetAllAssignedGroupByUserId(string UserId, string LoginUserId)
        {
            List<Chatting> chattingGroups = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllAssignedGroupByUserId/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattingGroups = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattingGroups;
        }
        //public async Task<string> AssignGroupToCaregiver(string ChattingGroupId, string UserId)
        //{
        //    string result = "";
        //    try
        //    {
        //        var json = "";
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignGroupToCaregiver/" + ChattingGroupId + "/" + UserId, this.cancellationToken).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    return result;
        //}
        public async Task<string> AssignGroupToUser(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignGroupToUser/" + ChattingGroupId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        //started on 26th Oct,2017
        #region Supervisor/Support can assign multiple member to group
        public async Task<List<CareGivers>> GetAllUnAssignedMemberList(string ChattingGroupId, string UserId, string OrganisationId)
        {
            List<CareGivers> careGivers = new List<CareGivers>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllUnAssignedMemberList/" + ChattingGroupId + "/" + UserId + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    careGivers = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).CaregiverList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return careGivers;
        }
        public async Task<List<ChattingGroupMember>> GetAssignedMemberListGroupWise(string ID)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignedMemberListGroupWise/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }
        #endregion

        //Hardik Masalawala 30-10-2017
        #region GetGroupDetailFromGroupName
        public async Task<Chatting> GetGroupDetailFromGroupName(string GroupName)
        {

            var objDialogDetail = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetGroupDetailFromGroupName/" + GroupName, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objDialogDetail = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objDialogDetail;
        }
        #endregion


        public async Task<string> SaveQBIdOfAdmin(string Email, string QuickBloxId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SaveQBIdOfAdmin/" + Email + "/" + QuickBloxId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }


        public async Task<List<Chatting>> GetPatientChattingGroupByOfficeId(string OfficeId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientChattingGroupByOfficeId/" + OfficeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattings;
        }

        public async Task<List<Chatting>> GetChatGroupListByTypeIdForUser(string UserId, string GroupTypeId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetChatGroupListByTypeIdForUser/" + UserId + "/" + GroupTypeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattings;
        }

        //*****************************

        public async Task<Chatting> AddGroupDialogId(GroupChat GroupChat)
        {

            var objDialogDetail = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddGroupDialogId", new { GroupChat }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objDialogDetail = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objDialogDetail;
        }


        public async Task<Chatting> AddOrganisationGroupDialogId(GroupChat GroupChat)
        {

            var objDialogDetail = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddOrganisationGroupDialogId", new { GroupChat }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objDialogDetail = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objDialogDetail;
        }




        public async Task<List<ChattingGroupMember>> GetAllMemberByOffice(string LoginUserId, string OfficeId, string OrganisationId)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAllMemberByOffice/" + LoginUserId + "/" + OfficeId + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }

        public async Task<Chatting> GetOfficeGroupDetailByOfficeId(string OfficeId, string UserId)
        {

            var objDialogDetail = new Chatting();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOfficeGroupDetailByOfficeId/" + OfficeId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objDialogDetail = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objDialogDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objDialogDetail;
        }

        public async Task<string> DeleteGroupChat(string ChattingGroupId, string DialogId, string LoginUserId)
        {

            var Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteGroupChat/" + ChattingGroupId + "/" + DialogId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return Result;
        }

        #region Update Group Detail

        public async Task<string> UpdateGroupDetail(string ChattingGroupId, string DialogId, string GroupName, string GroupSubject, string LoginUserID)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "UpdateGroupDetail/" + ChattingGroupId + "/" + DialogId + "/" + GroupName + "/" + GroupSubject + "/" + LoginUserID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        #endregion


        public async Task<bool> IsGroupNameAndSubjectExist(Chatting Chatting, string LoginUserId)
        {

            bool result = false;
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "IsGroupNameAndSubjectExist/" + LoginUserId, new { Chatting }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ResultInBool;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }


        public async Task<string> ExitMemberFromGroupChat(string ChattingGroupId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "ExitMemberFromGroupChat/" + ChattingGroupId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        public async Task<string> AssignGroupAdminToUser(string ChattingGroupId, string UserId, string LoginUserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AssignGroupAdminToUser/" + ChattingGroupId + "/" + UserId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }



        public async Task<List<Chatting>> GetOneToOneChatListByUserId(string UserId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOneToOneChatListByUserId/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {

            }
            return chattings;
        }

        public async Task<string> DeleteOneToOneChatByUserId(string DialogId, string UserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteOneToOneChatByUserId/" + DialogId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return result;
        }


        //public async Task<string> AddMemberIntoGroup(string ChattingGroupId, string QuickBloxId)
        //{
        //    string result = "";
        //    try
        //    {
        //        var json = "";
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddMemberIntoGroup/" + ChattingGroupId + "/" + QuickBloxId, this.cancellationToken).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            json = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    return result;
        //}


        #region AssignPermission
        public async Task<List<ChattingGroupMember>> GetGroupMemberListWithPermissionAndRole(string ID)
        {

            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetGroupMemberListWithPermissionAndRole/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }

        public async Task<string> SetGroupChatMemberPermission(string ChattingGroupId, string ChattingGroupMemberId, string Permission, string LoginUserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "SetGroupChatMemberPermission/" + ChattingGroupId + "/" + ChattingGroupMemberId + "/" + Permission + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }

        public async Task<string> GetGroupChatMemberPermission(string ChattingGroupId, string LoginUserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetGroupChatMemberPermission/" + ChattingGroupId + "/" +  LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return result;
        }


        public async Task<List<Chatting>> GetChatGroupListByOfficeIdForUser(string UserId, string GroupTypeId,string OfficeId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetChatGroupListByOfficeIdForUser/" + UserId + "/" + GroupTypeId + "/" + OfficeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattings;
        }

        #endregion

        public async Task<List<Chatting>> GetChatGroupListForDeleteManually(string UserId, string GroupTypeId)
        {
            List<Chatting> chattings = new List<Chatting>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetChatGroupListForDeleteManually/" + UserId + "/" + GroupTypeId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return chattings;
        }

        //public async Task<ChattingsList> GetPatientRoomGroupList(PatientChatModel PatientChatModel,string LogInUserId)
        //{
        //    ChattingsList ChattingsList = new ChattingsList();
        //    try
        //    {
        //        var json = "";
        //        // Send request to server
        //     //   HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "IsGroupNameAndSubjectExist/" + LoginUserId, new { Chatting }).Result;
        //        HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientRoomGroupList/" + LogInUserId, new { PatientChatModel }).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse the response body. Blocking!
        //            json = await response.Content.ReadAsStringAsync();
        //            ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ErrorLog.LogError(ex);
        //    }
        //    return ChattingsList;
        //}

        public async Task<List<PatientChatList>> GetPatientRoomGroupList(PatientChatModel PatientChatModel, string LogInUserId, string OrganisationId)
        {
            List<PatientChatList> chattings = new List<PatientChatList>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientRoomGroupList/" + LogInUserId + "/" + OrganisationId, new { PatientChatModel }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    chattings = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingGroupList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return chattings;
        }

        public async Task<ChattingsList> GetOneToOneChatList(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, string OfficeId, string OrganisationId)
        {
            ChattingsList ChattingsList = new ChattingsList();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetOneToOneChatList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + search + "/" + OfficeId + "/" + OrganisationId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ChattingsList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).ChattingsList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return ChattingsList;
        }


        public async Task<List<ScheduleInfo>> GetUnassignedSchedulerList(string ChattingGroupId)
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetUnassignedSchedulerList/" + ChattingGroupId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    SchedulerList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).SchedulerList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            { }
            return SchedulerList;
        }

        public async Task<List<ChattingGroupMember>> GetAssignedSchedulerListGroupWise(string ID)
        {
            var objGroupMemberDetailList = new List<ChattingGroupMember>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAssignedSchedulerListGroupWise/" + ID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    objGroupMemberDetailList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).objGroupMemberDetailList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.LogError(ex);
            }
            return objGroupMemberDetailList;
        }

        public async Task<string> AddMemberIntoGroup(string ChattingGroupId, string QuickBloxId, string LoginUserId)
        {
            string result = "";
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddMemberIntoGroup/" + ChattingGroupId + "/" + QuickBloxId + "/" + LoginUserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }

        public async Task<List<ScheduleInfo>> GetALLSuperadminList()
        {
            List<ScheduleInfo> SchedulerList = new List<ScheduleInfo>();
            try
            {
                var json = "";
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetALLSuperadminList" , this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    SchedulerList = JsonConvert.DeserializeObject<ChattingServiceProxy>(json).SchedulerList;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return SchedulerList;
        }
    }
}