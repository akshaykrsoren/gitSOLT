using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaregiverLite.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CaregiverLite.Controllers
{
    public class AddSuperAdminLoginController : Controller
    {
        string ConStringCareGiverNew = @"Data Source=pasevalite.cwbk1q3wsvsy.us-west-2.rds.amazonaws.com;Initial Catalog=CaregiverLite;User ID=pasevalite_sa;Password=Pa53vAL!TeD82oi7";
        // string ConStringCareGiverNew = @"Data Source=LAPTOP-JTSC0UIS;Initial Catalog=CaregiverLite;User ID=sa;Password=1234";
        // GET: AddSuperAdminLogin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string txtAddSuperAdminLoginUser, string txtAddSuperAdminPassword)
        {
            Session["AddSuperAdminSession"] = null;

            if (txtAddSuperAdminLoginUser == "SuperAdmin" && txtAddSuperAdminPassword == "Df&6897!23#@")
            {
                Session["AddSuperAdminSession"] = "SuperAdmin";
            }

            if (Session["AddSuperAdminSession"].ToString() == "SuperAdmin")
            {
                return RedirectToAction("AddUsersToRoles");

            }

            return View();
        }


        public ActionResult AddUsersToRoles()
        {

            if (Session["AddSuperAdminSession"] == null || string.IsNullOrEmpty(Session["AddSuperAdminSession"].ToString()))
            {
                return RedirectToAction("Index");
            }

            else
            {

                AddSuperAdminLoginDetails objAddSuperAdminLoginDetails = new AddSuperAdminLoginDetails();
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConStringCareGiverNew))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllUserAdminInfo_Vin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        List<AddSuperAdminLoginDetails> AdminLoginlist = new List<AddSuperAdminLoginDetails>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            AddSuperAdminLoginDetails uobj = new AddSuperAdminLoginDetails();
                            uobj.UserId = ds.Tables[0].Rows[i]["UserId"].ToString();
                            // uobj.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                            uobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                            // uobj.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                            uobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();

                            uobj.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();

                            uobj.RoleId = ds.Tables[0].Rows[i]["RoleId"].ToString();

                            AdminLoginlist.Add(uobj);
                        }
                        objAddSuperAdminLoginDetails.AddSuperAdminLoginInfo = AdminLoginlist;
                    }
                    con.Close();
                }
                return View(objAddSuperAdminLoginDetails);
            }

        }


        //Assign Role to User


        public string AssignRolerToUser(string UserId)
        {

            string result = "";

            using (SqlConnection con = new SqlConnection(ConStringCareGiverNew))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateUserToSuperAdmin_Vin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            result = "Success";
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            
            }

            return result;
        }




        //Reassign Role to User


        public string ReassignRoleToUser(string UserId, string Email)
        {

            string result = "";

            using (SqlConnection con = new SqlConnection(ConStringCareGiverNew))
            {

                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateUserToReassignSuperAdmin_Vin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@Email", Email);

                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            result = "Success";
                        }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            return result;

        }




    }
}