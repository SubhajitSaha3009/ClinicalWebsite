CREATE TRIGGER TRG_TEST_SUB_DEPARTMENT_ACTIVITY
ON [dbo].[TestSub_Departments]
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
  DECLARE @testSubDeptID INT
  DECLARE @testSubDeptName VARCHAR(50)
  DECLARE @aRemarks VARCHAR(2000)
  DECLARE @updateInColmName VARCHAR(20)

  IF @type = 'Inserted'
  BEGIN
     SELECT
     @aName    = 'Change in TestSub_Departments table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @testDeptID = i.testDeptID,
	 @testSubDeptName = i.testSub_DeptID,
	 @testSubDeptName = i.testSub_DeptName,
	 @aRemarks = CONCAT('Test Sub Department name: ', @testSubDeptName,' and Department ID: ', @testDeptID,' are added on Test Sub Dept ID: ', @testDeptID,' Succesfully')
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
	IF UPDATE(testSub_DeptName)
	   BEGIN
	     SET @updateInColmName = 'testSub_DeptName'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
     IF UPDATE(testDeptID)
	   BEGIN
	     SET @updateInColmName = 'testDeptID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 SELECT
     @aName    = 'Change in TestSub_Departments table',
	 @aTYPE    = 'Update',
	 @aAdminID = u.ModifierID,
	 @testDeptID = u.testDeptID,
	 @testSubDeptName = u.testSub_DeptID,
	 @testSubDeptName = u.testSub_DeptName,
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
     @aName    = 'Change in TestSub_Departments table',
	 @aTYPE    = 'Delete',
	 @aAdminID = d.ModifierID,
	 @testDeptID = d.testDeptID,
	 @testSubDeptName = d.testSub_DeptID,
	 @testSubDeptName = d.testSub_DeptName,
	 @aRemarks = CONCAT('Test Sub Dept ID: ', @testDeptID,' DELETED Succesfully')
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