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
    public class EmployeeProjectAssignmentsController : Controller
    {
        private AttendenceProjectEntities1 db = new AttendenceProjectEntities1();

        // GET: EmployeeProjectAssignments
        public ActionResult Index()
        {
            var employeeProjectAssignments = db.EmployeeProjectAssignments.Include(e => e.Project).Include(e => e.User);
            return View(employeeProjectAssignments.ToList());
        }

        // GET: EmployeeProjectAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.Users.Include(u => u.EmployeeProjectAssignments.Select(a => a.Project))
                               .SingleOrDefault(u => u.UserID == id);

            if (user == null)
                return HttpNotFound();

            var assignment = user.EmployeeProjectAssignments?.FirstOrDefault();
            if (assignment == null)
            {
                ViewBag.Message = "No project assigned.";
            }
            else
            {
                var projectAssignment = db.EmployeeProjectAssignments.Include(a => a.Project)
                                                                      .SingleOrDefault(a => a.AssignmentID == assignment.AssignmentID);

                if (projectAssignment != null)
                {
                    ViewBag.AssignmentDate = projectAssignment.AssignmentDate;
                    ViewBag.ProjectName = projectAssignment.Project.ProjectName;
                }
                else
                {
                    ViewBag.Message = "Project not found.";
                }
            }

            ViewBag.UserFullName = user.FullName;

            return View();
        }


        // GET: EmployeeProjectAssignments/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName");
            return View();
        }

        // POST: EmployeeProjectAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentID,UserID,ProjectID,AssignmentDate")] EmployeeProjectAssignment employeeProjectAssignment)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeProjectAssignments.Add(employeeProjectAssignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", employeeProjectAssignment.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", employeeProjectAssignment.UserID);
            return View(employeeProjectAssignment);
        }

        // GET: EmployeeProjectAssignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeProjectAssignment employeeProjectAssignment = db.EmployeeProjectAssignments.Find(id);
            if (employeeProjectAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", employeeProjectAssignment.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", employeeProjectAssignment.UserID);
            return View(employeeProjectAssignment);
        }

        // POST: EmployeeProjectAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignmentID,UserID,ProjectID,AssignmentDate")] EmployeeProjectAssignment employeeProjectAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeProjectAssignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectID", "ProjectName", employeeProjectAssignment.ProjectID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FullName", employeeProjectAssignment.UserID);
            return View(employeeProjectAssignment);
        }

        // GET: EmployeeProjectAssignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeProjectAssignment employeeProjectAssignment = db.EmployeeProjectAssignments.Find(id);
            if (employeeProjectAssignment == null)
            {
                return HttpNotFound();
            }
            return View(employeeProjectAssignment);
        }

        // POST: EmployeeProjectAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeProjectAssignment employeeProjectAssignment = db.EmployeeProjectAssignments.Find(id);
            db.EmployeeProjectAssignments.Remove(employeeProjectAssignment);
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
