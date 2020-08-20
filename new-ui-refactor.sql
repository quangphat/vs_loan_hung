﻿--EXEC sp_rename 'dbo.NHAN_VIEN', 'Employee'
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
select @roleId = isnull(RoleId, 0) from  Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(Xoa,0) =0
if(@roleId =5)
set @roleId = 1;
	Select e.Id,e.Ten_Dang_Nhap as UserName, e.Email, e.Ho_Ten as FullName, e.Dien_Thoai as Phone,
	e.Ma as Code, e.Status as IsActive, r.Code as RoleCode, @roleId as RoleId, e.OrgId,R.Code as Rolecode
	 From Nhan_Vien e left join [Role] r
	on e.RoleId = r.Id
	 where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(e.Xoa,0) =0
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
	@userId int,
	@groupId int =0,
	@memberId int = 0,
	@fromDate datetime,
	@toDate datetime,
	@dateType int,
	@status varchar(50),
	@offset int,
	@limit int,
	@freeText as nvarchar(50) = '',
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

  alter PROCEDURE [dbo].[sp_NHOM_LayDSChonTheoNhanVien_v3]
	-- Add the parameters for the stored procedure here
	@userId int
AS
BEGIN
declare @orgId int =0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where id = @userId;
	Select g.Id, g.Ten as Name, g.Chuoi_Ma_Cha as ParentSequenceCode, g.Ma_Nhom_Cha as ParentCode, g.Ten_Viet_Tat as ShortName 
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

insert into ProfileStatus(Name, Value, OrgId,Code) values
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

insert into ProfileStatus(Name, Value, OrgId,Code) values
(N'Không nợ xấu',0,1,'duplicate'),
(N'Nợ chú ý',1,1,'duplicate'),
(N'Nợ xấu',2,1,'duplicate')

------------


create function [dbo].[fn_getStatusName](@profileType varchar(30), @orgId int, @value int)
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

create procedure [dbo].[sp_ProfileStatus_GetsByroleCode]
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

GO
create procedure [dbo].[sp_generateProfileCode]
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

	update AUTOID set Prefix=@prefix,Suffixes=@suffixes,[Value]=@value where ID=1
select @result
end

--------

create PROCEDURE sp_HO_SO_Them_v2
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
CREATE TABLE #AutoId (code varchar(20))
insert into #AutoId exec sp_generateProfileCode
set @code = (select top 1 code  from #AutoId) 
	Insert into HO_SO (Ma_Ho_So,Ten_Khach_Hang,CMND,Dia_Chi,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,CreatedTime,CreatedBy,Ho_So_Cua_Ai,UpdatedTime,Ngay_Nhan_Don,Ma_Trang_Thai,San_Pham_Vay,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Ghi_Chu,Courier_Code,IsDeleted,BirthDay,CMNDDay)
	values(@code,@Ten_Khach_Hang,@CMND,@Dia_Chi,@Ma_Khu_Vuc,@SDT,@SDT2,@Gioi_Tinh,GETDATE(),@CreatedBy,@Ho_So_Cua_Ai,GETDATE(),@Ngay_Nhan_Don,@Ma_Trang_Thai,@San_Pham_Vay,@Co_Bao_Hiem,@So_Tien_Vay,@Han_Vay,@Ghi_Chu,@Courier_Code,@IsDeleted,@BirthDay,@CMNDDay)
	drop table #AutoId
SET @id=@@IDENTITY
END
GO




--------------
go
create PROCEDURE [dbo].[sp_HO_SO_LayChiTiet_v2]
	-- Add the parameters for the stored procedure here
	@profileId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select hs.ID
	,Ma_Ho_So
	,Ten_Khach_Hang
	,CMND,Dia_Chi
	,Courier_Code
	,SDT,SDT2
	,Gioi_Tinh
	,hs.CreatedTime
	,hs.CreatedBy 
	,hs.DisbursementDate
	,Ma_Khu_Vuc
	,Ho_So_Cua_Ai
	,UpdatedTime 
	,UpdatedBy
	,Ngay_Nhan_Don
	,Ma_Trang_Thai
	,Ma_Ket_Qua
	,San_Pham_Vay
	,Co_Bao_Hiem 
	,So_Tien_Vay
	,Han_Vay
	,Ghi_Chu 
	,BirthDay
	,CmndDay
	,dt.ID as PartnerId
	,dbo.getProvinceIdFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceId
	from HO_SO hs
	left join SAN_PHAM_VAY sv on hs.San_Pham_Vay = sv.id
	left join DOI_TAC dt on sv.Ma_Doi_Tac = dt.ID
	 where hs.ID=@profileId
END





--------------


create 
 procedure [dbo].[sp_GetGhichuByHosoId_v2] 

@profileId as int,

@profileTypeId int = 1

as 

begin

select  g.HosoId as ProfileId, g.CommentTime ,g.Noidung as Content, g.TypeId as ProfileTypeId
,g.UserId,  Concat(CONCAT(n.Ma, ' - '),n.Ho_Ten) as Commentator from Ghichu g left join NHAN_VIEN n
on g.UserId = n.ID where g.HosoId= @profileId and TypeId = @profileTypeId

order by g.CommentTime desc

end

---------------

alter table Tai_lieu_HS
add GuidId varchar(50)

------------


--exec sp_TAI_LIEU_HS_Them_v2 0, '8d704f62-abe3-9adf-54d1-470f287c2d88',0

alter PROCEDURE [dbo].[sp_TAI_LIEU_HS_Them_v2]
(
@Id int out,
@FileKey int,
@FileName nvarchar(200),
@FilePath nvarchar(max),
@ProfileId int,
@Deleted bit = 0,
@ProfileTypeId int,
@DocumentName nvarchar(500) = null,
@DocumentCode varchar(400) =null,
@MC_DocumentId int = 0,
@MC_MapBpmVar varchar(400) = null,
@MC_GroupId int  = 0,
@OrderId int= 0,
@Folder nvarchar(max) = null,
@MCId varchar(20) = null,
@GuidId varchar(50) = null,
@FileId int =0
) 
AS
BEGIN
select top 1 @Id = isnull(Id,0) from TAI_LIEU_HS where (GuidId = @GuidId or ID = @FileId) 
if (@Id > 0 and @Id is not null)
begin
	update TAI_LIEU_HS set
	[FileName] = @FileName,
	FilePath = @FilePath,
	Folder = @Folder,
	MCId = @MCId,
	DocumentName = @DocumentName,
	DocumentCode = @DocumentCode,
	MC_DocumentId = @MC_DocumentId,
	MC_MapBpmVar = @MC_MapBpmVar,
	MC_GroupId = @MC_GroupId
	where ID = @Id
end
else
begin

	Insert into TAI_LIEU_HS 
	(FileKey,FileName,FilePath,ProfileId
	,Deleted,ProfileTypeId,DocumentName
	,DocumentCode,MC_DocumentId,MC_MapBpmVar,MC_GroupId, OrderId,Folder, MCId, GuidId)
	values(@FileKey,@FileName,@FilePath
	,@ProfileId,@Deleted,@ProfileTypeId
	,@DocumentName,@DocumentCode,@MC_DocumentId
	,@MC_MapBpmVar,@MC_GroupId,@OrderId,@Folder, @MCId, @GuidId)
	SET @Id=@@IDENTITY
end
END

---------------


create procedure sp_ProfileFileDeleteById(@fileId int, @guidId varchar(50))
as
begin
update TAI_LIEU_HS set Deleted = 1 where Id = @fileId or GuidId = @guidId;
end

-----------

ALTER PROCEDURE [dbo].[sp_TAI_LIEU_HS_Them_v2]
(
@Id int out,
@FileKey int,
@FileName nvarchar(200),
@FilePath nvarchar(max),
@ProfileId int,
@Deleted bit = 0,
@ProfileTypeId int,
@DocumentName nvarchar(500) = null,
@DocumentCode varchar(400) =null,
@MC_DocumentId int = 0,
@MC_MapBpmVar varchar(400) = null,
@MC_GroupId int  = 0,
@OrderId int= 0,
@Folder nvarchar(max) = null,
@MCId varchar(20) = null,
@GuidId varchar(50) = null,
@FileId int =0
) 
AS
BEGIN
select top 1 @Id = isnull(Id,0) from TAI_LIEU_HS where (GuidId = @GuidId or ID = @FileId) 
if (@Id > 0 and @Id is not null)
begin
delete TAI_LIEU_HS where ID = @Id
	--update TAI_LIEU_HS set
	--[FileName] = @FileName,
	--FilePath = @FilePath,
	--Folder = @Folder,
	--MCId = @MCId,
	--DocumentName = @DocumentName,
	--DocumentCode = @DocumentCode,
	--MC_DocumentId = @MC_DocumentId,
	--MC_MapBpmVar = @MC_MapBpmVar,
	--MC_GroupId = @MC_GroupId
	--where ID = @Id
end

	Insert into TAI_LIEU_HS 
	(FileKey,FileName,FilePath,ProfileId
	,Deleted,ProfileTypeId,DocumentName
	,DocumentCode,MC_DocumentId,MC_MapBpmVar,MC_GroupId, OrderId,Folder, MCId, GuidId)
	values(@FileKey,@FileName,@FilePath
	,@ProfileId,@Deleted,@ProfileTypeId
	,@DocumentName,@DocumentCode,@MC_DocumentId
	,@MC_MapBpmVar,@MC_GroupId,@OrderId,@Folder, @MCId, @GuidId)
	SET @Id=@@IDENTITY
END

-----------


ALTER procedure [dbo].[getTailieuByHosoId](@profileId int, @profileTypeId int =1)

as

begin
--3 is mcredit
if(@profileTypeId =3)
begin
	select tl.ID as FileId,
	tl.FileKey as [Key] 
	, tl.FileName 
		, tl.FilePath as FileUrl
		,tl.DocumentName as KeyName
		,tl.ProfileId
		,tl.ProfileTypeId
		,tl.Folder,
		tl.DocumentName ,
		tl.DocumentCode ,
		tl.MC_DocumentId,
		tl.MC_MapBpmVar,
		tl.MC_GroupId
	from TAI_LIEU_HS tl where ProfileId = @profileId 
	and ProfileTypeId = 3
	and ISNULL(tl.Deleted,0) = 0
end
else
begin
	select tl.Id as FileId
		, tl.FileKey as [Key]
		, tl.FileName 
		, tl.FilePath as FileUrl
		,isnull(ltl.Ten,tl.DocumentName) as KeyName
		, ltl.Bat_Buoc as IsRequire
		,tl.ProfileId
		,tl.ProfileTypeId
		,tl.Folder,
		tl.DocumentName ,
		tl.DocumentCode ,
		tl.MC_DocumentId,
		tl.MC_MapBpmVar,
		tl.MC_GroupId,
		tl.GuidId
		from TAI_LIEU_HS tl
		left join LOAI_TAI_LIEU ltl on tl.FileKey = ltl.ID
		where tl.ProfileId = @profileId and ISNULL(tl.Deleted,0) = 0
		and tl.ProfileTypeId = @profileTypeId
		order by ltl.Bat_Buoc desc
end;
end



------------------

alter PROCEDURE sp_update_HO_SO_v2
@Ten_Khach_Hang nvarchar(100),
@CMND nvarchar(50),
@Dia_Chi nvarchar(200),
@Ma_Khu_Vuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@Gioi_Tinh int,
@Ho_So_Cua_Ai int,
@Ngay_Nhan_Don datetime,
@Ma_Trang_Thai int,
@San_Pham_Vay int,
@Co_Bao_Hiem bit,
@So_Tien_Vay decimal,
@Han_Vay float,
@Courier_Code int,
@BirthDay datetime,
@CMNDDay datetime,
@UpdatedBy int,
@ID int 
AS
BEGIN
	UPDATE [dbo].HO_SO SET Ten_Khach_Hang=@Ten_Khach_Hang,CMND=@CMND,Dia_Chi=@Dia_Chi,Ma_Khu_Vuc=@Ma_Khu_Vuc,SDT=@SDT,SDT2=@SDT2
	,Gioi_Tinh=@Gioi_Tinh,Ho_So_Cua_Ai=@Ho_So_Cua_Ai,UpdatedTime=GETDATE()
	,UpdatedBy=@UpdatedBy,Ngay_Nhan_Don=@Ngay_Nhan_Don,Ma_Trang_Thai=@Ma_Trang_Thai
	,San_Pham_Vay=@San_Pham_Vay,Co_Bao_Hiem=@Co_Bao_Hiem,So_Tien_Vay=@So_Tien_Vay,Han_Vay=@Han_Vay
	,Courier_Code=@Courier_Code,IsDeleted=0,BirthDay=@BirthDay,CMNDDay=@CMNDDay
	WHERE ID=@ID 
end
GO

-------------


create PROCEDURE [dbo].[sp_NHOM_LayDSNhomDuyetChonTheoNhanVien_v3]
	-- Add the parameters for the stored procedure here
	@userId int
AS
BEGIN
declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select NHOM.ID , NHOM.Ten Name, NHOM.Chuoi_Ma_Cha as ParentSequenceCode, NHOM.Ma_Nhom_Cha as ParentCode, NHOM.Ten_Viet_Tat 
	as ShortName From NHOM
	Where isnull(NHOM.OrgId,0) = @orgId and NHOM.ID in 
	(Select NHAN_VIEN_CF.Ma_Nhom 
	From NHAN_VIEN_CF 
	--Where NHAN_VIEN_CF.Ma_Nhan_Vien = @UserID
	)
END


------------

create function [dbo].[getProvinceName](@provinceId int)
returns nvarchar(200)
as
begin
declare @name nvarchar(200)
 select @name = Ten from KHU_VUC 
where Id = @provinceId and Loai = 1
return @name;
end

---------

create function [dbo].[getPartnerName](@partnerId int)
returns nvarchar(200)
as
begin
declare @name nvarchar(200)
 select @name = Ten from DOI_TAC 
where Id = @partnerId
return @name;
end

--------------

alter procedure [dbo].[sp_GetCustomer_v2]
(
@freeText nvarchar(30),
@offset int,
@limit int,
@userId int = 0
)
as
begin
if(@userId>0)
begin
declare @isAdmin int = 0;
select @isAdmin = dbo.fn_CheckIsAdmin(@userId);
declare @query  nvarchar(max);
declare @wherequery nvarchar(500) = ' where dt.AllowCheckDup =1 and Createdby is not null';
declare @param nvarchar(max) = '@freetext_tmp nvarchar(30), @userId_tmp int,@offset_tmp int = 0,@limit_tmp int  =10 '
if @freeText = '' begin set @freeText = null end;
if(@userId >0 and @isAdmin = 0)
begin
	set @wherequery += ' and n.CreatedBy = @userId_tmp'
end
if(@freeText is not null)
begin
	set @wherequery += N' and (@freetext_tmp is null 
		or n.FullName like N''%'' +' + ' @freetext_tmp' + '+''%'' 
		or n.Cmnd like ''%'' + ' + '@freetext_tmp' + '+''%''
		)'
end

	set @query = N'Select count(*) over() as TotalRecord, n.*,  dbo.fn_getStatusName(''duplicate'',1, n.CICStatus) as CICStatusName ,
	 dbo.fn_getStatusName(''checkdup-partner-status'',1, n.PartnerStatus) as PartnerStatusName,
	 dbo.getProvinceName(n.ProvinceId) as ProvinceName, dbo.getPartnerName(n.PartnerId) as PartnerName
	from Customer n left join Doi_tac dt on n.PartnerId = dt.Id ' + @wherequery 
	+ ' order by n.Id desc 
	offset @offset_tmp ROWS FETCH NEXT @limit_tmp ROWS ONLY';
	print @query;
	execute sp_executesql @query, @param,@freetext_tmp = @freeText, @userId_tmp = @userId, @offset_tmp = @offset,@limit_tmp = @limit;
end

end




------------


  alter table customer
  add PartnerStatus int

  ---------

  alter procedure [dbo].[sp_InsertCustomer_v2]
  (
  @id int out,
  @fullname nvarchar(200),
  @checkdate datetime,
  @cmnd varchar(15),
  @CICStatus int,
  @LastNote nvarchar(200),
  @partnerId int,
  @partnerStatus int,
  @createdby int,
  @ProvinceId int,
  @Address nvarchar(200),
  @Phone varchar(12),
  @Salary decimal(18,2)
  )
  as
  begin
	insert into Customer(
	FullName
	,CheckDate
	,Cmnd
	,CICStatus
	,LastNote
	,PartnerId
	,PartnerStatus
	,[Status]
	,UpdatedTime
	,CreatedTime
	,CreatedBy
	,ProvinceId
	,Address
	,Phone
	,Salary)
	values
	(@fullname
	,@checkdate
	,@cmnd
	,@CICStatus
	,@LastNote
	,@partnerId
	,@partnerStatus
	,1
	,GETDATE()
	,GETDATE()
	,@createdby
	,@ProvinceId
	,@Address
	,@Phone
	,@Salary);
	SET @id=@@IDENTITY;

  end

  ----------

    alter procedure [dbo].[sp_UpdateCustomer_v2]
  (
  @id int,
  @fullname nvarchar(200),
  @checkdate datetime,
  @cmnd varchar(15),
  @updatedby int,
  @LastNote nvarchar(200) = null,
  @ProvinceId int,
  @Address nvarchar(200),
  @Phone varchar(12),
  @Salary decimal(18,2),
  @PartnerId int,
  @PartnerStatus int,
  @CICStatus int
  )
  as
  begin
	update Customer 
		set FullName = @fullname,
		CheckDate = @checkdate,
		Cmnd = @cmnd,
		UpdatedTime = GETDATE(),
		UpdatedBy = @updatedby,
		[Status] = 1,
		ProvinceId = @ProvinceId,
		Address = @Address,
		Phone = @Phone,
		Salary = @Salary,
		PartnerId = @PartnerId,
		PartnerStatus = @PArtnerStatus,
		CICStatus = @CICStatus
		where Id = @id
	  if(@LastNote is not null)
		update Customer set LastNote = @LastNote where Id = @id
  end

  ----------

  
alter table Company
add IsDeleted bit

--------

create procedure [dbo].[sp_GetCompany_v2]
(
@freeText nvarchar(30),
@offset int,
@limit int
)
as
begin
declare @query  nvarchar(max);
declare @where nvarchar(500) ='where isnull(c.IsDeleted,0) = 0 '
if @freeText = '' begin set @freeText = null end;
set @query ='Select count(*) over() as TotalRecord,c.* from Company c '
if(@freeText is not null)
begin
	set @where += N' and (c.FullName like N''%'' +' + ' @freetext' + '+''%'' 
			or c.Cmnd like ''%'' + ' + '@freetext' + '+''%'')'
end
set @where += ' order by c.Id desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @query+=@where
declare @param nvarchar(max) = '@freetext nvarchar(30), @offset int = 0,@limit int  =10 '
print @query;
execute sp_executesql @query, @param,@freetext = @freeText,  @offset = @offset,@limit = @limit;
end

---------

  create PROCEDURE [dbo].[sp_NHOM_LayDSNhom_v2](@userId int =0)
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select NHOM.ID, NHOM.Ten as Name, NHOM.Chuoi_Ma_Cha as ParentSequenceCode From NHOM where isnull(OrgId,0) = @orgId
END

-----------

--insert into ProfileStatus 
--(Name, Value, OrgId, ProfileType) values
--(N'Mới', 1, 1, 'courier'),
--(N'Đang xử lý', 2, 1, 'courier'),
--(N'Từ chối', 3, 1, 'courier'),
--(N'Chấp nhận', 4, 1, 'courier'),
--(N'Giải ngân', 5, 1, 'courier'),
--(N'Hoàn thành', 6, 1, 'courier')

--insert into ProfileStatus 
--(Name, Value, OrgId, ProfileType) values
--(N'Huỷ', 7, 1, 'courier')
-------------

ALTER procedure [dbo].[sp_GetHosoCourier]
(
@freeText nvarchar(30),
@assigneeId int = 0,
@status varchar(20) ='',
@provinceId int = 0,
@page int =1,
@limit_tmp int = 10,
@groupId int =0,
@saleCode varchar(20) ='',
@userId int)as
begin
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freeText = '' begin set @freeText = null end;
if @saleCode = '' begin set @saleCode = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select count(*) over() as TotalRecord, hc.Id, dbo.fn_getStatusName(''common'',1, hc.Status) as StatusName,
hc.CustomerName,hc.Cmnd,hc.Status,hc.SaleCode, hc.Phone, hc.AssignUserId, hc.CreatedBy, hc.UpdatedBy,
 hc.CreatedTime, hc.UpdatedTime,fintechcom_vn_PortalNew.fn_getGhichuByHosoId(hc.Id,2) as LastNote, nv2.Ho_Ten as AssignUser,
 nv1.Ho_Ten as CreatedUser,
 ISNULL(kv1.Ten,'''') as ProvinceName, isnull(kv2.Ten,'''') as DistrictName
  from HosoCourrier hc left join Nhan_Vien nv1 on hc.CreatedBy = nv1.ID left join Nhan_Vien nv2 on hc.AssignUserId = nv2.ID 
   left join Khu_vuc kv1 on hc.ProvinceId = kv1.Id
    left join Khu_vuc kv2 on hc.DistrictId = kv2.Id'
	if(@freeText  is not null)
	begin
	set @where = ' (hc.CustomerName like  N''%' + @freeText +'%''';
	set @where = @where + ' or hc.cmnd like  N''%' + @freeText +'%''';
	set @where = @where + ' or hc.phone like  N''%' + @freeText +'%''';
	set @where = @where + ' or nv2.Ho_Ten like  N''%' + @freeText +'%'' )';
	end;
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' (@userId in (select * from fn_GetUserIDCanViewMyProfile (hc.CreatedBy)) 
or (hc.Id in (select CourierId from CourierAssignee where @userId = AssigneeId))
or (@userId in (select * from fn_MaNguoiQuanlyByHoSocourierId (hc.Id)))
or (hc.SaleCode = (select Ma from Nhan_Vien where id = @userId))'
if(@assigneeId > 0)
begin
set @where += ' or hc.SaleCode = (select Ma from Nhan_Vien where Id = @assigneeId))'; 
end;
else
begin
set @where += ')'
end
if(@status <> '')
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' hc.Status in ('+ @status +')'; 
end;
if(@groupId <> 0)
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' hc.GroupId = @groupId';
end;
if(@provinceId <> 0)
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' hc.ProvinceId = @provinceId'; 
end;
if(@saleCode is not null)
begin
if(@where <> '')  set @where = @where + ' and';
set @where = @where + ' hc.SaleCode = @saleCode'; 
end;
if(@where <>'')
begin
set @where= ' where ' + @where
end
set @where = @where + ' and isnull(hc.IsDeleted,0) = 0  order by hc.createdTime desc';
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N'@assigneeId  int,@status varchar(20), @offset int, @limit int, @groupId int, @provinceId int
, @saleCode varchar(20),@userId int';
EXECUTE sp_executesql @mainClause,@params, @assigneeId = @assigneeId, @provinceId = @provinceId,
@status = @status, @offset = @offset, @limit = @limit_tmp, @groupId = @groupId, @saleCode = @saleCode,@userId = @userId;
print @mainClause;
end


---------------


  insert into LOAI_TAI_LIEU (Ten, Bat_Buoc, ProfileTypeId)
  select Ten, Bat_Buoc, 2 from LOAI_TAI_LIEU

  ------------
