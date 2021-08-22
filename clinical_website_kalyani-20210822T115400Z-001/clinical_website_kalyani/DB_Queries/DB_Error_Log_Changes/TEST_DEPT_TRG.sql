CREATE TRIGGER TRG_TEST_DEPARTMENT_ACTIVITY
ON [dbo].[TestDepartments]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
   SET NOCOUNT ON;
    -- Insert statements for trigger here
       DECLARE @type NVARCHAR(15)=
         CASE 
		   WHEN NOT EXISTS(SELECT * FROM inserted)
               THEN 'Deleted'
           WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
               THEN 'Updated'
           ELSE
               'Inserted'
         END
  DECLARE @aName VARCHAR(40)
  DECLARE @aTYPE VARCHAR(40)
  DECLARE @aAdminID INT
  DECLARE @testDeptID INT
  DECLARE @testDeptName VARCHAR(50)
  DECLARE @aRemarks VARCHAR(2000)
  DECLARE @updateInColmName VARCHAR(20)

  IF @type = 'Inserted'
  BEGIN
     SELECT
     @aName    = 'Change in TestDepartments table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @testDeptID = i.testDeptID,
	 @testDeptName = i.testDeptName,
	 @aRemarks = CONCAT('Test Department name: ', @testDeptName,' on Test Dept ID: ', @testDeptID,' Added Succesfully')
  FROM INSERTED i

  INSERT INTO ACTIVITY_LOG VALUES(
     @aName,
	 @aTYPE,
	 @aAdminID,
	 DEFAULT,
	 @aRemarks
	 )
  END
  ELSE IF @type = 'Updated'
  BEGIN
    DECLARE @temp VARCHAR(2000) = ''
     IF UPDATE(testDeptName)
	   BEGIN
	     SET @updateInColmName = 'testDeptName'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 SELECT
     @aName    = 'Change in TestDepartments table',
	 @aTYPE    = 'Update',
	 @aAdminID = u.ModifierID,
	 @testDeptID = u.testDeptID,
	 @testDeptName = u.testDeptName,
	 @aRemarks = CONCAT(@temp,' Updated',' on Test Dept ID: ', @testDeptID,' Succesfully')
  FROM INSERTED u

  INSERT INTO ACTIVITY_LOG VALUES(
     @aName,
	 @aTYPE,
	 @aAdminID,
	 DEFAULT,
	 @aRemarks
	 )
  END
  ELSE IF @type = 'Deleted'
  BEGIN
	 SELECT
     @aName    = 'Change in TestDepartments table',
	 @aTYPE    = 'Delete',
	 @aAdminID = d.ModifierID,
	 @testDeptID = d.testDeptID,
	 @testDeptName = d.testDeptName,
	 @aRemarks = CONCAT('Test Dept ID: ', @testDeptID,' DELETED Succesfully')
  FROM DELETED d

  INSERT INTO ACTIVITY_LOG VALUES(
     @aName,
	 @aTYPE,
	 @aAdminID,
	 DEFAULT,
	 @aRemarks
	 )
  END
END