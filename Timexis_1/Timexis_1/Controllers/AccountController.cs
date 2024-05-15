using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Timexis_1.Models;

namespace Timexis_1.Controllers
{
    
    public class AccountController : Controller
    {
        AttendenceProjectEntities1 db = new AttendenceProjectEntities1();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        // POST: Account/Login
        //[HttpPost]
        //public ActionResult Login(User model)
        //{
        //    using (db = new AttendenceProjectEntities1())
        //    {
        //        bool isValiduser = db.Users.Any(user => user.Username.ToLower() ==
        //        model.Username.ToLower() && user.Password == model.Password);

        //        if (isValiduser)
        //        {
        //            FormsAuthentication.SetAuthCookie(model.Username, false);
        //            //TempData["UserID"] = 
        //            return RedirectToAction("CommonDasboardAfterLogin", "Users");
        //        }
        //        ModelState.AddModelError("", "Invalid User name or Password");
        //        return View();
        //    }

        //}
        [HttpPost]
        public ActionResult Login(User model)
        {
            using (var db = new AttendenceProjectEntities1())
            {
                var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == model.Username.ToLower() && u.Password == model.Password);

                if (user != null)
                {
                    // Store the user ID in TempData
                    Session["UserID"] = user.UserID;

                    // Set authentication cookie (if needed)
                    FormsAuthentication.SetAuthCookie(model.Username, false);

                    return RedirectToAction("CommonDasboardAfterLogin", "Users");
                }

                ModelState.AddModelError("", "Invalid User name or Password");
                return View();
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}