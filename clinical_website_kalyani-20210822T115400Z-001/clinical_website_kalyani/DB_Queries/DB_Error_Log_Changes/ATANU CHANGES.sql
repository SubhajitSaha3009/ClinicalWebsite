----------------Atanu changes--------------------------
create table LoginLog(
LogID int primary key Identity(1,1) not null,
LoginID int foreign key references Admins(adminID) not null,
IPAddress varchar(20),
LoginDTm DateTime not null,
LogoutDtm DateTime
)

truncate table LoginLog

CREATE proc SP_ErrorandLoginLogDetails(@ch int,@adminID int,@ipAddress varchar(20),@Exception varchar(2500))
as 
begin
 if @ch=0
 begin
    insert into [dbo].[LoginLog] values(@adminID,@ipAddress,GETDATE(),'');
 end
 if @ch=1
 begin
	update [dbo].[LoginLog] set LogoutDtm=GETDATE() where LogID = (select MAX(LogID) from [dbo].[LoginLog] where LoginID=@adminID)
 end

 if @ch=2
 begin
	insert into FailedEventsLog values(@adminID,@Exception,GETDATE())
 end

 end

drop proc SP_LogLoginDetails

 exec SP_ErrorandLoginLogDetails 1,7050,''

 create table FailedEventsLog(
 EventLogID int primary key identity(1,1) not null,
 AdminID int foreign key references Admins(adminID) not null,
 Exception Varchar(2500) not null,
 FailedDtm DateTime not null
 )
 drop table FailedEventsLog
 select * from FailedEventsLog


 ---------------- End Atanu changes--------------------------