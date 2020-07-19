--EXEC sp_rename 'dbo.NHAN_VIEN', 'Employee'
--EXEC sp_rename '[Employee].Ma', 'Code', 'COLUMN'
--EXEC sp_rename '[Employee].Ten_Dang_Nhap', 'UserName', 'COLUMN'
--EXEC sp_rename '[Employee].Mat_Khau', 'Password', 'COLUMN'
--EXEC sp_rename '[Employee].Ho_Ten', 'FullName', 'COLUMN'
--EXEC sp_rename '[Employee].Loai', 'TypeId', 'COLUMN'
--EXEC sp_rename '[Employee].Dien_Thoai', 'Phone', 'COLUMN'
--EXEC sp_rename '[Employee].Trang_Thai', 'Status', 'COLUMN'
--EXEC sp_rename '[Employee].Xoa', 'IsDeleted', 'COLUMN'

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




