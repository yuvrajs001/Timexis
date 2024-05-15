using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Timexis_1.Models;

namespace Timexis_1.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private AttendenceProjectEntities1 db = new AttendenceProjectEntities1();

        [Authorize(Roles ="Employee")]
        public ActionResult BalanceSummary(int? userId)
        {
            if (userId.HasValue && userId != 0)
            {
                var casualLeaveBalance = db.LeaveBalances
                    .Where(lb => lb.UserID == userId && lb.LeaveID == 1)
                    .Select(lb => lb.Balance)
                    .FirstOrDefault();

                var leaveRequests = db.LeaveRequests
                    .Where(lr => lr.UserID == userId)
                    .ToList();

                ViewBag.CasualLeaveBalance = casualLeaveBalance;
                return View(leaveRequests);
            }
            else
            {
                return View();
            }
        }

        [HttpPost] // This action handles form submissions
        public ActionResult BalanceSummary(FormCollection form)
        {
            int? userId = null;
            if (!string.IsNullOrEmpty(form["userId"]))
            {
                userId = int.Parse(form["userId"]);
            }

            return RedirectToAction("BalanceSummary, LeaveRequests", new { userId = userId });


        }


        //-----------------------------------------------------
        // GET: LeaveRequests/Create
        [Authorize(Roles ="Employee")]
        public ActionResult LeaveRequest()
        { 
            return View();
        }

       

        // POST: LeaveRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveRequest([Bind(Include = "UserID,FromDate,ToDate,Reason")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SqlParameter userIdParam = new SqlParameter("@UserID", leaveRequest.UserID);
                    SqlParameter fromDateParam = new SqlParameter("@FromDate", leaveRequest.FromDate);
                    SqlParameter toDateParam = new SqlParameter("@ToDate", leaveRequest.ToDate);
                    SqlParameter reasonParam = new SqlParameter("@Reason", leaveRequest.Reason);

                    db.Database.ExecuteSqlCommand("CreateLeaveRequest @UserID, @FromDate, @ToDate, @Reason",
                                                  userIdParam, fromDateParam, toDateParam, reasonParam);

                    ViewBag.SuccessMessage = "Leave request submitted successfully.";
                    return RedirectToAction("CommonDasboardAfterLogin", "Users");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Failed to submit leave request: " + ex.Message;
                }
            }

            return View(leaveRequest);
        }




        //--------------------------------------------------------------------------------------------------

        [Authorize(Roles = "Manager")]
        public ActionResult PendingRequests()
        {
            // Retrieve pending leave requests
            var pendingRequests = db.LeaveRequests.Where(lr => lr.Status == "Pending").ToList();
            return View(pendingRequests);
        }
        // GET: LeaveRequests/Reject/{id}
        public ActionResult Reject(int id)
        {
            try
            {
                // Find the leave request with the given ID
                var leaveRequest = db.LeaveRequests.Find(id);

                // Update the status to "Rejected"
                leaveRequest.Status = "Rejected";

                // Save changes to the database
                db.SaveChanges();

                TempData["Message"] = "Leave request rejected successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to reject leave request: " + ex.Message;
            }

            return RedirectToAction("PendingRequests");
        }

        // GET: LeaveRequests/Approve/{id}
        public ActionResult Approve(int id)
        {
            try
            {
                // Find the leave request with the given ID
                var leaveRequest = db.LeaveRequests.Find(id);

                // Update the status to "Approved"
                leaveRequest.Status = "Approved";

                // Save changes to the database
                db.SaveChanges();

                TempData["Message"] = "Leave request approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to approve leave request: " + ex.Message;
            }

            return RedirectToAction("PendingRequests");
        }

        //------------------------------------------------------------------------------------


       



    }
}
   
