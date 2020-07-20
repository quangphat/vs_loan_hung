--EXEC sp_rename 'dbo.NHAN_VIEN', 'Employee'
--EXEC sp_rename '[Employee].Ma', 'Code', 'COLUMN'
--EXEC sp_rename '[Employee].Ten_Dang_Nhap', 'UserName', 'COLUMN'
--EXEC sp_rename '[Employee].Mat_Khau', 'Password', 'COLUMN'
--EXEC sp_rename '[Employee].Ho_Ten', 'FullName', 'COLUMN'
--EXEC sp_rename '[Employee].Loai', 'TypeId', 'COLUMN'
--EXEC sp_rename '[Employee].Dien_Thoai', 'Phone', 'COLUMN'
--EXEC sp_rename '[Employee].Trang_Thai', 'Status', 'COLUMN'
--EXEC sp_rename '[Employee].Xoa', 'IsDeleted', 'COLUMN'


------------------
--EXEC sp_rename 'dbo.HO_SO', 'Profile'
--EXEC sp_rename '[Profile].Ma_Ho_so', 'Code', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Khach_Hang', 'CustomerCode', 'COLUMN'
--EXEC sp_rename '[Profile].Ten_Khach_Hang', 'CustomerName', 'COLUMN'
--EXEC sp_rename '[Profile].Dia_Chi', 'Address', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Khu_Vuc', 'DistricId', 'COLUMN'
--EXEC sp_rename '[Profile].SDT', 'Phone', 'COLUMN'
--EXEC sp_rename '[Profile].SDT2', 'Phone2', 'COLUMN'
--EXEC sp_rename '[Profile].Ngay_Tao', 'CreatedTime', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Nguoi_Tao', 'CreatedBy', 'COLUMN'
--EXEC sp_rename '[Profile].Ho_So_Cua_Ai', 'AssigneeId', 'COLUMN'
--EXEC sp_rename '[Profile].Ngay_Cap_Nhat', 'UpdatedTime', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Nguoi_Cap_Nhat', 'UpdatedBy', 'COLUMN'
--EXEC sp_rename '[Profile].Ngay_Nhan_Don', 'ReceiveDate', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Trang_Thai', 'StatusId', 'COLUMN'
--EXEC sp_rename '[Profile].Ma_Ket_Qua', 'ResultId', 'COLUMN'
--EXEC sp_rename '[Profile].San_Pham_Vay', 'ProductId', 'COLUMN'
--EXEC sp_rename '[Profile].Co_Bao_Hiem', 'IsInsurrance', 'COLUMN'
--EXEC sp_rename '[Profile].So_Tien_Vay', 'LoanMoney', 'COLUMN'
--EXEC sp_rename '[Profile].Han_Vay', 'LoanPeriod', 'COLUMN'
--EXEC sp_rename '[Profile].Ghi_Chu', 'LastNote', 'COLUMN'
--EXEC sp_rename '[Profile].Da_Xoa', 'IsDeleted', 'COLUMN'


----------------------------
create PROCEDURE [dbo].[sp_Employee_Login]
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
	Select *
	 From Employee where UserName=@UserName and Password=@Password and isnull(IsDeleted,0) =0
END


-------------------------
ALTER procedure [dbo].[sp_getPermissionByUserId]

(@userId int)

as

begin

	select Permission from RolePermission 
	where roleId in (select RoleId from Employee where Id =@userId) 

end


---------------------

ALTER procedure [dbo].[sp_Profile_Search]
(
	@offset int,
	@limit int,
	@userId int,
	@groupId int,
	@FromDate datetime,
	@ToDate datetime,
	@status varchar(50),
	@freeText as nvarchar(50) = '',
	@DateType int
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @where  nvarchar(1000) = 'where isnull(IsDeleted,0) = 0';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
set @mainClause = 'select count(*) over() as TotalRecord, p.Id, p.CustomerName,
p.Cmnd,p.Status, p.Phone, p.AssigneeId, p.CreatedTime, p.UpdatedTime
,fintecpom_vn_PortalNew.fn_getGhichuByHosoId(p.Id,1) as LastNote
,nv2.FullName as Assignee, nv1.FullName as CreatedUser,
ISNULL(kv1.Ten,'''') as ProvinceName, isnull(kv2.Ten,'''') as DistrictName
from Profile p left join Employee nv1 on p.CreatedBy = nv1.ID
left join Employee nv2 on p.AssigneeId = nv2.ID 
left join Khu_vuc kv1 on p.ProvinceId = kv1.Id
left join Khu_vuc kv2 on p.DistrictId = kv2.Id
left join San_Pham_Vay sp on p.ProductId = sp.Id
left join Doi_Tac dt on sp.Ma_Doi_Tac = dt.Id'
if(@freeText  is not null)
begin
 set @where = ' and (hc.CustomerName like  N''%' + @freeText +'%''';
 set @where = @where + ' or hc.cmnd like  N''%' + @freeText +'%''';
 set @where = @where + ' or hc.phone like  N''%' + @freeText +'%'')';
end;
if(@status<>'')
begin
	set @where += ' and p.Status in (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@status, '' + '',))'
end;
if(@DateType =1)
begin
	set @where += ' and p.CreatedTime between @fromDate and @toDate'
end
else
begin
	set @where += ' and p.UpdatedTime between @fromDate and @toDate'
end
set @where+= ' and p.Id in (select * from fn_GetUserIDCanViewMyProfile (@userId))'
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'

set @mainClause = @mainClause +  @where
set @params =N'@userIdId  int,@status varchar(20), @offset int, @limit int, @fromDate datetime, @toDate datetime';
EXECUTE sp_executesql @mainClause,@params, @userId = @userId, @status = @status
, @offset = @offset, @limit = @limit, @fromDate = @fromDate, @toDate = @toDate;
print @mainClause;
end


---------------------

create procedure sp_Employee_GetStatus(@userId int)
as
begin
 select Status from Employee where Id = @userId
end


------------------------

--keep old Name

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
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @groupId)) or NHOM.ID = @groupId)
			))
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