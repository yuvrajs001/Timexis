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
    public class ProjectsController : Controller
    {
        private AttendenceProjectEntities1 db = new AttendenceProjectEntities1();

        // GET: Projects
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,ProjectName,Description,ProjectStartDate,ProjectEndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectID,ProjectName,Description,ProjectStartDate,ProjectEndDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult AssignProject()
        {
            ViewBag.EmployeeList = new SelectList(db.Users, "FullName", "FullName");
            ViewBag.ProjectList = new SelectList(db.Projects, "ProjectName", "ProjectName");
            return View();
        }
        [HttpPost]
        public ActionResult AssignProject(string employeeList, string projectList)
        {
            try
            {
                if (!string.IsNullOrEmpty(employeeList) && !string.IsNullOrEmpty(projectList))
                {
                    // Find employee and project by name
                    var employee = db.Users.FirstOrDefault(e => e.FullName == employeeList);
                    var project = db.Projects.FirstOrDefault(p => p.ProjectName == projectList);

                    if (employee != null && project != null)
                    {
                        // Generate a random assignment ID between 1000 and 2000
                        Random rnd = new Random();
                        int assignmentId = rnd.Next(1000, 2001);

                        // Create a new assignment record
                        var assignment = new EmployeeProjectAssignment
                        {
                            AssignmentID = assignmentId,
                           UserID = employee.UserID,
                            ProjectID = project.ProjectID,
                            AssignmentDate = DateTime.Now
                        };


                        db.EmployeeProjectAssignments.Add(assignment);
                        db.SaveChanges();

                        // Return success response
                        return Content("Project assigned to employee successfully. Assignment ID: " + assignmentId);
                    }
                    else
                    {
                        // Return error response if either employee or project is not found
                        if (employee == null)
                        {
                            return Content("Employee not found.");
                        }
                        else
                        {
                            return Content("Project not found.");
                        }
                    }
                }
                else
                {
                    // Return error response if either employeeList or projectList is empty
                    return Content("Please select both an employee and a project.");
                }
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs
                return Content("An error occurred while assigning project to employee.");
            }
        }


        [Authorize(Roles = "Admin,Manager")]
        public ActionResult EmployeesAssignedToProject(int? projectId)
        {
            if (projectId == null)
            {
                return View(); // Show the form if no project ID is provided
            }

            // Find the project by its ID
            var project = db.Projects.Find(projectId);

            if (project == null)
            {
                return HttpNotFound(); // Project not found
            }

            // Retrieve the list of employees assigned to the project
            var employees = project.EmployeeProjectAssignments.Select(a => a.User).ToList();

            // Pass the list of employees to the view
            return View(employees);
        }

        [Authorize(Roles ="Employee")]
        public ActionResult MyProjects()
        {
            // Retrieve the user ID from TempData
            int? userId = TempData["UserID"] as int?;

            // Check if the user ID is null or not
            if (userId != null)
            {
                // Convert the user ID to int
                int employeeId = (int)userId;

                using (var db = new AttendenceProjectEntities1())
                {
                    // Query the database to get projects assigned to the employee
                    var projects = db.EmployeeProjectAssignments
                                        .Where(e => e.UserID == employeeId)
                                        .Select(e => e.Project)
                                        .ToList();

                    return View(projects);
                }
            }
            else
            {
                // Handle the case where the user ID is not found in TempData
                // You can redirect to a login page or display an error message
                return RedirectToAction("Login", "Account");
            }
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
