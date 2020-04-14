using EWebStore.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWebStore.Controllers
{
    public class HomeController : Controller
    {
        readonly string cs = ConfigurationManager.ConnectionStrings["AutoDB"].ConnectionString.ToString();
        public ActionResult Index()
        {
            if (TempData["alertMessage"] != null)
            {
                var msg = TempData["alertMessage"] as Messages;
                ViewBag.msg = msg;
            }
            return View();
        }

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
                    return RedirectToAction("Index");
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Registration failed. Please Try Again.",
                        type = "failed"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            //ID, Username, Firstname, Lastname, Email, Phone, Sex, Trans_ID
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
                        return RedirectToAction("Index");
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
                return RedirectToAction("Index");
             
            }
            catch (Exception)
            {
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
                return RedirectToAction("Index");
            } 

        }

        [HttpPost]
        public ActionResult UpdatePassword()
        {
            var userDetails = Session["UserDetails"] as UserDetails;
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spRegister", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID DBNull.Value);
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
                    return RedirectToAction("Index");
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Registration failed. Please Try Again.",
                        type = "failed"
                    };
                    TempData["alertMessage"] = msg;
                    return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }
            finally
            {
                con.Close();
            }
        }

        [HttpPost]
        public void UpdateProfile()
        {
            var userDetails = Session["UserDetails"] as UserDetails;
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
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
                    TempData["alertMessage"] = msg;
                    var user = new UserDetails
                    {

                    }
                }
                else
                {
                    var msg = new Messages
                    {
                        Message = "Update failed. Please Try Again.",
                        type = "failed"
                    };
                    TempData["alertMessage"] = msg;
                }

            }
            catch (Exception)
            {
                var msg = new Messages
                {
                    Message = "Updaate failed. Please Try Again.",
                    type = "error"
                };
                TempData["alertMessage"] = msg;
            }
            finally
            {
                con.Close();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }

        public ActionResult Logins(string email, string password)
        {
            var user = new UserDetails()
            {
                ID = 1,
                Username = password,
                Lastname = "west",
                Firstname = "urban",
                Email = email,
                Phone = 090528019874,
                Sex = "male",
                Trans_ID = "Exyc123456789"
            };
            Session["UserDetails"] = user;
            var userDetails = Session["UserDetails"] as UserDetails;
            return (userDetails.Trans_ID[0] == 'A') ? RedirectToAction("Index", "Admin") :
                (userDetails.Trans_ID[0] == 'E') ? RedirectToAction("Index", this) :
                RedirectToAction("Index", new { message = "Error! Incorrect login credentials." });
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Products(int? id)
        {
            //id = user id to get all vehicles bought and rented
            return View();
        }

        public ActionResult Shop(Vehicles vehicles)
        {
            //if vehicles is not null
            //ViewBag.Title = "result";
            //else 
            ViewBag.Title = "Car Shop";
            //pass similar vehicles in a tempdata
            return View(vehicles);
        }

        public ActionResult Review()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Vehicle(int? id)
        {
            return View();
        }

        public ActionResult Confirmation(string result)
        {
            //if result is success then 
            ViewData["Transaction"] = result;
            return View();
        }
        public new ActionResult Profile(int? id)
        {
            //if userid exists then show else throw a forbidden error message
            return View();
        }
    }
}