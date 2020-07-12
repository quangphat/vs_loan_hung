
EXEC sp_rename 'TAI_LIEU_HS.Maloai', 'FileKey', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.DuongDan', 'FilePath', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.Ten', 'FileName', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.MaHS', 'ProfileId', 'COLUMN'
EXEC sp_rename 'TAI_LIEU_HS.typeId', 'ProfileTypeId', 'COLUMN'

-------
EXEC sp_rename 'LOAI_TAI_LIEU.HosoType', 'ProfileTypeId', 'COLUMN'


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