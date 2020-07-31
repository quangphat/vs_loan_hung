

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


