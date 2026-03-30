$(document).ready(function () {

    LoadingUserDetails();
    //QUICK Blox//
    var QuickbloxApp_Id = "59230";
    var QuickbloxAuth_Key = "SV2czdXSOafbMNm";
    var QuickbloxAuth_Secret = "pru2MGmJxj7zedX";
    QB.init(QuickbloxApp_Id, QuickbloxAuth_Key, QuickbloxAuth_Secret, true);
    // createGlobalQBSession();
    $("#IntakeChatDiv1").hide();
    $("#Chat_ModlPage").hide();
    createGlobalQBSession()
       .then(function () {
           $('#loading').show();
           // ✅ All of this runs only after session is fully active

           $("#specialityTypeDiv").hide();
           $("#IntakePatientDiv_nodata").hide();

           $("#IntakeChtPatCount").hide();
           //  CheckInTakePatientData();
           $("#visitType").bind("click", OpenInTakePatientChat);//visitTypeDiv
           $("#Specilatily").bind("click", CheckInTakePatientData);
           AppenedOfficeList();
           $('#memberSelect').select2({
               placeholder: "Select Members"
           });

           document.getElementById("Fname").addEventListener("input", function () {
               this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
           });
           document.getElementById("Lname").addEventListener("input", function () {
               this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
           });
           //
           document.addEventListener("DOMContentLoaded", function () {
               const input = document.getElementById("ReferredBy");

               input.addEventListener("input", function () {
                   // Allow letters, spaces, and hyphens only
                   this.value = this.value.replace(/[^a-zA-Z\s-]/g, '');

                   // Enforce max 50 characters (extra safety)
                   if (this.value.length > 50) {
                       this.value = this.value.substring(0, 50);
                   }
               });
           });
           //

           document.getElementById('cityid').addEventListener('input', function (event) {
               const input = event.target;
               let value = input.value;

               // 1. Normalize the string to NFC (Composed Form)
               // This converts characters like 'e' + '´' into a single 'é' character
               value = value.normalize('NFC');

               // 2. Apply your existing regex to clean out any remaining disallowed characters.
               // The \u00C0-\u017F range is usually sufficient for most Western European accented characters
               // after normalization.
               const regex = /[^a-zA-Z\s-\u00C0-\u017F]/g;
               const cleanedValue = value.replace(regex, '');

               // Update the input field's value with the cleaned string
               input.value = cleanedValue;
           });

           document.getElementById('stateid').addEventListener('input', function (event) {
               const input = event.target;
               // This regex keeps only allowed characters: a-z, A-Z, spaces, hyphens
               // The \u00C0-\u017F range is for common accented characters like é, ü, ñ
               input.value = input.value.replace(/[^a-zA-Z\s-\u00C0-\u017F]/g, '');
           });

           //document.getElementById("zipcodeid").addEventListener("input", function () {
           //    // Allow only digits and one optional hyphen after 5 digits
           //    this.value = this.value.replace(/[^0-9\-]/g, '')               // remove invalid chars
           //                           .replace(/(\-)+/g, '-')                 // allow only one hyphen
           //                           .replace(/^(.{5})\-?(.*)$/, (_, a, b) => a + (b ? '-' + b.replace(/[^0-9]/g, '') : ''));

           //    // Limit to format: 12345 or 12345-6789
           //    if (!/^\d{0,5}(-\d{0,4})?$/.test(this.value)) {
           //        this.value = this.value.slice(0, 10);
           //    }
           //});
           $("#PatientID").keyup(CheckPatientValidID);
           $("#UPatID").keyup(UpdateCheckPatientValidID);

           /////////////////
           document.getElementById("UFname").addEventListener("input", function () {
               this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
           });
           document.getElementById("ULname").addEventListener("input", function () {
               this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
           });

           document.getElementById('UCity').addEventListener('input', function (event) {
               const input = event.target;
               let value = input.value;
               value = value.normalize('NFC');
               const regex = /[^a-zA-Z\s-\u00C0-\u017F]/g;
               const cleanedValue = value.replace(regex, '');
               input.value = cleanedValue;
           });
           document.getElementById('UState').addEventListener('input', function (event) {
               const input = event.target;
               // This regex keeps only allowed characters: a-z, A-Z, spaces, hyphens
               // The \u00C0-\u017F range is for common accented characters like é, ü, ñ
               input.value = input.value.replace(/[^a-zA-Z\s-\u00C0-\u017F]/g, '');
           });
           //document.getElementById("UZipcode").addEventListener("input", function () {
           //    // Allow only digits and one optional hyphen after 5 digits
           //    this.value = this.value.replace(/[^0-9\-]/g, '')               // remove invalid chars
           //                           .replace(/(\-)+/g, '-')                 // allow only one hyphen
           //                           .replace(/^(.{5})\-?(.*)$/, (_, a, b) => a + (b ? '-' + b.replace(/[^0-9]/g, '') : ''));

           //    // Limit to format: 12345 or 12345-6789
           //    if (!/^\d{0,5}(-\d{0,4})?$/.test(this.value)) {
           //        this.value = this.value.slice(0, 10);
           //    }
           //});
           //createQBSession();
           //Added by sumit jha // start
           //const dropdownContainer = $('#dropdownContainer');
           //const dropdownMenu = $('#dropdownMenu');
           //const selectedTags = $('#selectedTags');
           //let tempSelected = [];


           //dropdownContainer.click(function (e) {
           //    e.stopPropagation();
           //    dropdownMenu.toggle();
           //    $('#searchBox').focus();


           //    tempSelected = $('.options-list input:checked').map(function () {
           //        return $(this).val();
           //        const userName = $(this).parent().text().trim();
           //        tempSelected.push({ userId, userName });
           //    }).get();
           //});


           //$(document).click(function () {
           //    dropdownMenu.hide();
           //});

           //dropdownMenu.click(function (e) {
           //    e.stopPropagation();
           //});


           //$('#searchBox').on('keyup', function () {
           //    const val = $(this).val().toLowerCase();
           //    $('.options-list label').each(function () {
           //        const text = $(this).text().toLowerCase();
           //        $(this).toggle(text.includes(val));
           //    });
           //});


           //$('#clearFilter').click(function () {
           //    $('#searchBox').val('').trigger('keyup');
           //    $('.options-list input[type="checkbox"]').prop('checked', false);
           //    updateTags();
           //});


           //$('#saveBtn').click(function () {
           //    $('#dropdownArrow').removeClass('fa-chevron-up').addClass('fa-chevron-down');
           //    $('#searchBox').val('').trigger('input');
           //    dropdownMenu.hide();
           //    updateTags();

           //});


           //$('#cancelBtn').click(function () {
           //    dropdownMenu.hide();
           //    $('.options-list input[type="checkbox"]').prop('checked', false);
           //    updateTags();
           //});
           //selectedTags.on('click', '.remove', function () {
           //    const name = $(this).parent().data('name');
           //    $(`.options-list input[value="${name}"]`).prop('checked', false);
           //    updateTags();
           //});
           // updateTags();
           //End Added by sumit jha//
           if (window.localStorage.getItem("BackBtnToIntakeChatTab") == null || window.localStorage.getItem("BackBtnToIntakeChatTab") == "") {
               CheckInTakePatientData();

           }
           else if (window.localStorage.getItem("BackBtnToIntakeChatTab") == "1") {
               OpenInTakePatientChat();
           }
           // $('#loading').hide();
       })
       .catch(function (err) {
           toastr.error("Failed to create QB session ,Please refresh", err);
           //alert("Failed to initialize chat. Please refresh.");
       });




    // createQBSession();

});

var attachmentArray = [];
var sFromQBId;
var Email;
var useridLogin = "";
var tmpDialogId = "";
var GroupMemberList = [];
var tmpOfficeArr = [];
var lastInsertedRowId = "";
function LoadingUserDetails() {
    sFromQBId = $("#InTake_QBID").val();
    Email = $("#InTake_Email").val();
    useridLogin = $("#InTake_LoginUserId").val();
}

function AddInTakePatientDetails() {

    if ($("#PatientID").val() == "" || $("#Fname").val() == "" || $("#Lname").val() == "" || $("#primaryMd").val() == "" || $("#phoneNo").val() == "" || $("#officeId").val() == "0" || $("#streetid").val() == "" || $("#stateid").val() == "" || $("#cityid").val() == "" || $("#zipcodeid").val() == "" || $("#Address").val() == "") {
        toastr.error('Please fill mandatory fields');
        return false;
    }

    if (PatientValidID == 1) {
        toastr.error("PatientID exist,enter another ID");
        return false;
    }
    if ($("#PatientID").val().length < 10) {
        toastr.error("Invalid Patientid!");
        return false;
    }
    if (PatientValidID == 2) {
        toastr.error("Invalid Patientid!");
        return false;
    }
    if ($("#phoneNo").val().length < 10) {
        toastr.error("Invalid PhoneNo. !");
        return false;
    }
    if ($("#zipcodeid").val() < 5) {
        toastr.error("Invalid zipcode.!");
        return false;
    }
    if (!GetLanLat()) {
        $('#loading').hide();
        return false;
    }
    $('#loading').show();
    $.ajax({
        url: '/InTake/AddPatientDetails',
        type: 'POST',
        data: JSON.stringify({
            InTake_PatientId: $("#PatientID").val(),
            FirstName: $("#Fname").val(),
            LastName: $("#Lname").val(),
            PhoneNo: $("#phoneNo").val(),
            OfficeId: $("#officeId").val(),
            street: $("#streetid").val(),
            City: $("#cityid").val(),
            State: $("#stateid").val(),
            ZipCode: $("#zipcodeid").val(),
            ReferredBy: $("#ReferredBy").val(),
            jurisdictioncode: $("#jurisdictioncode").val(),
            PayerId: $("#PayerId").val(),
            procedurecode: $("#procedurecode").val(),
            DateOfBirth: $("#DateOfBirth").val(),
            Address: $("#Address").val(),
            payerprogram: $("#payerprogramid").val(),
            Latitide: $("#Latitude").val(),
            Longitude: $("#Longitude").val(),
            TimeZoneId: $("#hdntimeZoneId").val(),
            TimezonePostfix: $("#hdnTimezonePostfix").val(),
            TimezoneOffset: $("#hdnTimezoneOffset").val(),
            primarymd: $("#primaryMd").val()
        }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {

            if (response != "failed") {
                // $('#ServiceType_Add_Modal').modal('hide');
                lastInsertedRowId = response;
                toastr.success("Patient added successfully");
                SavingDataIntoChattingGrouptbl();
                //setTimeout(function () {
                //    CheckInTakePatientData();
                //}, 2000);


            } else {
                toastr.error("Failed to add patient");
                // $('#ServiceType_Add_Modal').modal('hide');
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
            alert("Server error occurred: " + error);
        }
    });

    $('#ServiceType_Add_Modal').modal('hide');
}


function CheckInTakePatientData() {
    $("#IntakeChatDiv1").show();
    $("#TotalPatientCount").html('');
    $("#visitTypeDiv").hide();
    $(".InTakePatientDiv tbody").html('');
    $("#visitType").removeClass('activeDiv');
    $("#Specilatily").addClass('activeDiv');
    $("#loading").show();
    $("#Chat_ModlPage").hide();
    $.ajax({
        url: '/InTake/CheckInTakePatientData',
        async: false,
        type: 'GET',
        data: {},
        success: function (response) {

            var data = JSON.parse(response);
            $("#TotalPatientCount").html("Total Patient (" + data.length + ")");
            if (data.length == 0) {
                $(".InTakePatientDiv").html("No Record Found!");
                $("#specialityTypeDiv").hide();
                $("#IntakePatientDiv_nodata").show();
                $("#loading").hide();
            }
            else {
                $("#specialityTypeDiv").show();
                $("#IntakePatientDiv_nodata").hide();
                $(".InTakePatientDiv").html('<table class="table-container"><thead></thead><tbody></tbody></table>');
                $(".InTakePatientDiv thead").append(`<tr class="theadtr"><th>Patient Name</th><th>Patient ID</th><th>Mobile No</th><th>Office</th><th>Referred by</th><th>Actions</th></tr>`);

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    var row = `
                    <tr role="row"  style="cursor: pointer;" onclick="FetchIntakePatientDetail(${item.IntakePatientId})">
                        <td>${item.PatientName}</td>
                        <td>${item.MedicalId}</td>
                        <td>${item.PhoneNo}</td>
                        <td>${item.OfficeId}</td>
                        <td>${item.ReferredBy}</td>
                        <td>
                            <a href="javascript:;" onclick="event.stopPropagation(); ConfirmDelete(${item.IntakePatientId},'${item.DialogId}')" title="Reject" style="padding: 10px 25px; border: 1px solid #EB5757; background: #FEE6E6; color: red; border-radius: 20px;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="5" height="6" viewBox="0 0 5 6" fill="none">
                                    <circle cx="2.5" cy="3" r="2.5" fill="#EB5757" />
                                </svg> Reject
                            </a>
                            <a href="javascript:;" onclick="event.stopPropagation(); ConfirmAccept(${item.IntakePatientId},'${item.DialogId}','${item.ChattingGrpOfficeId}')" title="Accept" style="margin-left:30px; padding: 10px 25px; color: #239450; border: 1px solid #239450; background: #D4FFE8; border-radius: 20px;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="5" height="6" viewBox="0 0 5 6" fill="none"> //AcceptInTakePatient
                                    <circle cx="2.5" cy="3" r="2.5" fill="#239450" />
                                </svg> Accept
                            </a>
                            <a style="float: inline-end; margin-right: 20px;">
                                <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                </svg>
                            </a>
                        </td>
                    </tr>
                `;
                    $(".InTakePatientDiv tbody").append(row);
                }
                $("#loading").hide();
            }

        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            $("#loading").hide();
        }
    });

}

//function AcceptInTakePatient(key) {
//    $.ajax({
//        url: '/InTake/RejectInTakePatient',
//        async:false,
//        type: 'DELETE',
//        data: {},
//        success: function (response) {

//            var data = JSON.parse(response);
//            if (data == "success") {
//                alert("Patient Data Remove Successfully");
//               // CheckInTakePatientData();
//            }
//            else {
//                alert("Patient Data Not Removed");
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error('Error:', error);
//        }
//    });

//}

function RejectInTakePatient() {
    $('#loading').show();
    $.ajax({
        url: '/InTake/RejectInTakePatient',
        async: false,
        type: 'POST',
        data: { "key": Selected_intakePatientKey },
        success: function (response) {


            if (response == "success") {
                //alert("Patient Data Remove Successfully");
                removeDial(InTakePatDialogId);
                setTimeout(function () {
                    CheckInTakePatientData();
                    $('#loading').hide();
                }, 2000);

            }
            else {
                toastr.error("Patient Data Not Removed");
                $('#loading').hide();
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });

    $('#Delete_Modal1').modal('hide');
}

var Selected_intakePatientKey = "";
var InTakePatDialogId = "";
function ConfirmDelete(IntakeDelkey, DialogId) {
    Selected_intakePatientKey = IntakeDelkey;
    $('#Delete_Modal1').modal('show');
    InTakePatDialogId = DialogId;

}
var Selected_DialID = "";
var Selected_chattingGrpOfficeID = "";
function ConfirmAccept(IntakeAcceptkey, DialID, chattingGrpOfficeID) {
    Selected_intakePatientKey = IntakeAcceptkey;
    Selected_DialID = DialID;
    Selected_chattingGrpOfficeID = chattingGrpOfficeID;
    $('#Delete_Modal').modal('show');

}

//function AcceptInTakePatient() {




//    $.ajax({
//        url: '/InTake/Transfer_Intake_PatientDetails',
//        async: false,
//        type: 'POST',
//        data: { "IntakePatientID": Selected_intakePatientKey },   //IntakeAcceptkey
//        success: function (response) {
//            if (response != null) {

//                var dialogId = response;

//                var toUpdateParams = {            
//                    data: JSON.stringify({ category: 2 })
//                };


//                var user = {
//                    id: sFromQBId,
//                    login: Email,//"Superadmin@PaSeva.com",
//                    password: "Welcome007!"
//                };
//                QB.createSession({ login: user.login, password: user.password }, function (err, result) {

//                    if (err) {
//                        alert(err.message);
//                    } else {

//                        QB.dialog.update(dialogId, toUpdateParams, function (error, dialog) {

//                            if (err) {
//                                alert(err.message);
//                            } else {
//                                toastr.success("Patient Data transfer Successfully");
//                            }

//                        })
//                    }
//                });




//                toastr.success("Patient Data transfer Successfully");
//                $('#Delete_Modal').modal('hide');
//                CheckInTakePatientData();
//            }
//            else {
//                toastr.error("Patient Data Not transferred");
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error('Error:', error);
//        }
//    });
//    $('#Delete_Modal').modal('hide');
//}

function AcceptInTakePatient() {

    QB.chat.dialog.update(Selected_DialID, {   // this line of code used for update the "ChatCategory"  5 to 2
        data: {
            "ChatCategory": "2",
            "OfficeID": Selected_chattingGrpOfficeID,
            "class_name": "ChatDialogType"
        }
    }, function (error, updatedDialog) {
        if (error) {
            //  console.error("Dialog update failed:", error);
        } else {
            // console.log("Dialog updated successfully:", updatedDialog);
            $('#loading').show();
            $.ajax({
                url: '/InTake/Transfer_Intake_PatientDetails',
                async: false,
                type: 'POST',
                data: { "IntakePatientID": Selected_intakePatientKey },   //IntakeAcceptkey
                success: function (response) {
                    if (response == "success") {
                        toastr.success("Patient Data transfer Successfully");
                        $('#Delete_Modal').modal('hide');
                        CheckInTakePatientData();
                        $('#loading').hide();
                    }
                    else {
                        toastr.error("Patient Data Not transferred");
                        $('#loading').hide();
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
            $('#Delete_Modal').modal('hide');
        }
    });


}
function OpenInTakePatientChat() {
    // $("#IntakeChatDiv1").show();
    $("#specialityTypeDiv").hide();
    $("#IntakePatientDiv_nodata").hide();
    $("#visitTypeDiv").show();
    $("#visitType").addClass('activeDiv');
    $("#Specilatily").removeClass('activeDiv');
    window.localStorage.removeItem("BackBtnToIntakeChatTab");
    // IntakePatientChatList();
    InTakePatientChatListFrmChantMemberTbl();

}

function OpenService_AddPopup() {

    $("#PatientID").val('');
    $("#Fname").val('');
    $("#Lname").val('');
    $("#primarymd").val('');
    $("#phoneNo").val('');
    $("#officeId").val('0');

    $("#streetid").val('');
    $("#cityid").val('');
    $("#stateid").val('');
    $("#zipcodeid").val('');
    $("#ReferredBy").val('');
    $("#jurisdictioncode").val('');
    $("#PayerId").val('');
    $("#procedurecode").val('');
    $("#DateOfBirth").val('');
    $("#Address").val('');
    $("#payerprogramid").val('');
    $("#primaryMd").val('');
    $('#loading').show();
    $('#ServiceType_Add_Modal').modal('show');
    //AppenedOfficeList();
    $('#loading').hide();
}
function FetchIntakePatientDetail(intakePatId) {  // fetchIntakePatientDetail //
    $('#loading').show();
    $("#intakepatientid").val(intakePatId);
    setTimeout(function () {
        $.ajax({
            url: '/InTake/FetchIntakePatientDetail',
            // async: false,
            type: 'POST',
            data: { "IntakePatientID": intakePatId },
            success: function (response) {
                var data = JSON.parse(response);

                var result = $.grep(tmpOfficeArr, function (item) {
                    return item.officeId == data[0].OfficeId; // Match the desired officeId
                });
                $("#EPName").html(data[0].PatientName);
                $("#EPatID").html(data[0].MedicalId);
                $("#EDOB").html(data[0].DateOfBirth);
                $("#Epnone").html(data[0].PhoneNo);
                $("#Eoffice").html(result[0].officeName);
                $("#EPrimarymd").html(data[0].primarymd);
                $("#ERef").html(data[0].ReferredBy);
                $("#EAddress").html(data[0].Address);
                $("#Estate").html(data[0].State);
                $("#EStreet").html(data[0].Street);
                $("#Ecity").html(data[0].City);
                $("#Ezipcode").html(data[0].ZipCode);
                $("#Epayerid").html(data[0].PayerId);
                $("#Ejuridiction").html(data[0].JurisdictionCode);
                $("#EpayerProg").html(data[0].PayerProgram);
                $("#EProcCode").html(data[0].ProcedureCode);  //
                // $("#EProcCode").html(data[0].Street);
                $('#loading').hide();
                $('#AppointmentDetailModal').modal('show');
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                // $('#loading').hide();
                toastr.error("Failed to fetch patient details");

            }
        });
    }, 100);

}
function editdetais() {

    var result = $.grep(tmpOfficeArr, function (item) {
        return item.officeName == $("#Eoffice").html(); // Match the desired officeId
    });
    var patientname = $("#EPName").html().split(" ");
    $("#UDateOfBirth").val($("#EDOB").html());
    $("#UPhone").val($("#Epnone").html());
    $("#URef").val($("#ERef").html());
    $("#UState").val($("#Estate").html());
    $("#UJurisdiction").val($("#Ejuridiction").html());
    $("#UPayerID").val($("#Epayerid").html());
    $("#UProc").val($("#EProcCode").html());
    $("#UFname").val(patientname[0]);
    $("#ULname").val(patientname[1]);
    // $("#Uofficeid").val($("#Eoffice").html());
    $("#UPatID").val($("#EPatID").html());
    $("#Uaddress").val($("#EAddress").html());
    $("#UCity").val($("#Ecity").html());
    $("#UZipcode").val($("#Ezipcode").html());
    $("#UPayerProg").val($("#EpayerProg").html());
    $("#UStreet").val($("#EStreet").html());
    $("#UPrimaryMD").val($("#EPrimarymd").html());
    for (var i = 0; i < tmpOfficeArr.length; i++) {
        $("#Uofficeid").append("<option value='" + tmpOfficeArr[i].officeId + "'>" + tmpOfficeArr[i].officeName + "</option>");
    }
    $("#Uofficeid").val(result[0].officeId);

    $('#EditDetailModal').modal('show');
}

function UpdateIntakePatDetails() {

    if ($("#UFname").val() == "" || $("#ULname").val() == "" || $("#UPhone").val() == "" || $("#Uofficeid").val() == "" || $("#UStreet").val() == "" || $("#UCity").val() == "" || $("#UState").val() == "" || $("#UPrimaryMD").val() == "") {
        toastr.error("Please enter data into mandatory fields!");
        return false;
    }
    if (UPatientValidID == 1) {
        toastr.error("PatientID exist,enter another ID");
        return false;
    }
    if ($("#UPatID").val().length < 10) {
        toastr.error("Invalid Patientid!");
        return false;
    }
    if ($("#UZipcode").val().length < 5) {
        toastr.error("Invalid zipcode!");
        return false;
    }
    if (UPatientValidID == 2) {
        toastr.error("Invalid Patientid!");
        return false;
    }
    if ($("#UPhone").val().length < 10) {
        toastr.error("Invalid PhoneNo. !");
        return false;
    }
    if (!UGetLanLat()) {
        $('#loading').hide();
        return false;
    }
    $('#loading').show();
    setTimeout(function () {
        $.ajax({
            url: '/InTake/updatePatientDetails',
            // async: false,
            type: 'POST',
            // data: { "IntakePatientID": $("#intakepatientid").val() },
            data: JSON.stringify({
                IntakePatientID: $("#intakepatientid").val(),
                PatMedicalID: $("#UPatID").val(),   //PatientId
                FirstName: $("#UFname").val(),
                LastName: $("#ULname").val(),
                PhoneNo: $("#UPhone").val(),
                OfficeId: $("#Uofficeid").val(),
                street: $("#UStreet").val(),
                City: $("#UCity").val(),
                State: $("#UState").val(),
                ZipCode: $("#UZipcode").val(),
                ReferredBy: $("#URef").val(), //
                jurisdictioncode: $("#UJurisdiction").val(), // 
                PayerId: $("#UPayerID").val(),
                procedurecode: $("#UProc").val(),
                DateOfBirth: $("#UDateOfBirth").val(),
                // Address: $("#Uaddress").val(),
                payerprogram: $("#UPayerProg").val(),
                primarymd: $("#UPrimaryMD").val(),
                Latitide: $("#Latitude").val(),
                Longitude: $("#Longitude").val(),
                TimeZoneId: $("#hdntimeZoneId").val(),
                TimezonePostfix: $("#hdnTimezonePostfix").val(),
                TimezoneOffset: $("#hdnTimezoneOffset").val()
            }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response == "success") {
                    toastr.success("Patient Data Updated!");
                    $('#loading').hide();
                    $('#EditDetailModal').modal('hide');
                    $('#AppointmentDetailModal').modal('hide');
                    CheckInTakePatientData();
                }
                else {
                    toastr.error("Patient Data Not Updated!");
                }
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                $('#loading').hide();
            }
        });
    }, 100);
}


/*function IntakePatientChatList() {
    $.ajax({
        url: '/InTake/CheckInTakePatientData',
        async: false,
        type: 'GET',
        success: function (response) {
            var data = JSON.parse(response);

            // Clear previous content
            $("#IntakePatientChatTbl thead").empty();
            $("#IntakePatientChatTbl tbody").empty();

            if (data.length === 0) {
                // Optionally show "No records" row or message
            } else {
                var head = `
                    <tr class="theadtr">
                        <th>Patient Name</th>
                        <th>Office</th>
                        <th>Chat</th>
                        <th></th>
                    </tr>`;
                $("#IntakePatientChatTbl thead").append(head);

                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    var row = `
                        <tr role="row" class="odd" onclick="window.location = '/InTake/IntakePatientChatPage'" style="cursor: pointer;">
                            <td>${item.PatientName}</td>
                            <td>${item.OfficeId}</td>
                            <td><button class="countbtn">20</button></td>
                            <td>
                                <button style="color: #1B4066; font-family: 'Open Sans'; font-size: 12px; font-weight: 600; border-radius: 6px; border: 1px solid #1B4066; background: #FFF;" id="gridSystemModalLabel">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                    $("#IntakePatientChatTbl tbody").append(row);
                }
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
} */

/*function InTakePatientChatListFrmChantMemberTbl() {  //6479
    const officeIds = tmpOfficeArr.map(o => String(o.officeId));  //Sample data ['1','2'.'3']
 
        var filters = {
            limit: 1000,
            skip: 0,
            sort_desc: "last_message_date_sent",
            data: {
                class_name: "ChatDialogType",
                ChatCategory: "4",
                "OfficeID[$in]": officeIds   //['20']  // ✅ Use this syntax for `$in`
            }
        };
    QB.chat.dialog.list(filters, function (error, response) {
        if (error) {
           // alert("QB API error:", error);
        } else {
            var dialogs = response.items
            var patientgroupDialogIds = "";
            for (i = 0; i < dialogs.length ; i++) {
                debugger;
                if (i != 99)
                    patientgroupDialogIds += dialogs[i]._id + ","
                else
                    patientgroupDialogIds += dialogs[i]._id
            }
            
            $("#loading").show();
            $.ajax({
                url: '/InTake/InTakePatientChatListFrmChantMemberTbl',
                async: false,
                type: 'GET',
                success: function (response) {
                    var data = JSON.parse(response);
                    AppenedOffices();
                    $("#IntakeChtPatCount").html("Total Patient (" + data.length + ")");
                    // Clear previous content
                    $("#IntakePatientChatTbl thead").empty();
                    $("#IntakePatientChatTbl tbody").empty();

                    if (data.length === 0) {
                        // Optionally show "No records" row or message
                        $("#IntakeChtPatCount").hide();
                        $("#Chat_ModlPage").show();
                    } else {
                        $("#Chat_ModlPage").hide();
                        $("#IntakeChtPatCount").show();
                        var head = `
                    <tr class="theadtr">
                        <th>Patient Name</th>
                        <th>Office</th>
                        <th>Chat</th>
                        <th></th>
                    </tr>`;
                        $("#IntakePatientChatTbl thead").append(head);
                        for (var i = 0; i < data.length; i++) {

                            var matchedOffice = GetOfficesList.filter(function (item) {
                                return item.officeId === data[i].OfficeId;
                            });

                            var item = data[i];
                            var row = `
                        <tr role="row" class ="odd" style="cursor: pointer;" onclick="window.location='/InTakechatting/InTakePatientChatDetailPage?Id=${item.ChattingGroupId}'">
                            <td onclick="window.location='/InTakechatting/InTakePatientChatDetailPage?Id=${item.ChattingGroupId}'">${item.GroupName}</td>
                            <td onclick="window.location='/InTakechatting/InTakePatientChatDetailPage?Id=${item.ChattingGroupId}'">${matchedOffice[0].officeName}</td>
                            <td onclick="window.location='/InTakechatting/InTakePatientChatDetailPage?Id=${item.ChattingGroupId}'"><button class ="countbtn">1</button></td>
                            <td>
                                <button style="color: #1B4066;font-family: 'Open Sans';font-size: 12px;font-weight: 600;border-radius: 6px;border: 1px solid #1B4066;background: #FFF;padding: 7px;" class ="add-member-btn" onclick="event.stopPropagation(); Confirmsave('Patient name : ${item.GroupName}','${item.ChattingGroupId}')">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                            $("#IntakePatientChatTbl tbody").append(row);
                        }
                    }
                    $("#loading").hide();
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                    $("#loading").hide();
                }
            });
        }
    });

   
} */
var gbl_UnreadMsgCount = "";
function InTakePatientChatListFrmChantMemberTbl() {  //6479
    gbl_UnreadMsgCount = 0;
    $("#loading").show();
    const officeIds = tmpOfficeArr.map(o => String(o.officeId));  //Sample data ['1','2'.'3']

    var filters = {
        limit: 1000,
        skip: 0,
        sort_desc: 'created_at', //"last_message_date_sent",
        data: {
            class_name: "ChatDialogType",
            ChatCategory: "5",
            "OfficeID[$in]": officeIds
        }
    };

    var user = {
        id: sFromQBId,//,
        login: Email,//,
        password: "Welcome007!"
    };
    // $('#loading').show();
    //QB.createSession({ login: user.login, password: user.password }, function (err, session) {
    //if (err) {
    //    console.error("QB session error:", err);
    //    // alert(err.message);
    //    $('#loading').hide();
    //} else {


    //    // connectQBChat();
    //}
    //});

    //qbSessionToken = session.token;
    //console.log("QB session created:", qbSessionToken);
    // $('#loading').hide();

    QB.chat.dialog.list(filters, function (error, response) {
        if (error) {
            $("#loading").hide();
            toastr.error("Failed to load data from the QuickBlox APIs!");
            // alert("QB API error:", error);
        } else {
            if (response.items.length == 0) {
                $("#IntakeChatDiv1").show();
                $('#loading').hide();
                $("#IntakeChtPatCount").hide();
                $("#IntakePatientChatTbl").hide();
                $("#Chat_ModlPage").show();
                return false;
            }
            var dialogs = response.items
            var patientgroupDialogIds = "";
            for (i = 0; i < dialogs.length ; i++) {
                if (i != 99)
                    patientgroupDialogIds += dialogs[i]._id + ","
                else
                    patientgroupDialogIds += dialogs[i]._id
            }


            $.ajax({
                url: '/InTake/Get_IntakePatientRoomGroupList',
                data: { QBGroupDialogIds: patientgroupDialogIds, GroupType: 1 },
                async: false,
                type: 'POST',
                success: function (response) {
                    if (response.length === 0) {
                        // Optionally show "No records" row or message
                        $("#IntakeChatDiv1").show();  //6479
                        $("#IntakeChtPatCount").hide();
                        $("#Chat_ModlPage").show();
                        $("#IntakePatientChatTbl").hide();
                        $("#loading").hide();
                    } else {
                        var data = response;// JSON.parse(response);
                        var dataSource = data;
                        //debugger;
                        for (var d = 0; d < dialogs.length; d++) {
                            var dt;
                            dt = dialogs[d].last_message_date_sent;
                            count = dialogs[d].unread_messages_count;
                            try {
                                for (var x = 0; x < dataSource.length; x++) {
                                    if (dialogs[d]._id == dataSource[x].DialogId) {
                                        dataSource[x]["Time"] = dt;
                                        dataSource[x]["unreadMsgCount"] = count;
                                    }
                                }
                            } catch (e) { }
                        }


                        var sortData = dataSource;    //dataSource.sort(function (a, b) { return b.Time - a.Time });
                        var groupChat = [];
                        groupChat = dataSource;
                        //groupChat = $.merge($.merge([], groupChat), sortData);
                        //groupChat = groupChat.sort(function (a, b) { return b.Time - a.Time });
                        //groupChat = groupChat.sort(function (a, b) { return b.Time - a.Time });
                        // AppenedOffices();
                        $("#IntakeChtPatCount").html("Total Patient (" + data.length + ")");
                        // Clear previous content
                        $("#IntakePatientChatTbl thead").empty();
                        $("#IntakePatientChatTbl tbody").empty();

                        $("#Chat_ModlPage").hide();
                        $("#IntakeChtPatCount").show();
                        $("#IntakePatientChatTbl").show();
                        var head = `
                    <tr class="theadtr">
                        <th>Patient Name</th>
                        <th>Office</th>
                        <th>Chat</th>
                        <th></th>
                    </tr>`;
                        $("#IntakePatientChatTbl thead").append(head);

                        for (var tb1 = 0; tb1 < groupChat.length; tb1++) {
                            var unReadHtml = "";


                            //if (groupChat[tb1].unreadMsgCount > 0) {
                            //    unReadHtml = groupChat[tb1].unreadMsgCount;//"<span class='label label-danger unReadCount'>" + groupChat[tb1].unreadMsgCount + "</span>";
                            //}

                            // var item = data[i];
                            if (groupChat[tb1].unreadMsgCount == 0) {
                                var row = `
                            <tr role="row" class ="odd" style="cursor: pointer;" onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].GroupName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].OfficeName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td>
                                <button style="color: #1B4066;font-family: 'Open Sans';font-size: 12px;font-weight: 600;border-radius: 6px;border: 1px solid #1B4066;background: #FFF;padding: 7px;" class ="add-member-btn" onclick="event.stopPropagation(); Confirmsave('Patient name : ${groupChat[tb1].GroupName}','${groupChat[tb1].ChattingGroupId}')">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                            }
                            else {
                                gbl_UnreadMsgCount += groupChat[tb1].unreadMsgCount;
                                var row = `
                            <tr role="row" class ="odd" style="cursor: pointer;" onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].GroupName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].OfficeName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})"><button class ="countbtn">${groupChat[tb1].unreadMsgCount}</button></td>
                            <td>
                                <button style="color: #1B4066;font-family: 'Open Sans';font-size: 12px;font-weight: 600;border-radius: 6px;border: 1px solid #1B4066;background: #FFF;padding: 7px;" class ="add-member-btn" onclick="event.stopPropagation(); Confirmsave('Patient name : ${groupChat[tb1].GroupName}','${groupChat[tb1].ChattingGroupId}')">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                            }

                            $("#IntakePatientChatTbl tbody").append(row);
                        }
                        $("#IntakeChatDiv1").show();
                        $("#loading").hide();
                    }

                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                    $("#loading").hide();
                }
            });
            // $("#loading").hide();
        }
    });

}

/////////////   Quick Blox code   //////////////

function startChat() {
    $("#loading").show();
    //var QuickbloxApp_Id = "59230";
    //var QuickbloxAuth_Key = "SV2czdXSOafbMNm";
    //var QuickbloxAuth_Secret = "pru2MGmJxj7zedX";
    //QB.init(QuickbloxApp_Id, QuickbloxAuth_Key, QuickbloxAuth_Secret, true);
    //QB.destroySession(function (error) {
    //    debugger;
    //    // callback function
    //});

    //QB.createSession(function (err, result) {


    //    if (err) {
    //        alert(err.message);
    //    } else {

    //        sessionStorage.setItem("sToken", result.token);

    //        if (sFromQBId == '0' || sFromQBId == "") {

    //            //RegisterNewUser("Superadmin@PaSeva.com", "d59ff81a-5f78-4d23-be3b-c8630313e2ba");
    //            RegisterNewUser(Email, useridLogin);
    //        }
    //    }
    //});

    //var user = {
    //    id: sFromQBId,
    //    login: Email,//"Superadmin@PaSeva.com",
    //    password: "Welcome007!"
    //};

    CreateNewGroupDialod();
    //QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {

    //    if (err) {
    //        //console.log('SERVER ERROR RESPONSE ');

    //    } else {
    //        //console.log('SERVER SUCCESS RESPONSE ');
    //        //var tmpDialogId = "5c6a7fd9d4594d0c0db4dc4f";


    //        CreateNewGroupDialod();
    //        // loadChatMessages(100, 0); // 6479
    //        // joinChatRoom();           // 6479

    //        // toastr.success("Now You Can Start Chatting.");
    //        // $("#loading").hide();  //6479

    //        //if (tmpDialogId == "0") {



    //        //}
    //        // else {
    //        //CreateSession();
    //        //loadChatMessages(100, 0);
    //        //joinChatRoom();

    //        //toastr.success("Now You Can Start Chatting.");
    //        //$("#loading").hide();
    //        //For Permission Start

    //        /* if ('@ViewBag.role' != null){

    //             if ('@Session["Role"]' != 'SuperAdmin' && '@Session["Role"]' != 'Admin' )
    //             {
    //                 $('#lblPermissionInfo').css('display', 'block');
    //                 if ('@ViewBag.permission' == "1"){
    //                     toastr.info("You Have Only Read Permission For This Group.");
    //                     $('#lblPermissionInfo').html('You Have Only Read Permission For This Group.');
    //                     $('#chatInput').css('display','none')
    //                     $('#chatInput').css('display','none')

    //                 }
    //                 if ('@ViewBag.permission' == "2"){
    //                     toastr.info("You Have Only Read / Write Permission For This Group.");
    //                     $('#lblPermissionInfo').html('You Have Only Read / Write Permission For This Group.');
    //                     $('#selectFileIcon').css('display','none');

    //                 }
    //                 if ('@ViewBag.permission' == "3" || '@ViewBag.permission' == "0"){

    //                     toastr.info("You Have Read / Write / Attachment Permission For This Group.");
    //                     $('#lblPermissionInfo').html('You Have Read / Write / Attachment Permission For This Group.');
    //                 }
    //                 if ('@ViewBag.permission' == "4" || '@ViewBag.permission' == "0"){

    //                     toastr.info("You Have Only Write Permission For This Group.");
    //                     $('#lblPermissionInfo').html('You Have Only Write Permission For This Group.');
    //                     // $('#chatMsg').css('display','none')
    //                     // $('#chatMsg').css('display','none')
    //                 }

    //             }
    //             else
    //             {$('#lblPermissionInfo').css('display', 'none');}
    //         } */
    //        //For Permission End
    //        //  }

    //    }


    //});
}

var QBId = "";
function RegisterNewUser(Login, UserId1, Index) {
    var RegisterUser = { 'login': Login, 'password': "Welcome007!", 'tag': "differenzsystem", 'email': Login };

    QB.users.create(RegisterUser, function (err, user1) {
        if (user1) {
            //Add new QuickBlox Id For Specific User
            //debugger;
            // var url = INITURL + '/Chatting/SaveQBId';
            QBId = user1.id;
            sFromQBId = "" + QBId + "";
            sessionStorage.setItem("FromQBId", QBId);
            startChat();
            ///**/ $.ajax({
            //     url: '/InTake/SavingDataIntoChattingGrouptbl',
            //     data: { UserId: UserId1, QuickBloxId: QBId },
            //     cache: false,
            //     type: 'POST',
            //     async: false,
            //     success: function (data) {
            //         if (data == "Success") {

            //             if (UserId1 == "d59ff81a-5f78-4d23-be3b-c8630313e2ba") {
            //                 sFromQBId = "" + QBId + "";
            //                // GroupMemberList[Index].QuickBloxId = sFromQBId;

            //                 sessionStorage.setItem("FromQBId", QBId);
            //                 startChat();

            //             }
            //             if (UserId1 == GroupMemberList[Index].UserId) {

            //                 //sToQBId = "" + QBId + "";
            //                 //GroupMemberList[Index].QuickBloxId = sToQBId;
            //                 sessionStorage.setItem("ToQBId", QBId);
            //             }
            //         }
            //         //CreateNewGroupDialod();
            //     },
            //     error: function (reponse) {
            //         alert("error : " + reponse);
            //         $("#loading").hide();
            //     }
            // });


            // success
        } else {
            return;
            // error
        }
    });
}

function SavingDataIntoChattingGrouptbl() {
    startChat();

}

//Create New Group Chat Dialog Start

function CreateNewGroupDialod() {
    var patientname = "";
    var user = {
        id: "32132455",//sFromQBId, //6479
        login: "superadmin@paseva.com",//Email,//"@Session["FromUserEmail"]",
        password: "Welcome007!"
    };
    //QB.destroySession(function (error) {
    //    debugger;
    //    // callback function
    //});
    //QB.createSession({ login: user.login, password: user.password }, function (err, result) {

    //    if (err) {
    //        alert(err.message);
    //        $("#loading").hide();
    //        //  CheckInTakePatientData();
    //    } else {
    //        //alert("Session Token- " + result.token);
    //        sessionStorage.setItem("sToken", result.token);

    //        //var occupants = new Array(sToQBId, sFromQBId);

    //    }
    //});
    var occupants = new Array();
    for (var v1 = 0; v1 < GroupMemberList.length ; v1++) {
        occupants.push(GroupMemberList[v1].QuickBloxId);
    }
    fetchBydefaultMembersToAddInChatRoom();
    var finalQBIDs = [sFromQBId].concat(GrpQBIDs);
    patientname = $("#Fname").val() + " " + $("#Lname").val();
    if ($("#Lname").val() == "") {
        patientname.trim();
    }

    var dialogOccupants = {

        type: 2,
        occupants_ids: finalQBIDs,//[sFromQBId],//occupants,//[sFromQBId, sToQBId],
        name: patientname,//"Chat_" + "@ViewBag.GroupName",
        data: {
            class_name: "ChatDialogType",
            ChatCategory: "5",//"2",
            OfficeID: $("#officeId").val()
        }

    };
    //Create New Group Chat Dialog
    QB.chat.dialog.create(dialogOccupants, function (err, createdDialog) {
        if (err) {
            //console.log(err);
            //console.log("DIALOG CREATED FAIL");
        } else {
            //Add new QuickBlox Id For Specific User

            // var url = INITURL + '/Chatting/SaveDialogId';


            $.ajax({
                url: '/InTake/SavingDataIntoChattingGrouptbl',
                data: {
                    //ChattingGroupId:ChattingGroupId,//@ViewBag.GroupDialogId,
                    dialogid: createdDialog._id,
                    grpname: patientname,
                    userid: useridLogin,
                    QBID: sFromQBId,//QBId,
                    Officename: $("#officeId").val(),
                    IntakePatientId: lastInsertedRowId,
                },
                cache: false,
                type: 'POST',
                async: false,
                success: function (data) {
                    if (data != "failed") {  //"success"  //6479//
                        AddLoginUserIntoPatientChatRoom(data);
                        if (DefaultMembers.length > 0) {
                            AddBydefaultMembersInto_ChattingGroupMemberTbl(data);
                        }
                        CheckInTakePatientData();
                        tmpDialogId = createdDialog._id;
                        $("#loading").hide();
                    }
                    // loadChatMessages(100,0);
                    // joinChatRoom();

                    //toastr.success("Now You Can Start Chatting.");
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

function loadChatMessages(vLimit, vSkip) {

    var user = {
        id: sFromQBId,
        login: Email,//"Superadmin@PaSeva.com",
        password: "Welcome007!"
    };
    QB.destroySession(function (error) {
        // callback function
    });
    QB.createSession({ login: user.login, password: user.password }, function (err, result) {

        if (err) {
            alert(err.message);
        } else {
            //alert("Session Token- " + result.token);
            sessionStorage.setItem("sToken", result.token);

            loadMessageV1(1);

        }
    });

}

function AppenedOfficeList() {
    $("#officeId").append('<option value="0">--Select Office --</option>');
    $.ajax({
        url: '/InTake/AppendOfficeList',
        async: false,
        type: 'GET',
        data: {},
        success: function (response) {
            var data = JSON.parse(response);
            tmpOfficeArr = data;
            for (var i = 0; i < data.length; i++) {
                $("#officeId").append("<option value='" + data[i].officeId + "'>" + data[i].officeName + "</option>");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

/*function removeDial() {
    var YOUR_APP_ID = "59230";
    var YOUR_AUTH_KEY = "SV2czdXSOafbMNm";
    var YOUR_AUTH_SECRET = "pru2MGmJxj7zedX";
    var userCredentials = {
        login: "aryaashish200@gmail.com",//"superadmin@paseva.com",
        password: "Welcome007!"
    };
    var encodedApiKey = btoa("SV2czdXSOafbMNm");
    QB.init(YOUR_APP_ID, YOUR_AUTH_KEY, YOUR_AUTH_SECRET);
    QB.createSession({ login: 'aryaashish200@gmail.com', password: 'Welcome007!' }, function (err, session) {
        if (session) {
            console.log("QB-Token:", session.token);
            var token = session.token;  // Token is now available, use it immediately

            // Now make the DELETE request inside this callback
            $.ajax({
                url: "https://apisoltapps.quickblox.com/chat/Dialog/683d9fa2e7affd001a0bc87e.json",
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

            //$.ajax({
            //    url: "https://api.quickblox.com/chat/Dialog/683d9fa2e7affd001a0bc87e.json",
            //    type: "DELETE",
            //    headers: {
            //        "Content-Type": "application/json",
            //        "Authorization": "ApiKey" + encodedApiKey
            //    },
            //    data: { force: 1 },
            //    success: function (response) {
            //        alert("Dialog deleted successfully!", response);
            //    },
            //    error: function (xhr, status, error) {
            //        alert("Error deleting dialog:", error);
            //    }
            //});


        } else {
            alert("Session creation failed: " + err);
        }
    });
}
 */

//delete dialogID
//function removeDial(dialogId) {
//    const APP_ID = "59230";
//    const AUTH_KEY = "SV2czdXSOafbMNm";
//    const AUTH_SECRET = "pru2MGmJxj7zedX";

//    const userCredentials = {
//        login: "superadmin@paseva.com",
//        password: "Welcome007!"
//    };

//    QB.init(APP_ID, AUTH_KEY, AUTH_SECRET);

//    // Create session
//    QB.createSession(userCredentials, function (err, session) {
//        if (err || !session) {
//            alert("Session creation failed: " + (err ? err.message : "Unknown error"));
//            return;
//        }

//        const token = session.token;
//        const deleteUrl = `https://apisoltapps.quickblox.com/chat/Dialog/${dialogId}.json`;

//        $.ajax({
//            url: deleteUrl,
//            type: "DELETE",
//            headers: {
//                "Content-Type": "application/json",
//                "QB-Token": token
//            },
//            success: function () {
//                alert("Dialog deleted successfully!");
//            },
//            error: function (xhr, status, error) {
//                alert("Error deleting dialog: " + xhr.responseText || error);
//                console.error("Delete error:", xhr);
//            }
//        });
//    });
//}


/*function removeDial(dialogId) {
    const APP_ID = "59230";
    const AUTH_KEY = "SV2czdXSOafbMNm";
    const AUTH_SECRET = "pru2MGmJxj7zedX";

    const userCredentials = {
        login: "superadmin@paseva.com",
        password: "Welcome007!"
    };

    QB.init(APP_ID, AUTH_KEY, AUTH_SECRET);

    // Create session
    QB.createSession(userCredentials, function (err, session) {
        if (err || !session) {
            toastr.error("Session creation failed: " + (err ? err.message : "Unknown error"));
            return;
        }

        const token = session.token;

        // Step 1: Fetch the dialog
        QB.chat.dialog.get(dialogId, function (fetchErr, dialog) {
            if (fetchErr || !dialog) {
                toastr.error("Failed to fetch dialog: " + (fetchErr ? fetchErr.message : "Unknown error"));
                return;
            }

            // Step 2: Check if current user is the creator
            if (dialog.created_by !== session.user_id) {
                toastr.success("You are not the creator of this chat room. Only the creator can delete it.");
                return;
            }

            // Step 3: Delete the dialog
            const deleteUrl = `https://apisoltapps.quickblox.com/chat/Dialog/${dialogId}.json`;

            $.ajax({
                url: deleteUrl,
                type: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    "QB-Token": token
                },
                success: function () {
                    toastr.success("Dialog deleted successfully!");
                },
                error: function (xhr, status, error) {
                    toastr.error("Error deleting dialog: " + (xhr.responseText || error));
                    console.error("Delete error:", xhr);
                }
            });
        });
    });
} */

function removeDial(dialogId) {
    //const APP_ID = 59230;
    //const AUTH_KEY = "SV2czdXSOafbMNm";
    //const AUTH_SECRET = "pru2MGmJxj7zedX";

    //const userCredentials = {
    //    login: "superadmin@paseva.com",
    //    password: "Welcome007!"
    //};

    //// Initialize QuickBlox SDK
    //QB.init(APP_ID, AUTH_KEY, AUTH_SECRET);
    //QB.destroySession(function (error) {
    //    // callback function
    //});

    // Step 1: Create session
    // QB.createSession(userCredentials, function (err, session) {
    //if (err || !session) {
    //    toastr.error("Session creation failed: " + (err ? err.message : "Unknown error"));
    //    return;
    //}

    //const token = session.token;
    //const currentUserId = session.user_id;

    // Step 2: Fetch dialog (NOTE: use QB.chat.dialog.list, not QB.chat.dialog.get ❌)
    //   QB.chat.dialog.list({ _id: dialogId }, function (fetchErr, result) {
    //if (fetchErr || !result || result.items.length === 0) {
    //    toastr.error("Failed to fetch dialog: " + (fetchErr ? fetchErr.message : "Not found"));
    //    return;
    //}

    //  const dialog = result.items[0];

    // Step 3: Check if current user is the creator
    //if (dialog.user_id !== currentUserId) {
    //    toastr.info("You are not the creator of this chat room. Only the creator can delete it.");
    //    return;
    //}

    // Step 4: Delete dialog via REST API

    /*  $.ajax({
          url: `https://apisoltapps.quickblox.com/chat/Dialog/${dialogId}.json?force=1`,
          type: "DELETE",
          headers: {
              "QB-Token": token
          },
          success: function () {
              toastr.success("Dialog deleted successfully!");
          },
          error: function (xhr, status, error) {
              toastr.error("Error deleting dialog: " + (xhr.responseText || error));
              console.error("Delete error:", xhr);
          }
      }); */
    var token = sessionStorage.getItem("token");
    $.ajax({
        url: `https://apisoltapps.quickblox.com/chat/Dialog/${dialogId}.json?force=1`,
        type: "DELETE",
        headers: {
            "QB-Token": token
        },
        success: function () {
            toastr.success("Patient data removed successfully!");  //Dialog deleted successfully with force=1!
        },
        statusCode: {
            204: function () {
                toastr.success("Patient data removed successfully!");  //Dialog deleted successfully
            }
        },
        error: function (xhr, status, error) {
            // Ignore 204 as an error
            if (xhr.status === 204 || xhr.status === 200) {
                toastr.success("Patient data removed successfully!");  //Dialog deleted successfully
                return;
            }

            //toastr.error("Error deleting dialog: " + (xhr.responseText || error));
            toastr.success("Patient data removed successfully!");
            console.error("Delete error:", xhr);
        }
    });

    // });
    // });
}





var dialogJid = "...";
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
var ChattingGrpId = "";
function Confirmsave(name, Id) {
    ChattingGrpId = Id;
    AppenedOfficeMemberList(ChattingGrpId, name);

    $("#ChatGroupName").html(name);
    $('#Save_Modal').modal('show');

}
function HideAddMemberModal() {
    $('#Save_Modal').modal('hide');
    $('.memberSelect').prop('checked', false);       // Uncheck all
    $('#selectedTags').html('');                     // Clear tags
    $('#searchBox').val('');                         // Clear search
    $('#dropdownMenu').hide();                       // Hide dropdown
    $('#dropdownArrow').removeClass('fa-chevron-up').addClass('fa-chevron-down');
    window.tempSelected = [];
}

var AddedMembersInChatRoom = [];
var dialogid = "";

function AppenedOfficeMemberList(ChattingGrpId, name) {
    $("#memberSelect").html('');
    $("#memberSelect").append('<option value="0">--Select Members --</option>');

    $.ajax({
        url: '/InTake/AppendMemberList',
        async: false,
        type: 'GET',
        data: { "ChattingGrpId": ChattingGrpId },
        success: function (response) {
            debugger;
            var data = JSON.parse(response.split(';')[0]);
            dialogid = JSON.parse(response.split(';')[1])[0].dialogId
            var result = data;
            if (result.length > 0) {

                for (var i = 0; i < result.length; i++) {
                    $("#memberSelect").append(
                        "<option value='" + result[i].UserId + "' data-qbid='" + result[i].QuickbloxId + "' data-groupname='" + name + "' data-dialogid='" + dialogid + "'>" + result[i].UserName + "</option>"
                    );
                }
            }
            else {

                $("#memberSelect").append('<option value="0">--No Members Found --</option>');

            }

        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}



function SaveSelectedMembers() {

    debugger;

    // $('#loading').show();
    var selectedUserIds = [];

    var groupname = "";
    var dialogId = "";
    var occupants = [];

    $("#memberSelect option:selected").each(function () {
        var userId = $(this).val();
        var qbid = $(this).data("qbid");

        /* occupants = new Array();*/

        occupants.push(qbid);

        groupname = $(this).data("groupname");

        dialogId = $(this).data("dialogid");

        console.log("UserId:", userId);
        console.log("QuickBloxId:", qbid);
    });

    groupname = groupname.split(':')[1];
    dialogid = dialogid;

    if ($("#memberSelect").val() == null) {
        toastr.warning("Please select at least one member.");
        return false;
    }

    $.ajax({
        url: '/InTake/SaveSelectedMembers',
        async: false,
        type: 'POST',
        traditional: true,
        //data: {
        //    userId: selectedUserIds, // array of IDs
        //    chattingGroupId: ChattingGrpId
        //},
        data: { "userId": $("#memberSelect").val(), "chattingGroupId": ChattingGrpId },
        success: function (response) {
            debugger;
            // var data = JSON.parse(response);
            if (response == "success") {

                UpdateMembersIntoChatRoom(occupants, dialogId, groupname);
                // toastr.success("Members Added Successfully!");
                $('#Save_Modal').modal('hide');
            }
            else if (response == "failed") {
                toastr.success("Falied to Add!");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            toastr.error("Something went wrong!");
        }
    });

}




function UpdateMembersIntoChatRoom(occupants, _Dialogids, groupname) {
    $("#loading").show();

    groupname = $.trim(groupname);
    debugger;

    var dialogid = "";
    var QBIDs = [];
    var name = "";
    debugger;


    debugger;
    dialogid = _Dialogids;
    name = groupname;
    QBIDs = occupants;


    let inputData = QBIDs;//['12323', '45678', '45678', '56789'];
    let transformedData = inputData.map(Number);

    // Step 4: Update dialog with new members
    QB.chat.dialog.update(dialogid, {
        name: name,
        push_all: {
            occupants_ids: transformedData
        }
    }, function (err, updatedDialog) {
        debugger;
        if (err) {
            console.error("Failed to update dialog:", err);
            toastr.error("Failed to update members.");
            $('#loading').hide();
        } else {
            debugger;
            console.log("Dialog updated:", updatedDialog);
            toastr.success("Members added successfully!");
            $('#Save_Modal').modal('hide');
            $('#loading').hide();
        }
    });

}



/*var AddedMembersInChatRoom = [];*/
function GetAddedMembersList() {
    AddedMembersInChatRoom = [];
    debugger;
    $.ajax({
        url: '/InTake/GetAllAddedMembersListByChattingGrpID',
        async: false,
        type: 'GET',
        data: { chattingGroupId: ChattingGrpId },
        success: function (response) {
            debugger;
            AddedMembersInChatRoom = JSON.parse(response);

        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            toastr.error("Something went wrong!");
        }
    });
}

function CancelReject() {
    $('#Delete_Modal1').modal('hide');
}
var GetOfficesList = [];
function AppenedOffices() {
    $("#officeId").append('<option value="0">--Select Office --</option>');
    $.ajax({
        url: '/InTake/AppendOfficeList',
        async: false,
        type: 'GET',
        data: {},
        success: function (response) {
            var data = JSON.parse(response);
            GetOfficesList = data;
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

function CancalAcceptModal() {
    $('#Delete_Modal').modal('hide');
}
function CloseAddPatientModal() {
    $('#ServiceType_Add_Modal').modal('hide');
}

function AddLoginUserIntoPatientChatRoom(data) {

    $.ajax({
        url: '/InTake/AddLoginUserIntoChatRoom',
        async: false,
        type: 'POST',
        data: { "chattingGroupId": data },
        success: function (response) {
            if (response == "success") {
                // toastr.success("Login user added successfully in the chatroom!");  //6479
            }
            else if (response == "failed") {
                toastr.error("Failed to Add Login user in the chatroom!");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            toastr.error("Failed to Add Login user!");
        }
    });
}

function ClosepopupEditDetails() {
    $('#AppointmentDetailModal').modal('hide');
}
function ClosepopupEditModal() {
    $('#EditDetailModal').modal('hide');
}



//------------------------------------------------------------------------------------------------
function validatePhone(input) {

    input.value = input.value.replace(/[^0-9()+-]/g, '');


}

$(function () {

    flatpickr("#DateOfBirth", {
        maxDate: "today",
        dateFormat: "m-d-Y"

    });
    document.querySelector('.input-group-addon').addEventListener('click', function () {
        document.querySelector('#DateOfBirth').focus();
    });
});
$(function () {

    flatpickr("#UDateOfBirth", {
        maxDate: "today",
        dateFormat: "m-d-Y"
    });
});
$(function () {
    $('#UDateOfBirth').datetimepicker({
        format: "MM-DD-YYYY",
        maxDate: moment()
    });


    $('#dobCalendarIcon').on('click', function () {
        $('#UDateOfBirth').focus(); // or show the picker
    });
});


function limitLength(ele) {
    if (ele.value.length > 10) {
        ele.value = ele.value.slice(0, 10);
    }
}

var PatientValidID = "";
function CheckPatientValidID() {

    var pattern = /^(?=(.*[A-Za-z]){2})(?=(.*\d){8})[A-Za-z0-9]*$/;
    if ($("#PatientID").val().length == 10) {
        // Regex for 2 letters + 8 digits
        var inputVal = $("#PatientID").val();
        if (inputVal != "") {
            if (!pattern.test(inputVal)) {

                alert("Invalid Patient Id format! Enter  2 letters and 8 digits.");
                PatientValidID = 2;
                //  $("#MedicalId").focus();
                // e.preventDefault(); // Stop form submission
                return false;
            }
            else {
                $.ajax({
                    url: '/InTake/CheckPatientValidID',
                    async: false,
                    type: 'POST',
                    data: { patientid: $("#PatientID").val() },
                    success: function (response) {
                        var data = JSON.parse(response);
                        if (data[0].MedicalId == 1) {
                            PatientValidID = data[0].MedicalId;
                            toastr.error("PatientID exist,enter another ID");  //1 means exists
                        }
                        else {
                            PatientValidID = 0;  //0 means not exist
                        }

                    },
                    error: function (xhr, status, error) {
                        console.error('Error:', error);
                        toastr.error("Something went wrong!");
                    }
                });
            }
        }

    }

}


var UPatientValidID = "";
function UpdateCheckPatientValidID() {

    var pattern = /^(?=(.*[A-Za-z]){2})(?=(.*\d){8})[A-Za-z0-9]*$/;
    if ($("#UPatID").val().length == 10) {
        // Regex for 2 letters + 8 digits
        var inputVal = $("#UPatID").val();
        if (inputVal != "") {
            if (!pattern.test(inputVal)) {

                alert("Invalid Patient Id format! Enter  2 letters and 8 digits.");
                UPatientValidID = 2;
                //  $("#MedicalId").focus();
                // e.preventDefault(); // Stop form submission
                return false;
            }
            else {
                $.ajax({
                    url: '/InTake/CheckPatientValidID',
                    async: false,
                    type: 'POST',
                    data: { patientid: $("#UPatID").val() },
                    success: function (response) {
                        var data = JSON.parse(response);
                        if (data[0].MedicalId == 1) {
                            UPatientValidID = data[0].MedicalId;
                            toastr.error("PatientID exist,enter another ID");  //1 means exists
                        }
                        else {
                            UPatientValidID = 0;  //0 means not exist
                        }

                    },
                    error: function (xhr, status, error) {
                        console.error('Error:', error);
                        toastr.error("Something went wrong!");
                    }
                });
            }
        }
    }
}


function GetLanLat() {
    var Address = $("#streetid").val() + $("#cityid").val() + $("#stateid").val();//$("#Address").val();
    var Pincode = $("#zipcodeid").val();
    var flag = true;
    $.ajax({
        type: "GET",
        url: "https://maps.google.com/maps/api/geocode/json",
        data: "address=" + Address + ",  zip code=" + Pincode + "&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw",
        async: false,
        contentType: "text/plain; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result["status"] == "OK") {
                //add = result;
                $("#Latitude").val(result["results"][0]["geometry"]["location"]["lat"]);
                $("#Longitude").val(result["results"][0]["geometry"]["location"]["lng"]);
                var Latitude = result["results"][0]["geometry"]["location"]["lat"];
                var Longitude = result["results"][0]["geometry"]["location"]["lng"];

                //$("#City").val(result.results[0].address_components[2].long_name);
                //$("#State").val(result.results[0].address_components[3].long_name);

                var today = new Date();
                //var UTCDate = today.toUTCString();
                //var UtcNow = Math.floor(today.getTime() / 1000);

                var timestamp = today.getTime() / 1000 + today.getTimezoneOffset() * 60;
                $.ajax({
                    type: "GET",
                    url: "https://maps.googleapis.com/maps/api/timezone/json?location=" + Latitude + "," + Longitude + "&timestamp=" + timestamp + "&sensor=false&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw",
                    async: false,
                    contentType: "text/plain; charset=utf-8",
                    dataType: "json",
                    success: function (data, result) {


                        if (data["status"] == "OK" && result == "success") {
                            //  $("#hdndstOffset").val(data.dstOffset);
                            // $("#hdnrawOffset").val(data.rawOffset);
                            //  $("#hdntimeZoneFullName").val(data.timeZoneName);
                            $("#hdntimeZoneId").val(data.timeZoneId);

                            var str = data.timeZoneName;
                            var splitString = str.split(" ");
                            var abbr = "";
                            for (i = 0; i < splitString.length; i++) {
                                abbr += splitString[i].charAt(0);
                            }
                            $("#hdnTimezonePostfix").val(abbr);
                            //var TimeZoneOffset = data.rawOffset;
                            var TimeZoneOffset = data.dstOffset + data.rawOffset
                            // console.log(TimeZoneOffset);

                            TimeZoneOffset = TimeZoneOffset < 0 ? "-" + Math.abs(TimeZoneOffset) / 60 : "+" + Math.abs(TimeZoneOffset) / 60;
                            $("#hdnTimezoneOffset").val(parseInt(TimeZoneOffset));
                            return true;
                        }
                    }
                });
            }
            else if (result.status === "ZERO_RESULTS") {
                toastr.error('Please enter valid address');
                flag = false;
            }
        }
    });
    //----------End Pincode+Address----------------
    return flag;
}


function UGetLanLat() {
    var Address = $("#UStreet").val() + $("#UCity").val() + $("#UState").val();//$("#Address").val();
    var Pincode = $("#UZipcode").val();
    var flag = true;
    $.ajax({
        type: "GET",
        url: "https://maps.google.com/maps/api/geocode/json",
        data: "address=" + Address + ",  zip code=" + Pincode + "&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw",
        async: false,
        contentType: "text/plain; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result["status"] == "OK") {
                //add = result;
                $("#Latitude").val(result["results"][0]["geometry"]["location"]["lat"]);
                $("#Longitude").val(result["results"][0]["geometry"]["location"]["lng"]);
                var Latitude = result["results"][0]["geometry"]["location"]["lat"];
                var Longitude = result["results"][0]["geometry"]["location"]["lng"];

                //$("#City").val(result.results[0].address_components[2].long_name);
                //$("#State").val(result.results[0].address_components[3].long_name);

                var today = new Date();
                //var UTCDate = today.toUTCString();
                //var UtcNow = Math.floor(today.getTime() / 1000);

                var timestamp = today.getTime() / 1000 + today.getTimezoneOffset() * 60;
                $.ajax({
                    type: "GET",
                    url: "https://maps.googleapis.com/maps/api/timezone/json?location=" + Latitude + "," + Longitude + "&timestamp=" + timestamp + "&sensor=false&key=AIzaSyCqG0NdAH_5gP1_D8jGhmTGeqNR-9z_afw",
                    async: false,
                    contentType: "text/plain; charset=utf-8",
                    dataType: "json",
                    success: function (data, result) {


                        if (data["status"] == "OK" && result == "success") {
                            //  $("#hdndstOffset").val(data.dstOffset);
                            // $("#hdnrawOffset").val(data.rawOffset);
                            //  $("#hdntimeZoneFullName").val(data.timeZoneName);
                            $("#hdntimeZoneId").val(data.timeZoneId);

                            var str = data.timeZoneName;
                            var splitString = str.split(" ");
                            var abbr = "";
                            for (i = 0; i < splitString.length; i++) {
                                abbr += splitString[i].charAt(0);
                            }
                            $("#hdnTimezonePostfix").val(abbr);
                            //var TimeZoneOffset = data.rawOffset;
                            var TimeZoneOffset = data.dstOffset + data.rawOffset
                            // console.log(TimeZoneOffset);

                            TimeZoneOffset = TimeZoneOffset < 0 ? "-" + Math.abs(TimeZoneOffset) / 60 : "+" + Math.abs(TimeZoneOffset) / 60;
                            $("#hdnTimezoneOffset").val(parseInt(TimeZoneOffset));
                            return true;
                        }
                    }
                });
            }
            else if (result.status === "ZERO_RESULTS") {
                toastr.error('Please enter valid address');
                flag = false;
            }
        }
    });
    //----------End Pincode+Address----------------
    return flag;
}

function Closepopup() {
    $('#ServiceType_Add_Modal').modal('hide');
    $('#EditDetailModal').modal('hide');
}

//function getlistofdialogsIDS() {
//    var filters = {
//        limit: 1000,
//        skip: 0,
//        sort_desc: "last_message_date_sent",
//        data: {
//            class_name: "ChatDialogType",
//            ChatCategory: "2",
//            "OfficeID[$in]": ['20']  // ✅ Use this syntax for `$in`
//        }
//    };


//    var QuickbloxApp_Id = "59230";
//    var QuickbloxAuth_Key = "SV2czdXSOafbMNm";
//    var QuickbloxAuth_Secret = "pru2MGmJxj7zedX";
//    QB.init(QuickbloxApp_Id, QuickbloxAuth_Key, QuickbloxAuth_Secret, true);
//    var user = {
//        id: '32132455',
//        login: "superadmin@paseva.com",
//        password: "Welcome007!"
//    };
//    QB.createSession({ login: user.login, password: user.password }, function (err, result) {

//        if (err) {
//            alert(err.message);
//        } else {
//            var user = {
//                id: '32132455',
//                login: 'Superadmin@PaSeva.com',
//                password: "Welcome007!"
//            };
//            QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {
//                QB.chat.dialog.list(filters, function (error, response) {
//                    if (error) {
//                        alert("QB API error:", error);
//                    } else {
//                        alert("Dialogs:", response.items.length);
//                    }
//                });

//            });
//        }
//    });




//}


var qbSessionToken = "";
// Function to create QB session
function createQBSession() {

    if (QB.service.qbInst.session && QB.service.qbInst.session.token) {
        console.log("Session already exists:", QB.service.qbInst.session.token);
        connectQBChat(); // safe to proceed
    } else {
        //session creation  //

        var user = {
            id: sFromQBId,//,
            login: Email,//,
            password: "Welcome007!"
        };
        $('#loading').show();
        QB.createSession({ login: user.login, password: user.password }, function (err, session) {
            if (err) {
                console.error("QB session error:", err);
                // alert(err.message);
                $('#loading').hide();
            } else {
                qbSessionToken = session.token;
                console.log("QB session created:", qbSessionToken);
                connectQBChat();
                // if (callback) callback();
            }
        });
    }


}

// Function to connect to chat
function connectQBChat() {
    var user = {
        id: sFromQBId,//,
        login: Email,//,
        password: "Welcome007!"
    };
    QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {
        if (err) {
            console.error("QB chat connect error:", err);
            //alert(err.message);
            $('#loading').hide();
        } else {
            console.log("QB chat connected");
            // if (callback) callback();
            $('#loading').hide();
            // QB.chat.onMessageListener = onMessage;
        }
    });
}

function updateTags() {
    const selected = [];

    debugger;

    $("#selectedTags").html(""); // Clear previous

    $(".memberSelect:checked").each(function () {
        const userName = $(this).data("username");
        selected.push(userName);
    });

    if (selected.length <= 2) {
        selected.forEach(name => {
            $("#selectedTags").append(`<span class="tag">${name}</span>`);
        });
    } else {
        selected.slice(0, 2).forEach(name => {
            $("#selectedTags").append(`<span class="tag">${name}</span>`);
        });
        $("#selectedTags").append(`<span class="tag-summary">+${selected.length - 2}</span>`);
    }
}

function AddBydefaultMembersInto_ChattingGroupMemberTbl(ChattingGrpId) {
    var selectedUserIds = [];
    selectedUserIds = DefaultMembers;

    $.ajax({
        url: '/InTake/SaveSelectedMembers',
        async: false,
        type: 'POST',
        traditional: true,
        data: {
            userId: selectedUserIds, // array of IDs
            chattingGroupId: ChattingGrpId
        },
        // data: { "userId": $("#memberSelect").val(), "chattingGroupId": ChattingGrpId },
        success: function (response) {
            // var data = JSON.parse(response);
            if (response == "success") {

            }
            else if (response == "failed") {
                toastr.error("Falied to Add!");

            }

        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            toastr.error("Something went wrong!");
        }
    });

}
var DefaultMembers = [];
var GrpQBIDs = [];
function fetchBydefaultMembersToAddInChatRoom() {
    $.ajax({
        url: '/InTake/fetchBydefaultMembersToAddInChatRoom',
        async: false,
        type: 'POST',
        data: {},
        success: function (response) {

            var data = JSON.parse(response);
            DefaultMembers.push(data);
            DefaultMembers = DefaultMembers[0].map(member => member.UserId);
            GrpQBIDs.push(data);
            GrpQBIDs = GrpQBIDs[0].map(member => member.QBID);

        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            toastr.error("Failed to fetch default members!");
        }
    });
}


function OpenInTakePatientChatDetailPage(id) {
    // $('#loading').show();
    window.location.href = '/InTakechatting/InTakePatientChatDetailPage?Id=' + id;
}


function onMessage() {
    // $("#loading").show();
    const officeIds = tmpOfficeArr.map(o => String(o.officeId));  //Sample data ['1','2'.'3']
    var filters = {
        limit: 1000,
        skip: 0,
        sort_desc: 'created_at', //"last_message_date_sent",
        data: {
            class_name: "ChatDialogType",
            ChatCategory: "2",
            "OfficeID[$in]": officeIds
        }
    };

    var user = {
        id: sFromQBId,//,
        login: Email,//,
        password: "Welcome007!"
    };

    QB.chat.dialog.list(filters, function (error, response) {
        if (error) {
            $("#loading").hide();
            toastr.error("Failed to load data from the QuickBlox APIs!");
        } else {
            var dialogs = response.items
            var patientgroupDialogIds = "";
            for (i = 0; i < dialogs.length ; i++) {
                if (i != 99)
                    patientgroupDialogIds += dialogs[i]._id + ","
                else
                    patientgroupDialogIds += dialogs[i]._id
            }
            $.ajax({
                url: '/InTake/Get_IntakePatientRoomGroupList',
                data: { QBGroupDialogIds: patientgroupDialogIds, GroupType: 1 },
                async: false,
                type: 'POST',
                success: function (response) {
                    var data = response;// JSON.parse(response);
                    var dataSource = data;
                    //debugger;
                    for (var d = 0; d < dialogs.length; d++) {
                        var dt;
                        dt = dialogs[d].last_message_date_sent;
                        count = dialogs[d].unread_messages_count;
                        try {
                            for (var x = 0; x < dataSource.length; x++) {
                                if (dialogs[d]._id == dataSource[x].DialogId) {
                                    dataSource[x]["Time"] = dt;
                                    dataSource[x]["unreadMsgCount"] = count;
                                }
                            }
                        } catch (e) { }
                    }


                    var sortData = dataSource;
                    var groupChat = [];
                    groupChat = dataSource;

                    $("#IntakeChtPatCount").html("Total Patient (" + data.length + ")");
                    // Clear previous content
                    $("#IntakePatientChatTbl thead").empty();
                    $("#IntakePatientChatTbl tbody").empty();

                    if (data.length === 0) {
                        // Optionally show "No records" row or message
                        $("#IntakeChtPatCount").hide();
                        $("#Chat_ModlPage").show();
                    } else {
                        $("#Chat_ModlPage").hide();
                        $("#IntakeChtPatCount").show();
                        var head = `
                    <tr class="theadtr">
                        <th>Patient Name</th>
                        <th>Office</th>
                        <th>Chat</th>
                        <th></th>
                    </tr>`;
                        $("#IntakePatientChatTbl thead").append(head);
                        for (var tb1 = 0; tb1 < groupChat.length; tb1++) {
                            var unReadHtml = "";
                            if (groupChat[tb1].unreadMsgCount == 0) {
                                var row = `
                            <tr role="row" class ="odd" style="cursor: pointer;" onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].GroupName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].OfficeName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td>
                                <button style="color: #1B4066;font-family: 'Open Sans';font-size: 12px;font-weight: 600;border-radius: 6px;border: 1px solid #1B4066;background: #FFF;padding: 7px;" class ="add-member-btn" onclick="event.stopPropagation(); Confirmsave('Patient name : ${groupChat[tb1].GroupName}','${groupChat[tb1].ChattingGroupId}')">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                            }
                            else {
                                var row = `
                            <tr role="row" class ="odd" style="cursor: pointer;" onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].GroupName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})">${groupChat[tb1].OfficeName}</td>
                            <td onclick="OpenInTakePatientChatDetailPage(${groupChat[tb1].ChattingGroupId})"><button class ="countbtn">${groupChat[tb1].unreadMsgCount}</button></td>
                            <td>
                                <button style="color: #1B4066;font-family: 'Open Sans';font-size: 12px;font-weight: 600;border-radius: 6px;border: 1px solid #1B4066;background: #FFF;padding: 7px;" class ="add-member-btn" onclick="event.stopPropagation(); Confirmsave('Patient name : ${groupChat[tb1].GroupName}','${groupChat[tb1].ChattingGroupId}')">Add Member</button>
                                <a style="float: inline-end; margin-right: 20px;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="8" height="13" viewBox="0 0 8 13" fill="none">
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M0.361154 0.339034C0.842693 -0.113011 1.62342 -0.113011 2.10496 0.339034L7.52772 5.42965C8.15743 6.02079 8.15742 6.97921 7.52772 7.57035L2.10496 12.661C1.62342 13.113 0.842693 13.113 0.361154 12.661C-0.120385 12.2089 -0.120385 11.476 0.361154 11.024L5.18029 6.5L0.361154 1.97603C-0.120385 1.52399 -0.120385 0.791079 0.361154 0.339034Z" fill="#999999" />
                                    </svg>
                                </a>
                            </td>
                        </tr>`;
                            }

                            $("#IntakePatientChatTbl tbody").append(row);
                        }
                    }
                    $("#loading").hide();
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                    $("#loading").hide();
                }
            });
            $("#loading").hide();
        }
    });
    // });
}

//function createGlobalQBSession() {
//    $('#loading').show();
//     var user = {
//         id: '32132455',
//         login:"superadmin@paseva.com",//Email,//
//         password: "Welcome007!"
//     };
//          QB.createSession({ login: user.login, password: user.password }, function (err, result) {

//                            if (err) {
//                                alert(err.message);
//                                $('#loading').hide();
//                            } else {
//                                //chat connected for some operations //
//                                sessionStorage.setItem("token", result.token);
//                                QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {
//                                    if (err) {

//                                    } else {  //connected
//                                        $('#loading').hide();
//                                }
//                                });
//                   }
//        });
//}


function createGlobalQBSession() {
    return new Promise(function (resolve, reject) {
        $('#loading').show();

        var user = {
            id: '32132455',
            login: "superadmin@paseva.com",
            password: "Welcome007!"
        };
        QB.createSession({ login: user.login, password: user.password }, function (err, result) {
            if (err) {
                //  alert(err.message);
                $('#loading').hide();
                reject(err);
            } else {

                sessionStorage.setItem("token", result.token);

                QB.chat.connect({ userId: user.id, password: user.password }, function (err, roster) {
                    $('#loading').hide();

                    if (err) {
                        reject(err);
                    } else {
                        resolve(); // everything done
                    }
                });
            }
        });
    });
}
