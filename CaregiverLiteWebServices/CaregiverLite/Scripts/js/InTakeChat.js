$(document).ready(function () {
    //callfun();
    $("#loading").hide();
    $("#AuthAndInsuranceForm").hide();
    loadUserInfo();
    $("#btnSendMsg").bind("click", sendMessage);
    $("#lblGroupInfo").bind("click", ShowAddedMembersInChatRoom);
    ChattingGroupMemberList();
    startChat();
    // loadMessageV1(1);


    $(document).on('change', 'input#file1', function (event) {
        ////1

        var filePathsss = $(this).val();
        var tmppath = URL.createObjectURL(event.target.files[0]);

        debugger;
        var totalSelectedFiles = $("input[type=file]")[0].files.length;
        var totalFiles = $("input[type=file]")[0].files;
        var totalFileType = "";

        if (totalFiles != null) {
            for (var l = 0; l < totalSelectedFiles; l++) {
                File = totalFiles[l];
                //console.log(File);
                //console.log(temptotalSelectedFiles+" + "+File.name);
                totalFileType = File.type.split('/');
                if (totalFileType[0] != 'image' && totalFileType[0] != 'video' && totalFileType[0] != 'audio') {
                    totalFileType[0] = 'doc';
                }

                var ext = File.name.split('.').pop();
                var Filesize = (File.size / (1024 * 1024)).toFixed(2);

                if (File) {
                    if (Filesize < 95) {
                        if (ext.toLowerCase() == "ppt"
                            || ext.toLowerCase() == "pptx"
                            || ext.toLowerCase() == "xls"
                            || ext.toLowerCase() == "doc"
                            || ext.toLowerCase() == "docx"
                            || ext.toLowerCase() == "png"
                            || ext.toLowerCase() == "jpg"
                            || ext.toLowerCase() == "jpeg"
                            || ext.toLowerCase() == "mp4"
                            || ext.toLowerCase() == "pdf") {
                            toastr.success("File Selected");
                            $("#fileCall").css({ 'color': "deepskyblue" });
                            $("#FileAlert").css({ 'display': "block" });
                            $("#FileAlertContent").html("<b>" + (l + 1) + " File Selected.</b>");
                            //Remove
                            $("#fileRemove").css({ 'display': "block" });
                            $("#fileCall").css({ 'display': "none" });
                            //
                            //$("#txtSendMsg").attr('disabled','true')

                        }
                        else {
                            toastr.info("You Choose Wrong format file... Please Select File having following extension .ppt, .pptx,  .xls, .doc, .docx, .png, .jpg, .jpeg, .mp4, .pdf");
                            $("#file1").val('');
                            $("#fileCall").css({ 'color': "" });
                            $("#FileAlert").css({ 'display': "none" });

                            $("#fileRemove").css({ 'display': "none" });
                            $("#fileCall").css({ 'display': "block" });
                            toastr.info("No File Selected... Please Open again for select a file.");
                            $("#txtSendMsg").removeAttr('disabled')
                        }

                    } else {
                        if (Filesize > 95) {
                            toastr.info("Please Select a file having size less then 95MB.");
                            $("#file1").val('');
                            $("#fileCall").css({ 'color': "" });
                            $("#FileAlert").css({ 'display': "none" });

                            $("#fileRemove").css({ 'display': "none" });
                            $("#fileCall").css({ 'display': "block" });
                            toastr.info("No File Selected... Please Open again for select a file.");
                            $("#txtSendMsg").removeAttr('disabled')
                        }

                    }
                }
                else {
                    toastr.info("No File Selected... Please Open again for select a file.");
                }

            }//--For looop end
        }


    }); 




    $('#txtSendMsg').on('keyup', function (event) {
        if (event.key === 'Enter' || event.keyCode === 13) {
            if ($('#txtSendMsg').val().trim() == '') {
                toastr.info("Please Enter Message.");
                return false;
                
            }
            else {
                sendMessage();
            }
            
        }
    });
    $("#FileAlert").hide();
    $("#fileRemove").click(function () {

        $("#fileRemove").css({ 'display': "none" });
        $("#fileCall").css({ 'display': "block" });
        $("#file1").val('');
        $("#fileCall").css({ 'color': "" });
        $("#FileAlert").css({ 'display': "none" });
        toastr.info("Attachment Removed");
        $("#txtSendMsg").removeAttr('disabled')
    });
});




//var sFromQBName = "Ashish";
//var dialogJid = "...";
//var sFromQBId = "32969860";
//var ssFromQBId = "32969860";
//var tmpDialogId = "683d9fa2e7affd001a0bc87e";
//var Email = "aryaashish200@gmail.com";//"Superadmin@PaSeva.com";

//var sFromQBName = "SuperAdmin";
//var dialogJid = "...";
//var sFromQBId = "32132455";
//var ssFromQBId = "32132455";
//var tmpDialogId = "683d9fa2e7affd001a0bc87e";
//var Email = "superadmin@paseva.com"; 

//var sFromQBName = "Raju";
//var dialogJid = "...";
//var sFromQBId = "32939723";
//var ssFromQBId = "32939723";
//var tmpDialogId = "683d9fa2e7affd001a0bc87e";
//var Email = "raju33@gmail.com";

var sFromQBName = "";
var dialogJid = "...";
var sFromQBId = "";
var ssFromQBId = "";
var tmpDialogId = "";
var Email = "";
var grpname = "";
var useridLogin = "";
var loaderFlag = 0;
function loadUserInfo() {

    sFromQBName = $("#InTake_sFromQBName").val();
    dialogJid = "...";
    sFromQBId = $("#InTak_FromQBId").val();
    ssFromQBId = $("#InTak_FromQBId").val();
    tmpDialogId = $("#InTake_Dialogid").val();
    Email = $("#InTake_Email").val();
    grpname = $("#InTake_GrpName").val();
    useridLogin = $("#InTake_LoginUserUserId").val();
    $(".ChatRoomName").html(grpname);

}

//var grpname = "Ashish";
//var useridLogin = "d59ff81a-5f78-4d23-be3b-c8630313e2ba";

//var tmpDialogId="";
var msgUserList = [];
var QuickBloxInit = "https://apisoltapps.quickblox.com";
var attachmentArray = [];
//var GroupMemberList = [{ "ChattingGroupMemberId": 2764, "ChattingGroupId": 668, "UserId": "d59ff81a-5f78-4d23-be3b-c8630313e2ba", "Type": "SuperAdmin", "QuickBloxId": "32132455", "Email": "superadmin@paseva.com", "MemberName": "Ben Moss", "MemberId": "18", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": true, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 2765, "ChattingGroupId": 668, "UserId": "cd1be839-42e3-4b2a-89da-01088f7d18e1", "Type": "Scheduler", "QuickBloxId": "32133194", "Email": "", "MemberName": "", "MemberId": "", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 2766, "ChattingGroupId": 668, "UserId": "bd8ac249-0dad-4188-a785-e170962c5a41", "Type": "CareGiver", "QuickBloxId": "32139374", "Email": "drdbrar@homecareforyou.com", "MemberName": "Dr Dev Brar", "MemberId": "170", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 4117, "ChattingGroupId": 668, "UserId": "b468e820-5cc7-4644-a58b-4db9383b600a", "Type": "CareGiver", "QuickBloxId": "32141284", "Email": "", "MemberName": "Michele Correia", "MemberId": "175", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 4159, "ChattingGroupId": 668, "UserId": "04a79f9c-d789-41ea-bdf1-98b71f3f4c1c", "Type": "CareGiver", "QuickBloxId": "32134584", "Email": "", "MemberName": "Jennifer Dufault", "MemberId": "105", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 5551, "ChattingGroupId": 668, "UserId": "9ad42caa-c2ca-40c6-b326-4dbfd31dabbf", "Type": "CareGiver", "QuickBloxId": "32157739", "Email": "Kpavao@fakemail.com", "MemberName": "Karen Pavao", "MemberId": "218", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12523, "ChattingGroupId": 668, "UserId": "a8f73019-0ec5-4752-9723-8fa6e97158e3", "Type": "Office Staff", "QuickBloxId": "32156252", "Email": "ASouza@yahoo.com", "MemberName": "Adam Souza", "MemberId": "43", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12525, "ChattingGroupId": 668, "UserId": "1d8e05a8-50ad-46d6-988a-cd61b6e287be", "Type": "Office Staff", "QuickBloxId": "32158381", "Email": "Avirzania@homecareforyou.com", "MemberName": "Amit Virzania", "MemberId": "57", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12529, "ChattingGroupId": 668, "UserId": "74b843f4-71dc-45eb-b1a3-acb2d3839871", "Type": "Office Staff", "QuickBloxId": "32158386", "Email": "adua@homecareforyou.com", "MemberName": "Amit Dua", "MemberId": "61", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12532, "ChattingGroupId": 668, "UserId": "6e06e4bd-86d3-4a77-813f-5b9e317b3793", "Type": "Office Staff", "QuickBloxId": "32158374", "Email": "Asarkar@homecareforyou.com", "MemberName": "Anup Sarkar", "MemberId": "51", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12535, "ChattingGroupId": 668, "UserId": "69bd5084-bb2d-4d57-a5c8-e85307846c22", "Type": "Office Staff", "QuickBloxId": "32158377", "Email": "Asharma@homecareforyou.com", "MemberName": "Archita Sharma", "MemberId": "53", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12539, "ChattingGroupId": 668, "UserId": "ff09896d-0580-482c-93f2-8d86f71b0fe9", "Type": "Office Staff", "QuickBloxId": "32139379", "Email": "Dmarshman@homecareforyou.com", "MemberName": "Dannielle Marshman", "MemberId": "19", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12542, "ChattingGroupId": 668, "UserId": "3cfbbc0a-e560-43a0-aafe-482f7271207b", "Type": "Office Staff", "QuickBloxId": "32158657", "Email": "dev@yopmail.com", "MemberName": "Dev Angrej", "MemberId": "62", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12546, "ChattingGroupId": 668, "UserId": "5b855e3f-aad2-443f-84c4-e891b942f757", "Type": "Office Staff", "QuickBloxId": "32158383", "Email": "dbisht@homecareforyou.com", "MemberName": "Devinder Singh Bisht", "MemberId": "59", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12548, "ChattingGroupId": 668, "UserId": "f52bcf92-dac6-4483-8db9-22b505dfe36b", "Type": "Office Staff", "QuickBloxId": "32158385", "Email": "jsingh@homecareforyou.com", "MemberName": "Jagjit Singh", "MemberId": "60", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12554, "ChattingGroupId": 668, "UserId": "ff3171a8-c59b-446e-8a70-91e544ee5d9c", "Type": "Office Staff", "QuickBloxId": "32139381", "Email": "mafonso@comcast.net", "MemberName": "Melissa Afonso", "MemberId": "20", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12559, "ChattingGroupId": 668, "UserId": "5fa20ca6-82c4-4cfe-a67d-74965d34b024", "Type": "Office Staff", "QuickBloxId": "32158379", "Email": "ryadav@homecareforyou.com", "MemberName": "Rahul Yadav", "MemberId": "55", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12561, "ChattingGroupId": 668, "UserId": "53f0fcab-bf68-49bc-b301-a2df4722308e", "Type": "Office Staff", "QuickBloxId": "32158382", "Email": "Sandeep.kumar@homecareforyou.com", "MemberName": "Sandeep Kumar", "MemberId": "58", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12564, "ChattingGroupId": 668, "UserId": "92257547-340d-4d89-9a66-0c0e5df2ae09", "Type": "Office Staff", "QuickBloxId": "32158378", "Email": "Sushil.sharma@homecareforyou.com", "MemberName": "Sushil Kumar", "MemberId": "54", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12566, "ChattingGroupId": 668, "UserId": "5c87dd74-2e98-4f25-9d72-4046abc5e24f", "Type": "Office Staff", "QuickBloxId": "32158380", "Email": "vdungdung@homecareforyou.com", "MemberName": "Vijay Kumar Dungdung", "MemberId": "56", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 12569, "ChattingGroupId": 668, "UserId": "31e4e086-61aa-4080-8f43-a8c6d404fdf8", "Type": "Office Staff", "QuickBloxId": "32158375", "Email": "Vipinsharma@homecareforyou.com", "MemberName": "Vipin Sharma", "MemberId": "52", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 17632, "ChattingGroupId": 668, "UserId": "006f1bea-b29e-485f-adb0-4d0811c9f2f3", "Type": "CareGiver", "QuickBloxId": "32134587", "Email": "", "MemberName": "Jen Sylvia", "MemberId": "108", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 30405, "ChattingGroupId": 668, "UserId": "30f34110-5cc2-42ba-8b64-acb721aa64c1", "Type": "CareGiver", "QuickBloxId": "32155755", "Email": "", "MemberName": "Rajesh", "MemberId": "201", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 54884, "ChattingGroupId": 668, "UserId": "d0655019-f009-49ba-a100-6bdc32102ac0", "Type": "CareGiver", "QuickBloxId": "32134582", "Email": "", "MemberName": "Megan Fontes", "MemberId": "103", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 63566, "ChattingGroupId": 668, "UserId": "35031b49-1c28-4268-9707-900bc26eff6c", "Type": "OfficeManager", "QuickBloxId": "32496095", "Email": "GroupAdminForMA@gmail.com", "MemberName": "GroupAdminForMA Singh", "MemberId": "52", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }];
//var GroupMemberList = [{ "ChattingGroupMemberId": 2764, "ChattingGroupId": 668, "UserId": "d59ff81a-5f78-4d23-be3b-c8630313e2ba", "Type": "SuperAdmin", "QuickBloxId": "32132455", "Email": "superadmin@paseva.com", "MemberName": "Ben Moss", "MemberId": "18", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": true, "Permission": 0, "PermissionType": null }, { "ChattingGroupMemberId": 63566, "ChattingGroupId": 668, "UserId": "35031b49-1c28-4268-9707-900bc26eff6c", "Type": "OfficeManager", "QuickBloxId": "32496095", "Email": "GroupAdminForMA@gmail.com", "MemberName": "GroupAdminForMA Singh", "MemberId": "52", "OfficeId": 0, "OfficeName": null, "IsGroupAdmin": false, "Permission": 0, "PermissionType": null }];
//var GroupMemberList = [
//    {
//        "ChattingGroupMemberId": 2764,
//        "ChattingGroupId": 668,
//        "UserId": "d59ff81a-5f78-4d23-be3b-c8630313e2ba",
//        "Type": "SuperAdmin",
//        "QuickBloxId": "32132455",
//        "Email": "superadmin@paseva.com",
//        "MemberName": "Ben Moss",
//        "MemberId": "18",
//        "OfficeId": 0,
//        "OfficeName": null,
//        "IsGroupAdmin": true,
//        "Permission": 0,
//        "PermissionType": null
//    },
//    {
//        "ChattingGroupMemberId": 63566,
//        "ChattingGroupId": 668,
//        "UserId": "35031b49-1c28-4268-9707-900bc26eff6c",
//        "Type": "OfficeManager",
//        "QuickBloxId": "32969860",
//        "Email": "aryaashish200@gmail.com",
//        "MemberName": "Ashish",
//        "MemberId": "52",
//        "OfficeId": 0,
//        "OfficeName": null,
//        "IsGroupAdmin": false,
//        "Permission": 0,
//        "PermissionType": null
//    }
//];

var GroupMemberList = [];
function sendMessage() {
    //  alert("ashish");


    var currentMessage = $('#txtSendMsg').val();
    $('#txtSendMsg').val("");
    //var opponentId = sToQBId;
    var sToken = sessionStorage.getItem("sToken");



    ////1
    var totalSelectedFiles = $("input[type=file]")[0].files.length;
    totalFiles = $("input[type=file]")[0].files;
    var totalFileType = "";

    if (totalSelectedFiles > 0) {
        $("#loading").show();
        toastr.info("File Uploading Stared");
        debugger;
        uploadFile(totalSelectedFiles - 1, totalFiles[totalSelectedFiles - 1], currentMessage);
        //$("#loading").hide();

    }
    else {
        //debugger;
        var msg = {
            type: 'groupchat',
            body: currentMessage,
            extension: {
                save_to_history: 1,
                userName: sFromQBName,
                chatType: 2,//"@ViewBag.GroupTypeId",//"2",//Custom Parameter pass to QuickBlox
                MsgType: 'text'
            },
            markable: 1
        };

        if (currentMessage.length != 0) {
            QB.chat.send(dialogJid, msg);


            $("#loading").show();
            // Start for broadcast message
            debugger;
            let params = (new URL(document.location)).searchParams;
            let UrlIdValue = params.get("Id");
            if (UrlIdValue == '11582') {
                var msg = {
                    type: 'groupchat',
                    body: currentMessage,
                    extension: {
                        save_to_history: 1,
                        userName: sFromQBName,
                        chatType: "1",
                        MsgType: 'text'
                    },
                    markable: 1
                };

                for (var i1 = 0; i1 < GetAllDialogIdBroadCast.length; i1++) {
                    QB.chat.send(GetAllDialogIdBroadCast[i1].room_jid, msg);

                }
            }



            debugger;
            let chtids = (new URL(document.location)).searchParams;
            let GroupChatID = chtids.get("Id");

            debugger;
            //var MemberToNotify =[{

            //    MemberToNotify:msgUserList

            //}]

            var MemberToNotify = {
                MemberToNotify: msgUserList,
                ChatId: GroupChatID,
                vMsg: currentMessage,
                UserName: sFromQBName,
                GroupName: "@ViewBag.GroupName",
                //QuickBloxId:sFromQBId,
                DialogId: tmpDialogId,
                //Type:"@ViewBag.GroupTypeId"
                Type: "1"
            }



            /*  var url = INITURL + '/Chatting/MemberToNotifyforTaggedNotification';
              // var url='http://localhost:15177'+'/Chatting/MemberToNotifyforTaggedNotification';
              $.ajax({
                  url: url,
                  data:{
                      // MemberToNotify:msgUserList[i],
                      ChatId:GroupChatID,
                      vMsg: currentMessage,
                      UserName : sFromQBName,
                      GroupName : "@ViewBag.GroupName",
                      //QuickBloxId:sFromQBId,
                      DialogId : tmpDialogId,
                      //Type:"@ViewBag.GroupTypeId"
                      Type:"1"
                  },
                  cache: false,
                  type: 'POST',
                  async: false,
                  success: function (data) {
                      debugger;
  
  
                  },
                  error: function (reponse) {
                      $("#loading").hide();
                  }
              });  */



            $("#loading").hide();
        }

        else {
            toastr.info("Please Enter Message.");
        }
    }


    $('#txtSendMsg').focus();
    QB.chat.onMessageListener = onMessage;
    // loadMessageV1(1);
}



function uploadFile(temptotalSelectedFiles, File, currentMessage) {
    $("#loading").show();
    debugger;
    File = totalFiles[temptotalSelectedFiles];

    totalFileType = File.type.split('/');

    if (totalFileType[0] != 'image' && totalFileType[0] != 'video' && totalFileType[0] != 'audio') {
        totalFileType[0] = 'doc';
    }


    if (File != null) {
        debugger;
        var params = { name: File.name, file: File, type: File.type, size: File.size, 'public': false };
        $("#loading").show();
        debugger;
        QB.content.createAndUpload(params, function (err, response) {
            if (err) {
                console.log(err);
            } else {
                var uploadedFileId = response.id;

                debugger;
                var msg = {
                    type: 'groupchat',
                    body: totalFileType[0] + ". " + currentMessage,
                    extension: {
                        save_to_history: 1,
                        userName: sFromQBName,
                        chatType: "@ViewBag.GroupTypeId",//"2",//Custom Parameter pass to QuickBlox
                        MsgType: totalFileType[0],
                        file_name: File.name,
                        file_size: File.size

                    },
                    markable: 1
                };
                msg["extension"]["attachments"] = [{ id: uploadedFileId, type: File.type }];

                QB.chat.send(dialogJid, msg);
                toastr.success(File.name + " " + File.type + " uploaded");
                $("#FileAlert").css({ 'display': "none" });
                $("#fileRemove").css({ 'display': "none" });
                $("#fileCall").css({ 'display': "block" });
                $("#fileCall").css({ 'color': "" });

                //Send PushNotification start

                var url = INITURL + '/Chatting/NotifyUserForChatMessage';
                $.ajax({
                    url: url,
                    data: {
                        UserId: 1,
                        Msg: totalFileType[0] + ". ",               //+ currentMessage,
                        QuickBloxId: sFromQBId,
                        DialogId: tmpDialogId,
                        Type: "@ViewBag.GroupTypeId",
                        UserName: sFromQBName,
                        GroupName: "@ViewBag.GroupName"
                    },
                    cache: false,
                    type: 'POST',
                    async: false,
                    success: function (data) {
                        //console.log(temptotalSelectedFiles+" + "+File.name);
                        //console.log("temptotalSelectedFiles - " + temptotalSelectedFiles);
                        temptotalSelectedFiles -= 1;
                        if (temptotalSelectedFiles >= 0) {
                            uploadFile(temptotalSelectedFiles, totalFiles[temptotalSelectedFiles]);
                        }
                        else {
                            toastr.info("File Uploading Finished.");
                            $("#file1").val('');
                            $("#txtSendMsg").removeAttr('disabled');
                            $("#loading").hide();
                        }
                    },
                    error: function (reponse) {
                        //alert("error : " + reponse);
                        $("#loading").hide();
                    }
                });


            }

        });

    }

}

function RegisterNewUser(Login, UserId1, Index) {
    var RegisterUser = { 'login': Login, 'password': "Welcome007!", 'tag': "differenzsystem", 'email': Login };

    QB.users.create(RegisterUser, function (err, user1) {
        if (user1) {
            //Add new QuickBlox Id For Specific User
            //debugger;
            var url = INITURL + '/Chatting/SaveQBId';
            var QBId = user1.id;
            $.ajax({
                url: url,
                data: { UserId: UserId1, QuickBloxId: QBId },
                cache: false,
                type: 'POST',
                async: false,
                success: function (data) {
                    if (data == "Success") {

                        if (UserId1 == "d59ff81a-5f78-4d23-be3b-c8630313e2ba") {
                            sFromQBId = "" + QBId + "";
                            GroupMemberList[Index].QuickBloxId = sFromQBId;

                            sessionStorage.setItem("FromQBId", QBId);
                            startChat();

                        }
                        if (UserId1 == GroupMemberList[Index].UserId) {

                            //sToQBId = "" + QBId + "";
                            //GroupMemberList[Index].QuickBloxId = sToQBId;
                            sessionStorage.setItem("ToQBId", QBId);
                        }
                    }
                    //CreateNewGroupDialod();
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                    $("#loading").hide();
                }
            });


            // success
        } else {
            return;
            // error
        }
    });
}

function onMessage(sFromQBId, sentMsg) {

    if (parseInt(sFromQBId) != parseInt(ssFromQBId)) {

        playAudio();
    }


    //debugger;
    if (sentMsg.markable) {
        //     debugger;
        var params = {
            messageId: sentMsg.id,
            userId: sFromQBId,
            dialogId: sentMsg.dialog_id
        };
        QB.chat.sendReadStatus(params);

    }
    //debugger;

    loadChatMessages(100, 0);
    //$("#file1").val('');
    $("#fileCall").css({ 'color': "" });
    $("#loading").hide();
}
function loadChatMessages(vLimit, vSkip) {

    //    var user = {
    //        id: sFromQBId,
    //        login: Email,
    //        password: "Welcome007!"
    //};
    //QB.createSession({ login: user.login, password: user.password }, function (err, result) {

    //    if (err) {
    //        alert(err.message);
    //    } else {
    //        //alert("Session Token- " + result.token);
    //        sessionStorage.setItem("sToken", result.token);

    //        loadMessageV1(1);

    //    }
    //});
    loadMessageV1(1);
}

function loadMessageV1(scroll) {

    //debugger;
    var dialogId = tmpDialogId;


    var params = { chat_dialog_id: dialogId, sort_desc: 'date_sent', limit: 50, skip: 0 };
    QB.chat.message.list(params, function (err, messages) {


        if (messages) {
            var latestMessages = "";

            var len = messages.items.length;
            if (len > 0) {
                for (var i = len - 1; i >= 0; i--) {



                    var strMsg = messages.items[i].message;
                    strMsg = strMsg.replace(/((http|https|ftp|ftps|www)\:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(\/\S*)?)/g, '<a href="$1">$1</a>');

                    var filenamedata = messages.items[i].file_name;

                    // start login for read status ********************************************************************************
                    var r = messages.items[i].read_ids; // all read ids


                    //debugger;
                    if (parseInt(messages.items[i].sender_id) != parseInt(sFromQBId)) {
                        if (!r.includes(parseInt(sFromQBId))) {
                            playAudio();

                            //debugger;
                            var params = {
                                messageId: messages.items[i]._id,
                                userId: messages.items[i].sender_id,
                                dialogId: messages.items[i].chat_dialog_id
                            };

                            QB.chat.sendReadStatus(params);

                        }

                    }

                    // end login for read status ********************************************************************************

                    //Image Display Start
                    var imageHTML = "";
                    var fileType = null;
                    if (messages.items[i].attachments.length > 0) {
                        if (messages.items[i].hasOwnProperty("attachments")) {
                            var fileId = messages.items[i].attachments[0].id;
                            var qbSessionToken = sessionStorage.getItem("sToken");
                            var privateUrl = QuickBloxInit + "/blobs/" + fileId + "/download?token=" + qbSessionToken;
                            var fileType1 = messages.items[i].attachments[0].type;
                            fileType = messages.items[i].MsgType;
                            var ancherStart = "<a target='_blank'  href='" + privateUrl + "' alt=''>";
                            var ancherEnd = "</a>";

                            var UserData = messages.items[i].userName;
                            var _id = messages.items[i]._id;

                            debugger;
                            var my_object = {};

                            var messagedate = new Date(messages.items[i].date_sent * 1000).toLocaleTimeString() + " " + new Date(messages.items[i].date_sent * 1000).toLocaleDateString();

                            my_object.privateUrl = privateUrl;
                            my_object.userName = UserData;
                            my_object.fileType = fileType;
                            my_object.anchertag = ancherStart + ancherEnd;
                            my_object.filenamedata = filenamedata;
                            my_object.messagedate = messagedate;
                            my_object.fileId = fileId;
                            my_object.qbSessionToken = qbSessionToken;
                            my_object._id = _id;



                            if (fileType == "doc") {

                                if (String(messages.items[i].attachments[0].type) == "application/vnd.openxmlformats-officedocument.presentationml.presentation" || String(messages.items[i].attachments[0].type) == "pptx" || String(messages.items[i].attachments[0].type) == "ppt") {
                                    imageHTML = ancherStart + "<i class='fa fa-file-powerpoint-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;
                                }
                                else if (String(messages.items[i].attachments[0].type) == "application/vnd.ms-excel" || String(messages.items[i].attachments[0].type) == "xls" || String(messages.items[i].attachments[0].type) == "xlsx") {
                                    imageHTML = ancherStart + "<i class='fa fa-file-excel-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;
                                }
                                else if (String(messages.items[i].attachments[0].type) == "application/pdf" || String(messages.items[i].attachments[0].type) == "pdf") {
                                    imageHTML = "<a  onclick='openMedia(\"" + privateUrl + "\",3);' data-toggle='modal' data-target='#myModalOpenMedia' >" + "<i class='fa fa-file-pdf-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;//myModalOpenMedia
                                }
                                else if (String(messages.items[i].attachments[0].type) == "application/msword" || String(messages.items[i].attachments[0].type) == "docx") {
                                    imageHTML = ancherStart + "<i class='fa fa-file-word-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;
                                }
                                else if (String(messages.items[i].attachments[0].type) == "text/x-ms-contact") {
                                    imageHTML = ancherStart + "<i class='fa fa-phone fa-2x'></i style='font-size: 20px;'>" + ancherEnd;
                                }
                                else {
                                    imageHTML = ancherStart + "<i class='fa fa-file-text'></i style='font-size: 20px;'>" + ancherEnd;
                                }
                            }
                            if (fileType == "audio") {
                                imageHTML = "<audio  src='" + privateUrl + "' controls></audio>";
                            }
                            if (fileType == "image") {
                                imageHTML = "<a  onclick='openMedia(\"" + privateUrl + "\",1);' data-toggle='modal' data-target='#myModalOpenMedia' >" + "<i class='fa fa-file-image-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;//myModalOpenMedia
                            }
                            else if (fileType == "video") {
                                imageHTML = "<a  onclick='openMedia(\"" + privateUrl + "\",2);' data-toggle='modal' data-target='#myModalOpenMedia' >" + "<i class='fa fa-file-video-o fa-2x' style='font-size: 20px;'></i>" + ancherEnd;//myModalOpenMedia
                            }
                        }

                        my_object.imageHTML = imageHTML;
                        attachmentArray.push(my_object);


                        fileType = "";



                    }

                    //Image Display End
                    var date1 = new Date(messages.items[i].date_sent * 1000).toLocaleTimeString() + " " + new Date(messages.items[i].date_sent * 1000).toLocaleDateString();
                    if (messages.items[i].sender_id == sFromQBId) {

                        // start logic for read message *********************************************************************
                        var x = [];
                        var readStatus = "";
                        if (GroupMemberList.length > 1) {
                            for (var r1 = 0; r1 < GroupMemberList.length; r1++) {
                                x.push(parseInt(GroupMemberList[r1].QuickBloxId));
                            }
                            x = x.sort();

                            Array.prototype.compare = function (testArr) {
                                if (this.length != testArr.length) return false;
                                for (var i = 0; i < testArr.length; i++) {
                                    if (this[i].compare) { //To test values in nested arrays
                                        if (!this[i].compare(testArr[i])) return false;
                                    }
                                    else if (this[i] !== testArr[i]) return false;
                                }
                                return true;
                            }


                            if (r.sort().compare(x)) {
                                readStatus += "<span><i class='fa fa-check' style='color: #50b3e2;'></i></span>";
                            }
                            else {
                                readStatus += "<span><i class='fa fa-check'></i></span>";
                            }
                        }



                        // end logic for read message *********************************************************************
                        if (imageHTML) {
                            if (strMsg == "doc. ") {
                                //console.log("Sender Id " + messages.items[i].sender_id + " - " + messages.items[i].message + " Sent By You");

                                latestMessages += "<tr><td></td><td class='sent'><div class='message-header'><p class='sender'></p><span class='time'>" + date1 + "</span></div> <p class='message-textSend'><span>" + imageHTML + " " + filenamedata + "</span></p></td></tr>";

                                //        "<tr ><td style='background:#E3E3E3'><b>You Said </b>: <span>" + imageHTML + " " + filenamedata + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + readStatus + "</span>" + " </td></tr>";
                            }
                            else {
                                latestMessages += "<tr><td></td><td class='sent'><div class='message-header'><p class='sender'></p><span class='time'>" + date1 + "</span></div> <p class='message-textSend'><span>" + imageHTML + " " + strMsg + "</span></p></td></tr>";

                                //        "<tr ><td style='background:#E3E3E3'><b>You Said </b>: <span>" + imageHTML + " " + strMsg + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + readStatus + "</span>" + " </td></tr>";
                            }
                        }
                        else {
                            latestMessages += "<tr><td></td><td class='sent'><div class='message-header'><p class='sender'></p><span class='time'>" + date1 + "</span></div> <p class='message-textSend'><span>" + imageHTML + " " + strMsg + "</span></p></td></tr>";

                            //        "<tr ><td style='background:#E3E3E3'><b>You Said</b> : <span>" + strMsg + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + readStatus + "</span>" + imageHTML + " </td></tr>";
                        }

                    } else {
                        var receiver = "";
                        var Userid = "";
                        var Id = "";
                        if ('@ViewBag.permission' != "4") {
                            if (GroupMemberList.length > 1) {
                                for (var i1 = 0; i1 < GroupMemberList.length; i1++) {
                                    if (GroupMemberList[i1].QuickBloxId == messages.items[i].sender_id) {
                                        receiver = GroupMemberList[i1].MemberName;

                                        UserId = GroupMemberList[i1].UserId;

                                        Id = GroupMemberList[i1].MemberId;
                                    }
                                }
                                if (receiver == "") {
                                    receiver = messages.items[i].userName;

                                }
                            }

                            if (imageHTML) {

                                //console.log("Reveiver Id " + messages.items[i].sender_id + " - " + messages.items[i].message + " Sent By  @Session["ToQBUserName"] ");
                                if (messages.items[i].message == null) {
                                    latestMessages += "<tr><td class='received'><div class='message-header'><p class='sender'>" + receiver + "</p><span class='time'>" + date1 + "</span></div><p class='message-text card-text'>" + imageHTML + "</p></td><td></td></tr>";

                                    //"<tr><td style='background:#D1E5EF'><b>" + receiver + " Said </b>: <span>" + imageHTML + " " + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + "</span></td></tr>";
                                } else {
                                    if (strMsg == "doc. ") {
                                        debugger;
                                        latestMessages += "<tr><td class='received'><div class='message-header'><p class='sender'>" + receiver + "</p><span class='time'>" + date1 + "</span></div><p class='message-text card-text'>" + imageHTML + " " + filenamedata + "</p></td><td></td></tr>";
                                        //"<tr><td style='background:#D1E5EF'><b  onclick='OpenPrivateChatToggle(\"" + UserId + "\" ,\"" + Id + "\");'>" + receiver + " Said </b>: <span>" + imageHTML + " " + filenamedata + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + "</span></td></tr>";
                                    }
                                    else {
                                        latestMessages += "<tr><td class='received'><div class='message-header'><p class='sender'>" + receiver + "</p><span class='time'>" + date1 + "</span></div><p class='message-text card-text'>" + imageHTML + " " + strMsg + "</p></td><td></td></tr>";
                                        //"<tr><td style='background:#D1E5EF'><b  onclick='OpenPrivateChatToggle(\"" + UserId + "\" ,\"" + Id + "\");'>" + receiver + " Said </b>: <span>" + imageHTML + " " + strMsg + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + "</span></td></tr>";
                                    }
                                }
                            }
                            else {

                                latestMessages += "<tr><td class='received'><div class='message-header'><p class='sender'>" + receiver + "</p><span class='time'>" + date1 + "</span></div><p class='message-text card-text'>" + strMsg + "</p></td><td></td></tr>";
                                //"<tr><td style='background:#D1E5EF'><b   onclick='OpenPrivateChatToggle(\"" + UserId + "\" ,\"" + Id + "\");'>" + receiver + " Said </b>: <span>" + strMsg + "</span><span style='float:right'><img src='http://ec2-52-38-33-252.us-west-2.compute.amazonaws.com/image/chat_black_24dp.svg' style='vertical-align:middle' width='10%' height='10%' alt='image'/>" + date1 + "</span>" + imageHTML + " </td></tr>";

                            }
                        }
                    }
                }

            }
            else {
                if (loaderFlag == 0) {
                    loaderFlag = 1;
                    latestMessages += "<tr><td><strong>Conversation Not Started.</strong> </td></tr>";
                }

            }
            $("#chatMsg").empty();
            //document.getElementById("chatMsg").innerHTML = latestMessages;

            if (latestMessages.length > 0) {

                var div = document.getElementById('chatMsg');
                div.innerHTML = latestMessages;
            }
            if (len > 0) {

                if (scroll)
                    $('#chat1').scrollTop($('#chat1')[0].scrollHeight);
            }

        } else {
            console.log(err);
        }

    });
}

function startChat() {
    $("#loading").show();
    var QuickbloxApp_Id = "59230";
    var QuickbloxAuth_Key = "SV2czdXSOafbMNm";
    var QuickbloxAuth_Secret = "pru2MGmJxj7zedX";
    QB.init(QuickbloxApp_Id, QuickbloxAuth_Key, QuickbloxAuth_Secret, true);

    //QB.createSession(function (err, result) {


    //    if (err) {
    //        alert(err.message);
    //    } else {

    //        sessionStorage.setItem("sToken", result.token);

    //        if (sFromQBId == '0') {

    //            RegisterNewUser(Email, useridLogin);
    //        }
    //    }
    //});

    QB.destroySession(function (error) {
        // callback function
    });

    var user = {
        id: sFromQBId,
        login: Email,
        password: "Welcome007!"
    };
    QB.createSession({ login: user.login, password: user.password }, function (err, result) {

        if (err) {
            alert(err.message);
        } else {

            sessionStorage.setItem("sToken", result.token);

            if (sFromQBId == '0') {

                RegisterNewUser(Email, useridLogin);
            }
            QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {

                if (err) {
                    //console.log('SERVER ERROR RESPONSE ');

                } else {
                    //console.log('SERVER SUCCESS RESPONSE ');
                    // var tmpDialogId = "5c6a7fd9d4594d0c0db4dc4f";

                    if (tmpDialogId == "0") {
                        CreateNewGroupDialod();
                    }
                    else {
                        //CreateSession();
                        loadChatMessages(100, 0);
                        joinChatRoom();

                        toastr.success("Now You Can Start Chatting.");
                        $("#loading").hide();
                    }
                }
            });
        }
    });

}

//Join Group Chat Start
function joinChatRoom() {
    dialogJid = QB.chat.helpers.getRoomJidFromDialogId(tmpDialogId);
    QB.chat.muc.join(dialogJid, function (resultStanza) {

        var joined = true;

        for (var i = 0; i < resultStanza.childNodes.length; i++) {
            var elItem = resultStanza.childNodes.item(i);
            if (elItem.tagName === 'error') {
                joined = false;
            }
        }
    });
}
//Join Group Chat End

// Create New Group Chat Dialog Start //

function CreateNewGroupDialod() {

    var user = {
        id: sFromQBId,
        login: Email,//"@Session["FromUserEmail"]",
        password: "Welcome007!"
    };
    QB.createSession({ login: user.login, password: user.password }, function (err, result) {

        if (err) {
            alert(err.message);
        } else {
            //alert("Session Token- " + result.token);
            sessionStorage.setItem("sToken", result.token);

            //var occupants = new Array(sToQBId, sFromQBId);
            var occupants = new Array();
            for (var v1 = 0; v1 < GroupMemberList.length ; v1++) {
                occupants.push(GroupMemberList[v1].QuickBloxId);
            }



            var dialogOccupants = {

                type: 2,
                occupants_ids: occupants,//[sFromQBId, sToQBId],
                name: "Chat_" + grpname,//"@ViewBag.GroupName",
                data: {
                    class_name: "ChatDialogType",
                    ChatCategory: "3"
                }
                //name: "Chat_" + sFromQBId + "_" + sToQBId
            };



            //Create New Group Chat Dialog
            QB.chat.dialog.create(dialogOccupants, function (err, createdDialog) {
                if (err) {
                    //console.log(err);
                    //console.log("DIALOG CREATED FAIL");
                } else {
                    //Add new QuickBlox Id For Specific User

                    var url = INITURL + '/Chatting/SaveDialogId';


                    $.ajax({
                        url: url,
                        data: {
                            ChattingGroupId: 4,//@ViewBag.GroupDialogId,
                            DialogId: createdDialog._id
                        },
                        cache: false,
                        type: 'POST',
                        async: false,
                        success: function (data) {
                            if (data == "Success") {
                                tmpDialogId = createdDialog._id; //creating the dialogID//
                            }
                            loadChatMessages(100, 0);
                            joinChatRoom();

                            toastr.success("Now You Can Start Chatting.");
                            $("#loading").hide();
                        },
                        error: function (reponse) {
                            alert("error : " + reponse);
                            $("#loading").hide();
                        }
                    });
                }
            });
        }
    });

}

function ChattingGroupMemberList() {
    //$.ajax({
    //    url: '/InTakechatting/RejectInTakePatient',
    //    async: false,
    //    type: 'POST',
    //    data: { "key": Selected_intakePatientKey },
    //    success: function (response) {


    //        if (response == "success") {
    //            alert("Patient Data Remove Successfully");
    //            CheckInTakePatientData();
    //        }
    //        else {
    //            alert("Patient Data Not Removed");
    //        }
    //    },
    //    error: function (xhr, status, error) {
    //        console.error('Error:', error);
    //    }
    //});

    GroupMemberList = JSON.parse(window.localStorage.getItem("grpmemberlist"));
    $("#lblGroupInfo").html(GroupMemberList.length + ' Members');
}
function playAudio() {
    var x = document.getElementById("myAudio");
    x.play();
}
function openMedia(url, type) {

    debugger;
    var media = '';
    $("#mediaDiv").css("max-height", "320px");
    $("#mediaDiv").css("z-index", "2000");
    $("#mediaDiv").html(media);
    if (type == 1) {
        media = "<img src='" + url + "' style='vertical-align:middle' width='90%' height='90%' alt='image'/>";
        $("#mediaDiv").css("max-height", "320px");
        $("#mediaDiv").html(media);
    }
    else if (type == 2) {
        media = "<video id='openVideo'  controls>" + "<source  src='" + url + "' type='video/mp4'>" + "<source src='" + url + "' type='video/ogg'>" + "</video>";
        $("#mediaDiv").css("height", "310px"); $("#mediaDiv").css("max-height", "400px");
        $("#mediaDiv").html(media);
    }
    else if (type == 3) {
        PDFObject.embed(url, "#mediaDiv");
        $("#mediaDiv").css("height", "810px");
        $("#mediaDiv").css("max-height", "810px");

        // window.open(url);

    }

}

//function removeParticipants() {
//    $.ajax({
//        url: "https://api.quickblox.com/chat/Dialog/683d9fa2e7affd001a0bc87e.json",
//        type: "PUT",
//        headers: {
//            "QB-Token": sessionStorage.getItem("sToken")
//        },
//        contentType: "application/json",
//        data: JSON.stringify({
//            occupants_ids: "32969860"  // IDs to KEEP
//        }),
//        success: function (response) {
//            console.log("Participants updated successfully:", response);
//        },
//        error: function (error) {
//            console.error("Error updating participants:", error);
//        }
//    });
//}


function removeDial() {
    var YOUR_APP_ID = "59230";
    var YOUR_AUTH_KEY = "SV2czdXSOafbMNm";
    var YOUR_AUTH_SECRET = "pru2MGmJxj7zedX";

    var userCredentials = {
        login: "superadmin@paseva.com",
        password: "Welcome007!"
    };

    QB.init(YOUR_APP_ID, YOUR_AUTH_KEY, YOUR_AUTH_SECRET);
    QB.createSession({ login: user.login, password: user.password }, function (err, session) {
        if (session) {
            console.log("QB-Token:", session.token);
            var token = session.token;  // Token is now available, use it immediately

            // Now make the DELETE request inside this callback
            $.ajax({
                url: "https://api.quickblox.com/chat/Dialog/683d9fa2e7affd001a0bc87e.json",
                type: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    "QB-Token": token
                },
                success: function (response) {
                    alert("Dialog deleted successfully!");
                },
                error: function (xhr, status, error) {
                    alert("Error deleting dialog: " + error);
                }
            });

        } else {
            alert("Session creation failed: " + err);
        }
    });
}

var strtblMemberDetail = "";
var strMemberList = "";
//function ShowAddedMembersInChatRoom() {
//    $("#tblMemberDetail").html('');
//    var MembersList = JSON.parse(window.localStorage.getItem("grpmemberlist"));
//    if (MembersList.length > 0) {
//        strMemberList += "<option selected>Select any one</option>";

//        strtblMemberDetail += "<td> <b>Member Name</b> </td>" +
//                            "<td><b> Email-Id </b></td>" +
//                            "<td> <b>Role Type</b> </td>" +
//                            "</tr></thead><tbody>";

//        for (var tb1 = 0; tb1 < MembersList.length ; tb1++) {
//            //var html = "";
//            //        html += "<span style='color: #1dca75;'> (Group Admin)</span>"

//            //    strtblMemberDetail += "<tr>" +
//            //                            "<td>" + MembersList[tb1].MemberName + "</td>" +
//            //                            "<td><a href='mailto:" + MembersList[tb1].Email + "' >" + MembersList[tb1].Email + "</a></td>" +
//            //                            "<td>" + (MembersList[tb1].Type == "NurseCoordinator" ? "Office Staff" : MembersList[tb1].Type) + html + "</td>" +
//            //                            "</tr>";
//            //    strMemberList += "<option value='" + MembersList[tb1].MemberName + "'>" + MembersList[tb1].MemberName + "</option>";
//            "<td> <b>Member Name</b> </td>" +
//                            "<td><b> Email-Id </b></td>" +
//                            "<td> <b>Role Type</b> </td>" +
//                            "</tr></thead><tbody>";
//            <tr><td>Ben Moss</td><td><a href="mailto:superadmin@paseva.com">superadmin@paseva.com</a></td><td>SuperAdmin</td></tr>

//        }
//       // strtblMemberDetail += "</tbody>" +
//        $("#tblMemberDetail").append(strtblMemberDetail);
//    }
//    else {
//        $("#tblMemberDetail").html("No Members are added!");
//    }

//}

function ShowAddedMembersInChatRoom() {
    $("#tblMemberDetail").html('');
    var MembersList = JSON.parse(window.localStorage.getItem("grpmemberlist"));

    if (MembersList && MembersList.length > 0) {
        var strtblMemberDetail = "<thead><tr>" +
            "<td><b>Member Name</b></td>" +
            "<td><b>Email-Id</b></td>" +
            "<td><b>Role Type</b></td>" +
            "</tr></thead><tbody>";

        for (var i = 0; i < MembersList.length; i++) {
            var member = MembersList[i];

            strtblMemberDetail += "<tr>" +
                "<td>" + member.MemberName + "</td>" +
                "<td><a href='mailto:" + member.Email + "'>" + member.Email + "</a></td>" +
                "<td>" + member.Type + "</td>" +
                "</tr>";
        }

        strtblMemberDetail += "</tbody>";
        $("#tblMemberDetail").append(strtblMemberDetail);
    } else {
        $("#tblMemberDetail").html("No Members are added!");
    }
}



//$(document).on('change', 'input#file1', function (event) {
//    ////1

//    var filePathsss = $(this).val();
//    var tmppath = URL.createObjectURL(event.target.files[0]);

//    debugger;
//    var totalSelectedFiles = $("input[type=file]")[0].files.length;
//    var totalFiles = $("input[type=file]")[0].files;
//    var totalFileType = "";

//    if (totalFiles != null) {
//        for (var l = 0; l < totalSelectedFiles; l++) {
//            File = totalFiles[l];
//            //console.log(File);
//            //console.log(temptotalSelectedFiles+" + "+File.name);
//            totalFileType = File.type.split('/');
//            if (totalFileType[0] != 'image' && totalFileType[0] != 'video' && totalFileType[0] != 'audio') {
//                totalFileType[0] = 'doc';
//            }

//            var ext = File.name.split('.').pop();
//            var Filesize = (File.size / (1024 * 1024)).toFixed(2);

//            if (File) {
//                if (Filesize < 95) {
//                    if (ext.toLowerCase() == "ppt"
//                        || ext.toLowerCase() == "pptx"
//                        || ext.toLowerCase() == "xls"
//                        || ext.toLowerCase() == "doc"
//                        || ext.toLowerCase() == "docx"
//                        || ext.toLowerCase() == "png"
//                        || ext.toLowerCase() == "jpg"
//                        || ext.toLowerCase() == "jpeg"
//                        || ext.toLowerCase() == "mp4"
//                        || ext.toLowerCase() == "pdf") {
//                        toastr.success("File Selected");
//                        $("#fileCall").css({ 'color': "deepskyblue" });
//                        $("#FileAlert").css({ 'display': "block" });
//                        $("#FileAlertContent").html("<b>" + (l + 1) + " File Selected.</b>");
//                        //Remove
//                        $("#fileRemove").css({ 'display': "block" });
//                        $("#fileCall").css({ 'display': "none" });
//                        //
//                        //$("#txtSendMsg").attr('disabled','true')

//                    }
//                    else {
//                        toastr.info("You Choose Wrong format file... Please Select File having following extension .ppt, .pptx,  .xls, .doc, .docx, .png, .jpg, .jpeg, .mp4, .pdf");
//                        $("#file1").val('');
//                        $("#fileCall").css({ 'color': "" });
//                        $("#FileAlert").css({ 'display': "none" });

//                        $("#fileRemove").css({ 'display': "none" });
//                        $("#fileCall").css({ 'display': "block" });
//                        toastr.info("No File Selected... Please Open again for select a file.");
//                        $("#txtSendMsg").removeAttr('disabled')
//                    }

//                } else {
//                    if (Filesize > 95) {
//                        toastr.info("Please Select a file having size less then 95MB.");
//                        $("#file1").val('');
//                        $("#fileCall").css({ 'color': "" });
//                        $("#FileAlert").css({ 'display': "none" });

//                        $("#fileRemove").css({ 'display': "none" });
//                        $("#fileCall").css({ 'display': "block" });
//                        toastr.info("No File Selected... Please Open again for select a file.");
//                        $("#txtSendMsg").removeAttr('disabled')
//                    }

//                }
//            }
//            else {
//                toastr.info("No File Selected... Please Open again for select a file.");
//            }

//        }//--For looop end
//    }


//});

function openMedia(url, type) {
    $("#myModalOpenMedia").modal('hide');
    debugger;
    var media = '';
    $("#mediaDiv").css("max-height", "320px");
    $("#mediaDiv").html(media);
    if (type == 1) {
        media = "<img src='" + url + "' style='vertical-align:middle' width='90%' height='90%' alt='image'/>";
        $("#mediaDiv").css("max-height", "320px");
        $("#mediaDiv").html(media);
    }
    else if (type == 2) {
        media = "<video id='openVideo'  controls>" + "<source  src='" + url + "' type='video/mp4'>" + "<source src='" + url + "' type='video/ogg'>" + "</video>";
        $("#mediaDiv").css("height", "310px"); $("#mediaDiv").css("max-height", "400px");
        $("#mediaDiv").html(media);
    }
    else if (type == 3) {
        //PDFObject.embed(url, "#mediaDiv");
        //$("#mediaDiv").css("height", "810px");
        //$("#mediaDiv").css("max-height", "810px");

        window.open(url);

    }

}

function BackToInTakePage(event) {
    event.preventDefault();
    window.localStorage.setItem("BackBtnToIntakeChatTab", "1");
    // window.location.href = '/InTake/InTakePatientDetails';
    window.history.back();
}