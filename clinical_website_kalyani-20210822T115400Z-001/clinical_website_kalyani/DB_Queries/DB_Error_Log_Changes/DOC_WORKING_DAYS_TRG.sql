CREATE TRIGGER TRG_DOC_WORKING_ACTIVITY
ON [dbo].[DoctorWorkingDays]
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
  DECLARE @docID INT
  DECLARE @aRemarks VARCHAR(2000)
  DECLARE @updateInColmName VARCHAR(20)
  DECLARE @sun   VARCHAR(1)
  DECLARE @mon   VARCHAR(1)
  DECLARE @tue   VARCHAR(1)
  DECLARE @wed   VARCHAR(1)
  DECLARE @thrus VARCHAR(1)
  DECLARE @fri   VARCHAR(1)
  DECLARE @sat   VARCHAR(1)

  SELECT
	 @sun = c.Sunday,
	 @mon = c.Monday,
	 @tue = c.Tuesday,
	 @wed = c.Wednesday,
	 @thrus = c.Thursday,
	 @fri = c.Friday,
	 @sat = c.Saturday
  FROM INSERTED c

  IF @type = 'Inserted'
  BEGIN
    DECLARE @tempIn VARCHAR(2000) = ''
     IF @mon = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Monday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @tue = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Tuesday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @wed = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Wednesday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @thrus = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Thursday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @fri = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Friday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @sat = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Saturday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END
	 IF @sun = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Sunday'
		 SET @tempIn = CONCAT(@tempIn,@updateInColmName,',')
	   END

     SELECT
     @aName    = 'Change in DoctorWorkingDays table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @docID    = i.doctorID,
	 @aRemarks = CONCAT('Working days of Doctor ID : ', @docID, ' are ', @tempIn,' Added Succesfully')
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
     IF @mon = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Monday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @tue = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Tuesday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @wed = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Wednesday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @thrus = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Thursday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @fri = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Friday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @sat = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Saturday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF @sun = 'Y'
	   BEGIN
	     SET @updateInColmName = 'Sunday'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 SELECT
     @aName    = 'Change in DoctorWorkingDays table',
	 @aTYPE    = 'Update',
	 @aAdminID = u.ModifierID,
	 @docID    = u.doctorID,
	 @aRemarks = CONCAT(@temp,' Upadted on Doctor ID : ', @docID, ' Succesfully')
  FROM INSERTED u

  INSERT INTO ACTIVITY_LOG VALUES(
     @aName,
	 @aTYPE,
	 @aAdminID,
	 DEFAULT,
	 @aRemarks
	 )
  END
END