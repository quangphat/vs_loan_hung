--EXEC sp_rename 'dbo.NHAN_VIEN', 'Employee'
EXEC sp_rename 'dbo.Employee', 'Nhan_Vien'
--EXEC sp_rename '[Employee].Ma', 'Code', 'COLUMN'
--EXEC sp_rename '[Employee].Ten_Dang_Nhap', 'UserName', 'COLUMN'
--EXEC sp_rename '[Employee].Mat_Khau', 'Password', 'COLUMN'
--EXEC sp_rename '[Employee].Ho_Ten', 'FullName', 'COLUMN'
--EXEC sp_rename '[Employee].Loai', 'TypeId', 'COLUMN'
--EXEC sp_rename '[Employee].Dien_Thoai', 'Phone', 'COLUMN'
--EXEC sp_rename '[Employee].Trang_Thai', 'Status', 'COLUMN'
--EXEC sp_rename '[Nhan_Vien].Xoa', 'IsDeleted', 'COLUMN'


------------------
--EXEC sp_rename 'dbo.HO_SO', 'Profile'
--EXEC sp_rename '[Profile].Ma_Ho_so', 'Code', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ma_Khach_Hang', 'CustomerCode', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ten_Khach_Hang', 'CustomerName', 'COLUMN'
--EXEC sp_rename '[Profile].Dia_Chi', 'Address', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Khu_Vuc', 'DistricId', 'COLUMN'
--EXEC sp_rename '[HO_SO].SDT', 'Phone', 'COLUMN'
--EXEC sp_rename '[Profile].SDT2', 'Phone2', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ngay_Tao', 'CreatedTime', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ma_Nguoi_Tao', 'CreatedBy', 'COLUMN'
--EXEC sp_rename '[Profile].Ho_So_Cua_Ai', 'AssigneeId', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ngay_Cap_Nhat', 'UpdatedTime', 'COLUMN'
--EXEC sp_rename '[HO_SO].Ma_Nguoi_Cap_Nhat', 'UpdatedBy', 'COLUMN'
--EXEC sp_rename '[Profile].Ngay_Nhan_Don', 'ReceiveDate', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Trang_Thai', 'StatusId', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Ket_Qua', 'ResultId', 'COLUMN'
--EXEC sp_rename '[Profile].San_Pham_Vay', 'ProductId', 'COLUMN'
--EXEC sp_rename '[Profile].Co_Bao_Hiem', 'IsInsurrance', 'COLUMN'
--EXEC sp_rename '[Profile].So_Tien_Vay', 'LoanMoney', 'COLUMN'
--EXEC sp_rename '[Profile].Han_Vay', 'LoanPeriod', 'COLUMN'
--EXEC sp_rename '[Profile].Ghi_Chu', 'LastNote', 'COLUMN'
--EXEC sp_rename '[HO_SO].Da_Xoa', 'IsDeleted', 'COLUMN'


----------------------------

alter PROCEDURE [dbo].[sp_Employee_Login]
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
declare @roleId int = 1;
select @roleId = isnull(RoleId, 0) from  Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(IsDeleted,0) =0
if(@roleId =5)
set @roleId = 1;
	Select e.Id,e.Ten_Dang_Nhap as UserName, e.Email, e.Ho_Ten as FullName, e.Dien_Thoai as Phone,
	e.Ma as Code, e.Status as IsActive, r.Code as RoleCode, @roleId as RoleId, e.OrgId,R.Code as Rolecode
	 From Nhan_Vien e left join [Role] r
	on e.RoleId = r.Id
	 where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(e.IsDeleted,0) =0
END


-------------------------
ALTER procedure [dbo].[sp_getPermissionByUserId]

(@userId int)

as

begin

	select Permission from RolePermission 
	where roleId in (select RoleId from Nhan_Vien where Id =@userId) 

end




---------------------
go
create procedure sp_Employee_GetStatus(@userId int)
as
begin
 select Status from Nhan_Vien where Id = @userId
end


------------------------

--keep old Name
go
ALTER PROCEDURE [dbo].[sp_HO_SO_TimHoSoQuanLy] 
	-- Add the parameters for the stored procedure here
	@offset int,
	@limit int,
	@userId int,
	@groupId int,
	@memberId int,
	@fromDate datetime,
	@toDate datetime,
	@status varchar(50),
	@freeText as varchar(50) = '',
	@dateType int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	set @status += ''
	Declare @DSNhomQL TABLE(ID int)
	if(@groupId = 0)
	Begin
	-- Lay nhom la thanh vien
	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @userId
	-- Lay nhom con truc tiep (nv la nguoi quan ly)
	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where 
	(Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') 
		Where CONVERT(int, value) in (Select * From @DSNhomQL)) > 0
	End
	Select count(*) over() as TotalRecord, HO_SO.ID, HO_SO.Ma_Ho_So as ProfileCode
	, HO_SO.Ngay_Tao as CreatedTime, DOI_TAC.Ten as PartnerName, HO_SO.CMND,
	 HO_SO.Ten_Khach_Hang as CustomerName,HO_SO.Ma_Trang_Thai as Status 
	 ,TRANG_THAI_HS.Ten as StatusName, HO_SO.Ngay_Cap_Nhat as UpdatedTime, NV1.Ho_Ten as EmployeeName,
	  HO_SO.Dia_Chi as Address,kvt.Ten as ProvinceName, 
	  fintechcom_vn_PortalNew.fn_getGhichuByHosoId(HO_SO.ID,1) as LastNote
	  , NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) 
	THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao)
	 END as DoiNguBanHang, 
	 SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY,TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@memberId > 0 and HO_SO.Ma_Nguoi_Tao = @memberId) 
			or (@groupId > 0 and @groupId = 0 and HO_SO.Ma_Nguoi_Tao in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @groupId) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @groupId)) or NHOM.ID = @groupId))
			)
			or (@groupId = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @memberId = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and ((HO_SO.Ngay_Tao between @fromDate and @toDate and @dateType = 1) 
	or (HO_SO.Ngay_Cap_Nhat between @fromDate and @toDate and @dateType = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@status, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
	order by HO_SO.Ngay_Cap_Nhat desc
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END

---------------

--exec sp_Profile_Search 0,10,1,0,0,'2020-01-01','2020-07-30','','',1
--exec sp_Profile_Search 0,10,4375,0,0,'2020-01-01','2020-07-30','','',1
alter procedure [dbo].[sp_Profile_Search]
(
	@offset int,
	@limit int,
	@userId int,
	@groupId int =0,
	@memberId int = 0,
	@fromDate datetime,
	@toDate datetime,
	@status varchar(50),
	@freeText as nvarchar(50) = '',
	@dateType int,
	@sort varchar(10) = 'desc',
	@sortField varchar(20) = 'updatedtime'
)
as
begin
if @freeText = '' begin set @freeText = null end;
if(@sort is null)
set @sort ='desc';
if(@sortField is null)
set @sortField ='updatedtime'
declare @where  nvarchar(1000) = ' where isnull(p.IsDeleted,0) = 0';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
set @mainClause = 'Select count(*) over() as TotalRecord, p.ID, p.Ma_Ho_So as ProfileCode
	, p.CreatedTime, dt.Ten as PartnerName, p.CMND, p.SDT as Phone,
	 p.Ten_Khach_Hang as CustomerName,dbo.fn_getStatusName(''common'',1, s.Value) as StatusName, p.UpdatedTime, NV1.Ho_Ten as CreatedBy,nv2.Ho_Ten as UpdatedBy,
	  dtrict.Ten as DistrictName ,prov.Ten as ProvinceName, 
	  fintechcom_vn_PortalNew.fn_getGhichuByHosoId(p.ID,1) as LastNote
	  , NV3.Ma as CourierCode,dt.Ten as PartnerName,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = p.CreatedBy) = 0) 
	THEN '''' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = p.CreatedBy)
	 END as SellTeam, 
	 sv.Ten as ProductName from HO_SO p left join NHAN_VIEN as nv1 on p.CreatedBy = nv1.Id
		left join NHAN_VIEN as nv2 on p.UpdatedBy = NV2.ID
		left join NHAN_VIEN as nv3 on p.Courier_Code = nv3.ID
		left join KHU_VUC as dtrict on dtrict.ID = p.Ma_Khu_Vuc
		left join KHU_VUC as prov on prov.ID = dtrict.Ma_Cha
		left join San_Pham_Vay sv on p.San_Pham_Vay = sv.Id
		left join Doi_Tac dt on sv.Ma_Doi_Tac = dt.Id
		left join ProfileStatus s on p.Ma_Trang_Thai = s.Value and s.OrgId = 1'
if(@freeText  is not null)
begin
 set @where += ' and (p.Ten_Khach_Hang like  N''%' + @freeText +'%''';
 set @where += ' or p.cmnd like  N''%' + @freeText +'%''';
 set @where += ' or p.SDT like  N''%' + @freeText +'%'')';
end;
if(@memberId > 0)
begin
	set @where += ' and p.CreatedBy = @memberId'
end;
--else
--begin
--	if(@groupId > 0)
--	begin
--		set @where += ' and p.CreatedBy in (select Ma_Nhan_Vien from NHAN_VIEN_NHOM where MA_Nhom = @groupId)'
--	end;
--end;
if(@status<>'' and @status is not null)
begin
	set @where += ' and p.Ma_Trang_Thai in (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@status, '','')) and s.OrgId = 1'
end;
if(@dateType =1)
begin
	set @where += ' and p.CreatedTime between @fromDate and @toDate'
end
else
begin
	set @where += ' and p.UpdatedTime between @fromDate and @toDate'
end
set @where += ' and @userId in (select * from fn_GetUserIDCanViewMyProfile_v2(p.CreatedBy,0))'
if(@sortField = 'updatedtime')
set @where+= ' order by p.UpdatedTime ' + @sort + ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
else
set @where +=' order by p.CreatedTime ' + @sort + ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N'@userId  int,@status varchar(20), @offset int, @limit int, @fromDate datetime, @toDate datetime, @dateType int,
@memberId int, @groupId int';
EXECUTE sp_executesql @mainClause,@params, @userId = @userId, @status = @status
, @offset = @offset, @limit = @limit, @fromDate = @fromDate, @toDate = @toDate,
 @dateType = @dateType, @memberId = @memberId, @groupId = @groupId;
print @mainClause;
end



-----------------

GO
create procedure [dbo].[sp_getPermissionByRoleCode]

(@rolecode varchar(50))

as

begin

	select Permission from RolePermission where RoleCode  = @rolecode

	--in (select RoleId from userrole where userId =@userId) 

end


-------------

create PROCEDURE [dbo].[sp_NHOM_LayCayNhomCon_v3] 
	-- Add the parameters for the stored procedure here
	@parentGroupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select g.ID as Id, g.Ten as Name, g.Chuoi_Ma_Cha as ParentCode, g.Ma_Nguoi_QL as LeaderId , g.Ten_Viet_Tat as ShortName From NHOM g
	Where 
	((g.Chuoi_Ma_Cha + '.' + Convert(nvarchar, g.ID)) like '%.' + Convert(nvarchar, @parentGroupId) + '.%')
	 or ((g.Chuoi_Ma_Cha + '.' + Convert(nvarchar, g.ID)) like '%.' + Convert(nvarchar, @parentGroupId))
END

-----------------

  create PROCEDURE [dbo].[sp_NHOM_LayDSChonTheoNhanVien_v3]
	-- Add the parameters for the stored procedure here
	@userId int
AS
BEGIN
declare @orgId int =0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where id = @userId;
	Select g.Id, g.Ten as Name, g.Chuoi_Ma_Cha as ParentCode, g.Ma_Nhom_Cha as Code, g.Ten_Viet_Tat as ShortName 
	From NHOM  g
	Where isnull(g.OrgId,0) = @orgId and
	g.ID in (Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @userId)
END

-------------

create PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v3]
	-- Add the parameters for the stored procedure here
	@groupId int,
	@userId int = 0
AS
BEGIN
	declare @orgId int = 0;
	select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select e.ID, e.Ma + ' - ' + e.Ho_Ten as Name, e.Ma as Code 
	From Nhan_Vien e
	Where isnull(e.OrgId,0) = @orgId and e.ID in (
Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM 
Where NHAN_VIEN_NHOM.Ma_Nhom in 
(Select NHOM.ID From NHOM 
Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
like '%.' + Convert(nvarchar, @groupId) + '.%') 
or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
like '%.' + Convert(nvarchar, @groupId)) or NHOM.ID = @groupId)
	)
END

----------


create PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhom_v3] 
	-- Add the parameters for the stored procedure here
	--Get User By group
	@groupId int,
	@userId int = 0
AS
BEGIN
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select e.Id, e.Ma + ' - ' + e.Ho_Ten as Name 
	From Nhan_Vien e, NHAN_VIEN_NHOM Where e.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @groupId
	and isnull(e.OrgId,0) = @orgId
END

----------------

insert into ProfileStatus(Name, Value, OrgId,Code)
values
(N'Thẩm định',0,1,'common'),
(N'Từ chối',0,1,'common'),
(N'Bồ sung hồ sơ',0,1,'common'),
(N'Giải ngân',0,1,'common'),
(N'Đã đối chiếu',0,1,'common'),
(N'Hủy',0,1,'common'),
(N'Lỗi PCB',0,1,'common'),
(N'Mới',0,1,'common'),
(N'Đang xử lý',0,1,'common'),
(N'Chấp nhận',0,1,'common'),
(N'Hoàn thành',0,1,'common')

------------


alter function [dbo].[fn_getStatusName](@profileType varchar(30), @orgId int, @value int)
returns nvarchar(500)
as begin
declare @noidung as nvarchar(500)
select top(1) @noidung = Name from ProfileStatus 
where OrgId = @orgId and ProfileType = @profileType and Value = @value
return @noidung;
end



-----------


ALTER procedure [dbo].[sp_ProfileStatus_Gets]
(@orgId int = 0,
@profileType varchar(50),
@roleId int =0
)
as begin
declare @isGetAll bit = 0;
if(@roleId = 1 and @profileType is null)
set @isGetAll = 1;
if(@isGetAll = 1)
begin
select Value as Id, Code, (Code +' - ' + Name) as Name  from ProfileStatus where  OrgId = @orgId and isnull(IsDeleted,0) = 0
end
else
begin
select @profileType = ProfileType from ProfileStatus where ProfileType = (Select Code from [Role] where Id = @roleId)
select Value as Id, Code, (Code +' - ' + Name) as Name from ProfileStatus where  ProfileType = @profileType and OrgId = @orgId and isnull(IsDeleted,0) = 0
end
end

---------

alter procedure [dbo].[sp_ProfileStatus_GetsByroleCode]
(@orgId int = 0,
@profileType varchar(50),
@roleCode varchar(50)
)
as begin
declare @isGetAll bit = 0;
if(@orgId =0)
set @orgId = 1
if(@roleCode = 'admin' and @profileType is null)
set @isGetAll = 1;
if(@isGetAll = 1)
begin
select Value as Id, Code, (Code +' - ' + Name) as Name  from ProfileStatus where  OrgId = @orgId and isnull(IsDeleted,0) = 0
end
else
begin
select Value as Id, Code, (isnull(Code,'') +' - ' + Name) as Name from ProfileStatus where  ProfileType = @profileType and OrgId = @orgId and isnull(IsDeleted,0) = 0
end
end

------------

create PROCEDURE [dbo].[sp_LOAI_TAI_LIEU_GetsByType]
(@profileType int = 0)
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@profileType = 0)
		select ID ,Ten as Name,Bat_Buoc as IsRequire ,ProfileTypeId, Id as FileKey from LOAI_TAI_LIEU
	else
		select ID ,Ten as Name,Bat_Buoc as IsRequire ,ProfileTypeId, Id as FileKey from LOAI_TAI_LIEU where ProfileTypeId = @profileType
END
  

-----------
alter table  DOI_TAC add OrgId int


create PROCEDURE [dbo].[sp_DOI_TAC_LayDS_v2](@orgId int)
  -- Add the parameters for the stored procedure here

AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  select ID,Ten as Name from DOI_TAC where isnull(OrgId,0) = @orgId
END


--------------

create PROCEDURE [dbo].[sp_KHU_VUC_LayDSTinh_v2]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten as Name from KHU_VUC where Loai=1
END

---------

create PROCEDURE [dbo].[sp_KHU_VUC_LayDSHuyen_v2]
	-- Add the parameters for the stored procedure here
	@provinceId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten as Name from KHU_VUC where Ma_Cha=@provinceId and Ma_Cha <>0
END

-----------

create PROCEDURE [dbo].[sp_SAN_PHAM_VAY_LayDSByID_v2]
	-- Add the parameters for the stored procedure here
	@partnerId int,
	@orgId int
AS
BEGIN
	select ID,Ten as Name from SAN_PHAM_VAY 
		where Ma_Doi_Tac=@partnerId 
		--and Loai=1
		and isnull(IsDeleted,0) = 0
END


-----------

create PROCEDURE [dbo].[sp_NHAN_VIEN_LayDSCourierCode_v2]
	-- Add the parameters for the stored procedure here
	@orgId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma+'-'+Ho_Ten as Name from NHAN_VIEN where Loai=10 and isnull(OrgId,0) = @orgId
END


-------------

update DOI_TAC set orgId = 1
update Nhan_Vien set OrgId = 1 where ISNULL(OrgId,0) = 0
update NHOM set OrgId = 1 where ISNULL(OrgId,0) = 0


--------------

go
alter function fn_generateProfileCode()
returns varchar(20)
as
begin
declare @prefix varchar(2)
declare @value int
declare @suffixes varchar(10)
declare @result varchar(20) = '000000' 
declare @suffix_temp varchar(10) = concat('00', month(getdate()))
declare @year varchar(4) = year(getdate())
set @year = SUBSTRING(@year,1,2)
select @prefix = isnull(Prefix,''), @suffixes = isnull(Suffixes,''),@value = isnull([Value],0) from AUTOID where ID=1
set @suffix_temp = SUBSTRING(@suffix_temp,len(@suffix_temp) - 1,2)

if(@prefix = @year)
begin
	if(@suffixes = @suffix_temp)
		set @value+=1;
	else
		begin
			set @suffixes = @suffix_temp;
			set @value = 1;
		end
end
else
begin
	set @prefix = @year;
	set @suffixes = @suffix_temp;
	set @value = 1;
end
set @result = concat(@result,@value)
set @result = SUBSTRING(@result, len(@result) - 5, 6);
set @result = @prefix + @suffixes + @result
return @result
end

--------------------

alter PROCEDURE sp_HO_SO_Them_v2
@id int out,
@Ten_Khach_Hang nvarchar(100),
@CMND nvarchar(50),
@Dia_Chi nvarchar(200),
@Ma_Khu_Vuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@Gioi_Tinh int,
@CreatedBy int,
@Ho_So_Cua_Ai int,
@Ngay_Nhan_Don datetime  = null,
@Ma_Trang_Thai int,
@San_Pham_Vay int,
@Co_Bao_Hiem bit,
@So_Tien_Vay decimal,
@Han_Vay float,
@Ghi_Chu nvarchar(500),
@Courier_Code int,
@IsDeleted bit,
@BirthDay datetime  = null,
@CMNDDay datetime  = null 
AS
BEGIN
declare @code varchar(20);
select @code = dbo.fn_generateProfileCode()
	Insert into HO_SO (Ma_Ho_So,Ten_Khach_Hang,CMND,Dia_Chi,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,CreatedTime,CreatedBy,Ho_So_Cua_Ai,UpdatedTime,Ngay_Nhan_Don,Ma_Trang_Thai,San_Pham_Vay,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Ghi_Chu,Courier_Code,IsDeleted,BirthDay,CMNDDay)
	values(@code,@Ten_Khach_Hang,@CMND,@Dia_Chi,@Ma_Khu_Vuc,@SDT,@SDT2,@Gioi_Tinh,GETDATE(),@CreatedBy,@Ho_So_Cua_Ai,GETDATE(),@Ngay_Nhan_Don,@Ma_Trang_Thai,@San_Pham_Vay,@Co_Bao_Hiem,@So_Tien_Vay,@Han_Vay,@Ghi_Chu,@Courier_Code,@IsDeleted,@BirthDay,@CMNDDay)

SET @id=@@IDENTITY
END
GO




--------------