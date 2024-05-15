using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Timexis_1.Controllers
{[Authorize]
    public class NavigationController : Controller
    {
        // GET: Navigation
        [Authorize(Roles = "Employee")]
        public ActionResult EmployeeDashboard()
        {
            ViewBag.UserID= Session["UserID"];
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            ViewBag.UserID = Session["UserID"];
            return View();
        }
        [Authorize(Roles = "Manager")]
        public ActionResult ManagerDashboard()
        {
            return View();
        }
    }
}