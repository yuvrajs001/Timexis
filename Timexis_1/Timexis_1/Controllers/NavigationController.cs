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
        [Authorize(Roles = "Employee, Admin")]
        public ActionResult EmployeeDashboard()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [Authorize(Roles = "Manager")]
        public ActionResult ManagerDashboard()
        {
            return View();
        }
    }
}