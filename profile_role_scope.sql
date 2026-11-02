-- Role-scoped profile queries for Admin (1), Teamlead (2), and Sale/member (3).
-- Deploy this migration before publishing the application changes.
CREATE OR ALTER PROCEDURE dbo.sp_Profile_SearchScoped
    @UserId int,
    @UserType int,
    @Flow int,              -- 1: profile management, 2: approval
    @Mode int,              -- 0: count, 1: page
    @GroupId int = 0,
    @MemberId int = 0,
    @FromDate datetime,
    @ToDate datetime,
    @DateType int,
    @ProfileCode nvarchar(50) = N'',
    @IdentityNumber nvarchar(50) = N'',
    @Statuses nvarchar(100) = N'',
    @FreeText nvarchar(50) = N'',
    @Offset int = 0,
    @Limit int = 10
AS
BEGIN
    SET NOCOUNT ON;

    SET @ProfileCode = ISNULL(@ProfileCode, N'');
    SET @IdentityNumber = ISNULL(@IdentityNumber, N'');
    SET @FreeText = NULLIF(LTRIM(RTRIM(ISNULL(@FreeText, N''))), N'');
    SET @Statuses = ISNULL(@Statuses, N'');
    IF @Flow = 1 SET @Statuses = @Statuses + N',6,7,8';

    DECLARE @AllowedGroups TABLE (ID int PRIMARY KEY);
    IF @UserType = 2
    BEGIN
        INSERT INTO @AllowedGroups (ID)
        SELECT g.ID
        FROM dbo.NHOM AS g
        WHERE EXISTS
        (
            SELECT 1
            FROM dbo.NHOM AS rootGroup
            WHERE rootGroup.Ma_Nguoi_QL = @UserId
              AND
              (
                  g.ID = rootGroup.ID
                  OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                      N'.' + ISNULL(g.Chuoi_Ma_Cha, N'') + N'.') > 0
                  OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                      N'.' + ISNULL(g.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), g.ID) + N'.') > 0
              )
        );
    END;

    CREATE TABLE #ScopedProfiles (ID int NOT NULL PRIMARY KEY);

    INSERT INTO #ScopedProfiles (ID)
    SELECT h.ID
    FROM dbo.HO_SO AS h
    INNER JOIN dbo.SAN_PHAM_VAY AS product ON product.ID = h.San_Pham_Vay
    INNER JOIN dbo.DOI_TAC AS partner ON partner.ID = product.Ma_Doi_Tac
    LEFT JOIN dbo.NHAN_VIEN AS creator ON creator.ID = h.Ma_Nguoi_Tao
    LEFT JOIN dbo.NHAN_VIEN AS updater ON updater.ID = h.Ma_Nguoi_Cap_Nhat
    LEFT JOIN dbo.NHAN_VIEN AS courier ON courier.ID = h.Courier_Code
    LEFT JOIN dbo.TRANG_THAI_HS AS statusRow ON statusRow.ID = h.Ma_Trang_Thai
    LEFT JOIN dbo.KET_QUA_HS AS resultRow ON resultRow.ID = h.Ma_Ket_Qua
    LEFT JOIN dbo.KHU_VUC AS district ON district.ID = h.Ma_Khu_Vuc
    LEFT JOIN dbo.KHU_VUC AS parentArea ON parentArea.ID = district.Ma_Cha
    WHERE ISNULL(h.Da_Xoa, 0) = 0
      AND
      (
          @UserType = 1
          OR (@UserType = 2 AND
              (h.Ma_Nguoi_Tao = @UserId OR EXISTS
              (
                  SELECT 1
                  FROM dbo.NHAN_VIEN_NHOM AS membership
                  WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
                    AND membership.Ma_Nhom IN (SELECT ID FROM @AllowedGroups)
              )))
          OR (@UserType NOT IN (1, 2) AND h.Ma_Nguoi_Tao = @UserId)
      )
      AND (@UserType <> 2 OR @GroupId <= 0 OR EXISTS
          (SELECT 1 FROM @AllowedGroups WHERE ID = @GroupId))
      AND (@MemberId <= 0 OR h.Ma_Nguoi_Tao = @MemberId)
      AND (@GroupId <= 0 OR EXISTS
          (
              SELECT 1
              FROM dbo.NHAN_VIEN_NHOM AS membership
              INNER JOIN dbo.NHOM AS memberGroup ON memberGroup.ID = membership.Ma_Nhom
              WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
                AND
                (
                    memberGroup.ID = @GroupId
                    OR CHARINDEX(N'.' + CONVERT(nvarchar(20), @GroupId) + N'.',
                        N'.' + ISNULL(memberGroup.Chuoi_Ma_Cha, N'') + N'.') > 0
                    OR CHARINDEX(N'.' + CONVERT(nvarchar(20), @GroupId) + N'.',
                        N'.' + ISNULL(memberGroup.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), memberGroup.ID) + N'.') > 0
                )
          ))
      AND h.Ma_Ho_So LIKE N'%' + @ProfileCode + N'%'
      AND ISNULL(h.CMND, N'') LIKE N'%' + @IdentityNumber + N'%'
      AND
      (
          (@DateType = 1 AND h.Ngay_Tao >= CONVERT(date, @FromDate)
             AND h.Ngay_Tao < DATEADD(day, 1, CONVERT(date, @ToDate)))
          OR (@DateType = 2 AND h.Ngay_Cap_Nhat >= CONVERT(date, @FromDate)
             AND h.Ngay_Cap_Nhat < DATEADD(day, 1, CONVERT(date, @ToDate)))
      )
      AND h.Ma_Trang_Thai IN
          (SELECT TRY_CONVERT(int, value)
           FROM dbo.fn_SplitStringToTable(@Statuses, N',')
           WHERE TRY_CONVERT(int, value) IS NOT NULL)
      AND
      (
          @FreeText IS NULL
          OR h.Ma_Ho_So LIKE N'%' + @FreeText + N'%'
          OR partner.Ten LIKE N'%' + @FreeText + N'%'
          OR h.CMND LIKE N'%' + @FreeText + N'%'
          OR h.Ten_Khach_Hang LIKE N'%' + @FreeText + N'%'
          OR statusRow.Ten LIKE N'%' + @FreeText + N'%'
          OR resultRow.Ten LIKE N'%' + @FreeText + N'%'
          OR creator.Ma LIKE N'%' + @FreeText + N'%'
          OR creator.Ho_Ten LIKE N'%' + @FreeText + N'%'
          OR updater.Ma LIKE N'%' + @FreeText + N'%'
          OR courier.Ma LIKE N'%' + @FreeText + N'%'
          OR parentArea.Ten LIKE N'%' + @FreeText + N'%'
          OR product.Ten LIKE N'%' + @FreeText + N'%'
      );

    IF @Mode = 0
    BEGIN
        SELECT COUNT(1) AS TotalRecord FROM #ScopedProfiles;
        RETURN;
    END;

    IF @Offset < 0 SET @Offset = 0;
    IF @Limit < 1 OR @Limit > 50000 SET @Limit = 10;

    SELECT h.ID,
           h.Ma_Ho_So AS MaHoSo,
           h.Ngay_Tao AS NgayTao,
           partner.Ten AS DoiTac,
           h.CMND,
           h.SDT AS Phone,
           h.Ten_Khach_Hang AS TenKH,
           statusRow.Ten AS TrangThaiHS,
           resultRow.Ten AS KetQuaHS,
           h.Ma_Trang_Thai AS MaTrangThai,
           h.Ngay_Cap_Nhat AS NgayCapNhat,
           CAST(NULL AS datetime) AS DisbursementDate,
           creator.Ma AS MaNV,
           creator.Ho_Ten AS NhanVienBanHang,
           updater.Ma AS MaNVSua,
           courier.Ma AS MaNVLayHS,
           ISNULL(team.Ten, N'') AS DoiNguBanHang,
           h.Co_Bao_Hiem AS CoBaoHiem,
           ISNULL(parentArea.Ten, N'') AS DiaChiKH,
           ISNULL(parentArea.Ten, N'') AS KhuVucText,
           dbo.fn_getGhichuByHosoId(h.ID) AS GhiChu,
           product.Ten AS TenSanPham,
           ISNULL(h.F88Result, 0) AS F88Result
    FROM #ScopedProfiles AS scoped
    INNER JOIN dbo.HO_SO AS h ON h.ID = scoped.ID
    INNER JOIN dbo.SAN_PHAM_VAY AS product ON product.ID = h.San_Pham_Vay
    INNER JOIN dbo.DOI_TAC AS partner ON partner.ID = product.Ma_Doi_Tac
    LEFT JOIN dbo.NHAN_VIEN AS creator ON creator.ID = h.Ma_Nguoi_Tao
    LEFT JOIN dbo.NHAN_VIEN AS updater ON updater.ID = h.Ma_Nguoi_Cap_Nhat
    LEFT JOIN dbo.NHAN_VIEN AS courier ON courier.ID = h.Courier_Code
    LEFT JOIN dbo.TRANG_THAI_HS AS statusRow ON statusRow.ID = h.Ma_Trang_Thai
    LEFT JOIN dbo.KET_QUA_HS AS resultRow ON resultRow.ID = h.Ma_Ket_Qua
    LEFT JOIN dbo.KHU_VUC AS district ON district.ID = h.Ma_Khu_Vuc
    LEFT JOIN dbo.KHU_VUC AS parentArea ON parentArea.ID = district.Ma_Cha
    OUTER APPLY
    (
        SELECT TOP (1) g.Ten
        FROM dbo.NHAN_VIEN_NHOM AS membership
        INNER JOIN dbo.NHOM AS g ON g.ID = membership.Ma_Nhom
        WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
        ORDER BY g.ID
    ) AS team
    ORDER BY h.Ngay_Cap_Nhat DESC, h.ID DESC
    OFFSET @Offset ROWS
    FETCH NEXT @Limit ROWS ONLY;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Profile_CanUserAccess
    @ProfileId int,
    @UserId int,
    @UserType int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CanAccess bit = 0;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.HO_SO AS h
        WHERE h.ID = @ProfileId
          AND ISNULL(h.Da_Xoa, 0) = 0
          AND
          (
              @UserType = 1
              OR h.Ma_Nguoi_Tao = @UserId
              OR
              (
                  @UserType = 2
                  AND EXISTS
                  (
                      SELECT 1
                      FROM dbo.NHAN_VIEN_NHOM AS membership
                      INNER JOIN dbo.NHOM AS memberGroup ON memberGroup.ID = membership.Ma_Nhom
                      WHERE membership.Ma_Nhan_Vien = h.Ma_Nguoi_Tao
                        AND EXISTS
                        (
                            SELECT 1
                            FROM dbo.NHOM AS rootGroup
                            WHERE rootGroup.Ma_Nguoi_QL = @UserId
                              AND
                              (
                                  memberGroup.ID = rootGroup.ID
                                  OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                                      N'.' + ISNULL(memberGroup.Chuoi_Ma_Cha, N'') + N'.') > 0
                                  OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                                      N'.' + ISNULL(memberGroup.Chuoi_Ma_Cha, N'') + N'.' + CONVERT(nvarchar(20), memberGroup.ID) + N'.') > 0
                              )
                        )
                  )
              )
          )
    )
        SET @CanAccess = 1;

    SELECT @CanAccess AS CanAccess;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Profile_CanUserAccessGroup
    @GroupId int,
    @UserId int,
    @UserType int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CanAccess bit = 0;

    IF @UserType = 1
        SET @CanAccess = 1;
    ELSE IF @UserType = 2 AND EXISTS
    (
        SELECT 1
        FROM dbo.NHOM AS selectedGroup
        WHERE selectedGroup.ID = @GroupId
          AND EXISTS
          (
              SELECT 1
              FROM dbo.NHOM AS rootGroup
              WHERE rootGroup.Ma_Nguoi_QL = @UserId
                AND
                (
                    selectedGroup.ID = rootGroup.ID
                    OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                        N'.' + ISNULL(selectedGroup.Chuoi_Ma_Cha, N'') + N'.') > 0
                    OR CHARINDEX(N'.' + CONVERT(nvarchar(20), rootGroup.ID) + N'.',
                        N'.' + ISNULL(selectedGroup.Chuoi_Ma_Cha, N'') + N'.'
                            + CONVERT(nvarchar(20), selectedGroup.ID) + N'.') > 0
                )
          )
    )
        SET @CanAccess = 1;

    SELECT @CanAccess AS CanAccess;
END;
GO

-- The legacy Test13 TAI_LIEU_HS table does not have ProfileTypeId.
-- Accept the repository's optional type parameter but keep the legacy query schema.
CREATE OR ALTER PROCEDURE dbo.getTailieuByHosoId
    @profileId int,
    @profileTypeId int = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT tl.ID AS FileId,
           tl.Ma_Loai AS [Key],
           tl.Ten AS FileName,
           tl.Duong_Dan_File AS FileUrl,
           ltl.Ten AS KeyName,
           ltl.Bat_Buoc AS IsRequire
    FROM dbo.TAI_LIEU_HS AS tl
    INNER JOIN dbo.LOAI_TAI_LIEU AS ltl ON ltl.ID = tl.Ma_Loai
    WHERE tl.Ma_Ho_So = @profileId
      AND ISNULL(tl.Deleted, 0) = 0
    ORDER BY ltl.Bat_Buoc DESC;
END;
GO

-- Accept the current repository payload and old parameter names while writing
-- only columns available in the legacy Test13 TAI_LIEU_HS table.
CREATE OR ALTER PROCEDURE dbo.sp_TAI_LIEU_HS_Them
    @FileKey int = NULL,
    @FileName nvarchar(500) = NULL,
    @FilePath nvarchar(max) = NULL,
    @ProfileId int = NULL,
    @Deleted bit = 0,
    @ProfileTypeId int = 1,
    @DocumentName nvarchar(500) = NULL,
    @DocumentCode varchar(400) = NULL,
    @MC_DocumentId int = 0,
    @MC_MapBpmVar varchar(400) = NULL,
    @MC_GroupId int = 0,
    @OrderId int = 0,
    @Folder nvarchar(max) = NULL,
    @McId nvarchar(50) = NULL,
    @Maloai int = NULL,
    @Ten nvarchar(500) = NULL,
    @DuongDan nvarchar(max) = NULL,
    @MaHS int = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ResolvedFileKey int = COALESCE(@FileKey, @Maloai);
    DECLARE @ResolvedFileName nvarchar(500) = COALESCE(@FileName, @Ten);
    DECLARE @ResolvedFilePath nvarchar(max) = COALESCE(@FilePath, @DuongDan);
    DECLARE @ResolvedProfileId int = COALESCE(@ProfileId, @MaHS);

    IF @ResolvedFileKey IS NULL OR @ResolvedProfileId IS NULL
        THROW 50001, 'FileKey and ProfileId are required.', 1;

    INSERT INTO dbo.TAI_LIEU_HS (Ma_Loai, Ten, Duong_Dan_File, Ma_Ho_So, Deleted)
    VALUES (@ResolvedFileKey, @ResolvedFileName, @ResolvedFilePath, @ResolvedProfileId, ISNULL(@Deleted, 0));
END;
GO
