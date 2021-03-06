﻿--EXEC sp_rename 'Nhan_Vien', 'Employee'

EXEC sp_rename 'Employee', 'Nhan_Vien'

-- revoke debt

alter table Nhan_Vien 
add OrgId int

EXEC sp_rename 'Nhan_Vien.Trang_Thai', 'Status', 'COLUMN'
-----------
go
alter PROCEDURE [dbo].[sp_Employee_Login]
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
declare @roleId int = 1;
select @roleId = isnull(RoleId, 0) from  Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(Xoa,0) =0
if(@roleId =5)
set @roleId = 1;
	Select Id,Ten_Dang_Nhap AS UserName, Mat_Khau as Passowrd, Ma as Code,
	Email, Ho_Ten as FullName, Dien_Thoai as Phone, Status as IsActive, isnull(OrgId,0) as OrgId, @roleId as RoleId
	 From Nhan_Vien where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(Xoa,0) =0
END



--------------x
go
create PROCEDURE [dbo].[sp_Profile_GetProfileHaveNotSeen] 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') 
	Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND
	, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS
	, KET_QUA_HS.Ten as KetQuaHS, HO_SO.UpdatedTime as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang
	, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy) = 0) 
	THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID 
	and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.CreatedBy) END as DoiNguBanHang
	From HO_SO_DUYET_XEM,HO_SO left join Nhan_Vien as NV1 on HO_SO.CreatedBy = NV1.ID
		left join Nhan_Vien as NV2 on HO_SO.UpdatedBy = NV2.ID
		left join Nhan_Vien as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where
	HO_SO_DUYET_XEM.Xem=0
	and HO_SO_DUYET_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM 
		Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
		Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) 
	or (HO_SO.UpdatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.IsDeleted = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
END


-----------------------xx

go
create PROCEDURE [dbo].[sp_Profile_GetMyProfilesNotSeen] 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@SDT nvarchar(50),
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.CreatedTime as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, 
	HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, 
	HO_SO.UpdatedTime as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua,
	 HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO_XEM,HO_SO left join Nhan_Vien as NV1 on HO_SO.CreatedBy = NV1.ID
		left join Nhan_Vien as NV2 on HO_SO.UpdatedBy = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	HO_SO_XEM.Xem=0
	and HO_SO_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.Ma_Nguoi_Tao = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.CreatedTime between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.IsDeleted = 0 
	--and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.Ngay_Tao desc
END


------------------x
go
create procedure [dbo].[sp_CountEmployee]

(

@workFromDate datetime,

@workToDate datetime,

@roleId int,

@freeText nvarchar(30)

)

as

begin

if @freeText = '' begin set @freeText = null end;

Select count(*) from Nhan_Vien n

where (convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))

and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)

	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'

		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'

		or n.Dien_Thoai like N'%' + @freeText + '%'

		or n.Email like N'%' + @freeText + '%')

		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))

		and n.Xoa = 0

end




--------------x
go
create procedure [dbo].[sp_GetEmployees]
(
@workFromDate datetime,
@workToDate datetime,
@freeText nvarchar(30),
@roleId int,
@page int,
@limit int
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @offset as int;
set @offset = (@page-1)*@limit;
Select n.ID,n.Ten_Dang_Nhap as UserName,n.Ho_Ten as FullName,n.RoleId,r.Name as RoleName,
n.Email, n.Dien_Thoai as Phone,n.CreatedTime,
n.Ma as Code,
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from Nhan_Vien n left join KHU_VUC k on n.DistrictId = k.ID
 left join KHU_VUC k2 on k.Ma_Cha = k2.ID
 left join Role r on n.RoleId = r.Id
where ( convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.Xoa = 0
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end






-------------x

go
create procedure sp_GetEmployeeById(@userId int)
as
begin
select * from Nhan_Vien where ID = @userId
end


---------x
go
create procedure [dbo].[sp_Employee_UpdateUser_v2]
(
@id int,
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@roleId int,
@updatedby int
)
as
begin
--declare @oldRoleId int = 0;
--select @oldRoleId = RoleId from NHAN_VIEN where ID = @id;
--if(@oldRoleId is not null and @oldRoleId >0 )
--begin

--end
update Nhan_Vien set 
		Ho_Ten = @fullName,
		Dien_Thoai = @phone,
		Email = @email,
		RoleId = @roleId,
		UpdatedBy = @updatedby
		
		where ID = @id
end

----------x

go
create procedure [dbo].[sp_Employee_InsertUser_v2]
(
@id int out,
@userName varchar(50),
@code varchar(50),
@password varchar(50),
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@roleId int,
@createdby int
)
as
begin
declare @orgId int  = 0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @createdby;
insert into Nhan_Vien(Ma,Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,RoleId,Status,Xoa,CreatedTime,CreatedBy, OrgId, UpdatedTime)
values (@code,@userName,@password,@fullName,@phone,@email,@roleId,1,0,GETDATE(),@createdby, @orgId, GETDATE());
SET @id=@@IDENTITY
end


---------x
go
create PROCEDURE [dbo].[sp_Employee_GetFull] 
(@orgId int =0)
AS
BEGIN
Select  Id, Ma + ' - ' + Ho_Ten as Name From Nhan_Vien where isnull(OrgId,0) = @orgId and isnull(Xoa,0) = 0 
 order by ID desc
END

-----------x
go
create PROCEDURE [dbo].[sp_Employee_LayDSByMaQL_v2]
	-- Add the parameters for the stored procedure here
	@MaQL int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma as Code, Ma + ' - ' + Ho_Ten as HoTen from Nhan_Vien where ID=@MaQL
END


--------x
alter table Nhom 
add OrgId int

--------xx
go
create PROCEDURE [dbo].[sp_Group_GetChildGroup] 
	-- Add the parameters for the stored procedure here
	@parentGroupId int,
	@userId int =0
AS
BEGIN
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select g.ID, g.Ten, 
	g.Ten_Viet_Tat as TenNgan, 
	e.Ho_Ten as NguoiQuanLy, 
	g.Chuoi_Ma_Cha as ChuoiMaCha 
	From NHOM g left join Nhan_Vien e on g.Ma_Nguoi_QL = e.ID
	Where g.Ma_Nhom_Cha = @parentGroupId
	and isnull(g.OrgId, 0) = @orgId
END

-------xx


go
create PROCEDURE [dbo].[sp_Group_GetById] 
	-- Add the parameters for the stored procedure here
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Ten_Viet_Tat as TenNgan, NHOMCHA.Ten as TenNhomCha, e.Ho_Ten as NguoiQuanLy
	From Nhan_Vien e, NHOM
	left join NHOM as NHOMCHA on NHOM.Ma_Nhom_Cha = NHOMCHA.ID
	Where NHOM.ID = @groupId and e.ID = NHOM.Ma_Nguoi_QL
END

----------x
go
create PROCEDURE [dbo].[sp_Employee_Group_GetEmployeeByGroup]
-- Add the parameters for the stored procedure here
@groupId int
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
Select e.ID, e.Ma, e.Ho_Ten as HoTen, e.Email, e.Dien_Thoai as SDT 
From Nhan_Vien e, NHAN_VIEN_NHOM 
Where e.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @groupId
END


--------xxx
go
alter PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v2]
	-- Add the parameters for the stored procedure here
	@groupId int,
	@userId int = 0
AS
BEGIN
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Employee where Id = @userId;
	Select e.ID, e.Ma + ' - ' + e.Ho_Ten as Name, e.Ma as Code 
	From Employee e
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

------xx


go
create PROCEDURE [dbo].[sp_employee_Group_LayDSKhongThanhVienNhom_v2]
	-- Add the parameters for the stored procedure here
	@groupId int,
	@userId int = 0
AS
BEGIN
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select e.Id, e.Ma + ' - ' + e.Ho_Ten as Name 
	From Nhan_Vien e 
	Where e.ID not in (Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = @groupId)
	and e.OrgId = @orgId
END

--------x


go
alter PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhom_v2] 
	-- Add the parameters for the stored procedure here
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


----------x


go
create PROCEDURE [dbo].[sp_Employee_LayDSByRule]
	-- Add the parameters for the stored procedure here
	@UserId int,
	@Rule int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select distinct e.ID, e.Ma as Code,e.Ho_Ten as FullName,e.Ten_Dang_Nhap as UserName,Dien_Thoai as Phone,Email as Email 
	from Nhan_Vien e,NHAN_VIEN_CF
	where --ID=@UserID
	 e.ID=NHAN_VIEN_CF.Ma_Nhan_Vien
	and NHAN_VIEN_CF.Quyen=@Rule
	and NHAN_VIEN_CF.Ma_Nhom in (Select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien=@UserId)

END

-------x

create table RevokeDebt
(Id int identity(1,1) not null,
AgreementNo varchar(50),
CustomerName nvarchar(200),
LastestPaymentDate varchar(20),
PaymentStore nvarchar(50),
OSPri varchar(20),
TotalCurros varchar(20),
LateFee varchar(20),
LiquidationFee varchar(20),
LateDate varchar(10),
InterestrateScheme varchar(20),
InstallmentPeriod varchar(10),
InstallmentNo varchar(10),
BillAmountOfCurrentMonth varchar(20),
ProductName nvarchar(100),
ProductBrand nvarchar(100),
CashPrice varchar(20),
DepositAmount varchar(20),
FinancePrice varchar(20),
FirstDueDate varchar(20),
AgentCode varchar(20),
Gender nvarchar(10),
Age varchar(5),
AgreementDate varchar(20),
MobilePhone varchar(12),
HomePhone varchar(12),
CompanyPhone varchar(12),
TotalPayableAmount varchar(20),
LastPaymentAmount varchar(20),
TotalPaidAmount varchar(20),
FirstPaymentAmount varchar(20),
FinalDueDate varchar(20),
ReferenceName nvarchar(200),
RefPhone varchar(12),
[Relative] nvarchar(200),
IdCardNumber varchar(12),
Bod varchar(20),
PermanentAddress nvarchar(300),
CompanyName nvarchar(300),
Department nvarchar(200),
WorkAddress nvarchar(300),
IsDeleted bit,
[Status] int
)


alter table RevokeDebt
add [Status] int
alter table RevokeDebt
add FinalPaymentAmount varchar(20),
UpdatedTime datetime,
UpdatedBy int,
CreatedTime datetime,
CreatedBy int

alter table RevokeDebt
add AssigneeGroupIds varchar(50),
AssigneeIds varchar(50)
 
----------------x


CREATE TABLE [dbo].[ImportExcel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Position] [int] NULL,
	[ImportType] [int] NULL,
	[ValueType] [varchar](20) NULL
)
-----------x

insert into ImportExcel(Name,Position,ImportType,ValueType)
select COLUMN_NAME,ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS rownum, 5, 'string'
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='RevokeDebt'

-------------x

update ImportExcel set Position = Position - 2 where ImportType = 5
-------------------
go
create procedure sp_ImportExcel_GetByType
(@type int)
as begin
select * from ImportExcel where ImportType = @type 
order by Position
end


------------xx
go
create PROCEDURE sp_update_RevokeDebt
@AgreementNo varchar(50),
@CustomerName nvarchar(200),
@LastestPaymentDate varchar(20),
@PaymentStore nvarchar(50),
@OSPri varchar(20),
@TotalCurros varchar(20),
@LateFee varchar(20),
@LiquidationFee varchar(20),
@LateDate varchar(10),
@InterestrateScheme varchar(20),
@InstallmentPeriod varchar(10),
@InstallmentNo varchar(10),
@BillAmountOfCurrentMonth varchar(20),
@ProductName nvarchar(100),
@ProductBrand nvarchar(100),
@CashPrice varchar(20),
@DepositAmount varchar(20),
@FinancePrice varchar(20),
@FirstDueDate varchar(20),
@AgentCode varchar(20),
@Gender nvarchar(10),
@Age varchar(5),
@AgreementDate varchar(20),
@MobilePhone varchar(12),
@HomePhone varchar(12),
@CompanyPhone varchar(12),
@TotalPayableAmount varchar(20),
@LastPaymentAmount varchar(20),
@TotalPaidAmount varchar(20),
@FirstPaymentAmount varchar(20),
@FinalDueDate varchar(20),
@FinalPaymentAmount varchar(20),
@ReferenceName nvarchar(200),
@RefPhone varchar(12),
@Relative nvarchar(200),
@IdCardNumber varchar(12),
@Bod varchar(20),
@PermanentAddress nvarchar(300),
@CompanyName nvarchar(300),
@Department nvarchar(200),
@WorkAddress nvarchar(300) ,
@UpdatedBy int,
@Id int ,
@AssigneeGroupIds varchar(50),
@AssigneeIds varchar(50)
AS
BEGIN
	UPDATE [dbo].RevokeDebt 
	SET AgreementNo=@AgreementNo
	,CustomerName=@CustomerName
	,LastestPaymentDate=@LastestPaymentDate
	,PaymentStore=@PaymentStore
	,OSPri=@OSPri
	,TotalCurros=@TotalCurros
	,LateFee=@LateFee
	,LiquidationFee=@LiquidationFee
	,LateDate=@LateDate
	,InterestrateScheme=@InterestrateScheme
	,InstallmentPeriod=@InstallmentPeriod
	,InstallmentNo=@InstallmentNo
	,BillAmountOfCurrentMonth=@BillAmountOfCurrentMonth
	,ProductName=@ProductName
	,ProductBrand=@ProductBrand
	,CashPrice=@CashPrice
	,DepositAmount=@DepositAmount
	,FinancePrice=@FinancePrice
	,FirstDueDate=@FirstDueDate
	,AgentCode=@AgentCode
	,Gender=@Gender
	,Age=@Age
	,AgreementDate=@AgreementDate
	,MobilePhone=@MobilePhone
	,HomePhone=@HomePhone
	,CompanyPhone=@CompanyPhone
	,TotalPayableAmount=@TotalPayableAmount
	,LastPaymentAmount=@LastPaymentAmount
	,TotalPaidAmount=@TotalPaidAmount
	,FirstPaymentAmount=@FirstPaymentAmount
	,FinalDueDate=@FinalDueDate
	,FinalPaymentAmount = @FinalPaymentAmount
	,ReferenceName=@ReferenceName
	,RefPhone=@RefPhone
	,[Relative]=@Relative
	,IdCardNumber=@IdCardNumber
	,Bod=@Bod
	,PermanentAddress=@PermanentAddress
	,CompanyName=@CompanyName
	,Department=@Department
	,WorkAddress=@WorkAddress 
	,UpdatedBy = @UpdatedBy
	,UpdatedTime = GETDATE(),
	AssigneeGroupIds = @AssigneeGroupIds,
	AssigneeIds = @AssigneeIds
	WHERE Id=@Id
end

----------------x
go
alter PROCEDURE sp_insert_RevokeDebt
@AgreementNo varchar(50),
@CustomerName nvarchar(200),
@LastestPaymentDate varchar(20),
@PaymentStore nvarchar(50),
@OSPri varchar(20),
@TotalCurros varchar(20),
@LateFee varchar(20),
@LiquidationFee varchar(20),
@LateDate varchar(10),
@InterestrateScheme varchar(20),
@InstallmentPeriod varchar(10),
@InstallmentNo varchar(10),
@BillAmountOfCurrentMonth varchar(20),
@ProductName nvarchar(100),
@ProductBrand nvarchar(100),
@CashPrice varchar(20),
@DepositAmount varchar(20),
@FinancePrice varchar(20),
@FirstDueDate varchar(20),
@AgentCode varchar(20),
@Gender nvarchar(10),
@Age varchar(5),
@AgreementDate varchar(20),
@MobilePhone varchar(12),
@HomePhone varchar(12),
@CompanyPhone varchar(12),
@TotalPayableAmount varchar(20),
@LastPaymentAmount varchar(20),
@TotalPaidAmount varchar(20),
@FirstPaymentAmount varchar(20),
@FinalDueDate varchar(20),
@FinalPaymentAmount varchar(20),
@ReferenceName nvarchar(200),
@RefPhone varchar(12),
@Relative nvarchar(200),
@IdCardNumber varchar(12),
@Bod varchar(20),
@PermanentAddress nvarchar(300),
@CompanyName nvarchar(300),
@Department nvarchar(200),
@WorkAddress nvarchar(300)  ,
@CreatedBy int,
@AssigneeIds varchar(20),
@AssigneeId int =0,
@DistrictId int =0,
@ProvinceId int = 0,
@Deleted int =0
AS
BEGIN
if(@Deleted > 0)
begin
 update RevokeDebt set IsDeleted = 1 where AgreementNo = @AgreementNo;
end
else
begin
	Insert into RevokeDebt (AgreementNo,CustomerName,LastestPaymentDate
	,PaymentStore,OSPri,TotalCurros,LateFee,LiquidationFee,LateDate
	,InterestrateScheme,InstallmentPeriod,InstallmentNo,BillAmountOfCurrentMonth
	,ProductName,ProductBrand,CashPrice,DepositAmount,FinancePrice,FirstDueDate,AgentCode
	,Gender,Age,AgreementDate,MobilePhone,HomePhone,CompanyPhone,TotalPayableAmount
	,LastPaymentAmount,TotalPaidAmount,FirstPaymentAmount,FinalDueDate,FinalPaymentAmount,ReferenceName
	,RefPhone,[Relative],IdCardNumber,Bod,PermanentAddress,CompanyName,Department,WorkAddress
	,CreatedTime,CreatedBy,UpdatedTime,AssigneeIds,Status, AssigneeId,DistrictId, ProvinceId)
	values(@AgreementNo,@CustomerName,@LastestPaymentDate
	,@PaymentStore,@OSPri,@TotalCurros,@LateFee,@LiquidationFee,@LateDate
	,@InterestrateScheme,@InstallmentPeriod,@InstallmentNo,@BillAmountOfCurrentMonth
	,@ProductName,@ProductBrand,@CashPrice,@DepositAmount,@FinancePrice,@FirstDueDate,@AgentCode
	,@Gender,@Age,@AgreementDate,@MobilePhone,@HomePhone,@CompanyPhone,@TotalPayableAmount
	,@LastPaymentAmount,@TotalPaidAmount,@FirstPaymentAmount,@FinalDueDate,@FinalPaymentAmount,@ReferenceName
	,@RefPhone,@Relative,@IdCardNumber,@Bod,@PermanentAddress,@CompanyName,@Department,@WorkAddress
	,GETDATE(),@CreatedBy, GETDATE(),@AssigneeIds, 0, @AssigneeId, @DistrictId, @ProvinceId)
end
END


-----------------x





GO
CREATE TABLE [dbo].[ProfileStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](100) NULL,
	[Name] [nvarchar](500) NULL,
	[Value] [int] NULL,
	[IsDeleted] [bit] NULL,
	[OrgId] [int] NULL,
	[ProfileType] [varchar](100) NULL
)


  insert into ProfileStatus (Code, Name, Value, ProfileType,IsDeleted, OrgId)
  Values ('BRP',N'Xe đã được thu hồi',1,'revoke_Field',0,2),
('DIE',N'Khách hàng chết, gia đình gặp khó khăn kinh tế',2,'revoke_Field',0,2),
('F_CGI',N'Khách hàng đi tù/nghĩa vụ/cai nghiện',3,'revoke_Field',0,2),
('F_HOS',N'Nhà đã bán',4,'revoke_Field',0,2),
('F_NAH',N'Không gặp được khách hàng, đã để lại thư báo. KH bỏ nhà đi, thỉnh thoảng mới về',5,'revoke_Field',0,2),
('F_NFH',N'Không tìm thấy nhà tại địa chỉ cung cấp. Nhà đã bị giải tỏa',6,'revoke_Field',0,2),
('F_NIW',N'Công ty không Hợp tác, Ko xác định được rõ KH còn làm hay không',7,'revoke_Field',0,2),
('F_NLA',N'Khách hàng chưa từng sống tại địa chỉ được cung cấp',8,'revoke_Field',0,2),
('F_OBT',N'Đã thu được tiền',9,'revoke_Field',0,2),
('F_RENT',N'Khách hàng thuê nhà trọ ở nhưng đã dọn đi',6,'revoke_Field',0,2),
('F_SOB',N'Xe đã bán',7,'revoke_Field',0,2),
('F_WAU',N'Khách hàng bỏ trốn, gia đình còn ở tại địa phương',8,'revoke_Field',0,2),
('F_WET',N'Khách hàng bỏ trốn, Không gặp gia đình',9,'revoke_Field',0,2),
('GSF',N'Nghi ngờ gian lận',10,'revoke_Field',0,2),
('IGN1',N'Khách hàng chưa nhận khoản vay',11,'revoke_Field',0,2),
('IGN2',N'Khách hàng báo đã hủy Hợp đồng',12,'revoke_Field',0,2),
('LEM',N'Để lại lời nhắn cho người thân',13,'revoke_Field',0,2),
('MCW',N'Khách hàng bị bệnh/ tai nạn',14,'revoke_Field',0,2),
('PTP',N'Hứa thanh toán',15,'revoke_Field',0,2),
('RTP',N'Từ chối thanh toán',16,'revoke_Field',0,2),
('WFP',N'Khách hàng đã thanh toán đang chờ phòng Thanh toán kiểm tra',17,'revoke_Field',0,2)

----------x

  insert into ProfileStatus (Code, Name, Value, ProfileType,IsDeleted, OrgId)
  Values ('CAB',N'call back – gọi lại ',18,'revoke_Call',0,2),
('CGI',N'Khách hàng đi tù/nghĩa vụ/cai nghiện',19,'revoke_Call',0,2),
('DIE',N'Khách hàng chết, gia đình gặp khó khăn kinh tế',20,'revoke_Call',0,2),
('GSF',N'Nghi ngờ gian lận',21,'revoke_Call',0,2),
('HUP',N'Cúp máy ngang',22,'revoke_Call',0,2),
('IGN1',N'Khách hàng chưa nhận khoản vay',23,'revoke_Call',0,2),
('IGN2',N'Khách hàng báo đã hủy Hợp đồng',24,'revoke_Call',0,2),
('LEM',N'Để lại lời nhắn cho người thân',25,'revoke_Call',0,2),
('MCW',N'Khách hàng bị bệnh/ tai nạn',26,'revoke_Call',0,2),
('NAB',N'phone number in not available - số không liên lạc được, số không tồn tại, số không đúng',27,'revoke_Call',0,2),
('NCP',N'not customer or other`s phone number - không phải số KH/ số người thân khác',28,'revoke_Call',0,2),
('NKP',N'has signal but not answer - không nhấc máy, máy bận, gọi vào hộp thư thoại',29,'revoke_Call',0,2),
('PTP',N'promise to pay – hứa thanh toán',30,'revoke_Call',0,2),
('RTP',N'refuse to pay – từ chối thanh toán',31,'revoke_Call',0,2),
('WFP',N'Khách hàng đã thanh toán đang chờ phòng Thanh toán kiểm tra',32,'revoke_Call',0,2)


---------------x

alter function [dbo].[fn_GetUserIDCanViewMyProfile_v2]
(@userIds varchar(50))
returns @tempTable TABLE (userId int)
as 
BEGIN
declare @parentCode varchar(20);
declare @tempGroupIds table(ids int PRIMARY key)
insert @tempGroupIds select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien in (select distinct value as UserId from dbo.fn_SplitStringToTable(@userIds, '.'))
select @parentCode = g.Chuoi_Ma_Cha
from NHOM g where g.id in  (select * from @tempGroupIds)
--(select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
insert into @tempTable
select Ma_Nguoi_QL as UserId from NHOM g where g.Id 
in ( select * from dbo.fn_SplitStringToTable(@parentCode, '.'))
union 
select Ma_Nguoi_QL as UserId  from NHOM g where g.id in (select * from @tempGroupIds)
--(select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
union select Ma_nhan_vien as UserId from NHAN_VIEN_CF where Ma_Nhom  in (select * from @tempGroupIds)
--(select Ma_nhom  as UserId from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
union
select distinct value as UserId from dbo.fn_SplitStringToTable(@userIds, '.')
union select Id  as UserId from Nhan_Vien where RoleId =1
delete @tempGroupIds
return
END

--------------x

create table SystemConfig
(Id int identity(1,1) not null,
Code varchar(50),
Name nvarchar(100),
Value int
)

--------------x

insert into SystemConfig(Code, Name, Value)
values('revoke_debt_max_row_import',N'Import thu hồi nợ', 1000)

--------------x

go
create procedure sp_SystemConfig_GetByCode(@code varchar(50))
as begin
select top 1 * from SystemConfig where Code = @code
end

------------x
go
ALTER procedure [dbo].[sp_GetEmployees]
(
@workFromDate datetime,
@workToDate datetime,
@freeText nvarchar(30),
@roleId int,
@page int,
@limit int,
@OrgId int =0
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @offset as int;
set @offset = (@page-1)*@limit;
Select count(*) over() as TotalRecord, n.ID,n.Ten_Dang_Nhap as UserName,n.Ho_Ten as FullName,n.RoleId,r.Name as RoleName,
n.Email, n.Dien_Thoai as Phone,n.CreatedTime,
n.Ma as Code,
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from Nhan_Vien n left join KHU_VUC k on n.DistrictId = k.ID
 left join KHU_VUC k2 on k.Ma_Cha = k2.ID
 left join Role r on n.RoleId = r.Id
where ( convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.Xoa = 0
		and isnull(n.OrgId,0) = @OrgId
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end



------------x



--------------x


alter table [Role]
add OrgId int
--------
insert into [Role] (Code,Name,Deleted, OrgId)
values
('admin',N'Admin',0, 2),
('sup',N'Sup',0, 2),
('call',N'Call',0, 2),
('field',N'Field',0, 2)

------------x

  alter table NHOM
  add 
  CreatedBy int,
CreatedTime datetime,
UpdatedBy int,
UpdatedTime datetime

  ----------x


  go
  ALTER PROCEDURE [dbo].[sp_NHOM_LayDSChonTheoNhanVien]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
declare @orgId int =0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where id = @UserID;
	Select g.ID, g.Ten, g.Chuoi_Ma_Cha as ChuoiMaCha, g.Ma_Nhom_Cha as MaNhomCha, g.Ten_Viet_Tat as TenQL 
	From NHOM  g
	Where isnull(g.OrgId,0) = @orgId and
	g.ID in (Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @UserID)
END

------------x

go
create PROCEDURE [dbo].[sp_NHOM_LayCayNhomCon_v2] 
	-- Add the parameters for the stored procedure here
	@parentGroupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select g.ID, g.Ten, g.Chuoi_Ma_Cha as ChuoiMaCha, g.Ma_Nguoi_QL as MaNguoiQL, g.Ten_Viet_Tat as TenQL From NHOM g
	Where 
	((g.Chuoi_Ma_Cha + '.' + Convert(nvarchar, g.ID)) like '%.' + Convert(nvarchar, @parentGroupId) + '.%')
	 or ((g.Chuoi_Ma_Cha + '.' + Convert(nvarchar, g.ID)) like '%.' + Convert(nvarchar, @parentGroupId))
END



--------------xx
go
ALTER PROCEDURE [dbo].[sp_NHOM_Them]
	-- Add the parameters for the stored procedure here
	@ID int output,
	@MaNhomCha int,
	@MaNguoiQL int,
	@TenVietTat nvarchar(50),
	@Ten nvarchar(200),
	@ChuoiMaCha nvarchar(100),
	@createdBy int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @orgId int =0;
select @orgId = isnull(OrgId,0) from Nhan_Vien where id = @createdBy;
	Insert Into NHOM (Ma_Nhom_Cha, Ma_Nguoi_QL, Ten_Viet_Tat, Ten, Chuoi_Ma_Cha, OrgId, CreatedBy,CreatedTime)
	 Values (@MaNhomCha, @MaNguoiQL, @TenVietTat, @Ten, @ChuoiMaCha, @orgId , @createdBy, GETDATE())
	Set @ID = @@IDENTITY
END


-----------
---count(*) over() as TotalRecord
ALTER procedure [dbo].[sp_MCredit_TempProfile_Gets]
(
@freeText nvarchar(30),
@userId int,
@status varchar(20),
@page int =1,
@limit_tmp int = 10
)
as
begin
declare @where  nvarchar(500) = 'where  isnull(mc.IsDeleted,0) = 0';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);

if @freeText = '' begin set @freeText = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select count(*) over() as TotalRecord, mc.Id,mc.McId, mc.CustomerName,isnull(mc.IdNumber,mc.CCCDNumber) as IDNumber
,mc.Status, mc.Phone, mc.CreatedBy, mc.UpdatedBy,mc.LoanMoney,mc.CreatedTime, mc.UpdatedTime,
isnull(fintechcom_vn_PortalNew.fn_getGhichuByHosoId(mc.Id,4),'''') as LastNote,
mc.SaleNumber +'' '' + isnull(mc.SaleName,'''') as SaleName,
nv1.Ho_Ten as CreatedUser, mcl.Name as LoanPeriodName, p.Name  as ProductName
from MCredit_TempProfile mc left join Employee nv1 on mc.CreatedBy = nv1.ID
left join MCreditProduct p on mc.ProductCode = p.Code
left join MCreditLoanPeriod mcl on mc.LoanPeriodCode = mcl.Code
'
if(@freeText  is not null)
begin
 set @where += 'and (mc.CustomerName like  N''%' + @freeText +'%''';
 set @where += ' or mc.IdNumber like  N''%' + @freeText +'%''';
 set @where += ' or mc.Phone like  N''%' + @freeText +'%'')';
 --set @where = @where + ' or nv2.CCCDNumber like  N''%' + @freeText +'%'' )';
end;

--if(@courierId > 0)
--begin
--if(@where <> '')
--set @where = @where + ' and';
--set @where = @where + ' mc.Id in (
--select CourierId from CourierAssignee 
--where AssigneeId = @CourierId)'; 
--end;
if(@status <> '')
begin
set @where += ' and mc.Status in ('+ @status +')'; 
end
set @where +=  ' and @userId in (select * from fn_GetUserIDCanViewMyProfile (mc.CreatedBy)) '


set @where +=' order by mc.UpdatedTime '; 
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @userId int';
EXECUTE sp_executesql @mainClause,@params,  @offset = @offset, @limit = @limit_tmp, @userId = @userId
print @mainClause;
end
  

  -----------------x

  go 
  create procedure sp_RevokeDebt_GetById
(@profileId int, @userId int)
as begin
select rv.*, s.Name as StatusName from RevokeDebt rv left join ProfileStatus s
on rv.Status = s.Value
 where rv.id = @profileId
end

--------------------x
go
ALTER procedure [dbo].[sp_ProfileStatus_Gets]
(@orgId int = 0,
@profileType varchar(50),
@roleId int =0
)
as begin
declare @isGetAll bit = 0;
if(@roleId = 1 or @roleId = 5)
set @isGetAll = 1;
if(@isGetAll = 1)
begin
select Value as Id, Code, (Code +' - ' + Name) as Name, 0 as IsSelect  from ProfileStatus 
where  isnull(OrgId,0) = @orgId and isnull(IsDeleted,0) = 0
end
else
begin
select @profileType = ProfileType from ProfileStatus where ProfileType = (Select Code from [Role] where Id = @roleId)
select Value as Id, Code, (Code +' - ' + Name) as Name , 0 as IsSelect from ProfileStatus where  ProfileType = @profileType 
and isnull(OrgId,0) = @orgId and isnull(IsDeleted,0) = 0
end
end


----------x

go

  create procedure sp_Role_GetRoles(@userId int)
  as
  begin
  declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
  select Id, Name from Role where isnull(Deleted,0) = 0 and isnull(OrgId,0) = @orgId
  end

  ----------------
  go
  ALTER procedure [dbo].[sp_GetEmployees]
(
@workFromDate datetime,
@workToDate datetime,
@freeText nvarchar(30),
@roleId int,
@page int,
@limit int,
@OrgId int =0
)
as
begin
if @freeText = '' begin set @freeText = null end;
declare @offset as int;
set @offset = (@page-1)*@limit;
Select count(*) over() as TotalRecord, n.ID,n.Ten_Dang_Nhap as UserName,n.Ho_Ten as FullName,n.RoleId,r.Name as RoleName,
n.Email, n.Dien_Thoai as Phone,n.CreatedTime,
n.Ma as Code,
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from Nhan_Vien n left join KHU_VUC k on n.DistrictId = k.ID
 left join KHU_VUC k2 on k.Ma_Cha = k2.ID
 left join Role r on n.RoleId = r.Id
where 
--( convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
--and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null) and 
(@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.Xoa = 0
		and isnull(n.OrgId,0) = @OrgId
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end


---------x

ALTER TABLE Nhan_Vien
ALTER COLUMN Ten_Dang_nhap varchar(50);

-----------x


  go
  alter procedure sp_Employee_GetByUsername(@userId int, @username varchar(40))
  as
  begin
  declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
  select * from Nhan_Vien where isnull(Xoa,0) = 0 and isnull(OrgId,0) = @orgId and Ten_Dang_Nhap = @username
  end

  ----------------x

  go
   ALTER procedure [dbo].[sp_Employee_GetByCode](@code varchar(20), @userId int = 0)
as
begin
  declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
select top 1 Id, Ma as Code from Nhan_Vien where Ma = @code and isnull(Xoa,0) = 0 and isnull(OrgId,0) = @orgId
end
  

  ------------------x


  go
  ALTER PROCEDURE [dbo].[sp_NHOM_LayDSNhom](@userId int =0)
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha From NHOM where isnull(OrgId,0) = @orgId
END
----------x


go
create PROCEDURE [dbo].[sp_Employee_GetPaging] 
(@orgId int =0, 
@freetext nvarchar(50) ='',
 @page int = 1,
  @limit int= 20)
AS
BEGIN
declare @offset int = 0;
set @offset = (@page-1)*@limit;
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freetext = '' begin set @freetext = null end;
set @mainClause = 'Select count(*) over() as TotalRecord, Id, Ma + '' - '' + Ho_Ten as Name From Nhan_Vien where isnull(OrgId,0) = @orgId and isnull(Xoa,0) = 0 ';
if(@freetext  is not null)
	begin
	set @where +=' and (Ma like  N''%' + @freetext +'%''';
	set @where +=' or Ho_Ten like  N''%' + @freetext +'%'' )';
end;
set @where += ' order by createdTime desc offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @orgId int';
EXECUTE sp_executesql @mainClause, @params, @offset = @offset, @limit = @limit,@orgId = @orgId;
print @mainClause;
END

------------x


go
ALTER function [dbo].[fn_GetUserIDCanViewMyProfile_v2]
(@userIds varchar(100), @assigneeIds varchar(50)='0')
returns @tempTable TABLE (userId int)
as 
BEGIN
if(@assigneeIds <>'' and @assigneeIds is not null)
	set @userIds += '.' + @assigneeIds
declare @parentCode varchar(20);
declare @tempGroupIds table(ids int PRIMARY key)
declare @tempUserIds table(ids int PRIMARY key)
insert @tempUserIds select distinct value from dbo.fn_SplitStringToTable(@userIds, '.');
insert @tempGroupIds select Ma_Nhom from NHAN_VIEN_NHOM 
where Ma_Nhan_Vien in (select * from @tempUserIds)
select @parentCode = g.Chuoi_Ma_Cha
from NHOM g where g.id in  (select * from @tempGroupIds)
--(select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
insert into @tempTable
select Ma_Nguoi_QL as UserId from NHOM g where g.Id 
in ( select * from dbo.fn_SplitStringToTable(@parentCode, '.'))
union 
select Ma_Nguoi_QL as UserId  from NHOM g where g.id in (select * from @tempGroupIds)
--(select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
union select Ma_nhan_vien as UserId from NHAN_VIEN_CF where Ma_Nhom  in (select * from @tempGroupIds)
--(select Ma_nhom  as UserId from NHAN_VIEN_NHOM where Ma_Nhan_Vien = @userId)
union
select distinct ids as UserId from @tempUserIds
union select Id  as UserId from Nhan_Vien where RoleId =1 
union select value as UserId  from dbo.fn_SplitStringToTable(@assigneeIds, '.')
delete @tempGroupIds
return
END

---------------x


go
ALTER procedure [dbo].[sp_RevokeDebt_Search]
(
@freeText nvarchar(30),
@status varchar(20) ='',
@page int =1,
@limit_tmp int = 10,
@groupId int =0,
@AssigneeId int =0,
@userId int)as
begin
declare @where  nvarchar(1000) = ' where isnull(rv.IsDeleted,0) = 0';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freeText = '' begin set @freeText = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select count(*) over() as TotalRecord, rv.* 
,fintechcom_vn_PortalNew.fn_getGhichuByHosoId(rv.Id,5) as LastNote,
isnull(s.Name,'''') as StatusName,nv3.Ho_Ten as AssigneeName,
nv1.Ho_Ten as CreatedUser, nv2.Ho_Ten as UpdatedUser
from RevokeDebt rv left join Nhan_Vien nv1 on rv.CreatedBy = nv1.ID
left join Nhan_Vien nv2 on rv.UpdatedBy = nv2.Id
left join Nhan_Vien nv3 on rv.AssigneeId = nv3.Id
left join ProfileStatus s on rv.Status = s.Value'
	if(@freeText  is not null)
	begin
	set @where += ' and (rv.CustomerName like  N''%' + @freeText +'%''';
	set @where += ' or rv.IdCardNumber like  N''%' + @freeText +'%''';
	set @where += ' or rv.MobilePhone like  N''%' + @freeText +'%'')';
	end;
set @where += ' and (@userId in (select * from fn_GetUserIDCanViewMyProfile_v2 (rv.CreatedBy,rv.AssigneeIds)) )'
if(@status <> '')
begin
set @where += ' and rv.Status in ('+ @status +')'; 
end;
--if(@groupId <> 0)
--begin
--set @where +=' and  @groupId in (select distinct value from dbo.fn_SplitStringToTable(rv.AssigneeGroupIds, ''.''))';
--end;
if(@assigneeId <> 0)
begin
set @where += ' and @AssigneeId in (select distinct value from dbo.fn_SplitStringToTable(rv.AssigneeIds, ''.''))';
end;										   
set @where += ' order by rv.createdTime desc  offset @offset ROWS FETCH NEXT @limit ROWS ONLY';
set @mainClause = @mainClause +  @where
set @params =N'@status varchar(20), @offset int, @limit int, @groupId int, @AssigneeId int ,@userId int';
EXECUTE sp_executesql @mainClause,@params,  
@status = @status, @offset = @offset, @limit = @limit_tmp, @groupId = @groupId, @AssigneeId = @AssigneeId, @userId = @userId;
print @mainClause;
end
---------------x


go
create procedure sp_RevokeDebt_Delete(@profileId int, @userId int)
as begin
update RevokeDebt set IsDeleted = 1, UpdatedBy  = @userId, UpdatedTime = GETDATE()
where id = @profileId
end

-----------x
go
create procedure sp_RevokeDebt_UpdateStatus(@profileId int, @userId int,@status int =0 )
as begin
update RevokeDebt set [status] = @status, UpdatedBy  = @userId, UpdatedTime = GETDATE()
where id = @profileId
end


--------------x

update [Role] set Code = 'revoke_Call' where Code ='Call'

update [Role] set Code = 'revoke_Field' where Code ='Field'

----------x

alter table RevokeDebt
add DistrictId int,
ProvinceId int

---------x

alter table RevokeDebt
add AssigneeId int

alter table RevokeDebt
add AssigneeId int

---------x

insert into ImportExcel(Name, Position, ImportType, ValueType)
values
('DistrictId',42,5,'int'),
('ProvinceId',43,5,'int'),
('AssigneeId',44,5,'int')

------------x

go
create procedure sp_RevokeDebt_UpdateSimple(@profileId int, @updateBy int,@provinceId int , @districtId int,@assigneeId int,@status int =0 )
as begin

declare @isHasRight bit =0
if exists (select top 1 * from NHOM where Ma_Nguoi_QL = @updateBy)
begin

	update RevokeDebt 
	set [status] = @status, 
	UpdatedBy  = @updateBy, 
	UpdatedTime = GETDATE()
	where id = @profileId
end
else
begin
	update RevokeDebt 
	set [status] = @status, 
	DistrictId = @districtId, 
	ProvinceId = @provinceId,
	AssigneeId = @assigneeId,
	UpdatedBy  = @updateBy, 
	UpdatedTime = GETDATE()
	where id = @profileId
end
end


-----------x

alter PROCEDURE [dbo].[sp_NHOM_LayDSNhomDuyetChonTheoNhanVien_v2]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
declare @orgId int = 0;
  select @orgId = isnull(OrgId,0) from Nhan_Vien where Id = @userId;
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nhom_Cha as MaNhomCha, NHOM.Ten_Viet_Tat 
	as TenQL From NHOM
	Where isnull(NHOM.OrgId,0) = @orgId and NHOM.ID in 
	(Select NHAN_VIEN_CF.Ma_Nhom 
	From NHAN_VIEN_CF 
	--Where NHAN_VIEN_CF.Ma_Nhan_Vien = @UserID
	)
END


----------x


 insert into LOAI_TAI_LIEU (Ten, Bat_Buoc, ProfileTypeId)
 values 
 (N'Biên lai Thu tiền',0,5),
  (N'Hình ảnh Nhà KH',0,5),
   (N'Ảnh KH/ Người thân',0,5),
    (N'Ảnh khác',0,5)

----------x

select * from ImportExcel where ImportType = 5
order by Position
insert into ImportExcel (Name,Position,ImportType,ValueType)
values ('Deleted', 45, 5,'int')

--------x

alter table RevokeDebt 
add PartnerName nvarchar(200)

insert into ImportExcel (Name,Position,ImportType,ValueType)
values ('PartnerName', 46, 5,'string')

go
alter PROCEDURE sp_insert_RevokeDebt
@AgreementNo varchar(50),
@CustomerName nvarchar(200),
@LastestPaymentDate varchar(20),
@PaymentStore nvarchar(50),
@OSPri varchar(20),
@TotalCurros varchar(20),
@LateFee varchar(20),
@LiquidationFee varchar(20),
@LateDate varchar(10),
@InterestrateScheme varchar(20),
@InstallmentPeriod varchar(10),
@InstallmentNo varchar(10),
@BillAmountOfCurrentMonth varchar(20),
@ProductName nvarchar(100),
@ProductBrand nvarchar(100),
@CashPrice varchar(20),
@DepositAmount varchar(20),
@FinancePrice varchar(20),
@FirstDueDate varchar(20),
@AgentCode varchar(20),
@Gender nvarchar(10),
@Age varchar(5),
@AgreementDate varchar(20),
@MobilePhone varchar(12),
@HomePhone varchar(12),
@CompanyPhone varchar(12),
@TotalPayableAmount varchar(20),
@LastPaymentAmount varchar(20),
@TotalPaidAmount varchar(20),
@FirstPaymentAmount varchar(20),
@FinalDueDate varchar(20),
@FinalPaymentAmount varchar(20),
@ReferenceName nvarchar(200),
@RefPhone varchar(12),
@Relative nvarchar(200),
@IdCardNumber varchar(12),
@Bod varchar(20),
@PermanentAddress nvarchar(300),
@CompanyName nvarchar(300),
@Department nvarchar(200),
@WorkAddress nvarchar(300)  ,
@CreatedBy int,
@AssigneeIds varchar(20),
@AssigneeId int =0,
@DistrictId int =0,
@ProvinceId int = 0,
@Deleted int =0,
@PartnerName nvarchar(200) = null
AS
BEGIN
if(@Deleted > 0)
begin
 update RevokeDebt set IsDeleted = 1 where AgreementNo = @AgreementNo;
end
else
begin
	Insert into RevokeDebt (AgreementNo,CustomerName,LastestPaymentDate
	,PaymentStore,OSPri,TotalCurros,LateFee,LiquidationFee,LateDate
	,InterestrateScheme,InstallmentPeriod,InstallmentNo,BillAmountOfCurrentMonth
	,ProductName,ProductBrand,CashPrice,DepositAmount,FinancePrice,FirstDueDate,AgentCode
	,Gender,Age,AgreementDate,MobilePhone,HomePhone,CompanyPhone,TotalPayableAmount
	,LastPaymentAmount,TotalPaidAmount,FirstPaymentAmount,FinalDueDate,FinalPaymentAmount,ReferenceName
	,RefPhone,[Relative],IdCardNumber,Bod,PermanentAddress,CompanyName,Department,WorkAddress
	,CreatedTime,CreatedBy,UpdatedTime,AssigneeIds,Status, AssigneeId,DistrictId, ProvinceId,PartnerName)
	values(@AgreementNo,@CustomerName,@LastestPaymentDate
	,@PaymentStore,@OSPri,@TotalCurros,@LateFee,@LiquidationFee,@LateDate
	,@InterestrateScheme,@InstallmentPeriod,@InstallmentNo,@BillAmountOfCurrentMonth
	,@ProductName,@ProductBrand,@CashPrice,@DepositAmount,@FinancePrice,@FirstDueDate,@AgentCode
	,@Gender,@Age,@AgreementDate,@MobilePhone,@HomePhone,@CompanyPhone,@TotalPayableAmount
	,@LastPaymentAmount,@TotalPaidAmount,@FirstPaymentAmount,@FinalDueDate,@FinalPaymentAmount,@ReferenceName
	,@RefPhone,@Relative,@IdCardNumber,@Bod,@PermanentAddress,@CompanyName,@Department,@WorkAddress
	,GETDATE(),@CreatedBy, GETDATE(),@AssigneeIds, 0, @AssigneeId, @DistrictId, @ProvinceId, @PartnerName)
end
END
----------