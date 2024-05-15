using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Timexis_1.Models;

namespace Timexis_1.Controllers
{
    public class UsersController : Controller
    {
        private AttendenceProjectEntities1 db = new AttendenceProjectEntities1();



        public ActionResult CommonDasboardAfterLogin()
        {
            return View();
        }
        // GET: Users
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Grade).Include(u => u.Role).Include(u => u.User2);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        [Authorize(Roles ="Admin,Employee,Manager")]
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName");
            ViewBag.ManagerID = new SelectList(db.Users, "UserID", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", user.GradeID);
            ViewBag.ManagerID = new SelectList(db.Users, "UserID", "FullName", user.ManagerID);

            return RedirectToAction("Approve");
        }
    
    // GET: Users/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", user.GradeID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            ViewBag.ManagerID = new SelectList(db.Users, "UserID", "FullName", user.ManagerID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,RoleID,GradeID,FullName,EmailID,Address,PhoneNumber,HireDate,ManagerID,Status,Salary,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradeID = new SelectList(db.Grades, "GradeID", "GradeName", user.GradeID);
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", user.RoleID);
            ViewBag.ManagerID = new SelectList(db.Users, "UserID", "FullName", user.ManagerID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Approve()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        // POST: UserRoleMappings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve([Bind(Include = "Id,UserID,RoleID")] UserRoleMapping userRoleMapping)
        {
            if (ModelState.IsValid)
            {
                db.UserRoleMappings.Add(userRoleMapping);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleName", userRoleMapping.RoleID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", userRoleMapping.UserID);
            return View(userRoleMapping);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
