using System;
using System.Collections.Generic;
using System.Linq;
using CaregiverLite.Models;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Web.Security;


namespace CaregiverLite.Controllers
{
    public class OpenMapFromExcelLinkController : Controller
    {
        // GET: OpenMapFromExcelLink
        public ActionResult Index(int id)
        {            
            ViewData["hdnFlag"] = id;
            return View();            
        }



        //public string SendPDFDataWithAttch(pdfdata pdfdata)
        //{

        //    string SupplyReportPdf = "";
        //    string Result = "";

        //    string strPDFFileName = string.Format("Nightingle_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "-" + ".pdf");

        //    string strAttachment = Server.MapPath("~/Downloadss/NightingleForm/" + strPDFFileName);

        //    byte[] bytes = Convert.FromBase64String(pdfdata.PdfFile);

        //    System.IO.FileStream stream1 = new FileStream(strAttachment, FileMode.CreateNew);
        //    System.IO.BinaryWriter writer1 = new BinaryWriter(stream1);
        //    writer1.Write(bytes, 0, bytes.Length);
        //    writer1.Close();

        //    string AttachmentFileName = strAttachment;
        //    string toAddress = ConfigurationManager.AppSettings["NightingleMail"].ToString();

        //    string subject = "Mail";
        //    bool IsFileAttachment = true;
        //    string body = " ";

        //    string CCMailID = ConfigurationManager.AppSettings["NightingleCCmail"].ToString();

        //    bool isBodyHtml = true;

        //    if (sendNightingleFormFormEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
        //    {
        //        return Result = "Success";
        //    }
        //    else
        //    {
        //        return Result = "fail";
        //    }
        //    return Result;

        //}


        //public bool sendNightingleFormFormEmailWithAttachment(string toAddress, string subject, string body, bool IsFileAttachment, string AttachmentFileName, string CCMailID, bool isBodyHtml = true)
        //{
        //    try
        //    {
        //        var mailMessage = new MailMessage();
        //        mailMessage.To.Add(toAddress);

        //        if (!(string.IsNullOrEmpty(CCMailID)))
        //        {
        //            mailMessage.CC.Add(CCMailID);
        //        }

        //        MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["NightingleOutLookMail"].ToString());
        //        mailMessage.From = ma;

        //        mailMessage.Subject = subject;

        //        if (IsFileAttachment == true)
        //        {
        //            if (!string.IsNullOrEmpty(AttachmentFileName))
        //            {
        //                Attachment attachFile = new Attachment(AttachmentFileName);
        //                mailMessage.Attachments.Add(attachFile);
        //            }
        //        }

        //        mailMessage.Body = body;
        //        mailMessage.IsBodyHtml = isBodyHtml;

        //        var smtpClient = new SmtpClient { EnableSsl = false };
        //        smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
        //        smtpClient.Host = ConfigurationManager.AppSettings["NightingleSMTPHostOutlook"];

        //        smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        //        smtpClient.UseDefaultCredentials = true;

        //        smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["NightingleOutLookMail"].ToString(), ConfigurationManager.AppSettings["NightingleOutlookPassword"].ToString());
        //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtpClient.Send(mailMessage);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return false;
        //}


        //public string Unsubscribeuser(string email, string reason)
        //{
        //    string result = "";

        //    try
        //    {
        //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
        //        SqlCommand cmd = new SqlCommand("InsertUpadateUserSubscription", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserEmail", email);
        //        cmd.Parameters.AddWithValue("@SubscriptionStatus", "0");
        //        cmd.Parameters.AddWithValue("@Reason", reason);
        //        con.Open();
        //        int i = cmd.ExecuteNonQuery();
        //        if (i > 0)
        //        {
        //            result = "Successfully Unsubscribed";
        //        }
        //        else
        //        {
        //            result = "Failed";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Failed";
        //    }
        //    return result;
        //}


        public JsonResult SendPDFDataWithAttch(pdfdata pdfdata, string Email, int Score)
        {
            RootResponse response = new RootResponse();
            string body = "";
            try
            {
                string strPDFFileName = string.Format("Nightingle_InService" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "-" + ".pdf");

                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                byte[] bytes = Convert.FromBase64String(pdfdata.PdfFile);

                System.IO.FileStream stream1 = new FileStream(strAttachment, FileMode.CreateNew);
                System.IO.BinaryWriter writer1 = new BinaryWriter(stream1);
                writer1.Write(bytes, 0, bytes.Length);
                writer1.Close();

                string AttachmentFileName = strAttachment;
                string toAddress = Email;
                string subject = "InService Quiz Form";
                bool IsFileAttachment = true;
                if (Score < 8)
                {
                    if (Score == 0)
                    {
                        body = "You have scored  0% and you will need to take the test again.";
                    }
                    else
                    {
                        body = "You have scored " + Score + "0% and you will need to take the test again.";
                    }

                }
                else
                {
                    body = "Congratulations you have passed the test and you scored " + Score + "0%.";
                }
               // string CCMailID = null;// ConfigurationManager.AppSettings["NightingleFromMail"].ToString();

                string CCMailID = ConfigurationManager.AppSettings["NightingleOutLookCCMail"].ToString();


                bool isBodyHtml = true;

                if (sendNightingleFormFormEmailWithAttachment(toAddress, subject, body, IsFileAttachment, AttachmentFileName, CCMailID, isBodyHtml))
                {
                    response.success = 1;
                    response.message = "Success";
                }
                else
                {
                    response.success = 0;
                    response.message = "Fail";
                }
            }
            catch (Exception ex)
            {
                response.success = 0;
                response.message = "Fail";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public bool sendNightingleFormFormEmailWithAttachment(string toAddress, string subject, string body, bool IsFileAttachment, string AttachmentFileName, string CCMailID, bool isBodyHtml = true)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls |
                                       SecurityProtocolType.Ssl3;

                var mailMessage = new MailMessage();
                mailMessage.To.Add(toAddress);

                if (!(string.IsNullOrEmpty(CCMailID)))
                {
                    mailMessage.CC.Add(CCMailID);
                }

                string BccMail = ConfigurationManager.AppSettings["NightingleOutLookBCCMail"].ToString();
                if (!(string.IsNullOrEmpty(BccMail)))
                {
                    mailMessage.Bcc.Add(BccMail);
                }


                MailAddress ma = new MailAddress(ConfigurationManager.AppSettings["NightingleOutLookMail"].ToString());
                mailMessage.From = ma;

                mailMessage.Subject = subject;

                if (IsFileAttachment == true)
                {
                    if (!string.IsNullOrEmpty(AttachmentFileName))
                    {
                        Attachment attachFile = new Attachment(AttachmentFileName);
                        mailMessage.Attachments.Add(attachFile);
                    }
                }

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                var smtpClient = new SmtpClient { EnableSsl = false };
                smtpClient.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Host = ConfigurationManager.AppSettings["NightingleSMTPHostOutlook"];
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["NightingleOutLookMail"].ToString(), ConfigurationManager.AppSettings["NightingleOutlookPassword"].ToString());
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public string Unsubscribeuser(string email, string reason)
        {
            string result = "";

            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                SqlCommand cmd = new SqlCommand("InsertUpadateUserSubscription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserEmail", email);
                cmd.Parameters.AddWithValue("@SubscriptionStatus", "0");
                cmd.Parameters.AddWithValue("@Reason", reason);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    result = "Successfully Unsubscribed";
                }
                else
                {
                    result = "Failed";
                }
            }
            catch (Exception ex)
            {
                result = "Failed";
            }
            return result;
        }


        public JsonResult UpdateUserLoginPassword(string UserName, string NewPassword)
        {
            RootResponse response = new RootResponse();
            try
            {
                string userid = "";
                if (!String.IsNullOrEmpty(NewPassword))
                {
                    MembershipUser mUser = Membership.GetUser(UserName);
                    if (mUser != null)
                    {
                        userid = mUser.ProviderUserKey.ToString();
                        mUser.ChangePassword(mUser.ResetPassword(), NewPassword);
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ToString());
                        SqlCommand cmd = new SqlCommand("ChangePassword", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        cmd.Parameters.AddWithValue("@UserId", userid);
                        cmd.Parameters.AddWithValue("@Password", NewPassword);

                        int i = cmd.ExecuteNonQuery();

                        if (i > 0)
                        {
                            response.success = 1;
                            response.message = "Success";
                        }
                        else
                        {
                            response.success = 0;
                            response.message = "Failed";
                        }
                    }
                    else
                    {
                        response.success = 0;
                        response.message = "No User Found";
                    }
                }
                else
                {
                    response.success = 0;
                    response.message = "Please Provide New Password";
                }
            }

            catch (Exception ex)
            {
                response.success = 0;
                response.message = ex.Message;

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}