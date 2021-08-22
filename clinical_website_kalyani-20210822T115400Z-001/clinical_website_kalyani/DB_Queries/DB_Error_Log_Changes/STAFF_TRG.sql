CREATE TRIGGER TRG_STAFF_ACTIVITY
ON [dbo].[Staffs]
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
  DECLARE @staffID INT
  DECLARE @aRemarks VARCHAR(2000)
  DECLARE @updateInColmName VARCHAR(20)

  DECLARE @staffName varchar(50)
  DECLARE @staffDOB DATE
  DECLARE @staffGender varchar(10)
  DECLARE @staffAddress varchar(100)
  DECLARE @staffMobNo Varchar(10)
  DECLARE @staffEmail varchar(30)
  DECLARE @staffStatus varchar(20)
  DECLARE @staffJoiningDate DATE
  DECLARE @staffLeavingDate DATE
  DECLARE @ModifierID INT

  SELECT
    @staffName        = c.staffName,
    @staffDOB         = c.staffDOB,
    @staffGender      = c.staffGender,
    @staffAddress     = c.staffAddress,
    @staffMobNo       = c.staffMobNo,
	@staffEmail       = c.staffEmail,
	@staffStatus      = c.staffStatus,
	@staffJoiningDate = c.staffJoiningDate,
    @staffLeavingDate = c.staffLeavingDate,
    @ModifierID       = c.ModifierID

  FROM INSERTED c

  IF @type = 'Inserted'
  BEGIN
  DECLARE @temp1 VARCHAR(8000) = ''
     IF @staffName != ''
	   BEGIN
	     SET @updateInColmName = 'Staff Name: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffName,',')
	   END
	 IF @staffDOB != ''
	   BEGIN
	     SET @updateInColmName = 'DOB: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffDOB,',')
	   END
	 IF @staffGender != ''
	   BEGIN
	     SET @updateInColmName = 'Gender: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffGender,',')
	   END
	 IF @staffAddress != ''
	   BEGIN
	     SET @updateInColmName = 'Address: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffAddress,',')
	   END
	 IF @staffMobNo != ''
	   BEGIN
	     SET @updateInColmName = 'Mob No.: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffMobNo,',')
	   END
	 IF @staffEmail != ''
	   BEGIN
	     SET @updateInColmName = 'Email: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffEmail,',')
	   END
	   IF @staffStatus != ''
	   BEGIN
	     SET @updateInColmName = 'Status: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffStatus,',')
	   END
	   IF @staffJoiningDate != ''
	   BEGIN
	     SET @updateInColmName = 'Joining Date: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffJoiningDate,',')
	   END
	   IF @staffLeavingDate != ''
	   BEGIN
	     SET @updateInColmName = 'Leaving Date: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @staffLeavingDate,',')
	   END
	 IF @ModifierID != ''
	   BEGIN
	     SET @updateInColmName = 'ModifierID: '
		 SET @temp1 = CONCAT(@temp1,@updateInColmName, @ModifierID,',')
	   END
     SELECT
     @aName    = 'Change in Staffs table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @staffID    = i.staffID,
	 @aRemarks = CONCAT(@temp1,' Added on Staff ID : ', @staffID, ' Succesfully')
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
    DECLARE @temp VARCHAR(8000) = ''
     IF UPDATE(staffName)
	   BEGIN
	      SET @updateInColmName = 'Staff Name: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffName,',')
	   END
	 IF UPDATE(staffDOB)
	   BEGIN
	     SET @updateInColmName = 'DOB: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffDOB,',')
	   END
	 IF UPDATE(staffGender)
	   BEGIN
	     SET @updateInColmName = 'Gender: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffGender,',')
	   END
	 IF UPDATE(staffAddress)
	   BEGIN
	     SET @updateInColmName = 'Address: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffAddress,',')
	   END
	 IF UPDATE(staffMobNo)
	   BEGIN
	     SET @updateInColmName = 'Mob No.: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffMobNo,',')
	   END
	 IF UPDATE(staffEmail)
	   BEGIN
	     SET @updateInColmName = 'Email: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffEmail,',')
	   END
	   IF UPDATE(staffStatus)
	   BEGIN
	     SET @updateInColmName = 'Status: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffStatus,',')
	   END
	   IF UPDATE(staffJoiningDate)
	   BEGIN
	     SET @updateInColmName = 'Joining Date: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffJoiningDate,',')
	   END
	   IF UPDATE(staffLeavingDate)
	   BEGIN
	     SET @updateInColmName = 'Leaving Date: '
		 SET @temp = CONCAT(@temp,@updateInColmName, @staffLeavingDate,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID: '
		 SET @temp = CONCAT(@temp,@updateInColmName,@ModifierID,',')
	   END
     SELECT
     @aName    = 'Change in Staff table',
	 @aTYPE    = 'Update',
	 @aAdminID = u.ModifierID,
	 @staffID    = u.staffID,
	 @aRemarks = CONCAT(@temp,' Upadted on Staff ID : ', @staffID, ' Succesfully')
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
     @aName    = 'Change in Staff table',
	 @aTYPE    = 'Delete',
	 @aAdminID = d.ModifierID,
	 @staffID    = d.staffID,
	 @aRemarks = CONCAT('Staff ID : ', @staffID, ' DELETED Succesfully')
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