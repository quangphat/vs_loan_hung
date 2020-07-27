--EXEC sp_rename 'Nhan_Vien', 'Employee'

-- revoke debt

alter table Employee 
add OrgId int

EXEC sp_rename 'Employee.Trang_Thai', 'Status', 'COLUMN'
-----------
go
create PROCEDURE [dbo].[sp_Employee_Login]
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
	Select Id,Ten_Dang_Nhap AS UserName, Mat_Khau as Passowrd, RoleId, Ma as Code,
	Email, Ho_Ten as FullName, Dien_Thoai as Phone, Status as IsActive, isnull(OrgId,1) as OrgId
	 From Employee where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and isnull(Xoa,0) =0
END

--------------
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
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang
	From HO_SO_DUYET_XEM,HO_SO left join Employee as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join Employee as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join Employee as NV3 on HO_SO.Courier_Code = NV3.ID
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
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) 
	or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
END


-----------------------

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
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu 
	From HO_SO_XEM,HO_SO left join Employee as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join Employee as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
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
	and (HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.Da_Xoa = 0 
	--and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.Ngay_Tao desc
END


------------------
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

Select count(*) from Employee n

where (convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))

and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)

	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'

		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'

		or n.Dien_Thoai like N'%' + @freeText + '%'

		or n.Email like N'%' + @freeText + '%')

		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))

		and n.Xoa = 0

end




--------------
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
 from Employee n left join KHU_VUC k on n.DistrictId = k.ID
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






-------------

go
create procedure sp_GetEmployeeById(@userId int)
as
begin
select * from Employee where ID = @userId
end


---------
go
create procedure [dbo].[sp_Employee_UpdateUser_v2]
(
@id int,
@fullName nvarchar(100),
@phone varchar(50),
@email varchar(50),
@roleId int,
@provinceId int,
@districtId int,
@updatedtime datetime,
@updatedby int,
@workDate datetime
)
as
begin
--declare @oldRoleId int = 0;
--select @oldRoleId = RoleId from NHAN_VIEN where ID = @id;
--if(@oldRoleId is not null and @oldRoleId >0 )
--begin

--end
update Employee set 
		Ho_Ten = @fullName,
		Dien_Thoai = @phone,
		Email = @email,
		RoleId = @roleId,
		ProvinceId = @provinceId,
		DistrictId = @districtId,
		UpdatedTime = @updatedtime,
		UpdatedBy = @updatedby,
		WorkDate = @workDate
		where ID = @id
end

----------

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
@provinceId int,
@districtId int,
@createdtime datetime,
@createdby int,
@workDate datetime
)
as
begin
insert into Employee(Ma,Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,RoleId,WorkDate,ProvinceId,DistrictId,Status,Xoa,CreatedTime,CreatedBy)
values (@code,@userName,@password,@fullName,@phone,@email,@roleId,@workDate,@provinceId,@districtId,1,0,@createdtime,@createdby);
SET @id=@@IDENTITY
end


---------
go
create PROCEDURE [dbo].[sp_Employee_GetFull] 
-- Add the parameters for the stored procedure here

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
Select ID, Ma + ' - ' + Ho_Ten as HoTen From Employee
END

-----------
go
create PROCEDURE [dbo].[sp_Employee_LayDSByMaQL_v2]
	-- Add the parameters for the stored procedure here
	@MaQL int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma as Code, Ma + ' - ' + Ho_Ten as HoTen from Employee where ID=@MaQL
END


--------
go
create PROCEDURE [dbo].[sp_Group_GetChildGroup] 
	-- Add the parameters for the stored procedure here
	@MaNhomCha int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, 
	NHOM.Ten_Viet_Tat as TenNgan, 
	e.Ho_Ten as NguoiQuanLy, 
	NHOM.Chuoi_Ma_Cha as ChuoiMaCha 
	From NHOM, Employee e
	Where NHOM.Ma_Nguoi_QL = e.ID 
	and NHOM.Ma_Nhom_Cha = @MaNhomCha
END

-------
go
create PROCEDURE [dbo].[sp_Group_GetById] 
	-- Add the parameters for the stored procedure here
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Ten_Viet_Tat as TenNgan, NHOMCHA.Ten as TenNhomCha, e.Ho_Ten as NguoiQuanLy
	From Employee e, NHOM
	left join NHOM as NHOMCHA on NHOM.Ma_Nhom_Cha = NHOMCHA.ID
	Where NHOM.ID = @groupId and e.ID = NHOM.Ma_Nguoi_QL
END

----------
go
create PROCEDURE [dbo].[sp_Employee_Group_GetEmployeeByGroup]
-- Add the parameters for the stored procedure here
@groupId int
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
Select e.ID, e.Ma, e.Ho_Ten as HoTen, e.Email, e.Dien_Thoai as SDT 
From Employee e, NHAN_VIEN_NHOM 
Where e.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @groupId
END


--------
go
create PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v2]
	-- Add the parameters for the stored procedure here
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select e.ID, e.Ma + ' - ' + e.Ho_Ten as Ten, e.Ma as Code 
	From Employee e
	Where e.ID in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM 
		Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM 
				Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
				like '%.' + Convert(nvarchar, @groupId) + '.%') 
					or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
						like '%.' + Convert(nvarchar, @groupId)) or NHOM.ID = @groupId)
	)
END

------
go
create PROCEDURE [dbo].[sp_employee_Group_LayDSKhongThanhVienNhom_v2]
	-- Add the parameters for the stored procedure here
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select e.ID, e.Ma + ' - ' + e.Ho_Ten as Ten 
	From Employee e 
	Where e.ID not in (Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = @groupId)
END

--------
go
create PROCEDURE [dbo].[sp_Employee_Group_LayDSChonThanhVienNhom_v2] 
	-- Add the parameters for the stored procedure here
	@groupId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select e.ID, e.Ma + ' - ' + e.Ho_Ten as Ten 
	From Employee e, NHAN_VIEN_NHOM Where e.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @groupId
END



----------
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
	from Employee e,NHAN_VIEN_CF
	where --ID=@UserID
	 e.ID=NHAN_VIEN_CF.Ma_Nhan_Vien
	and NHAN_VIEN_CF.Quyen=@Rule
	and NHAN_VIEN_CF.Ma_Nhom in (Select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien=@UserId)

END

-------

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
IsDeleted bit
)


alter table RevokeDebt
add FinalPaymentAmount varchar(20),
UpdatedTime datetime,
UpdatedBy int,
CreatedTime datetime,
CreatedBy int

alter table RevokeDebt
add AssigneeGroupIds varchar(50),
AssigneeIds varchar(50)
 
----------------


CREATE TABLE [dbo].[ImportExcel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Position] [int] NULL,
	[ImportType] [int] NULL,
	[ValueType] [varchar](20) NULL
)
-----------

insert into ImportExcel(Name,Position,ImportType,ValueType)
select COLUMN_NAME,ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS rownum, 5, 'string'
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME='RevokeDebt'

-------------

update ImportExcel set Position = Position - 2 where ImportType = 5
-------------------
go
create procedure sp_ImportExcel_GetByType
(@type int)
as begin
select * from ImportExcel where ImportType = @type 
order by Position
end


------------
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

----------------
go
create PROCEDURE sp_insert_RevokeDebt
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
@AssigneeIds varchar(20)
AS
BEGIN
	Insert into RevokeDebt (AgreementNo,CustomerName,LastestPaymentDate
	,PaymentStore,OSPri,TotalCurros,LateFee,LiquidationFee,LateDate
	,InterestrateScheme,InstallmentPeriod,InstallmentNo,BillAmountOfCurrentMonth
	,ProductName,ProductBrand,CashPrice,DepositAmount,FinancePrice,FirstDueDate,AgentCode
	,Gender,Age,AgreementDate,MobilePhone,HomePhone,CompanyPhone,TotalPayableAmount
	,LastPaymentAmount,TotalPaidAmount,FirstPaymentAmount,FinalDueDate,FinalPaymentAmount,ReferenceName
	,RefPhone,[Relative],IdCardNumber,Bod,PermanentAddress,CompanyName,Department,WorkAddress
	,CreatedTime,CreatedBy,UpdatedTime,AssigneeGroupIds)
	values(@AgreementNo,@CustomerName,@LastestPaymentDate
	,@PaymentStore,@OSPri,@TotalCurros,@LateFee,@LiquidationFee,@LateDate
	,@InterestrateScheme,@InstallmentPeriod,@InstallmentNo,@BillAmountOfCurrentMonth
	,@ProductName,@ProductBrand,@CashPrice,@DepositAmount,@FinancePrice,@FirstDueDate,@AgentCode
	,@Gender,@Age,@AgreementDate,@MobilePhone,@HomePhone,@CompanyPhone,@TotalPayableAmount
	,@LastPaymentAmount,@TotalPaidAmount,@FirstPaymentAmount,@FinalDueDate,@FinalPaymentAmount,@ReferenceName
	,@RefPhone,@Relative,@IdCardNumber,@Bod,@PermanentAddress,@CompanyName,@Department,@WorkAddress
	,GETDATE(),@CreatedBy, GETDATE(),@AssigneeIds)
END


-----------------


 create procedure [dbo].[sp_RevokeDebt_Search]
(
@freeText nvarchar(30),
@status varchar(20) ='',
@page int =1,
@limit_tmp int = 10,
@groupId int =0,
@AssigneeId int =0,
@userId int)as
begin
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freeText = '' begin set @freeText = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select count(*) over() as TotalRecord, rv.* 
,fintechcom_vn_PortalNew.fn_getGhichuByHosoId(rv.Id,2) as LastNote,
 nv1.Ho_Ten as CreatedUser, nv2.Ho_Ten as UpdatedUser
from RevokeDebt rv left join Employee nv1 on rv.CreatedBy = nv1.ID
left join Employee nv2 on rv.UpdatedBy = nv2.Id
'
	if(@freeText  is not null)
	begin
	set @where = ' (rv.CustomerName like  N''%' + @freeText +'%''';
	set @where = @where + ' or rv.IdCardNumber like  N''%' + @freeText +'%''';
	set @where = @where + ' or rv.MobilePhone like  N''%' + @freeText +'%''';
	end;
   if(@where <> '')
     set @where = @where + ' and';
	   set @where = @where + ' (@userId in (select * from fn_GetUserIDCanViewMyProfile_v2 (rv.CreatedBy)) )'
if(@status <> '')
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' rv.Status in ('+ @status +')'; 
end;
if(@groupId <> 0)
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' rv.GroupId = @groupId';
end;
if(@assigneeId <> 0)
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' @AssigneeId in (select distinct value from dbo.fn_SplitStringToTable(rv.AssigneeIds, ''.''))';
end;										   
if(@where <>'')
begin
set @where= ' where ' + @where
end
set @where = @where + ' and isnull(IsDeleted,0) = 0  order by rv.createdTime desc';
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N'@status varchar(20), @offset int, @limit int, @groupId int, @AssigneeId int ,@userId int';
EXECUTE sp_executesql @mainClause,@params,  
@status = @status, @offset = @offset, @limit = @limit_tmp, @groupId = @groupId, @AssigneeId = @AssigneeId, @userId = @userId;
print @mainClause;
end

-----------
  insert into ProfileStatus (Code, Name, Value, ProfileType,IsDeleted, OrgId)
  Values ('BRP',N'Xe đã được thu hồi',1,'revoke_Field',0,2),
    ('DIE',N'Khách hàng chết, gia đình gặp khó khăn kinh tế',2,'revoke_Field',0,2),
	  ('F_CGI',N'Khách hàng đi tù/nghĩa vụ/cai nghiện',3,'revoke_Field',0,2),
	    ('F_HOS',N'Nhà đã bán',4,'revoke_Field',0,2),
		  ('F_NAH',N'Không gặp được khách hàng, đã để lại thư báo. KH bỏ nhà đi, thỉnh thoảng mới về',5,'revoke_Field',0,2)


---------------

create function [dbo].[fn_GetUserIDCanViewMyProfile_v2]
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
union select Id  as UserId from Employee where RoleId =1
delete @tempGroupIds
return
END

--------------

create table SystemConfig
(Id int identity(1,1) not null,
Code varchar(50),
Name nvarchar(100),
Value int
)

--------------

insert into SystemConfig(Code, Name, Value)
values('revoke_debt_max_row_import',N'Import thu hồi nợ', 1000)

--------------

go
create procedure sp_SystemConfig_GetByCode(@code varchar(50))
as begin
select top 1 * from SystemConfig where Code = @code
end

------------
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
 from Employee n left join KHU_VUC k on n.DistrictId = k.ID
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



------------