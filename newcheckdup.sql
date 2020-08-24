  alter table customer
  add PartnerStatus int

  -------------
  CREATE TABLE [dbo].[ProfileStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](30) NULL,
	[Name] [nvarchar](500) NULL,
	[Value] [int] NULL,
	[IsDeleted] [bit] NULL,
	[OrgId] [int] NULL,
	[ProfileType] [varchar](30) NULL
) ON [PRIMARY]

insert into ProfileStatus(Name, Value, OrgId,ProfileType) values
(N'Không nợ xấu',0,1,'duplicate'),
(N'Nợ chú ý',1,1,'duplicate'),
(N'Nợ xấu',2,1,'duplicate'),

(N'Chưa check',0,1,'checkdup-partner-status'),
(N'Đủ điều kiện',1,1,'checkdup-partner-status'),
(N'Không đủ điều kiện',2,1,'checkdup-partner-status')

  ----------
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
  @Salary decimal(18,2),
  @IsMatch bit =0,
  @BirthDay datetime
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
	,Salary
	,IsMatch
	,BirthDay)
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
	,@Salary
	,@IsMatch
	,@BirthDay);
	SET @id=@@IDENTITY;

  end
---------------

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
  @CICStatus int,
  @IsMatch bit =0,
  @BirthDay datetime
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
		CICStatus = @CICStatus,
		IsMatch = @IsMatch,
		BirthDay = @BirthDay
		where Id = @id
	  if(@LastNote is not null)
		update Customer set LastNote = @LastNote where Id = @id
  end

  -----------

  
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

-----------

create function fn_getCheckDupNoteById(@profileId as int)
returns nvarchar(200)
as begin
declare @noidung as nvarchar(200)
select top(1) @noidung = isnull(Noidung,'') from Ghichu 
where HosoId = @profileId  and TypeId = 6
order by CommentTime desc
if(@noidung = '')
begin
select top(1) @noidung = isnull(Note,'') from CustomerNote where CustomerId= @profileId
order by CreatedTime desc
end
return @noidung;
end


-----------
alter table Customer 
add IsDeleted bit

---------
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
	 dbo.fn_getCheckDupNoteById(n.Id) as LastNote,
	 dbo.getProvinceName(n.ProvinceId) as ProvinceName, dbo.getPartnerName(n.PartnerId) as PartnerName
	from Customer n left join Doi_tac dt on n.PartnerId = dt.Id ' + @wherequery 
	+ ' order by n.Id desc 
	offset @offset_tmp ROWS FETCH NEXT @limit_tmp ROWS ONLY';
	print @query;
	execute sp_executesql @query, @param,@freetext_tmp = @freeText, @userId_tmp = @userId, @offset_tmp = @offset,@limit_tmp = @limit;
end

end

----------------


create procedure sp_getCheckDupNotesById(@profileId as int)
as begin
select g.Id as Id, g.UserId, g.Noidung,g.HosoId,g.TypeId,g.CommentTime,  Concat(CONCAT(n.Ma, ' - '),n.Ho_Ten) as Commentator 
from Ghichu g left join Nhan_Vien n
on g.UserId = n.ID where g.HosoId= @profileId and TypeId = 6
union 
select c.Id as Id,c.CreatedBy as UserId,c.Note as Noidung, c.CustomerId as HosoId, 6 as TypeId, c.CreatedTime as CommentTime
,  Concat(CONCAT(n.Ma, ' - '),n.Ho_Ten) as Commentator  
 from CustomerNote c left join Nhan_Vien n on c.CreatedBy = n.ID
  where c.CustomerId = @profileId
  order by CommentTime desc
end

---------