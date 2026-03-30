using CaregiverLiteWCF;
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
    public class PatientDetailsModel
    {

        [Required(ErrorMessageResourceType = typeof(CaregiverLite.Views.Resources.Patient), ErrorMessageResourceName = "RequiredMsgPatient")]
        public int PatientId { get; set; }


        [Required(ErrorMessage = "Please Enter Patient Name")]
        [DisplayName("PatientName")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [DisplayName("FirstName")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [DisplayName("LastName")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Medical-Id")]
        [DisplayName("MedicalId")]
        public string MedicalId { get; set; }

        //[Required(ErrorMessage = "Please Enter Address")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter PhoneNo")]
        [DisplayName("PhoneNo")]
        public string PhoneNo { get; set; }

        //[Required(ErrorMessage = "Please Enter Email-Id")]
        [DisplayName("Email")]
        public string Email { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        [Required(ErrorMessage = "Please Enter ZipCode")]
        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }

        public string DateOfBirth { get; set; }

        public string InsertUserId { get; set; }
        public string UpdateUserId { get; set; }

        public string TimezoneId { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezonePostfix { get; set; }

        [Required(ErrorMessage = "Please Enter PrimaryMD")]
        [DisplayName("PrimaryMD")]
        public string PrimaryMD { get; set; }
        [Required(ErrorMessage = "Please Enter Street")]
        [DisplayName("Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please Enter City")]
        [DisplayName("City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [DisplayName("State")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please Select Office")]
        [DisplayName("Office")]
        public int OfficeId { get; set; }

        public int OrganisationId { get; set; }

        public string IsActive { get; set; }

        public string quality { get; set; }

        public string description { get; set; }

        public string UserName { get; set; }
        //[Required(ErrorMessage = "Please Enter Password")]
        //[DisplayName("Password")]
        public string Password { get; set; }


        public string Medicaidid { get; set; }

        [DisplayName("PayerId")]
        public string PayerId { get; set; }

        [DisplayName("PayerProgram")]
        public string PayerProgram { get; set; }

        [DisplayName("ClientPayerId")]
        public string ClientPayerId { get; set; }

        [DisplayName("ProcedureCode")]
        public string ProcedureCode { get; set; }

        [DisplayName("JuridisctionCode")]
        public string JurisdictionCode { get; set; }


        [DisplayName("PayerProgramsID")]
        public int PayerProgramsID { get; set; }

        //[DisplayName("PayerID")]
        //public string PayerID { get; set; }


        [DisplayName("PayerIDs")]
        public int PayerIDs { get; set; }


        [DisplayName("ParocedureCodeId")]
        public int ProcedureCodeId { get; set; }

        //[DisplayName("ParocedureCodes")]
        //public string ParocedureCodes { get; set; }



        [DisplayName("PayerProgramId")]
        public int PayerProgramIdss { get; set; }

        //[DisplayName("PayerProgramCodes")]
        //public string PayerProgramCodes { get; set; }



        [DisplayName("JurisdictionCodeId")]
        public int JurisdictionCodeId { get; set; }

        //[DisplayName("JurisdictionCodes")]
        //public string JurisdictionCodes { get; set; }

        [DisplayName("ProgramsID")]
        public string ProgramsID { get; set; }

        public IEnumerable<SelectListItem> OfficeSelectList { get; set; }

        public IEnumerable<SelectListItem> PayerIDSelectList { get; set; }

        public IEnumerable<SelectListItem> PayerProgramSelectList { get; set; }

        public IEnumerable<SelectListItem> ProcedureCodeSelectList { get; set; }

        public IEnumerable<SelectListItem> JurisdictionEntitiesSelectList { get; set; }

        public List<PayerInformation> PayerInformations { get; set; }
        //public bool IsActive { get;  set; }
    }



    public class PayerInformation
    {

        [DisplayName("PayerId")]
        public string PayerId { get; set; }

        [DisplayName("PayerProgram")]
        public string PayerProgram { get; set; }

        [DisplayName("ClientPayerId")]
        public string ClientPayerId { get; set; }

        [DisplayName("ProcedureCode")]
        public string ProcedureCode { get; set; }

        [DisplayName("JuridisctionCode")]
        public string JurisdictionCode { get; set; }

        //[DisplayName("PayerProgramsID")]
        //public int PayerProgramsID { get; set; }

        //[DisplayName("PayerID")]
        //public string PayerID { get; set; }

        //[DisplayName("PayerIDs")]
        //public int PayerIDs { get; set; }

        //[DisplayName("ParocedureCodeId")]
        //public int ProcedureCodeId { get; set; }

        ////[DisplayName("ParocedureCodes")]
        ////public string ParocedureCodes { get; set; }

        //[DisplayName("PayerProgramId")]
        //public int PayerProgramIdss { get; set; }

        ////[DisplayName("PayerProgramCodes")]
        ////public string PayerProgramCodes { get; set; }

        //[DisplayName("JurisdictionCodeId")]
        //public int JurisdictionCodeId { get; set; }

        ////[DisplayName("JurisdictionCodes")]
        ////public string JurisdictionCodes { get; set; }

        //[DisplayName("ProgramsID")]
        //public string ProgramsID { get; set; }


    }

    public class PatientDetailsServiceProxy : CaregiverLiteBaseService
    {
        public string Result { get; set; }
        public bool ResultInBool { get; set; }
        public List<PatientsDetail> PatientDetailsList { get; set; }
        public PatientDetailsList ListPatientDetails { get; set; }
        public PatientsDetail PatientDetail { get; set; }
        public PatientDetailsModel PatientModelDetail { get; set; }
        public InTakePatientDetails InTakePatientModelDetail  { get; set; }
        public PatientDetailsServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<string> AddPatient(PatientsDetail PatientsDetail)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "AddPatient", new { PatientsDetail }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).Result;
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

        public async Task<PatientDetailsList> GetAllPatientDetail(string LogInUserId, int pageno, int recordperpage, string search, string sortfield, string sortOrder, int OfficeId, int OrganisationId, string IsActiveStatus)
        {

            List<PatientsDetail> objPatientDetailsList = new List<PatientsDetail>();
            PatientDetailsList PatientDetail = new PatientDetailsList();
           try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientDetailsList/" + LogInUserId + "/" + pageno + "/" + recordperpage + "/" + sortfield + "/" + sortOrder + "/" + OfficeId +  "/" + search + "/" + OrganisationId.ToString() + "/" + IsActiveStatus, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientDetail = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).ListPatientDetails;
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
            return PatientDetail;
        }


        public async Task<string> DeleteService(string ServiceId, string UserId)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteService/" + ServiceId + "/" + UserId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ServicesServiceProxy>(json).Result;
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

        public async Task<PatientsDetail> GetPatientDetailById(string PatientDetailId)
        {

            PatientsDetail PatientDetails = new PatientsDetail();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetPatientDetailById/" + PatientDetailId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    PatientDetails = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).PatientDetail;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception)
            {
                //ErrorLog.LogError(ex);
            }
            return PatientDetails;
        }

        public async Task<string> EditPatientDetails(PatientsDetail PatientDetails)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "EditPatientDetails", new { PatientDetails }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).Result;
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

        public async Task<string> DeletePatientDetail(string PatientDetailId,string UserID)
        {

            string result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeletePatientDetail/" + PatientDetailId + "/"+ UserID, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).Result;
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

        public async Task<bool> IsMedicalIdExist(PatientsDetail PatientsDetail)
        {

            bool result = false;
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "IsMedicalIdExist", new { PatientsDetail }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PatientDetailsServiceProxy>(json).ResultInBool;
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


    }

}