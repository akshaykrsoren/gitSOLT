using CaregiverLiteWCF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CaregiverLite.Models
{
    public class CareGiverTimeSlotModel
    {
    }

    public class TimeSlotServiceProxy : CaregiverLiteBaseService
    {
        public List<CareGiverTimeSlots> TimeSlotsList { get; set; }
        public string Result { get; set; }

        public TimeSlotServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<List<CareGiverTimeSlots>> GetTimeSlotByNurseId(string NurseId, string Week, string Year)
        {

            List<CareGiverTimeSlots> TimeSlotsList = new List<CareGiverTimeSlots>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetTimeSlotByNurseId/" + NurseId + "/" + Week + "/" + Year, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    TimeSlotsList = JsonConvert.DeserializeObject<TimeSlotServiceProxy>(json).TimeSlotsList;
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
            return TimeSlotsList;
        }

        public async Task<string> InsertTimeSlotForNurse(CareGiverTimeSlots CareGiverTimeSlot)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertTimeSlotForNurse", new { CareGiverTimeSlot }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<TimeSlotServiceProxy>(json).Result;
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
        public async Task<string> DeleteTimeSlotByTimeSlotId(CareGiverTimeSlots CareGiverTimeSlot)
        {

            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteTimeSlotByTimeSlotId", new { CareGiverTimeSlot }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<TimeSlotServiceProxy>(json).Result;
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
    }

    public class ScheduleServiceProxy : CaregiverLiteBaseService
    {
        public List<CareGiverSchedule> ScheduleList { get; set; }
        public string Result { get; set; }

        public ScheduleServiceProxy()
        {
            rootSuffix = "CaregiverLiteService.svc/";
        }

        public async Task<List<CareGiverSchedule>> GetScheduleByTimeSlotId(string TimeSlotId)
        {

            List<CareGiverSchedule> ScheduleList = new List<CareGiverSchedule>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetScheduleByTimeSlotId/" + TimeSlotId, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ScheduleList = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).ScheduleList;
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
            return ScheduleList;
        }

        public async Task<List<CareGiverSchedule>> GetAppointmentByTimeSlotId(string TimeSlotId, string SlotDate)
        {

            List<CareGiverSchedule> ScheduleList = new List<CareGiverSchedule>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentByTimeSlotId/" + TimeSlotId + "/" + SlotDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ScheduleList = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).ScheduleList;
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
            return ScheduleList;
        }



        public async Task<List<CareGiverSchedule>> GetAppointmentsForUnavailability(string TimeSlotId, string SlotDate)
        {

            List<CareGiverSchedule> ScheduleList = new List<CareGiverSchedule>();
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "GetAppointmentsForUnavailability/" + TimeSlotId + "/" + SlotDate, this.cancellationToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    ScheduleList = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).ScheduleList;
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
            return ScheduleList;
        }


        public async Task<string> UpdateNurseSchedule(CareGiverMultipleTimeSlots CareGiverMultipleTimeSlots)
        {
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response;
                response = this.client.PostAsJsonAsync(rootSuffix + "UpdateNurseSchedule", new { objCareGiverMultipleTimeSlots = CareGiverMultipleTimeSlots }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).Result;
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

        public async Task<string> InsertScheduleForNurse(CareGiverSchedule CareGiverSchedule)
        {
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "InsertScheduleForNurse", new { CareGiverSchedule }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).Result;
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

        public async Task<string> DeleteScheduleByTimeSlotId(CareGiverSchedule CareGiverSchedule)
        {
            string Result = "";
            try
            {
                var json = "";
                // Send request to server
                HttpResponseMessage response = this.client.PostAsJsonAsync(rootSuffix + "DeleteScheduleByTimeSlotId", new { CareGiverSchedule }).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    json = await response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<ScheduleServiceProxy>(json).Result;
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
    }
}