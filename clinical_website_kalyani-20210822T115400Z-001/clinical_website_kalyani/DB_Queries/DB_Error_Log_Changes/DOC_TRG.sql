CREATE TRIGGER TRG_DOC_ACTIVITY
ON [dbo].[Doctors]
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

  IF @type = 'Inserted'
  BEGIN
     SELECT
     @aName    = 'Change in Doctors table',
	 @aTYPE    = 'Insert',
	 @aAdminID = i.ModifierID,
	 @docID    = i.doctorID,
	 @aRemarks = CONCAT('Doctor ID : ', @docID, ' Added Succesfully')
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
     IF UPDATE(doctorName)
	   BEGIN
	     SET @updateInColmName = 'Doctor Name'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(doctorDesignation)
	   BEGIN
	     SET @updateInColmName = 'Doctor Designation'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(doctorSpeciality)
	   BEGIN
	     SET @updateInColmName = 'Doctor Speciality'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(doctorDegree)
	   BEGIN
	     SET @updateInColmName = 'Doctor Degree'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(imageAddress)
	   BEGIN
	     SET @updateInColmName = 'imageAddress'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(doctorStatus)
	   BEGIN
	     SET @updateInColmName = 'Doctor Status'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(workingTiming)
	   BEGIN
	     SET @updateInColmName = 'Doctor workingTiming'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(numberForAppointment)
	   BEGIN
	     SET @updateInColmName = 'numberForAppointment'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(LnumberForAppointment)
	   BEGIN
	     SET @updateInColmName = 'LnumberForAppointment'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(LastModificationDate)
	   BEGIN
	     SET @updateInColmName = 'LastModificationDate'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
	 IF UPDATE(ModifierID)
	   BEGIN
	     SET @updateInColmName = 'ModifierID'
		 SET @temp = CONCAT(@temp,@updateInColmName,',')
	   END
     SELECT
     @aName    = 'Change in Doctors table',
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