using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EWebStore.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace EWebStore.Controllers
{
    public class AccountController : Controller
    {
        readonly string cs = ConfigurationManager.ConnectionStrings["AutoDB"].ConnectionString.ToString();

        [HttpPost]
        public ActionResult Register()
        {
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spRegister", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CUSTOMER_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("FIRSTNAME", Request.Form["firstname"]);
                cmd.Parameters.AddWithValue("LASTNAME", Request.Form["lastname"]);
                cmd.Parameters.AddWithValue("USERNAME", Request.Form["username"]);
                cmd.Parameters.AddWithValue("PASSWRD", Request.Form["password"]);
                cmd.Parameters.AddWithValue("EMAIL", Request.Form["email"]);
                cmd.Parameters.AddWithValue("PHONE", Request.Form["phone"]);
                cmd.Parameters.AddWithValue("SEX", Request.Form["sex"]);
                con.Open();
                var cron = cmd.ExecuteNonQuery();
                if (cron > 0)
                {
                    var msg = new Messages
                    {
                        Message = "Registration Successful. Please login.",
                        type = "success"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Registration failed. Please Try Again.",
                        type = "failed"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception)
            {
                var msg = new Messages
                {
                    Message = "Registration failed. Please Try Again.",
                    type = "error"
                };
                TempData["alertMessage"] = msg;
                return RedirectToAction("Index", "Home");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(cs);
                MySqlCommand cmd = new MySqlCommand("spLogin", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ENTRY", email);
                cmd.Parameters.AddWithValue("PASSWRD", password);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstColumn = reader.GetName(0).ToString();
                    if (firstColumn == "MSG")
                    {
                        var messages = new Messages
                        {
                            Message = "Incorrect Login Credentials.",
                            type = "Error"
                        };
                        TempData["alertMessage"] = messages;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (firstColumn == "Role")
                    {
                        var user = new UserDetails()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Role = reader["Role"].ToString(),
                            Username = reader["Username"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = long.Parse(reader["Phone"].ToString()),
                            Sex = reader["Sex"].ToString(),
                            Trans_ID = reader["Trans_ID"].ToString()
                        };
                        Session["UserDetails"] = user;
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (firstColumn == "ID")
                    {
                        var user = new UserDetails()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Username = reader["Username"].ToString(),
                            Lastname = reader["Lastname"].ToString(),
                            Firstname = reader["Firstname"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = long.Parse(reader["Phone"].ToString()),
                            Sex = reader["Sex"].ToString(),
                            Trans_ID = reader["Trans_ID"].ToString()
                        };
                        Session["UserDetails"] = user;
                        return RedirectToAction("Index", "Home");
                    }
                }

                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public ActionResult UpdatePassword()
        {
            string route;
            var userDetails = Session["UserDetails"] as UserDetails;
            if (userDetails.Trans_ID[0] == 'A')
            {
                route = "Admin";
            }
            else
            {
                route = "Home";
            }
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spRegister", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID);
                cmd.Parameters.AddWithValue("FIRSTNAME", DBNull.Value);
                cmd.Parameters.AddWithValue("LASTNAME", DBNull.Value);
                cmd.Parameters.AddWithValue("USERNAME", DBNull.Value);
                cmd.Parameters.AddWithValue("PASSWRD", Request.Form["password"]);
                cmd.Parameters.AddWithValue("EMAIL", DBNull.Value);
                cmd.Parameters.AddWithValue("PHONE", DBNull.Value);
                cmd.Parameters.AddWithValue("SEX", DBNull.Value);
                con.Open();
                var cron = cmd.ExecuteNonQuery();
                if (cron > 0)
                {
                    var msg = new Messages
                    {
                        Message = "Operation Successful.",
                        type = "success"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Profile", route);
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Operation failed.",
                        type = "failed"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Profile", route);
                }

            }
            catch (Exception)
            {
                var msg = new Messages
                {
                    Message = "Error. Please Try Again.",
                    type = "error"
                };
                TempData["alertMessage"] = msg;
                return RedirectToAction("Profile", route);
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost]
        public ActionResult UpdateProfile()
        {
            string route;
            var userDetails = Session["UserDetails"] as UserDetails;
            if (userDetails.Trans_ID[0] == 'A')
            {
                route = "Admin";
            }
            else
            {
                route = "Home";
            }
            MySqlConnection con = new MySqlConnection(cs);

            try
            {
               /* TransID, UserID, ROLE, FIRSTNAME, LASTNAME, EMAIL, PHONE, PASSWRD, USERNAME*/
                MySqlCommand cmd = new MySqlCommand("spRegister", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID);
                cmd.Parameters.AddWithValue("FIRSTNAME", Request.Form["firstname"]);
                cmd.Parameters.AddWithValue("LASTNAME", Request.Form["lastname"]);
                cmd.Parameters.AddWithValue("USERNAME", DBNull.Value);
                cmd.Parameters.AddWithValue("PASSWRD", DBNull.Value);
                cmd.Parameters.AddWithValue("EMAIL", DBNull.Value);
                cmd.Parameters.AddWithValue("PHONE", Request.Form["phone"]);
                cmd.Parameters.AddWithValue("SEX", Request.Form["sex"]);
                con.Open();
                var cron = cmd.ExecuteNonQuery();
                if (cron > 0)
                {
                    var msg = new Messages
                    {
                        Message = "Update Successful.",
                        type = "success"
                    };
                    ViewBag.msg = msg;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string firstColumn = reader.GetName(0).ToString();
                        if (firstColumn == "MSG")
                        {
                            var messages = new Messages
                            {
                                Message = "Operation failed.",
                                type = "failed"
                            };
                            ViewBag.msg = messages;
                            return RedirectToAction("Profile", route);
                        }
                        else if (firstColumn == "Role")
                        {
                            var user = new UserDetails()
                            {
                                ID = int.Parse(reader["ID"].ToString()),
                                Role = reader["Role"].ToString(),
                                Username = reader["Username"].ToString(),
                                Lastname = reader["Lastname"].ToString(),
                                Firstname = reader["Firstname"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = long.Parse(reader["Phone"].ToString()),
                                Sex = reader["Sex"].ToString(),
                                Trans_ID = reader["Trans_ID"].ToString()
                            };
                            Session["UserDetails"] = user;
                            return RedirectToAction("Profile", route);
                        }
                        else if (firstColumn == "ID")
                        {
                            var user = new UserDetails()
                            {
                                ID = int.Parse(reader["ID"].ToString()),
                                Username = reader["Username"].ToString(),
                                Lastname = reader["Lastname"].ToString(),
                                Firstname = reader["Firstname"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = long.Parse(reader["Phone"].ToString()),
                                Sex = reader["Sex"].ToString(),
                                Trans_ID = reader["Trans_ID"].ToString()
                            };
                            Session["UserDetails"] = user;
                            return RedirectToAction("Profile", route);
                        }

                    }
                    var msgs = new Messages
                    {
                        Message = "Update failed. Please Try Again.",
                        type = "failed"
                    };
                    ViewBag.msg = msgs;
                    return RedirectToAction("Profile", route);
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Update failed. Please Try Again.",
                        type = "failed"
                    };
                    ViewBag.msg = msg;
                    return RedirectToAction("Profile", route);
                }

            }
            catch (Exception)
            {
                var msg = new Messages
                {
                    Message = "Error. Please Try Again.",
                    type = "error"
                };
                TempData["alertMessage"] = msg;
                return RedirectToAction("Profile", route);
            }
            finally
            {
                con.Close();
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("UserDetails");
            Session.Remove("AddressList");
            return View("Index");
        }

        public void Intel()
        {
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UNAME", "mARTq");
                cmd.Parameters.AddWithValue("SNAME", DBNull.Value);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                string firstrow = reader.GetName(0).ToString();
                string firstColumn = reader.GetName(0).ToString();
                if (firstColumn == "MSG")
                {
                    var messages = new Messages
                    {
                        Message = "Error",
                        type = "Error"
                    };
                    TempData["alertMessage"] = messages;
                }
                
                else
                { 
                    var inseter = new List<inset>();
                    var inset = new inset();
                    int amt = reader.FieldCount;
                    //car_category_code, car_category_name
                    while (reader.Read())
                    {
                        var inste = new inset
                        {
                            username = reader.GetValue(1).ToString(),
                            surname = reader.GetValue(2).ToString()
                        };
                        inseter.Add(inste);
                        
                        // get the list of addresses associated with the customer
                        var addresslist = new List<inset>()
                        {
                            new inset{ username = reader["car_category_code"].ToString(),  surname = reader["car_category_name"].ToString() }
                        };
                        foreach (var ins in addresslist)
                        {
                            inseter.Add(ins);
                        }
                        Session["AddressList"] = inseter;
                    }
                }



                /*=======================================================*/
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;

            }
            catch (Exception)
            {
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
            }
            finally
            {
                con.Close();
            }
        }
    }
}