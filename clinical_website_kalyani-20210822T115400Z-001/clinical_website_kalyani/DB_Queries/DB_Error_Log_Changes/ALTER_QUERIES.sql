-------------Query needs to modify in table------------
ALTER TABLE DoctorWorkingDays
ADD
ModifierID INT FOREIGN KEY REFERENCES Admins(adminID)
GO

ALTER TABLE TestDepartments
ADD
ModifierID INT FOREIGN KEY REFERENCES Admins(adminID)
GO

ALTER TABLE TestSub_Departments
ADD
ModifierID INT FOREIGN KEY REFERENCES Admins(adminID)
GO

ALTER TABLE Tests
ADD
ModifierID INT FOREIGN KEY REFERENCES Admins(adminID)
GO

ALTER TABLE Staffs
ADD
ModifierID INT FOREIGN KEY REFERENCES Admins(adminID)
GO