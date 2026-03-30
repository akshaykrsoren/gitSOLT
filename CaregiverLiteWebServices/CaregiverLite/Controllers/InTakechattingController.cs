using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaregiverLiteWCF;
using CaregiverLiteWCF.Class;
using System.Web.Security;

namespace CaregiverLite.Controllers
{
    public class InTakechattingController : Controller
    {
        // GET: InTakechatting
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public void InTakePatientChatListFrmChantMemberTbl(int Id) // providing the Intake patients list
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Get_InTake_PatientDetailFrmChattingGrpTbl", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //Session["ChattingGroupId"] = dt.Rows[0]["ChattingGroupId"];
                //Session["DialogId"] = dt.Rows[0]["DialogId"];
                //Session["GroupName"] = dt.Rows[0]["GroupName"];
                //Session["OfficeId"] = dt.Rows[0]["OfficeId"];
                //Session["QBID"] = dt.Rows[0]["QBID"];
                ViewBag.ChattingGroupId = dt.Rows[0]["ChattingGroupId"];
                ViewBag.DialogId = dt.Rows[0]["DialogId"];
                ViewBag.GroupName = dt.Rows[0]["GroupName"];
                ViewBag.OfficeId = dt.Rows[0]["OfficeId"];
                ViewBag.QBID = dt.Rows[0]["QBID"];

                msg = JsonConvert.SerializeObject(dt);
                //return Json(msg, JsonRequestBehavior.AllowGet);
                conn.Close();
            }
            catch (Exception ex)
            {
                // msg = "failed";
            }
           // return Json(msg);
        }
        public ActionResult InTakePatientChatDetailPage(int Id)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                MembershipUser user = Membership.GetUser();
                string LogInUserId = user.ProviderUserKey.ToString();

                ViewBag.GroupMemberDetails =  GetGroupMemberDetail(Id, "0");
                DataTable LoginUserQBIDFromUserID = GetLoginUserQBID(LogInUserId);

                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("PR_Get_InTake_PatientDetailFrmChattingGrpTbl", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //Session["ChattingGroupId"] = dt.Rows[0]["ChattingGroupId"];
                //Session["DialogId"] = dt.Rows[0]["DialogId"];
                //Session["GroupName"] = dt.Rows[0]["GroupName"];
                //Session["OfficeId"] = dt.Rows[0]["OfficeId"];
                //Session["QBID"] = LoginUserQBIDFromUserID.Rows[0]["QuickBloxId"];
                ViewBag.ChattingGroupId = dt.Rows[0]["ChattingGroupId"];
                ViewBag.DialogId = dt.Rows[0]["DialogId"];
                ViewBag.GroupName = dt.Rows[0]["GroupName"];
                ViewBag.OfficeId = dt.Rows[0]["OfficeId"];
                ViewBag.QBID = LoginUserQBIDFromUserID.Rows[0]["QuickBloxId"];

                msg = JsonConvert.SerializeObject(dt);
                
                conn.Close();
            }
            catch (Exception ex)
            {
                // msg = "failed";
            }
            finally
            {
                conn.Close();
            }

            return View();
        }

        public DataTable GetLoginUserQBID(string userid)
        {
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("GetQuickBloxDetByUserId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userid;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
            return dt;
        }

        public List<ChattingGroupMember> GetGroupMemberDetail(int ID, string OrganisationId)
        {
            var objGroupMemberDetailListing = new List<ChattingGroupMember>();
            string msg = "";
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connectionString = ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString;
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                cmd = new SqlCommand("ORG_GetChattingGroupMemberByChattingGroupId", conn);  //Get_Intake_GroupMemberList
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = ID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    var member = new ChattingGroupMember
                    {
                        ChattingGroupMemberId = row["ChattingGroupMemberId"] != DBNull.Value ? Convert.ToInt32(row["ChattingGroupMemberId"]) : 0,
                        ChattingGroupId = row["ChattingGroupId"] != DBNull.Value ? Convert.ToInt32(row["ChattingGroupId"]) : 0,
                        UserId = row["UserId"] != DBNull.Value ? Convert.ToString(row["UserId"]) : "0",
                        QuickBloxId = row["QBID"] != DBNull.Value ? row["QBID"].ToString() : "",
                        MemberName = row["Name"] != DBNull.Value ? row["Name"].ToString() : "",
                        MemberId = row["ChattingGroupMemberId"] != DBNull.Value ? row["ChattingGroupMemberId"].ToString() : "",
                        Email = row["EmailId"] != DBNull.Value ? row["EmailId"].ToString() : "",
                        Type = row["Type"] != DBNull.Value ? row["Type"].ToString() : "",
                    };
                    objGroupMemberDetailListing.Add(member);
                }
               // msg = JsonConvert.SerializeObject(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                msg = "failed";
            }
          finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return objGroupMemberDetailListing;
        }



      /*  private const string AppId = "59230";
        private const string AuthKey = "SV2czdXSOafbMNm";
        private const string AuthSecret = "pru2MGmJxj7zedX";
        private const string ApiEndpoint = "https://api.quickblox.com";
        public async Task<ActionResult> RemoveDialog()
        {
            await DeleteDialogAsync("684023e5e7affd00140bca2a");
            return Content("Dialog deleted (if token was valid).");
        }

        // ✅ Deletes the Dialog
        private static async Task DeleteDialogAsync(string dialogId)
        {
            string token = await GetAdminTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("QB-Token", token);

                    HttpResponseMessage response = await client.DeleteAsync($"{ApiEndpoint}/chat/Dialog/{dialogId}.json");

                    string body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"✅ Dialog {dialogId} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Failed to delete dialog. Status: {response.StatusCode}, Reason: {body}");
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Failed to obtain admin token.");
            }
        }

        // ✅ Get Admin Token
        private static async Task<string> GetAdminTokenAsync()
        {
            //var nonce = new Random().Next(10000, 99999);
            //var timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; //DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            int nonce = new Random().Next(10000, 99999);
            int timestamp = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

            Console.WriteLine("Nonce: " + nonce);
            Console.WriteLine("Timestamp: " + timestamp);

            string signatureString = $"application_id={AppId}&auth_key={AuthKey}&nonce={nonce}&timestamp={timestamp}";
            string signature = GenerateHmacSha1(signatureString, AuthSecret);

            var postData = new Dictionary<string, string>
        {
            { "application_id", AppId },
            { "auth_key", AuthKey },
            { "timestamp", timestamp.ToString() },
            { "nonce", nonce.ToString() },
            { "signature", signature }
        };

            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(postData);
                var response = await client.PostAsync($"{ApiEndpoint}/session.json", content);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic session = JsonConvert.DeserializeObject(body);
                    return session.session.token;
                }
                else
                {
                    Console.WriteLine("Token request failed: " + body);
                    return null;
                }
            }
        }

        // ✅ Signature generator
        private static string GenerateHmacSha1(string input, string key)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        } */
    }
}