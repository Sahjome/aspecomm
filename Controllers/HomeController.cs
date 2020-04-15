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
        
        public new ActionResult Profile()
        {
            //if userid exists then show else throw a forbidden error message
            if (TempData["alertMessage"] != null)
            {
                var msg = TempData["alertMessage"] as Messages;
                ViewBag.msg = msg;
            }
            if (Session["UserDetails"] is null)
            {
                return View("Index");
            }
            else
            {
                var userDetails = Session["UserDetails"] as UserDetails;
                MySqlConnection con = new MySqlConnection(cs);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("spAddress", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ADDRESS_ID", DBNull.Value);
                    cmd.Parameters.AddWithValue("TRANSID", DBNull.Value);
                    cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID);
                    cmd.Parameters.AddWithValue("ADDRESS", DBNull.Value);
                    cmd.Parameters.AddWithValue("CITY", DBNull.Value);
                    cmd.Parameters.AddWithValue("STATE", DBNull.Value);
                    cmd.Parameters.AddWithValue("CODES", DBNull.Value);
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string firstColumn = reader.GetName(0).ToString();
                        if (firstColumn == "MSG")
                        {
                            var messages = new Messages
                            {
                                Message = "Error",
                                type = "Error"
                            };
                            TempData["alertMessage"] = messages;
                            return View();
                        }
                        else
                        {
                            var addresses = new List<AddressList>();
                            while (reader.Read())
                            {
                                // get the list of addresses associated with the customer
                                var addresslist = new AddressList()
                                {
                                    id = int.Parse(reader["address_ID"].ToString()),
                                    line = reader["address_line"].ToString(),
                                    city = reader["town_city"].ToString(),
                                    state = reader["state"].ToString(),
                                    code = int.Parse(reader["post_zip_code"].ToString())
                                };
                                addresses.Add(addresslist);
                                Session["AddressList"] = addresses;
                            }
                            return View();
                        }
                    }
                    return View();
                }
                catch (Exception)
                {
                    var message = new Messages
                    {
                        Message = "Error Please try again.",
                        type = "Error"
                    };
                    ViewBag.msg = message;
                    return View();
                }
                finally
                {
                    con.Close();
                }
            }   
        }

        public ActionResult Address(int? id)
        {
            //var addressList = Session["AddressList"] as AddressList;
            var userDetails = Session["UserDetails"] as UserDetails;
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spAddress", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ADDRESS_ID", id);
                cmd.Parameters.AddWithValue("TRANSID", DBNull.Value);
                cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID);
                cmd.Parameters.AddWithValue("ADDRESS", Request.Form["line"]);
                cmd.Parameters.AddWithValue("CITY", Request.Form["city"]);
                cmd.Parameters.AddWithValue("STATE", Request.Form["state"]);
                cmd.Parameters.AddWithValue("CODES", Request.Form["code"]);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstrow = reader.GetName(0).ToString();
                    //reader.HasRows;
                    string firstColumn = reader.GetName(0).ToString();
                    if (firstColumn == "MSG")
                    {
                        var messages = new Messages
                        {
                            Message = "Error",
                            type = "Error"
                        };
                        TempData["alertMessage"] = messages;
                        return View();
                    }
                    else
                    //address_ID	customer_ID	address_line	town_city	state	post_zip_code
                    {
                        // get the list of addresses associated with the customer
                        var addresslist = new AddressList()
                        {
                            id = int.Parse(reader["address_ID"].ToString()),
                            line = reader["address_line"].ToString(),
                            city = reader["town_city"].ToString(),
                            state = reader["state"].ToString(),
                            code = int.Parse(reader["post_zip_code"].ToString())
                        };
                        Session["AddressList"] = addresslist;
                        return View();
                    }
                }

                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
                return View();

            }
            catch (Exception)
            {
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                TempData["alertMessage"] = message;
                return View();
            }
            finally
            {
                con.Close();
            }
        }
    
        [HttpDelete]
        public ActionResult AddressDelete(int id)
        {
            var userDetails = Session["UserDetails"] as UserDetails;
            MySqlConnection con = new MySqlConnection(cs);
            try
            {
                MySqlCommand cmd = new MySqlCommand("spAddress", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ADDRESS_ID", id);
                cmd.Parameters.AddWithValue("TRANSID", DBNull.Value);
                cmd.Parameters.AddWithValue("CUSTOMER_ID", userDetails.ID);
                cmd.Parameters.AddWithValue("ADDRESS", DBNull.Value);
                cmd.Parameters.AddWithValue("CITY", DBNull.Value);
                cmd.Parameters.AddWithValue("STATE", DBNull.Value);
                cmd.Parameters.AddWithValue("CODES", DBNull.Value);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string firstrow = reader.GetName(0).ToString();
                    //reader.HasRows;
                    string firstColumn = reader.GetName(0).ToString();
                    if (firstColumn == "MSG")
                    {
                        var messages = new Messages
                        {
                            Message = "Error",
                            type = "Error"
                        };
                        ViewBag.msg = messages;
                        return View();
                    }
                    else
                    {
                        // get the list<addresses> associated with the customer
                        var addresslist = new AddressList()
                        {
                            id = int.Parse(reader["address_ID"].ToString()),
                            line = reader["address_line"].ToString(),
                            city = reader["town_city"].ToString(),
                            state = reader["state"].ToString(),
                            code = int.Parse(reader["post_zip_code"].ToString())
                        };
                        Session["AddressList"] = addresslist;
                        var messages = new Messages
                        {
                            Message = "Operation successful.",
                            type = "success"
                        };
                        ViewBag.msg = messages;
                        return View();
                    }
                }

                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                ViewBag.msg = message;
                return View();

            }
            catch (Exception)
            {
                var message = new Messages
                {
                    Message = "Error Please try again.",
                    type = "Error"
                };
                ViewBag.msg = message;
                return View();
            }
            finally
            {
                con.Close();
            }
        }
    }
}