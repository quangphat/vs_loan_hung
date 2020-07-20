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