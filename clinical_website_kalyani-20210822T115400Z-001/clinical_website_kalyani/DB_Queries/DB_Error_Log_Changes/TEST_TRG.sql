CREATE TRIGGER TRG_TEST_ACTIVITY
ON [dbo].[Tests]
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
  DECLARE @testID INT
  DECLARE @aRemarks VARCHAR(2000)
  DECLARE @updateInColmName VARCHAR(20)
  DECLARE @testName varchar(80)
  DECLARE @testDescription varchar(5000)
  DECLARE @imageAddress varchar(100)
  DECLARE @testSchedule varchar(200)
  DECLARE @testDeptID INT
  DECLARE @testSub_DeptName varchar(100)
  DECLARE @ModifierID INT

  SELECT
    @testName        = c.testName,
    @testDescription = c.testDescription,
    @imageAddress    = c.imageAddress,
    @testSchedule    = c.testSchedule,
    @testDeptID      = c.testDeptID,
	@testSub_DeptName= c.testSub_DeptName,
	@ModifierID      = c.ModifierID
  FROM INSERTED c

  IF @type = 'Inserted'
  BEGIN
  DECLARE @temp1 VARCHAR(8000) = ''
     IF @testName != ''
	   BEGIN
	     SET @updateInColmName = 'Test Name: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @testName,',')
	   END
	 IF @testDescription != ''
	   BEGIN
	     SET @updateInColmName = 'Test Description: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @testDescription,',')
	   END
	 IF @imageAddress != ''
	   BEGIN
	     SET @updateInColmName = 'imageAddress: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @imageAddress,',')
	   END
	 IF @testSchedule != ''
	   BEGIN
	     SET @updateInColmName = 'Test Schedule: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @testSchedule,',')
	   END
	 IF @testDeptID != ''
	   BEGIN
	     SET @updateInColmName = 'Test Dept ID: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @testDeptID,',')
	   END
	 IF @testSub_DeptName != ''
	   BEGIN
	     SET @updateInColmName = 'Test Sub Dept Name: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @testSub_DeptName,',')
	   END
	 IF @ModifierID != ''
	   BEGIN
	     SET @updateInColmName = 'ModifierID: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @ModifierID,',')
	   END
     SELECT
     @aName    = 'Change in Tests table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @testID    = i.testID,
	 @aRemarks = CONCAT(@temp1,' Added on Test ID : ', @testID, ' Succesfully')
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
     IF UPDATE(testName)
	   BEGIN
	     SET @updateInColmName = 'Test Name'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(testDescription)
	   BEGIN
	     SET @updateInColmName = 'Test Description'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(imageAddress)
	   BEGIN
	     SET @updateInColmName = 'imageAddress'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(testSchedule)
	   BEGIN
	     SET @updateInColmName = 'Test Schedule'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(testDeptID)
	   BEGIN
	     SET @updateInColmName = 'Test Dept ID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(testSub_DeptName)
	   BEGIN
	     SET @updateInColmName = 'Test Sub Dept Name'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
     SELECT
     @aName    = 'Change in Tests table',
	 @aTYPE    = 'Update',
	 @aAdminID = u.ModifierID,
	 @testID    = u.testID,
	 @aRemarks = CONCAT(@temp,' Upadted on Test ID : ', @testID, ' Succesfully')
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
     @aName    = 'Change in Tests table',
	 @aTYPE    = 'Delete',
	 @aAdminID = d.ModifierID,
	 @testID    = d.testID,
	 @aRemarks = CONCAT('Test ID : ', @testID, ' DELETED Succesfully')
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