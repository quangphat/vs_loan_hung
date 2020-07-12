
EXEC sp_rename 'TAI_LIEU_HS.Maloai', 'FileKey', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.DuongDan', 'FilePath', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.Ten', 'FileName', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.MaHS', 'ProfileId', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.typeId', 'ProfileTypeId', 'COLUMN'

-------
EXEC sp_rename 'LOAI_TAI_LIEU.HosoType', 'ProfileTypeId', 'COLUMN'
-------------------------
alter table Tai_lieu_HS
add DocumentName nvarchar(500),
DocumentCode varchar(400),
MC_DocumentId int,
MC_MapBpmVar varchar(400),
MC_GroupId int

----------------------
ALTER procedure [dbo].[getTailieuByHosoId](@profileId int, @profiletTypeId int =1)

as

begin

select tl.Id as FileId
, tl.FileKey as [Key], tl.FileName 
, tl.FilePath as FileUrl
,ltl.Ten as KeyName
, ltl.Bat_Buoc as IsRequire
,tl.ProfileId
,tl.ProfileTypeId,
tl.DocumentName ,
tl.DocumentCode ,
tl.MC_DocumentId,
tl.MC_MapBpmVar,
tl.MC_GroupId
from TAI_LIEU_HS tl
left join LOAI_TAI_LIEU ltl on tl.FileKey = ltl.ID
where tl.ProfileId = @profileId and ISNULL(tl.Deleted,0) = 0
and tl.ProfileTypeId = @profiletTypeId

order by ltl.Bat_Buoc desc

end

------------
ALTER PROCEDURE [dbo].[sp_TAI_LIEU_HS_Them]
	-- Add the parameters for the stored procedure here
	@FileKey int,
	@FileName nvarchar(500),
	@FilePath ntext,
	@ProfileId int,
	@ProfileTypeId int =1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	insert TAI_LIEU_HS(FileKey,FileName,FilePath,ProfileId,ProfileTypeId)
	values(@FileKey,@FileName,@FilePath,@ProfileId,@ProfileTypeId)
END
--------------
ALTER PROCEDURE [dbo].[sp_TAI_LIEU_HS_XoaTatCa]
	-- Add the parameters for the stored procedure here
	@ProfileId int,
	@ProfileTypeId int =1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	delete from TAI_LIEU_HS where ProfileId=@ProfileId and ProfileTypeId = @ProfileTypeId
END
---------------
create table FilesGroup
(
Id int identity(1,1) not null,
Name nvarchar(200),
IsRequire bit
)

------------------

ALTER procedure [dbo].[getTailieuByHosoId](@profileId int, @profiletTypeId int =1)

as

begin

select tl.Id as FileId, tl.FileKey as [Key], tl.FileName 

, tl.FilePath as FileUrl,ltl.Ten as KeyName, ltl.Bat_Buoc as IsRequire  from TAI_LIEU_HS tl

inner join LOAI_TAI_LIEU ltl on tl.FileKey = ltl.ID

where tl.ProfileId = @profileId and ISNULL(tl.Deleted,0) = 0

and tl.ProfileTypeId = @profiletTypeId

order by ltl.Bat_Buoc desc

end
----------------