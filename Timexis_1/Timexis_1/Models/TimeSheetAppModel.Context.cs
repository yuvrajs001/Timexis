﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Timexis_1.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AttendenceProjectEntities : DbContext
    {
        public AttendenceProjectEntities()
            : base("name=AttendenceProjectEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<EmployeeProjectAssignment> EmployeeProjectAssignments { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Leave> Leaves { get; set; }
        public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }
        public virtual DbSet<LeaveHistory> LeaveHistories { get; set; }
        public virtual DbSet<LeaveInfo> LeaveInfoes { get; set; }
        public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
