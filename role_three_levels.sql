-- Normalize account roles to Admin (1), Quản lý (2), Sale (3) for Test13.
USE [Test13];
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- Preserve the existing security sources: NHAN_VIEN_QUYEN grants Admin;
-- a team leader is a user assigned to a team they manage.
UPDATE employee
SET RoleId = CASE
    WHEN EXISTS
    (
        SELECT 1 FROM dbo.NHAN_VIEN_QUYEN AS adminPermission
        WHERE adminPermission.Ma_NV = employee.ID
    ) THEN 1
    WHEN EXISTS
    (
        SELECT 1
        FROM dbo.NHOM AS managedGroup
        INNER JOIN dbo.NHAN_VIEN_NHOM AS membership
            ON membership.Ma_Nhom = managedGroup.ID
        WHERE managedGroup.Ma_Nguoi_QL = employee.ID
          AND membership.Ma_Nhan_Vien = employee.ID
    ) THEN 2
    ELSE 3
END
FROM dbo.NHAN_VIEN AS employee
WHERE ISNULL(employee.Xoa, 0) = 0;

-- Reuse the existing role IDs, so user foreign keys and employee rows remain valid.
UPDATE dbo.Role SET Name = N'Admin', Code = N'admin', Deleted = 0 WHERE Id = 1;
UPDATE dbo.Role SET Name = N'Qu' + NCHAR(7843) + N'n l' + NCHAR(253), Code = N'manager', Deleted = 0 WHERE Id = 2;
UPDATE dbo.Role SET Name = N'Sale', Code = N'sale', Deleted = 0 WHERE Id = 3;
UPDATE dbo.Role SET Deleted = 1 WHERE Id NOT IN (1, 2, 3);

COMMIT TRANSACTION;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Role_GetRoles @userId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name
    FROM dbo.Role
    WHERE Id IN (1, 2, 3) AND ISNULL(Deleted, 0) = 0
    ORDER BY Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CheckIsAdmin @userId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT isAdmin = CASE WHEN EXISTS
    (
        SELECT 1 FROM dbo.NHAN_VIEN_QUYEN WHERE Ma_NV = @userId
    ) THEN 1 ELSE 0 END;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CheckIsTeamlead @userId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT isTeamLead = CASE WHEN EXISTS
    (
        SELECT 1
        FROM dbo.NHAN_VIEN AS employee
        WHERE employee.ID = @userId AND employee.RoleId = 2
    ) OR EXISTS
    (
        SELECT 1
        FROM dbo.NHAN_VIEN_NHOM AS membership
        INNER JOIN dbo.NHOM AS managedGroup ON managedGroup.ID = membership.Ma_Nhom
        WHERE managedGroup.Ma_Nguoi_QL = @userId
          AND membership.Ma_Nhan_Vien = @userId
    ) THEN 1 ELSE 0 END;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_InsertUser_v2
    @id int OUTPUT,
    @userName varchar(50),
    @code varchar(50),
    @password varchar(50),
    @fullName nvarchar(100),
    @phone varchar(50),
    @email varchar(50),
    @roleId int,
    @createdby int
AS
BEGIN
    SET NOCOUNT ON;
    IF @roleId NOT IN (1, 2, 3)
        THROW 50002, 'Invalid role. Use Admin, Manager, or Sale.', 1;

    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @createdby;

    INSERT dbo.NHAN_VIEN
        (Ma, Ten_Dang_Nhap, Mat_Khau, Ho_Ten, Dien_Thoai, Email, RoleId,
         Trang_Thai, Xoa, CreatedTime, CreatedBy, UpdatedTime, OrgId)
    VALUES
        (@code, @userName, @password, @fullName, @phone, @email, @roleId,
         1, 0, GETDATE(), @createdby, GETDATE(), @orgId);

    SET @id = CONVERT(int, SCOPE_IDENTITY());

    IF @roleId = 1 AND NOT EXISTS
        (SELECT 1 FROM dbo.NHAN_VIEN_QUYEN WHERE Ma_NV = @id)
        INSERT dbo.NHAN_VIEN_QUYEN (Ma_NV, Quyen) VALUES (@id, N'fffffffff');
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_UpdateUser_v2
    @id int,
    @fullName nvarchar(100),
    @phone varchar(50),
    @email varchar(50),
    @roleId int,
    @updatedby int
AS
BEGIN
    SET NOCOUNT ON;
    IF @roleId NOT IN (1, 2, 3)
        THROW 50002, 'Invalid role. Use Admin, Manager, or Sale.', 1;

    UPDATE dbo.NHAN_VIEN
    SET Ho_Ten = @fullName,
        Dien_Thoai = @phone,
        Email = @email,
        RoleId = @roleId,
        UpdatedBy = @updatedby,
        UpdatedTime = GETDATE()
    WHERE ID = @id AND ISNULL(Xoa, 0) = 0;

    IF @roleId = 1
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM dbo.NHAN_VIEN_QUYEN WHERE Ma_NV = @id)
            INSERT dbo.NHAN_VIEN_QUYEN (Ma_NV, Quyen) VALUES (@id, N'fffffffff');
    END
    ELSE
    BEGIN
        DELETE FROM dbo.NHAN_VIEN_QUYEN WHERE Ma_NV = @id;
    END;
END;
GO
