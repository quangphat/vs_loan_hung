---------old courier query

USE [fintechcom_vn_Portal]
GO
/****** Object:  StoredProcedure [dbo].[sp_CountHosoCourier]    Script Date: 23/07/2020 9:35:20 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 ALTER procedure [dbo].[sp_CountHosoCourier]
(
@freeText nvarchar(30),
@assigneeId int = 0,
@status varchar(20) ='',
@provinceId int = 0,
@groupId int =0,
@saleCode varchar(20) ='',
@userId int
)
as
begin 
declare @where  nvarchar(1000) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);
if @freeText = '' begin set @freeText = null end;
if @saleCode = '' begin set @saleCode = null end;
set @mainClause = 'select count(*) from HosoCourrier hc left join NHAN_VIEN nv1 on hc.CreatedBy = nv1.ID
left join NHAN_VIEN nv2 on hc.AssignUserId = nv2.ID'
if(@freeText  is not null)
begin
 set @where = ' (hc.CustomerName like  N''%' + @freeText +'%''';
 set @where = @where + ' or hc.cmnd like  N''%' + @freeText +'%''';
 set @where = @where + ' or hc.phone like  N''%' + @freeText +'%''';
 set @where = @where + ' or nv2.Ho_Ten like  N''%' + @freeText +'%'' )';
end;

if(@assigneeId > 0)
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' (hc.Id in (select CourierId from CourierAssignee where AssigneeId = @assigneeId)
	or hc.SaleCode = (select Ma from Nhan_Vien where Id = @assigneeId))'; 
end;
if(@status <> '')
begin
if(@where <> '')
set @where = @where + ' and';
set @where = @where + ' hc.Status in (' + @status +')'; 
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
		if(@where <> '')
			set @where = @where + ' and';
		set @where = @where + ' hc.SaleCode = @saleCode'; 
		end;
if(@where <>'')
begin
set @where= ' where ' + @where + ' and isnull(IsDeleted,0) = 0 '
end

set @mainClause = @mainClause +  @where
set @params =N'@assigneeId  int, @status varchar(20), @groupId int, @provinceId int, @saleCode varchar(20)';
EXECUTE sp_executesql @mainClause,@params, @assigneeId = @assigneeId, @status = @status
,@provinceId = @provinceId,@groupId = @groupId ,@saleCode = @saleCode;
print @mainClause;
print @where
end

-------------------------

USE [fintechcom_vn_Portal]
GO
/****** Object:  StoredProcedure [dbo].[sp_MCredit_TempProfile_Gets]    Script Date: 23/07/2020 9:59:00 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[sp_MCredit_TempProfile_Gets]
(
@freeText nvarchar(30),
@userId int,
@page int =1,
@limit_tmp int = 10
)
as
begin
declare @where  nvarchar(500) = '';
declare @mainClause nvarchar(max);
declare @params nvarchar(300);

if @freeText = '' begin set @freeText = null end;
declare @offset int = 0;
set @offset = (@page-1)*@limit_tmp;
set @mainClause = 'select mc.Id,mc.McId, mc.CustomerName,isnull(mc.IdNumber,mc.CCCDNumber) as IDNumber
,mc.Status, mc.Phone, mc.CreatedBy, mc.UpdatedBy,mc.LoanMoney,mc.CreatedTime, mc.UpdatedTime,
isnull(fintechcom_vn_PortalNew.fn_getGhichuByHosoId(mc.Id,4),'''') as LastNote,
mc.SaleNumber +'' '' + isnull(mc.SaleName,'''') as SaleName,
nv1.Ho_Ten as CreatedUser, mcl.Name as LoanPeriodName, p.Name  as ProductName
from MCredit_TempProfile mc left join NHAN_VIEN nv1 on mc.CreatedBy = nv1.ID
left join MCreditProduct p on mc.ProductCode = p.Code
left join MCreditLoanPeriod mcl on mc.LoanPeriodCode = mcl.Code
'
if(@freeText  is not null)
begin
 set @where = 'where (mc.CustomerName like  N''%' + @freeText +'%''';
 set @where = @where + ' or mc.IdNumber like  N''%' + @freeText +'%''';
 set @where = @where + ' or mc.Phone like  N''%' + @freeText +'%''';
 set @where = @where + ' or nv2.CCCDNumber like  N''%' + @freeText +'%'' )';
end;

--if(@courierId > 0)
--begin
--if(@where <> '')
--set @where = @where + ' and';
--set @where = @where + ' mc.Id in (
--select CourierId from CourierAssignee 
--where AssigneeId = @CourierId)'; 
--end;
--if(@status <> '')
--begin
--if(@where <> '')
--set @where = @where + ' and';
--set @where = @where + ' mc.Status in ('+ @status +')'; 
--end;
if(@where <>'')
begin
set @where= ' where ' + @where + ' and @userId in (select UserId in MCProfilePeople where ProfileId = mc.Id) '
end
else
begin
 set @where =' where  ((@userId in (select UserId from MCProfilePeople where ProfileId = mc.Id)) 
	 or (@userId in     (select Id from Nhan_vien where RoleId = 1)    ) or @userId = mc.CreatedBy)'
end
set @where = @where + ' order by mc.UpdatedTime '; 
set @where += ' offset @offset ROWS FETCH NEXT @limit ROWS ONLY'
set @mainClause = @mainClause +  @where
set @params =N' @offset int, @limit int, @userId int';
EXECUTE sp_executesql @mainClause,@params,  @offset = @offset, @limit = @limit_tmp, @userId = @userId
print @mainClause;
end


----------------------