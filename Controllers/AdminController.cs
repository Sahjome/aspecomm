using EWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EWebStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            UserDetails userDatails = null;
            if (Session["UserDetails"] != null)
            {
                userDatails = Session["UserDetails"] as UserDetails;
            }
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }
        public ActionResult Members()
        {
            return View();
        }
        public ActionResult Vehicle()
        {
            return View();
        }
        public ActionResult Income()
        {
            return View();
        }
        public ActionResult AddVehicle()
        {
            return View();
        }
        public ActionResult EditVehicle()
        {
            return View();
        }
        public ActionResult Vehicles()
        {
            return View();
        }
        public ActionResult Survey()
        {
            return View();
        }
    }
}