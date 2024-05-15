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
    public class AttendancesController : Controller
    {
        private AttendenceProjectEntities1 db = new AttendenceProjectEntities1();

        // GET: Attendances
        public ActionResult Index()
        {
            var attendances = db.Attendances.Include(a => a.Project).Include(a => a.User);
            return View(attendances.ToList());
        }

        // GET: Attendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            //ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Attendance a1)
        {
            Attendance attendance = new Attendance();
            User em = db.Users.SingleOrDefault(a => a.UserID == id);
            var lr = db.LeaveRequests.Where(a => a.UserID== em.UserID).ToList();
            DateTime currentDate = DateTime.Now.Date;
            //var z = lr.FirstOrDefault(a => a.UserID == id && a.FromDate == currentDate && a=>a.Status=="Approved");
            var z = lr.FirstOrDefault(a => a.UserID == id && a.FromDate == currentDate && a.Status == "Approved");

            //var x = lr.FirstOrDefault(t => t.FromDate == currentDate);
            Attendance xyz = db.Attendances.FirstOrDefault(a => a.UserID == id && a.AttendanceDate == currentDate);
            string dayOfWeekString = DateTime.Now.DayOfWeek.ToString();
            if (dayOfWeekString != "Saturday" || dayOfWeekString != "Sunday")
            {
                if (z == null)
                {
                    if (xyz == null)
                    {

                        Attendance at = db.Attendances.OrderByDescending(a => a.AttendanceID).FirstOrDefault();
                        attendance.AttendanceID = at.AttendanceID + 1;
                        attendance.UserID = em.UserID;
                        attendance.ProjectID = em.EmployeeProjectAssignments.Where(a => a.UserID == em.UserID).SingleOrDefault().ProjectID;
                        attendance.AttendanceDate = currentDate;
                        attendance.HoursWorked = a1.HoursWorked;
                        attendance.Approval = "Pending";
                        db.Attendances.Add(attendance);
                        db.SaveChanges();
                        return RedirectToAction("EmployeeDashboard", "Navigation");
                    }
                    ViewBag.error = "Already Filled";
                    return View();
                }
                ViewBag.error = "ON Leave";
                return View();
            }
            ViewBag.error = "Weekend";
            return View();
        }

        public ActionResult ApproveAttendence()
        {
            List<Attendance> a = db.Attendances.Where(z => z.Approval == "Pending").ToList();
            return View(a.ToList());

        }
        // GET: Attendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", attendance.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", attendance.UserID);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendanceID,UserID,ProjectID,AttendanceDate,HoursWorked,Approval")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", attendance.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", attendance.UserID);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
            db.SaveChanges();
            return RedirectToAction("Index");
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
