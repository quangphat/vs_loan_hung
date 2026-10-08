-- Local compatibility migration for the legacy app on the Test13 database.
-- This intentionally targets the schema from db.sql (Trang_Thai, Ngay_Tao, Da_Xoa).
-- It does not run the unrelated/destructive renames in revoke_debt.sql.
USE [Test13];
GO

IF COL_LENGTH(N'dbo.NHAN_VIEN', N'OrgId') IS NULL
    ALTER TABLE dbo.NHAN_VIEN ADD OrgId int NULL;
IF COL_LENGTH(N'dbo.NHOM', N'OrgId') IS NULL
    ALTER TABLE dbo.NHOM ADD OrgId int NULL;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_Login
        @UserName nvarchar(50),
        @Password nvarchar(200)
AS
BEGIN
        SET NOCOUNT ON;
        DECLARE @RoleId int = 0;

        SELECT TOP (1) @RoleId = CASE WHEN ISNULL(RoleId, 0) = 5 THEN 1 ELSE ISNULL(RoleId, 0) END
        FROM dbo.NHAN_VIEN
        WHERE Ten_Dang_Nhap = @UserName
            AND Mat_Khau = @Password
            AND ISNULL(Xoa, 0) = 0
            AND ISNULL(Trang_Thai, 0) = 1;

        SELECT ID,
                     Ten_Dang_Nhap AS UserName,
                     Mat_Khau AS Passowrd,
                     Ma AS Code,
                     Email,
                     Ho_Ten AS FullName,
                     Dien_Thoai AS Phone,
                     Trang_Thai AS IsActive,
                     ISNULL(OrgId, 0) AS OrgId,
                     @RoleId AS RoleId
        FROM dbo.NHAN_VIEN
        WHERE Ten_Dang_Nhap = @UserName
            AND Mat_Khau = @Password
            AND ISNULL(Xoa, 0) = 0
            AND ISNULL(Trang_Thai, 0) = 1;
END;
GO

-- The current repository passes @type; the db.sql schema has no type column,
-- so preserve the legacy behavior and return all document types.
CREATE OR ALTER PROCEDURE dbo.sp_LOAI_TAI_LIEU_LayDS @type int = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Ten, Bat_Buoc AS BatBuoc
    FROM dbo.LOAI_TAI_LIEU
    ORDER BY ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Profile_GetProfileHaveNotSeen
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
    SET NOCOUNT ON;
    DECLARE @AllowedGroups TABLE (ID int);

    IF ISNULL(@MaNhom, 0) = 0
    BEGIN
        INSERT INTO @AllowedGroups (ID)
        SELECT DISTINCT g.ID
        FROM dbo.NHOM AS g
        WHERE EXISTS
        (
            SELECT 1
            FROM dbo.fn_SplitStringToTable(ISNULL(g.Chuoi_Ma_Cha, N''), N'.') AS pathPart
            INNER JOIN dbo.NHAN_VIEN_CF AS permissionGroup
                ON TRY_CONVERT(int, pathPart.value) = permissionGroup.Ma_Nhom
            WHERE permissionGroup.Ma_Nhan_Vien = @MaNVDangNhap
        )
        OR EXISTS
        (
            SELECT 1
            FROM dbo.NHAN_VIEN_CF AS permissionGroup
            WHERE permissionGroup.Ma_Nhan_Vien = @MaNVDangNhap
              AND permissionGroup.Ma_Nhom = g.ID
        );
    END;

    SELECT h.ID,
           h.Ma_Ho_So AS MaHoSo,
           ISNULL(h.Ngay_Tao, GETDATE()) AS NgayTao,
           ISNULL(dt.Ten, N'') AS DoiTac,
           ISNULL(h.CMND, N'') AS CMND,
           ISNULL(h.Ten_Khach_Hang, N'') AS TenKH,
           ISNULL(h.Ma_Trang_Thai, 0) AS MaTrangThai,
           ISNULL(statusRow.Ten, N'') AS TrangThaiHS,
           ISNULL(resultRow.Ten, N'') AS KetQuaHS,
           h.Ngay_Cap_Nhat AS NgayCapNhat,
           ISNULL(creator.Ma, '') AS MaNV,
           ISNULL(creator.Ho_Ten, N'') AS NhanVienBanHang,
           ISNULL(updater.Ma, '') AS MaNVSua,
           ISNULL(h.Co_Bao_Hiem, 0) AS CoBaoHiem,
           ISNULL(parentArea.Ten, ISNULL(district.Ten, N'')) AS DiaChiKH,
           ISNULL(h.Ghi_Chu, N'') AS GhiChu,
           ISNULL(courier.Ma, '') AS MaNVLayHS,
           ISNULL(team.Ten, N'') AS DoiNguBanHang,
           ISNULL(product.Ten, N'') AS TenSanPham
    FROM dbo.HO_SO_DUYET_XEM AS unseen
    INNER JOIN dbo.HO_SO AS h ON h.ID = unseen.Ma_Ho_So
    LEFT JOIN dbo.NHAN_VIEN AS creator ON creator.ID = h.Ma_Nguoi_Tao
    LEFT JOIN dbo.NHAN_VIEN AS updater ON updater.ID = h.Ma_Nguoi_Cap_Nhat
    LEFT JOIN dbo.NHAN_VIEN AS courier ON courier.ID = h.Courier_Code
    LEFT JOIN dbo.KHU_VUC AS district ON district.ID = h.Ma_Khu_Vuc
    LEFT JOIN dbo.KHU_VUC AS parentArea ON parentArea.ID = district.Ma_Cha
    LEFT JOIN dbo.SAN_PHAM_VAY AS product ON product.ID = h.San_Pham_Vay
    LEFT JOIN dbo.DOI_TAC AS dt ON dt.ID = product.Ma_Doi_Tac
    LEFT JOIN dbo.TRANG_THAI_HS AS statusRow ON statusRow.ID = h.Ma_Trang_Thai
    LEFT JOIN dbo.KET_QUA_HS AS resultRow ON resultRow.ID = h.Ma_Ket_Qua
    OUTER APPLY
    (
        SELECT TOP (1) g.Ten
        FROM dbo.NHAN_VIEN_NHOM AS membership
        INNER JOIN dbo.NHOM AS g ON g.ID = membership.Ma_Nhom
        WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
        ORDER BY g.ID
    ) AS team
    WHERE ISNULL(unseen.Xem, 0) = 0
      AND ISNULL(h.Da_Xoa, 0) = 0
      AND
      (
          (@MaThanhVien > 0 AND h.Ma_Nguoi_Tao = @MaThanhVien)
          OR (@MaNhom > 0 AND ISNULL(@MaThanhVien, 0) = 0 AND EXISTS
          (
              SELECT 1
              FROM dbo.NHAN_VIEN_NHOM AS membership
              INNER JOIN dbo.NHOM AS g ON g.ID = membership.Ma_Nhom
              WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
                AND
                (
                    g.ID = @MaNhom
                    OR ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
                       LIKE N'%.' + CONVERT(nvarchar(20), @MaNhom) + N'.%'
                    OR ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
                       LIKE N'%.' + CONVERT(nvarchar(20), @MaNhom)
                )
          ))
          OR (ISNULL(@MaNhom, 0) = 0 AND ISNULL(@MaThanhVien, 0) = 0 AND EXISTS
          (
              SELECT 1
              FROM dbo.NHAN_VIEN_NHOM AS membership
              WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
                AND membership.Ma_Nhom IN (SELECT ID FROM @AllowedGroups)
          ))
          OR (ISNULL(@MaNhom, 0) = 0 AND ISNULL(@MaThanhVien, 0) = 0 AND EXISTS
          (
              SELECT 1 FROM dbo.NHAN_VIEN_QUYEN AS p
              WHERE p.Ma_NV = @MaNVDangNhap
          ))
      )
      AND h.Ma_Ho_So LIKE N'%' + ISNULL(@MaHS, N'') + N'%'
      AND ISNULL(h.SDT, N'') LIKE N'%' + ISNULL(@CMND, N'') + N'%'
      AND
      (
          (@LoaiNgay = 1 AND h.Ngay_Tao >= CONVERT(date, @TuNgay)
             AND h.Ngay_Tao < DATEADD(day, 1, CONVERT(date, @DenNgay)))
          OR (@LoaiNgay = 2 AND h.Ngay_Cap_Nhat >= CONVERT(date, @TuNgay)
             AND h.Ngay_Cap_Nhat < DATEADD(day, 1, CONVERT(date, @DenNgay)))
      )
      AND h.Ma_Trang_Thai IN
      (
          SELECT TRY_CONVERT(int, value)
          FROM dbo.fn_SplitStringToTable(ISNULL(@TrangThai, N''), N',')
          WHERE TRY_CONVERT(int, value) IS NOT NULL
      );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Profile_GetMyProfilesNotSeen
    @MaNhanVien int,
    @TuNgay datetime,
    @DenNgay datetime,
    @MaHS nvarchar(50),
    @SDT nvarchar(50),
    @TrangThai nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT h.ID,
           h.Ma_Ho_So AS MaHoSo,
           ISNULL(h.Ngay_Tao, GETDATE()) AS NgayTao,
           ISNULL(dt.Ten, N'') AS DoiTac,
           ISNULL(h.CMND, N'') AS CMND,
           ISNULL(h.Ten_Khach_Hang, N'') AS TenKH,
           ISNULL(h.Ma_Trang_Thai, 0) AS MaTrangThai,
           ISNULL(statusRow.Ten, N'') AS TrangThaiHS,
           ISNULL(resultRow.Ten, N'') AS KetQuaHS,
           h.Ngay_Cap_Nhat AS NgayCapNhat,
           ISNULL(creator.Ma, '') AS MaNV,
           ISNULL(creator.Ho_Ten, N'') AS NhanVienBanHang,
           ISNULL(updater.Ma, '') AS MaNVSua,
           ISNULL(h.Co_Bao_Hiem, 0) AS CoBaoHiem,
           ISNULL(h.Dia_Chi, N'') AS DiaChiKH,
           ISNULL(parentArea.Ten, ISNULL(district.Ten, N'')) AS KhuVucText,
           ISNULL(h.Ghi_Chu, N'') AS GhiChu
    FROM dbo.HO_SO_XEM AS unseen
    INNER JOIN dbo.HO_SO AS h ON h.ID = unseen.Ma_Ho_So
    LEFT JOIN dbo.NHAN_VIEN AS creator ON creator.ID = h.Ma_Nguoi_Tao
    LEFT JOIN dbo.NHAN_VIEN AS updater ON updater.ID = h.Ma_Nguoi_Cap_Nhat
    LEFT JOIN dbo.KHU_VUC AS district ON district.ID = h.Ma_Khu_Vuc
    LEFT JOIN dbo.KHU_VUC AS parentArea ON parentArea.ID = district.Ma_Cha
    LEFT JOIN dbo.SAN_PHAM_VAY AS product ON product.ID = h.San_Pham_Vay
    LEFT JOIN dbo.DOI_TAC AS dt ON dt.ID = product.Ma_Doi_Tac
    LEFT JOIN dbo.TRANG_THAI_HS AS statusRow ON statusRow.ID = h.Ma_Trang_Thai
    LEFT JOIN dbo.KET_QUA_HS AS resultRow ON resultRow.ID = h.Ma_Ket_Qua
    WHERE ISNULL(unseen.Xem, 0) = 0
      AND h.Ma_Nguoi_Tao = @MaNhanVien
      AND ISNULL(h.Da_Xoa, 0) = 0
      AND h.Ma_Ho_So LIKE N'%' + ISNULL(@MaHS, N'') + N'%'
      AND ISNULL(h.SDT, N'') LIKE N'%' + ISNULL(@SDT, N'') + N'%'
      AND h.Ngay_Tao >= CONVERT(date, @TuNgay)
      AND h.Ngay_Tao < DATEADD(day, 1, CONVERT(date, @DenNgay))
    ORDER BY h.Ngay_Tao DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CountEmployee
    @workFromDate datetime,
    @workToDate datetime,
    @roleId int,
    @freeText nvarchar(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*)
    FROM dbo.NHAN_VIEN AS n
    WHERE (n.WorkDate IS NULL OR (n.WorkDate >= CONVERT(date, @workFromDate)
       AND n.WorkDate < DATEADD(day, 1, CONVERT(date, @workToDate))))
      AND (@freeText IS NULL OR @freeText = N'' OR n.Ho_Ten LIKE N'%' + @freeText + N'%'
        OR n.Ten_Dang_Nhap LIKE N'%' + @freeText + N'%'
        OR n.Dien_Thoai LIKE N'%' + @freeText + N'%'
        OR n.Email LIKE N'%' + @freeText + N'%')
      AND (@roleId = 0 OR n.RoleId = @roleId)
      AND ISNULL(n.Xoa, 0) = 0;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetEmployees
    @workFromDate datetime,
    @workToDate datetime,
    @freeText nvarchar(30),
    @roleId int,
    @page int,
    @limit int,
    @OrgId int = 0,
    @currentUserId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    IF @page < 1 SET @page = 1;
    IF @limit < 1 OR @limit > 100 SET @limit = 10;
    DECLARE @offset int = (@page - 1) * @limit;
    SELECT COUNT(*) OVER() AS TotalRecord,
           n.ID, n.Ten_Dang_Nhap AS UserName, n.Ho_Ten AS FullName,
           n.RoleId, r.Name AS RoleName, n.Email, n.Dien_Thoai AS Phone,
           n.CreatedTime, n.Ma AS Code, n.WorkDate,
           CONCAT(ISNULL(district.Ten, N''), N' - ', ISNULL(parentArea.Ten, N'')) AS Location
    FROM dbo.NHAN_VIEN AS n
    LEFT JOIN dbo.KHU_VUC AS district ON district.ID = n.DistrictId
    LEFT JOIN dbo.KHU_VUC AS parentArea ON parentArea.ID = district.Ma_Cha
    LEFT JOIN dbo.Role AS r ON r.Id = n.RoleId
    WHERE (n.WorkDate IS NULL OR (n.WorkDate >= CONVERT(date, @workFromDate)
       AND n.WorkDate < DATEADD(day, 1, CONVERT(date, @workToDate))))
      AND (@freeText IS NULL OR @freeText = N'' OR n.Ho_Ten LIKE N'%' + @freeText + N'%'
        OR n.Ten_Dang_Nhap LIKE N'%' + @freeText + N'%'
        OR n.Dien_Thoai LIKE N'%' + @freeText + N'%'
        OR n.Email LIKE N'%' + @freeText + N'%')
      AND (@roleId = 0 OR n.RoleId = @roleId)
      AND ISNULL(n.Xoa, 0) = 0
      AND ISNULL(n.OrgId, 0) = ISNULL(@OrgId, 0)
    ORDER BY n.ID DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetEmployeeById @userId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.NHAN_VIEN WHERE ID = @userId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_GetByUsername
    @userName varchar(50),
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT TOP (1) *
    FROM dbo.NHAN_VIEN
    WHERE Ten_Dang_Nhap = @userName
      AND ISNULL(Xoa, 0) = 0
      AND ISNULL(OrgId, 0) = @orgId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_GetByCode
    @code varchar(50),
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT TOP (1) ID, Ma AS Code, Ten_Dang_Nhap AS UserName, Ho_Ten AS FullName
    FROM dbo.NHAN_VIEN
    WHERE Ma = @code
      AND ISNULL(Xoa, 0) = 0
      AND ISNULL(OrgId, 0) = @orgId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_GetFull @orgId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID AS Id, CONCAT(ISNULL(Ma, ''), ' - ', ISNULL(Ho_Ten, N'')) AS Name
    FROM dbo.NHAN_VIEN
    WHERE ISNULL(Xoa, 0) = 0 AND ISNULL(OrgId, 0) = ISNULL(@orgId, 0)
    ORDER BY ID DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_GetPaging
    @orgId int = 0,
    @page int = 1,
    @freeText nvarchar(50) = NULL,
    @limit int = 20
AS
BEGIN
    SET NOCOUNT ON;
    IF @page < 1 SET @page = 1;
    IF @limit < 1 OR @limit > 100 SET @limit = 20;
    DECLARE @offset int = (@page - 1) * @limit;
    SELECT COUNT(*) OVER() AS TotalRecord,
           ID AS Id, CONCAT(ISNULL(Ma, ''), ' - ', ISNULL(Ho_Ten, N'')) AS Name
    FROM dbo.NHAN_VIEN
    WHERE ISNULL(Xoa, 0) = 0
      AND ISNULL(OrgId, 0) = ISNULL(@orgId, 0)
      AND (@freeText IS NULL OR @freeText = N'' OR Ma LIKE N'%' + @freeText + N'%'
           OR Ho_Ten LIKE N'%' + @freeText + N'%')
    ORDER BY CreatedTime DESC, ID DESC
    OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Role_GetRoles @userId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name FROM dbo.Role WHERE ISNULL(Deleted, 0) = 0 ORDER BY Id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_LayDSByRule
    @UserId int,
    @Rule int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT e.ID,
           e.Ma AS Code,
           e.Ho_Ten AS FullName,
           e.Ten_Dang_Nhap AS UserName,
           e.Dien_Thoai AS Phone,
           e.Email
    FROM dbo.NHAN_VIEN AS e
    INNER JOIN dbo.NHAN_VIEN_CF AS permissionRow
        ON permissionRow.Ma_Nhan_Vien = e.ID
    WHERE permissionRow.Quyen = @Rule
      AND ISNULL(e.Xoa, 0) = 0
      AND permissionRow.Ma_Nhom IN
      (
          SELECT membership.Ma_Nhom
          FROM dbo.NHAN_VIEN_NHOM AS membership
          WHERE membership.Ma_Nhan_Vien = @UserId
      );
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
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @createdby;
    INSERT dbo.NHAN_VIEN
        (Ma, Ten_Dang_Nhap, Mat_Khau, Ho_Ten, Dien_Thoai, Email, RoleId,
         Trang_Thai, Xoa, CreatedTime, CreatedBy, UpdatedTime, OrgId)
    VALUES
        (@code, @userName, @password, @fullName, @phone, @email, @roleId,
         1, 0, GETDATE(), @createdby, GETDATE(), @orgId);
    SET @id = CONVERT(int, SCOPE_IDENTITY());
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
    UPDATE dbo.NHAN_VIEN
    SET Ho_Ten = @fullName,
        Dien_Thoai = @phone,
        Email = @email,
        RoleId = @roleId,
        UpdatedBy = @updatedby,
        UpdatedTime = GETDATE()
    WHERE ID = @id AND ISNULL(Xoa, 0) = 0;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_Delete_v2
    @id int,
    @DeletedBy int = NULL,
    @updatedby int = NULL,
    @Xoa int = 1
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.NHAN_VIEN
    SET Xoa = @Xoa,
        UpdatedBy = COALESCE(@updatedby, @DeletedBy),
        UpdatedTime = GETDATE()
    WHERE ID = @id;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ResetPassword
    @id int,
    @password varchar(50),
    @updatedBy int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.NHAN_VIEN
    SET Mat_Khau = @password,
        UpdatedBy = @updatedBy,
        UpdatedTime = GETDATE()
    WHERE ID = @id AND ISNULL(Xoa, 0) = 0;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_NHOM_LayDSNhom @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT ID, Ten, Chuoi_Ma_Cha AS ChuoiMaCha
    FROM dbo.NHOM
    WHERE ISNULL(OrgId, 0) = @orgId
    ORDER BY ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_NHOM_Them
    @ID int OUTPUT,
    @MaNhomCha int,
    @MaNguoiQL int,
    @TenVietTat nvarchar(50),
    @Ten nvarchar(200),
    @ChuoiMaCha nvarchar(100),
    @createdBy int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @createdBy;
    INSERT dbo.NHOM (Ma_Nhom_Cha, Ma_Nguoi_QL, Ten_Viet_Tat, Ten, Chuoi_Ma_Cha, OrgId)
    VALUES (@MaNhomCha, @MaNguoiQL, @TenVietTat, @Ten, @ChuoiMaCha, @orgId);
    SET @ID = CONVERT(int, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Group_GetChildGroup
    @parentGroupId int,
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT g.ID, g.Ten, g.Ten_Viet_Tat AS TenNgan,
           ISNULL(e.Ho_Ten, N'') AS NguoiQuanLy, g.Chuoi_Ma_Cha AS ChuoiMaCha
    FROM dbo.NHOM AS g
    LEFT JOIN dbo.NHAN_VIEN AS e ON e.ID = g.Ma_Nguoi_QL
    WHERE g.Ma_Nhom_Cha = @parentGroupId
      AND ISNULL(g.OrgId, 0) = @orgId
    ORDER BY g.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Group_GetById @groupId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT g.ID, g.Ten, g.Ten_Viet_Tat AS TenNgan,
           parent.Ten AS TenNhomCha, ISNULL(manager.Ho_Ten, N'') AS NguoiQuanLy
    FROM dbo.NHOM AS g
    LEFT JOIN dbo.NHOM AS parent ON parent.ID = g.Ma_Nhom_Cha
    LEFT JOIN dbo.NHAN_VIEN AS manager ON manager.ID = g.Ma_Nguoi_QL
    WHERE g.ID = @groupId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_Group_GetEmployeeByGroup @groupId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.ID, e.Ma, e.Ho_Ten AS HoTen, e.Email, e.Dien_Thoai AS SDT
    FROM dbo.NHAN_VIEN AS e
    INNER JOIN dbo.NHAN_VIEN_NHOM AS membership ON membership.Ma_Nhan_Vien = e.ID
    WHERE membership.Ma_Nhom = @groupId AND ISNULL(e.Xoa, 0) = 0
    ORDER BY e.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_NHOM_GetEmployeesByGroupId
    @groupId int,
    @isGetLeader bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    IF @isGetLeader = 1
    BEGIN
        SELECT e.ID AS Id, CONCAT(ISNULL(e.Ma, ''), ' - ', ISNULL(e.Ho_Ten, N'')) AS Name
        FROM dbo.NHOM AS g
        INNER JOIN dbo.NHAN_VIEN AS e ON e.ID = g.Ma_Nguoi_QL
        WHERE g.ID = @groupId AND ISNULL(e.Xoa, 0) = 0;
    END
    ELSE
    BEGIN
        SELECT e.ID AS Id, CONCAT(ISNULL(e.Ma, ''), ' - ', ISNULL(e.Ho_Ten, N'')) AS Name
        FROM dbo.NHAN_VIEN_NHOM AS membership
        INNER JOIN dbo.NHAN_VIEN AS e ON e.ID = membership.Ma_Nhan_Vien
        WHERE membership.Ma_Nhom = @groupId AND ISNULL(e.Xoa, 0) = 0
        ORDER BY e.ID;
    END;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_Group_LayDSChonThanhVienNhom_v2
    @groupId int,
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.ID AS Id, CONCAT(ISNULL(e.Ma, ''), ' - ', ISNULL(e.Ho_Ten, N'')) AS Name
    FROM dbo.NHAN_VIEN_NHOM AS membership
    INNER JOIN dbo.NHAN_VIEN AS e ON e.ID = membership.Ma_Nhan_Vien
    WHERE membership.Ma_Nhom = @groupId AND ISNULL(e.Xoa, 0) = 0
    ORDER BY e.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_employee_Group_LayDSKhongThanhVienNhom_v2
    @groupId int,
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT e.ID AS Id, CONCAT(ISNULL(e.Ma, ''), ' - ', ISNULL(e.Ho_Ten, N'')) AS Name
    FROM dbo.NHAN_VIEN AS e
    WHERE ISNULL(e.Xoa, 0) = 0
      AND ISNULL(e.OrgId, 0) = @orgId
      AND NOT EXISTS
      (
          SELECT 1 FROM dbo.NHAN_VIEN_NHOM AS membership
          WHERE membership.Ma_Nhom = @groupId AND membership.Ma_Nhan_Vien = e.ID
      )
    ORDER BY e.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Employee_Group_LayDSChonThanhVienNhomCaCon_v2
    @groupId int,
    @userId int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @orgId int = 0;
    SELECT @orgId = ISNULL(OrgId, 0) FROM dbo.NHAN_VIEN WHERE ID = @userId;
    SELECT DISTINCT e.ID AS Id, CONCAT(ISNULL(e.Ma, ''), ' - ', ISNULL(e.Ho_Ten, N'')) AS Name, e.Ma AS Code
    FROM dbo.NHAN_VIEN_NHOM AS membership
    INNER JOIN dbo.NHAN_VIEN AS e ON e.ID = membership.Ma_Nhan_Vien
    INNER JOIN dbo.NHOM AS g ON g.ID = membership.Ma_Nhom
    WHERE ISNULL(e.Xoa, 0) = 0
      AND ISNULL(e.OrgId, 0) = @orgId
      AND
      (
          g.ID = @groupId
          OR ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
             LIKE N'%.' + CONVERT(nvarchar(20), @groupId) + N'.%'
          OR ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
             LIKE N'%.' + CONVERT(nvarchar(20), @groupId)
      )
    ORDER BY e.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_NHOM_LayCayNhomCon_v2 @parentGroupId int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT g.ID, g.Ten, g.Chuoi_Ma_Cha AS ChuoiMaCha,
           ISNULL(g.Ma_Nguoi_QL, 0) AS MaNguoiQL,
           ISNULL(g.Ten_Viet_Tat, N'') AS TenQL
    FROM dbo.NHOM AS g
    WHERE ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
              LIKE N'%.' + CONVERT(nvarchar(20), @parentGroupId) + N'.%'
       OR ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID)
              LIKE N'%.' + CONVERT(nvarchar(20), @parentGroupId)
    ORDER BY g.ID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_NHOM_LayDSNhomDuyetChonTheoNhanVien_v2 @UserID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT g.ID, g.Ten, g.Chuoi_Ma_Cha AS ChuoiMaCha,
           g.Ma_Nhom_Cha AS MaNhomCha, ISNULL(manager.Ho_Ten, N'') AS TenQL
    FROM dbo.NHAN_VIEN_CF AS assigned
    INNER JOIN dbo.NHOM AS g ON g.ID = assigned.Ma_Nhom
    LEFT JOIN dbo.NHAN_VIEN AS manager ON manager.ID = g.Ma_Nguoi_QL
    WHERE assigned.Ma_Nhan_Vien = @UserID
    ORDER BY g.ID;
END;
GO
