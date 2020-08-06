

ALTER TABLE MCreditProduct
ALTER COLUMN IsCheckCat char(1)

------------------

go           
alter function fn_MCProduct_ISCheckCat(@productCode varchar(10))
returns int
as begin
declare @isCheckCat bit = 0;

select @isCheckCat  = ISCheckCat from MCreditProduct where Code = @productCode;
if(@isCheckCat = '1')
return 1
return 0
end

----------

alter table MCredit_TempProfile
add CatNumber varchar(20),
CatName varchar(50),
ComName nvarchar(300)

--------------------

ALTER PROCEDURE [dbo].[sp_insert_MCredit_TempProfile]
@id INT = 0 OUTPUT,
@CustomerName nvarchar(300),
@Hometown nvarchar(200),
@BirthDay datetime,
@Phone varchar(12),
@IdNumber varchar(10),
@CCCDNumber varchar(12),
@IssueDate datetime,
@IsAddr bit,
@Gender bit,
@ProvinceId int,
@DistrictId int,
@Address nvarchar(500),
@SaleNumber varchar(20),
@ProductCode varchar(10),
@LoanPeriodCode varchar(10),
@LoanMoney decimal,
@IsInsurrance bit,
@Status int,
@CreatedBy int,
@UpdatedBy int,
@LocSignCode varchar(10),
@IsDeleted bit =0,
@LastNote nvarchar(1000),
@SaleId int = 0,
@SaleName nvarchar(200) = null,
@CatNumber varchar(20) = null,
@ComName nvarchar(300) = null,
@CatName nvarchar(50) = null
AS
BEGIN
	Insert into MCredit_TempProfile 
	(CustomerName,Hometown,
	BirthDay,Phone,IdNumber,
	CCCDNumber,IssueDate,
	IsAddr,Gender,
	ProvinceId,DistrictId,
	Address,SaleNumber,
	ProductCode,LoanPeriodCode,
	LoanMoney,IsInsurrance,
	[Status],CreatedBy,
	CreatedTime,UpdatedBy,
	UpdatedTime,IsDeleted, LocSignCode,LastNote,SaleId,SaleName,CatNumber,ComName,CatName)
	values(@CustomerName,@Hometown,
	@BirthDay,@Phone,
	@IdNumber,@CCCDNumber,
	@IssueDate,@IsAddr,
	@Gender,@ProvinceId,
	@DistrictId,@Address,
	@SaleNumber,@ProductCode,
	@LoanPeriodCode,@LoanMoney,
	@IsInsurrance,@Status,
	@CreatedBy,GETDATE(),@UpdatedBy,GETDATE()
	,@IsDeleted,@LocSignCode,@LastNote,@SaleId, @SaleName,@CatNumber,@ComName,@CatName)
	SET @id=@@IDENTITY;
END

-----------------

go
 ALTER PROCEDURE [dbo].[sp_update_MCredit_TempProfile]
(
@Id int,
@CustomerName nvarchar(300),
@Hometown nvarchar(200),
@BirthDay datetime,
@Phone varchar(12),
@IdNumber varchar(10),
@CCCDNumber varchar(12),
@IssueDate datetime,
@IsAddr bit,
@Gender bit,
@ProvinceId int,
@DistrictId int,
@Address nvarchar(500),
@SaleNumber varchar(20),
@ProductCode varchar(10),
@LoanPeriodCode varchar(10),
@LocSignCode varchar(10),
@LoanMoney decimal,
@IsInsurrance bit,
@Status int,
@UpdatedBy int,
@IsDeleted bit,
@LastNote nvarchar(1000),
@SaleId int = 0,@SaleName nvarchar(200) = null, @MCId varchar(20) = null,
@CatNumber varchar(20) = null,
@ComName nvarchar(300) = null,
@CatName nvarchar(50) = null)
AS
BEGIN
	UPDATE [dbo].MCredit_TempProfile 
	SET CustomerName=@CustomerName
	,Hometown=@Hometown,BirthDay=@BirthDay
	,Phone=@Phone,IdNumber=@IdNumber
	,CCCDNumber=@CCCDNumber,IssueDate=@IssueDate
	,IsAddr=@IsAddr,Gender=@Gender
	,ProvinceId=@ProvinceId,DistrictId=@DistrictId
	,Address=@Address,SaleNumber=@SaleNumber
	,ProductCode=@ProductCode,LoanPeriodCode=@LoanPeriodCode
	,LoanMoney=@LoanMoney,IsInsurrance=@IsInsurrance
	,[Status]=@Status
	,UpdatedBy=@UpdatedBy
	,UpdatedTime=GETDATE()
	,IsDeleted=@IsDeleted 
	,LastNote = @LastNote,
	LocSignCode = @LocSignCode,
	SaleId = @SaleId,
	SaleName = @SaleName,
	McId = @MCId,
	CatNumber = @CatNumber,
	ComName = @ComName,
	CatName = @CatName
	WHERE Id=@Id 
end


----------------

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
,s.Name as StatusName, mc.Phone, mc.CreatedBy, mc.UpdatedBy,mc.LoanMoney,mc.CreatedTime, mc.UpdatedTime,
isnull(fintechcom_vn_PortalNew.fn_getGhichuByHosoId(mc.Id,4),'''') as LastNote,
mc.SaleNumber +'' '' + isnull(mc.SaleName,'''') as SaleName,
nv1.Ho_Ten as CreatedUser, mcl.Name as LoanPeriodName, p.Name  as ProductName
from MCredit_TempProfile mc left join NHan_Vien nv1 on mc.CreatedBy = nv1.ID
left join MCreditProduct p on mc.ProductCode = p.Code
left join MCreditLoanPeriod mcl on mc.LoanPeriodCode = mcl.Code
left join MCreditProfileStatus s on s.Code = mc.Status
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


set @where +=' order by mc.UpdatedTime desc'; 
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @userId int';
EXECUTE sp_executesql @mainClause,@params,  @offset = @offset, @limit = @limit_tmp, @userId = @userId
print @mainClause;
end

-------------
  insert into MCreditProfileStatus (Code, Name)
  values 
  ('-1', N'Đã gửi MC'),
    ('-3', N'Nộp hồ sơ'),
	  ('-4', N'Bổ sung hồ sơ')

alter table MCreditProfileStatus
alter column Code int

update MCredit_TempProfile set Status = -1 where Status = 1
 update MCredit_TempProfile set Status = -3 where Status = 3
 update MCredit_TempProfile set Status = -4 where Status = 4

 create procedure sp_MCredit_UpdateStatusFromMC(@profileId int, @status int)
 as begin
update MCredit_TempProfile set Status = @status where Id = @profileId
end


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
from MCredit_TempProfile mc left join NHan_Vien nv1 on mc.CreatedBy = nv1.ID
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


set @where +=' order by mc.UpdatedTime desc'; 
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @userId int';
EXECUTE sp_executesql @mainClause,@params,  @offset = @offset, @limit = @limit_tmp, @userId = @userId
print @mainClause;
end

-------------