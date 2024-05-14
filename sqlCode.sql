Create Database AttendenceProject
-- Create Role table
CREATE TABLE Role (
    RoleID INT PRIMARY KEY,
    RoleName VARCHAR(100) NOT NULL
);


INSERT INTO Role (RoleID, RoleName) VALUES (1, 'Admin');
INSERT INTO Role (RoleID, RoleName) VALUES (2, 'Manager');
INSERT INTO Role (RoleID, RoleName) VALUES (3, 'Employee');

-- Create Grade table
CREATE TABLE Grade (
    GradeID INT PRIMARY KEY,
    GradeName VARCHAR(100) NOT NULL,
    TotalCasualLeave INT NOT NULL CHECK (TotalCasualLeave >= 0)
);
INSERT INTO Grade (GradeID, GradeName, TotalCasualLeave) VALUES (1, 'Associate',10);
INSERT INTO Grade (GradeID, GradeName, TotalCasualLeave) VALUES (3, 'Manager', 20);

-- Create Project table
CREATE TABLE Project (
    ProjectID INT PRIMARY KEY,
    ProjectName VARCHAR(100) NOT NULL,
    Description TEXT NOT NULL,
    ProjectStartDate DATE NOT NULL,
    ProjectEndDate DATE NOT NULL,
    CHECK (ProjectStartDate <= ProjectEndDate)
);

-- Create Leave table
CREATE TABLE Leave (
    LeaveID INT PRIMARY KEY,
    LeaveType VARCHAR(50) UNIQUE NOT NULL
);

-- Create User table
CREATE TABLE [User] (
    UserID INT PRIMARY KEY,
    RoleID INT NOT NULL,
    GradeID INT NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    EmailID VARCHAR(100) UNIQUE NOT NULL,
    Address VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    HireDate DATE NOT NULL,
    ManagerID INT,
    Status VARCHAR(2) NOT NULL,
    Salary DECIMAL(10, 2) NOT NULL,
    Username VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID),
    FOREIGN KEY (ManagerID) REFERENCES [User](UserID),
    FOREIGN KEY (GradeID) REFERENCES Grade(GradeID)
);
INSERT INTO user 
VALUES 
(1034294, 2, 3, 'Sanjeev Ranjan', 'sanjeev200@gmail.com', 'Lucknow', '8877711223', '2024-01-01', NULL, 1, 30000.00, 'SanjeevR', 'sanjeev'),
(1034295, 1, 1, 'Yuvraj Singh', 'ys665252@gmail.com', 'Lucknow', '08630282265', '2024-01-01', NULL, 1, 50000.00, 'yuvraj@123', 'Yuvraj1'),
(1034297, 3, 1, 'Sanjana', 'sanjana015@gmail.com', 'Lucknow', '08623456789', '2024-01-01', 1034294, 1, 30000.00, 'sanjana015', 'sanjana'),
(1034330, 3, 1, 'Simran Pandey', 'simran015@gmail.com', 'Lucknow', '08630282265', '2024-01-01', NULL, 1, 30000.00, 'simran015', 'simran');

select*from [User]
select*from role



-- Create LeaveRequest table with FromDate, ToDate, and TotalDays columns
CREATE TABLE LeaveRequest (
    LeaveRequestID INT PRIMARY KEY,
    UserID INT NOT NULL,
    LeaveID INT NOT NULL,
    FromDate DATE NOT NULL,
    ToDate DATE NOT NULL,
    TotalDays INT NOT NULL, -- Total number of days for the leave request
    AppliedDate DATE DEFAULT GETDATE(),
    Status VARCHAR(20) DEFAULT 'Pending' NOT NULL,
    Reason TEXT,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (LeaveID) REFERENCES Leave(LeaveID),
    CHECK (Status IN ('Pending', 'Approved', 'Rejected'))
);

-- Create Attendance table with IsPending column
CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY,
    UserID INT NOT NULL,
    ProjectID INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    HoursWorked DECIMAL(5, 2) NOT NULL,
    Approval varchar(25) not null,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (ProjectID) REFERENCES Project(ProjectID)
);
drop table Attendance
-- Create LeaveBalance table
CREATE TABLE LeaveBalance (
    LeaveBalanceID INT PRIMARY KEY,
    UserID INT NOT NULL,
    LeaveID INT NOT NULL,
    Balance INT NOT NULL CHECK (Balance >= 0),
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (LeaveID) REFERENCES Leave(LeaveID)
);

-- Create LeaveHistory table
CREATE TABLE LeaveHistory (
    LeaveHistoryID INT PRIMARY KEY,
    UserID INT NOT NULL,
    LeaveID INT NOT NULL,
    LeaveDate DATE NOT NULL,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (LeaveID) REFERENCES Leave(LeaveID)
);

-- Create LeaveInfo table
CREATE TABLE LeaveInfo (
    LeaveInfoID INT PRIMARY KEY,
    DayOfWeek VARCHAR(20) NOT NULL,
    LeaveDate DATE NOT NULL,
    LeaveTypeID INT NOT NULL,
    HolidayName VARCHAR(100),
    FOREIGN KEY (LeaveTypeID) REFERENCES Leave(LeaveID)
);

-- Create EmployeeProjectAssignment table
CREATE TABLE EmployeeProjectAssignment (
    AssignmentID INT PRIMARY KEY,
    UserID INT NOT NULL,
    ProjectID INT NOT NULL,
    AssignmentDate DATETIME NOT NULL DEFAULT GETDATE(), -- Date of assignment
    CONSTRAINT FK_Assignment_User FOREIGN KEY (UserID) REFERENCES [User](UserID),
    CONSTRAINT FK_Assignment_Project FOREIGN KEY (ProjectID) REFERENCES Project(ProjectID)
);

-- Create UserRoleMapping table
CREATE TABLE UserRoleMapping (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    RoleID INT,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);

select*from UserRoleMapping
INSERT INTO UserRoleMapping (ID, UserID, RoleID)
VALUES
(1, 1034295, 1),
(2, 1034294, 2),
(3, 1034330, 3);




