USE [master]
GO
/****** Object:  Database [Test13]    Script Date: 09/01/2020 5:30:44 CH ******/
CREATE DATABASE [Test13]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Test13', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Test13.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Test13_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Test13_log.ldf' , SIZE = 1600KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Test13] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Test13].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Test13] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Test13] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Test13] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Test13] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Test13] SET ARITHABORT OFF 
GO
ALTER DATABASE [Test13] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Test13] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Test13] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Test13] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Test13] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Test13] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Test13] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Test13] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Test13] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Test13] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Test13] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Test13] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Test13] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Test13] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Test13] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Test13] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Test13] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Test13] SET RECOVERY FULL 
GO
ALTER DATABASE [Test13] SET  MULTI_USER 
GO
ALTER DATABASE [Test13] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Test13] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Test13] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Test13] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Test13] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Test13', N'ON'
GO
USE [Test13]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_getGhichuByHosoId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[fn_getGhichuByHosoId](@hosoId as int)
returns nvarchar(200)
as begin
declare @noidung as nvarchar(200)
select top(1) @noidung = Noidung from Ghichu where HosoId = @hosoId order by Id desc
return @noidung;
end

GO
/****** Object:  UserDefinedFunction [dbo].[fn_SplitStringToTable]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[fn_SplitStringToTable](@data nvarchar(max), @delimiter nvarchar(20) = ' ') 
RETURNS @Strings TABLE
(   
  value nvarchar(max)  
)
AS
BEGIN

DECLARE @index int
SET @index = -1

WHILE (LEN(@data) > 0)
  BEGIN 
    SET @index = CHARINDEX(@delimiter , @data) 
    IF (@index = 0) AND (LEN(@data) > 0) 
      BEGIN  
        INSERT INTO @Strings VALUES (@data)
          BREAK 
      END 
    IF (@index > 1) 
      BEGIN  
        INSERT INTO @Strings VALUES (LEFT(@data, @index - 1))  
        SET @data = RIGHT(@data, (LEN(@data) - @index)) 
      END 
    ELSE
      SET @data = RIGHT(@data, (LEN(@data) - @index))
    END
  RETURN
END








GO
/****** Object:  UserDefinedFunction [dbo].[getDoitacBySanphamId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getDoitacBySanphamId](@sanphamId int, @isGetId bit = 0)
returns nvarchar(200)
as
begin
if(@isGetId =0)
begin
declare @Doitac nvarchar(200) = '';
 select @Doitac = dt.Ten from DOI_TAC dt left join SAN_PHAM_VAY sp
on dt.ID = sp.Ma_Doi_Tac
where sp.ID = @sanphamId
return isnull(@doitac,'');
end;
else
begin
declare @id int = 0;
 select @id = dt.ID from DOI_TAC dt left join SAN_PHAM_VAY sp
on dt.ID = sp.Ma_Doi_Tac
where sp.ID = @sanphamId
return isnull(@id,0);
end;
return '';
end

GO
/****** Object:  UserDefinedFunction [dbo].[getProvinceIdFromDistrictId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getProvinceIdFromDistrictId](@districtId int)
returns nvarchar(200)
as
begin
declare @id int =0 ;
 select @id = ID from KHU_VUC 
where Id = (select Ma_Cha from KHU_VUC where ID = @districtId)
return isnull(@id,0);
end

GO
/****** Object:  UserDefinedFunction [dbo].[getProvinceNameFromDistrictId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getProvinceNameFromDistrictId](@districtId int)
returns nvarchar(200)
as
begin
declare @name nvarchar(200) = '';
 select @name = Ten from KHU_VUC 
where Id = (select Ma_Cha from KHU_VUC where ID = @districtId)
return isnull(@name,'');
end


GO
/****** Object:  Table [dbo].[AUTOID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AUTOID](
	[ID] [int] NOT NULL,
	[Name_ID] [nvarchar](50) NULL,
	[Prefix] [nvarchar](10) NULL,
	[Suffixes] [nvarchar](10) NULL,
	[Value] [int] NULL,
 CONSTRAINT [PK_AUTOID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](200) NULL,
	[Cmnd] [varchar](15) NULL,
	[CheckDate] [datetime] NULL,
	[CICStatus] [int] NULL,
	[Gender] [bit] NULL,
	[MatchCondition] [nvarchar](500) NULL,
	[NotMatch] [nvarchar](500) NULL,
	[LastNote] [nvarchar](200) NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerCheck]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerCheck](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[PartnerId] [int] NULL,
	[Status] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_CustomerCheck] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerNote]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerNote](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Note] [nvarchar](200) NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_CustomerNote] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DOI_TAC]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOI_TAC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](200) NULL,
	[AllowCheckDup] [bit] NULL,
	[F88Value] [int] NULL,
 CONSTRAINT [PK_DOI_TAC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[F88Error]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[F88Error](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HosoId] [int] NULL,
	[ErrorMessage] [nvarchar](500) NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_F88Error] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ghichu]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ghichu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Noidung] [nvarchar](300) NULL,
	[HosoId] [int] NULL,
	[CommentTime] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HO_SO]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HO_SO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma_Ho_So] [nvarchar](50) NULL,
	[Ma_Khach_Hang] [int] NULL,
	[Ten_Khach_Hang] [nvarchar](100) NULL,
	[CMND] [nvarchar](50) NULL,
	[Dia_Chi] [nvarchar](200) NULL,
	[Ma_Khu_Vuc] [int] NULL,
	[SDT] [nvarchar](50) NULL,
	[SDT2] [nvarchar](50) NULL,
	[Gioi_Tinh] [int] NULL,
	[Ngay_Tao] [datetime] NULL,
	[Ma_Nguoi_Tao] [int] NULL,
	[Ho_So_Cua_Ai] [int] NULL,
	[Ngay_Cap_Nhat] [datetime] NULL,
	[Ma_Nguoi_Cap_Nhat] [int] NULL,
	[Ngay_Nhan_Don] [datetime] NULL,
	[Ma_Trang_Thai] [int] NULL,
	[Ma_Ket_Qua] [int] NULL,
	[San_Pham_Vay] [int] NULL,
	[Ten_Cua_Hang] [nvarchar](200) NULL,
	[Co_Bao_Hiem] [bit] NULL,
	[So_Tien_Vay] [decimal](18, 0) NULL,
	[Han_Vay] [float] NULL,
	[Ghi_Chu] [nvarchar](500) NULL,
	[Courier_Code] [int] NULL,
	[Da_Xoa] [bit] NULL,
	[BirthDay] [datetime] NULL,
	[CMNDDay] [datetime] NULL,
	[F88Result] [int] NULL,
	[F88Reason] [nvarchar](200) NULL,
 CONSTRAINT [PK_HO_SO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HO_SO_DUYET_XEM]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HO_SO_DUYET_XEM](
	[Ma_Ho_So] [int] NOT NULL,
	[Xem] [int] NOT NULL,
 CONSTRAINT [PK_HO_SO_DUYET_XEM] PRIMARY KEY CLUSTERED 
(
	[Ma_Ho_So] ASC,
	[Xem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HO_SO_XEM]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HO_SO_XEM](
	[Ma_Ho_So] [int] NULL,
	[Xem] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KET_QUA_HS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KET_QUA_HS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](200) NULL,
	[Da_Xoa] [bit] NULL,
 CONSTRAINT [PK_KET_QUA_HS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KHACH_HANG]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHACH_HANG](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma] [nvarchar](50) NULL,
	[Ten] [nvarchar](100) NULL,
	[CMND] [nvarchar](50) NULL,
	[Gioi_Tinh] [int] NULL,
	[SDT] [nvarchar](50) NULL,
	[Dia_Chi] [nvarchar](200) NULL,
 CONSTRAINT [PK_KHACH_HANG] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KHU_VUC]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHU_VUC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](200) NULL,
	[Loai] [int] NULL,
	[Ma_Cha] [int] NULL,
 CONSTRAINT [PK_KHU_VUC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LOAI_TAI_LIEU]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOAI_TAI_LIEU](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](200) NULL,
	[Bat_Buoc] [int] NULL,
 CONSTRAINT [PK_LOAI_TAI_LIEU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Menus]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Menus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [varchar](50) NULL,
	[Deleted] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NHAN_VIEN]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NHAN_VIEN](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma] [varchar](50) NULL,
	[Ten_Dang_Nhap] [varchar](50) NULL,
	[Mat_Khau] [varchar](50) NULL,
	[Ho_Ten] [nvarchar](100) NULL,
	[Loai] [int] NULL,
	[Dien_Thoai] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Trang_Thai] [int] NULL,
	[Xoa] [int] NULL,
	[RoleId] [int] NULL,
	[ProvinceId] [int] NULL,
	[DistrictId] [int] NULL,
	[WorkDate] [datetime] NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_NHAN_VIEN] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NHAN_VIEN_CF]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHAN_VIEN_CF](
	[Ma_Nhan_Vien] [int] NOT NULL,
	[Quyen] [int] NOT NULL,
	[Ma_Nhom] [int] NOT NULL,
 CONSTRAINT [PK_NHAN_VIEN_CF] PRIMARY KEY CLUSTERED 
(
	[Ma_Nhan_Vien] ASC,
	[Quyen] ASC,
	[Ma_Nhom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NHAN_VIEN_NHOM]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHAN_VIEN_NHOM](
	[Ma_Nhan_Vien] [int] NOT NULL,
	[Ma_Nhom] [int] NOT NULL,
 CONSTRAINT [PK_NHAN_VIEN_NHOM] PRIMARY KEY CLUSTERED 
(
	[Ma_Nhan_Vien] ASC,
	[Ma_Nhom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NHAN_VIEN_QUYEN]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHAN_VIEN_QUYEN](
	[Ma_NV] [int] NOT NULL,
	[Quyen] [nvarchar](50) NULL,
 CONSTRAINT [PK_QUYEN] PRIMARY KEY CLUSTERED 
(
	[Ma_NV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NHOM]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHOM](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma_Nhom_Cha] [int] NULL,
	[Chuoi_Ma_Cha] [nvarchar](100) NULL,
	[Ma_Nguoi_QL] [int] NULL,
	[Ten_Viet_Tat] [nvarchar](50) NULL,
	[Ten] [nvarchar](200) NULL,
	[Loai] [int] NULL,
 CONSTRAINT [PK_NHOM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Code] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedTime] [datetime] NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RolePermission]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RolePermission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[RoleCode] [varchar](50) NULL,
	[Permission] [varchar](50) NULL,
 CONSTRAINT [PK_ScopeRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SAN_PHAM_VAY]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SAN_PHAM_VAY](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma_Doi_Tac] [int] NULL,
	[Ma] [nvarchar](50) NULL,
	[Ten] [nvarchar](200) NULL,
	[Ngay_Tao] [datetime] NULL,
	[Ma_Nguoi_Tao] [int] NULL,
	[Loai] [int] NULL,
 CONSTRAINT [PK_SAN_PHAM_VAY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TAI_LIEU_HS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAI_LIEU_HS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma_Loai] [int] NULL,
	[Ten] [nvarchar](200) NULL,
	[Duong_Dan_File] [nvarchar](max) NULL,
	[Ma_Ho_So] [int] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_TAI_LIEU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TRANG_THAI_HS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRANG_THAI_HS](
	[ID] [int] NOT NULL,
	[Ten] [nvarchar](200) NULL,
	[Da_Xoa] [bit] NULL,
 CONSTRAINT [PK_TRANG_THAI_HS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrangThaiHoso] [int] NULL,
 CONSTRAINT [PK_TRANGTHAI_HS_IGNORE_TEAMLEAD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoleMenu]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserRoleMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MenuId] [int] NOT NULL,
	[RoleCode] [varchar](50) NULL,
	[RoleId] [int] NULL,
	[MenuName] [nvarchar](100) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserScope]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserScope](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Role] [varchar](50) NULL,
 CONSTRAINT [PK_UserScope] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[AUTOID] ([ID], [Name_ID], [Prefix], [Suffixes], [Value]) VALUES (1, NULL, N'19', N'12', 6)
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [FullName], [Cmnd], [CheckDate], [CICStatus], [Gender], [MatchCondition], [NotMatch], [LastNote], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (1, N'Đinh Quang Phát', N'222222222', CAST(N'2019-11-02 00:00:00.000' AS DateTime), 1, 1, N'TP Bank,Việt Credit', N'Mcredit,Lotte Finance', N'hhhhhhhhhhh', CAST(N'2019-10-31 22:35:41.037' AS DateTime), 1, CAST(N'2019-11-02 15:08:05.353' AS DateTime), 0)
INSERT [dbo].[Customer] ([Id], [FullName], [Cmnd], [CheckDate], [CICStatus], [Gender], [MatchCondition], [NotMatch], [LastNote], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (2, N'Đinh Quang Phát', N'111111111', CAST(N'2019-11-02 00:00:00.000' AS DateTime), 0, 1, N'Mcredit,Việt Credit', N'TP Bank,Lotte Finance', N'hhhhhhhhhhh', CAST(N'2019-10-31 22:44:34.373' AS DateTime), 1, CAST(N'2019-11-02 15:07:59.163' AS DateTime), 0)
INSERT [dbo].[Customer] ([Id], [FullName], [Cmnd], [CheckDate], [CICStatus], [Gender], [MatchCondition], [NotMatch], [LastNote], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (3, N'Đinh Quang Phát', N'333333333', CAST(N'2019-11-02 00:00:00.000' AS DateTime), 2, 1, N'Mcredit', N'TP Bank,Việt Credit,Lotte Finance', N'jjjjjjjjjj', CAST(N'2019-10-31 22:45:34.440' AS DateTime), 1, CAST(N'2019-11-02 15:07:45.570' AS DateTime), 0)
INSERT [dbo].[Customer] ([Id], [FullName], [Cmnd], [CheckDate], [CICStatus], [Gender], [MatchCondition], [NotMatch], [LastNote], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (4, N'Đinh Phát', N'111111122', CAST(N'2019-11-21 00:00:00.000' AS DateTime), 0, 1, NULL, NULL, NULL, CAST(N'2019-11-02 15:10:26.653' AS DateTime), 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Customer] OFF
SET IDENTITY_INSERT [dbo].[CustomerCheck] ON 

INSERT [dbo].[CustomerCheck] ([Id], [CustomerId], [PartnerId], [Status], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (25, 3, 2, 1, CAST(N'2019-11-02 15:07:45.583' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerCheck] ([Id], [CustomerId], [PartnerId], [Status], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (26, 2, 2, 1, CAST(N'2019-11-02 15:07:59.167' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerCheck] ([Id], [CustomerId], [PartnerId], [Status], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (27, 2, 3, 1, CAST(N'2019-11-02 15:07:59.167' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerCheck] ([Id], [CustomerId], [PartnerId], [Status], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (28, 1, 1, 1, CAST(N'2019-11-02 15:08:05.357' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerCheck] ([Id], [CustomerId], [PartnerId], [Status], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (29, 1, 3, 1, CAST(N'2019-11-02 15:08:05.360' AS DateTime), 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[CustomerCheck] OFF
SET IDENTITY_INSERT [dbo].[CustomerNote] ON 

INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (1, 2, NULL, CAST(N'2019-10-31 22:44:34.407' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (2, 3, N'dddddddddddddddddddddddddd', CAST(N'2019-10-31 22:45:34.450' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (3, 3, N'éo cho vay', CAST(N'2019-11-01 23:51:03.083' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (4, 3, N'sssss', CAST(N'2019-11-02 14:39:23.053' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (5, 3, N'hhhhhhhhhhh', CAST(N'2019-11-02 14:39:55.883' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (6, 3, N'aaaaaaaaaaaaaaa', CAST(N'2019-11-02 14:40:18.437' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[CustomerNote] ([Id], [CustomerId], [Note], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (7, 3, N'jjjjjjjjjj', CAST(N'2019-11-02 14:40:28.010' AS DateTime), 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[CustomerNote] OFF
SET IDENTITY_INSERT [dbo].[DOI_TAC] ON 

INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (1, N'TP Bank', 1, 3)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (2, N'Mcredit', 1, NULL)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (3, N'Việt Credit', 1, NULL)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (4, N'Lotte Finance', 1, NULL)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (5, N'Mirae', NULL, NULL)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (6, N'FE Credit', NULL, NULL)
INSERT [dbo].[DOI_TAC] ([ID], [Ten], [AllowCheckDup], [F88Value]) VALUES (7, N'Giới thiệu khách hàng', NULL, NULL)
SET IDENTITY_INSERT [dbo].[DOI_TAC] OFF
SET IDENTITY_INSERT [dbo].[Ghichu] ON 

INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (1, 19, N'test ghi chuq', 12, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (2, 19, N'ghi chú 2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222', 12, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (3, 1, N'sdfsdf', 12, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (4, 19, N'ghi chú 1', 8, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (5, 19, N'ghi chú 2', 8, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (6, 1, N'ghi cái éo gì đây', 8, NULL)
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (7, 21, N'csdc', 19, CAST(N'2019-06-24 15:50:42.393' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (8, 21, N'dcsdc', 19, CAST(N'2019-06-24 16:09:51.813' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (9, 21, N'fffffffffffffff', 19, CAST(N'2019-06-24 16:10:01.977' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (10, 21, N'ddddddddddddd', 22, CAST(N'2019-06-24 16:25:54.140' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (12, 21, N'hồ sơ đầy đủ', 19, CAST(N'2019-06-24 19:22:20.777' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (13, 21, N'thieu file hinh kh', 23, CAST(N'2019-06-24 20:02:47.273' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (14, 21, N'tesst', 23, CAST(N'2019-06-24 20:32:21.117' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (15, 22, N'bs day du', 23, CAST(N'2019-06-24 20:33:06.583' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (18, 21, N'awzesxdcfgvgbh', 26, CAST(N'2019-06-26 20:07:13.337' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (19, 21, N'ghi chú 2', 26, CAST(N'2019-06-26 20:21:14.690' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (20, 1, N'ressssssssssssssssssssssssssssssssressssssssssssssssssssssssssssssss', 26, CAST(N'2019-06-29 17:37:49.790' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (21, 1, N'xxx', 26, CAST(N'2019-06-29 17:39:48.340' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (22, 1, N'ressssssssssssssssssssssssssssssssressssssssssssssssssssssssssssssss', 26, CAST(N'2019-06-29 17:44:23.917' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (23, 1, N'ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss', 27, CAST(N'2019-09-07 13:21:08.910' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (24, 1, N'ghi chú 1', 1045, CAST(N'2019-10-20 20:29:07.443' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (25, 1, N'ghi chú 1', 1045, CAST(N'2019-10-20 20:30:10.357' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (26, 1, N'ghi chú 1', 1045, CAST(N'2019-10-20 20:56:37.287' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (28, 1, N'xxxxxxxxxxxx', 1088, CAST(N'2019-10-23 21:40:41.793' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (29, 1, N'dddddddddd', 1089, CAST(N'2019-10-23 21:41:29.023' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (30, 1, N'xxxxxxxxxxxxxxx', 1090, CAST(N'2019-10-23 21:42:04.900' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (32, 1, N'ssssssssssssss', 1092, CAST(N'2019-10-23 21:43:05.347' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (33, 1, N'ccccccccccccccccc', 1092, CAST(N'2019-10-23 21:43:25.630' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (34, 1, N'hhhhhhhhhhhhhh', 1092, CAST(N'2019-10-23 21:43:32.923' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (37, 1, N'dddddd', 1110, CAST(N'2019-11-23 21:28:19.387' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (38, 1, N'ggggggggggg', 1133, CAST(N'2019-11-24 22:03:06.953' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (11, 1, N'dddddddddddddaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', 22, CAST(N'2019-06-24 18:14:56.673' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (39, 1, N'hhhhhhhhhhhhhhhhh', 1133, CAST(N'2019-11-24 22:04:10.130' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (40, 1, N'ffffffffffffff', 1162, CAST(N'2019-11-26 21:38:31.567' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (16, 1, N'zx zx', 4, CAST(N'2019-06-26 19:56:36.003' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (17, 1, N'ascssadc', 4, CAST(N'2019-06-26 19:59:00.707' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (27, 1, N'vfdfvd', 1087, CAST(N'2019-10-23 21:39:49.230' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (31, 1, N'ccccccccccccccccccc', 1091, CAST(N'2019-10-23 21:42:34.867' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (35, 1, N'cccccccccccccccccc', 1093, CAST(N'2019-10-23 21:44:02.210' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (36, 1, N'sssssssssssssssssss', 1093, CAST(N'2019-10-23 21:44:08.073' AS DateTime))
INSERT [dbo].[Ghichu] ([Id], [UserId], [Noidung], [HosoId], [CommentTime]) VALUES (41, 1, N'hhhhhhhhhhhhhhh', 1162, CAST(N'2019-11-26 21:38:44.617' AS DateTime))
SET IDENTITY_INSERT [dbo].[Ghichu] OFF
SET IDENTITY_INSERT [dbo].[HO_SO] ON 

INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (4, N'1904000005', NULL, N'TEST 1', N'121123123', N'Bến Nghé', 2, N'0349149240', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 1, 1, CAST(N'2019-06-26 19:59:00.693' AS DateTime), 1, CAST(N'2019-05-05 00:00:00.000' AS DateTime), 3, 1, 1, N'BBC', 0, CAST(5500000 AS Decimal(18, 0)), 6, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (5, N'1904000006', NULL, N'Đỗ Trung Vinh', N'215148870', N'30/20', 2, N'0349149240', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 1, 1, CAST(N'2019-05-10 23:12:01.280' AS DateTime), NULL, NULL, 0, 1, 1, N'', 1, CAST(2 AS Decimal(18, 0)), 0, NULL, NULL, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (8, N'1905000030', NULL, N'TEST', N'123456789', N'123', 2, N'1234567890', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 11, 11, CAST(N'2019-06-22 19:49:25.020' AS DateTime), 1, CAST(N'2019-05-18 00:00:00.000' AS DateTime), 4, 1, 1, N'', 0, CAST(12 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (9, N'1905000031', NULL, N'TEST', N'123456789', N'12345', 2, N'1234567890', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 1, 1, CAST(N'2019-06-22 18:26:56.920' AS DateTime), 1, CAST(N'2019-05-18 00:00:00.000' AS DateTime), 8, 1, 1, N'', 0, CAST(1235666 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (10, N'1905000032', NULL, N'Test1', N'', N'', 0, N'', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 2, 2, CAST(N'2019-05-19 14:11:56.103' AS DateTime), NULL, CAST(N'2019-05-19 00:00:00.000' AS DateTime), 0, 1, 0, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (11, N'1905000033', NULL, N'Test1', N'222', N'', 0, N'222', N'222', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 2, 2, CAST(N'2019-05-19 14:29:12.803' AS DateTime), NULL, CAST(N'2019-05-19 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(1000000 AS Decimal(18, 0)), 3, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (12, N'1906000001', NULL, N'WEDW', N'234234342', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'0198899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 2, 2, CAST(N'2019-06-24 19:55:17.990' AS DateTime), 1, CAST(N'2019-06-20 00:00:00.000' AS DateTime), 7, 1, 33, N'', 0, CAST(33333 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (13, N'1906000002', NULL, N'ĐINH QUANG PHÁT', N'124121314', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 38, N'0198899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 19, 19, CAST(N'2019-06-21 10:44:32.013' AS DateTime), NULL, CAST(N'2019-06-21 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(3000 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (14, N'1906000003', NULL, N'ĐINH QUANG PHÁT', N'123143144', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 38, N'0698899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 3, 3, CAST(N'2019-06-22 18:26:23.167' AS DateTime), 1, CAST(N'2019-06-22 00:00:00.000' AS DateTime), 8, 1, 18, N'', 0, CAST(30000 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (15, N'1906000004', NULL, N'ĐINH QUANG PHÁT', N'121312312', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'0198899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 20, 20, CAST(N'2019-06-22 17:07:59.923' AS DateTime), NULL, CAST(N'2019-06-11 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(2222 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (16, N'1906000005', NULL, N'ĐINH QUANG PHÁT', N'232321323', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 38, N'0168899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 20, 20, CAST(N'2019-06-22 17:11:09.527' AS DateTime), NULL, CAST(N'2019-06-22 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(2222 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (17, N'1906000006', NULL, N'ĐINH QUANG PHÁT', N'121312323', N'182 lê đại hành', 2, N'0198899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 19, 19, CAST(N'2019-06-22 22:39:26.203' AS DateTime), NULL, CAST(N'2019-06-10 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(1122 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (18, N'1906000007', NULL, N'ĐINH QUANG PHÁT', N'232323333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'0168899999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 19, 19, CAST(N'2019-06-22 22:45:39.857' AS DateTime), NULL, CAST(N'2019-06-22 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(23232 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (19, N'1906000008', NULL, N'ĐINH QUANG PHÁT', N'321321321', N'12', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 21, 22, CAST(N'2019-06-24 19:22:20.733' AS DateTime), 21, CAST(N'2019-06-23 00:00:00.000' AS DateTime), 6, 1, 2, N'', 0, CAST(12 AS Decimal(18, 0)), 12, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (20, N'1906000009', NULL, N'ĐINH QUANG PHÁT 1', N'123456789', N'1', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 22, 22, CAST(N'2019-06-23 17:37:25.177' AS DateTime), 1, CAST(N'2019-06-23 00:00:00.000' AS DateTime), 2, 1, 16, N'', 0, CAST(12 AS Decimal(18, 0)), 12, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (21, N'1906000010', NULL, N'ĐINH QUANG PHÁT', N'234234232', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 21, 21, CAST(N'2019-06-24 16:25:25.993' AS DateTime), NULL, CAST(N'2019-06-24 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(1222 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (22, N'1906000011', NULL, N'ĐINH QUANG PHÁT', N'234234232', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 21, 21, CAST(N'2019-06-24 18:14:56.670' AS DateTime), 1, CAST(N'2019-06-24 00:00:00.000' AS DateTime), 4, 1, 16, N'', 0, CAST(2222 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (23, N'1906000012', NULL, N'KH MOI', N'123123123', N'182 lê đại hành', 2, N'0398899548', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 22, 22, CAST(N'2019-06-24 20:33:06.580' AS DateTime), 22, CAST(N'2019-06-24 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(12 AS Decimal(18, 0)), 12, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (24, N'1906000013', NULL, N'ĐINH QUANG PHÁT', N'121332121', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 22, 22, CAST(N'2019-06-26 20:04:44.503' AS DateTime), 1, CAST(N'2019-06-25 00:00:00.000' AS DateTime), 3, 1, 16, N'', 0, CAST(2222 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (25, N'1906000014', NULL, N'ĐINH QUANG PHÁT', N'32423423', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 22, 22, CAST(N'2019-06-25 21:59:00.387' AS DateTime), NULL, CAST(N'2019-06-25 00:00:00.000' AS DateTime), 0, 1, 16, N'', 0, CAST(2222 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (26, N'1906000015', NULL, N'ĐINH QUANG ', N'123123123', N'1', 2, N'0169889999', N'', 1, CAST(N'2019-08-30 19:38:02.120' AS DateTime), 22, 22, CAST(N'2019-07-01 20:30:52.863' AS DateTime), 1, CAST(N'2019-06-26 00:00:00.000' AS DateTime), 4, 1, 1, N'', 0, CAST(1 AS Decimal(18, 0)), 12, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (27, N'1909000001', NULL, N'ĐINH QUANG PHÁT', N'355555545', N'26 lê duy nhuận', 2, N'0398899549', N'', 1, CAST(N'2019-09-01 21:35:12.257' AS DateTime), 1, 1, CAST(N'2019-09-07 13:21:08.907' AS DateTime), 1, CAST(N'2019-09-01 00:00:00.000' AS DateTime), 2, 1, 1, N'', 0, CAST(333 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (28, N'1910000001', NULL, N'QWWWWWWWWWWW', N'234234234', N'26 lê duy nhuận', 2, N'2342342323', N'', 1, CAST(N'2019-10-13 23:58:42.213' AS DateTime), 1, 1, CAST(N'2019-10-19 15:59:35.793' AS DateTime), 1, CAST(N'2019-10-13 00:00:00.000' AS DateTime), 4, 1, 1, N'', 0, CAST(3333 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1028, N'1910000002', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'3333333', 2, N'0498899999', N'', 1, CAST(N'2019-10-19 16:03:57.533' AS DateTime), 1, 1, CAST(N'2019-10-19 16:03:57.533' AS DateTime), NULL, CAST(N'2019-10-19 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1029, N'1910000003', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'222', 37, N'0698899999', N'', 1, CAST(N'2019-10-19 16:06:57.160' AS DateTime), 1, 1, CAST(N'2019-10-19 16:05:19.713' AS DateTime), NULL, CAST(N'2019-10-19 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1030, N'1910000004', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'2', 2, N'0198899999', N'', 1, CAST(N'2019-10-19 16:18:01.940' AS DateTime), 1, 1, CAST(N'2019-10-19 16:17:31.633' AS DateTime), NULL, CAST(N'2019-10-19 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1031, N'1910000005', NULL, N'ĐINH QUANG PHÁT', N'22222222', N'26 lê duy nhuận,P12, Tân bình', 2, N'0698899999', N'', 1, CAST(N'2019-10-20 01:29:40.287' AS DateTime), 1, 1, CAST(N'2019-10-20 01:28:24.457' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 17, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1032, N'1910000006', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 01:49:54.563' AS DateTime), 1, 1, CAST(N'2019-10-20 01:49:54.563' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1033, N'1910000007', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0698899999', N'', 1, CAST(N'2019-10-20 02:14:37.420' AS DateTime), 1, 1, CAST(N'2019-10-20 02:14:37.420' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1034, N'1910000008', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 701, N'0169889999', N'', 1, CAST(N'2019-10-20 02:17:18.763' AS DateTime), 1, 1, CAST(N'2019-11-09 23:52:49.997' AS DateTime), 1, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 2, 1, 2, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1035, N'1910000009', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:46:24.387' AS DateTime), 0, 0, CAST(N'2019-10-20 04:46:24.387' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1036, N'1910000009', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:46:39.533' AS DateTime), 0, 0, CAST(N'2019-10-20 04:46:39.533' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1037, N'1910000009', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:48:42.340' AS DateTime), 0, 0, CAST(N'2019-10-20 04:48:42.340' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1038, N'1910000009', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:49:37.430' AS DateTime), 0, 0, CAST(N'2019-10-20 04:49:37.430' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1039, N'1910000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:51:45.967' AS DateTime), 0, 0, CAST(N'2019-10-20 04:51:45.967' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1040, N'1910000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:52:06.420' AS DateTime), 0, 0, CAST(N'2019-10-20 04:52:06.420' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1041, N'1910000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:53:19.080' AS DateTime), 0, 0, CAST(N'2019-10-20 04:53:19.080' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1042, N'1910000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:55:49.373' AS DateTime), 0, 0, CAST(N'2019-10-20 04:55:49.373' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1043, N'1910000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 04:57:16.770' AS DateTime), 0, 0, CAST(N'2019-10-20 04:57:16.770' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1044, N'1910000011', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0168899999', N'', 1, CAST(N'2019-10-20 20:22:48.850' AS DateTime), 0, 0, CAST(N'2019-10-20 20:22:48.850' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1045, N'1910000012', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0198899999', N'', 1, CAST(N'2019-10-20 20:56:37.250' AS DateTime), 1, 1, CAST(N'2019-10-20 20:29:00.613' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1046, N'1910000013', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0168899999', N'', 1, CAST(N'2019-10-20 21:00:43.623' AS DateTime), 0, 0, CAST(N'2019-10-20 21:00:43.623' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1047, N'1910000014', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0198899999', N'', 1, CAST(N'2019-10-20 21:01:21.077' AS DateTime), 0, 0, CAST(N'2019-10-20 21:01:21.077' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1048, N'1910000015', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0698899999', N'', 1, CAST(N'2019-10-20 21:05:19.830' AS DateTime), 0, 0, CAST(N'2019-10-20 21:05:19.830' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1049, N'1910000016', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0698899999', N'', 1, CAST(N'2019-10-20 21:07:33.657' AS DateTime), 0, 0, CAST(N'2019-10-20 21:07:33.657' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1050, N'1910000017', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 21:23:19.660' AS DateTime), 0, 0, CAST(N'2019-10-20 21:23:19.660' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1051, N'1910000018', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 21:46:03.433' AS DateTime), 1, 1, CAST(N'2019-10-20 21:46:03.433' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1052, N'1910000019', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-20 21:46:49.060' AS DateTime), 1, 1, CAST(N'2019-10-20 21:46:49.060' AS DateTime), NULL, CAST(N'2019-10-20 00:00:00.000' AS DateTime), 0, 1, 2, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1053, N'1910000020', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 20:26:06.387' AS DateTime), 0, 0, CAST(N'2019-10-21 20:26:06.387' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 2, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1054, N'1910000021', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'0168899999', N'', 1, CAST(N'2019-10-21 20:28:21.133' AS DateTime), 0, 0, CAST(N'2019-10-21 20:28:21.133' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 2, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1055, N'1910000022', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 20:31:02.127' AS DateTime), 0, 0, CAST(N'2019-10-21 20:31:02.127' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 17, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1056, N'1910000023', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:01:23.033' AS DateTime), 0, 0, CAST(N'2019-10-21 21:01:23.033' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1057, N'1910000024', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:03:37.460' AS DateTime), 0, 0, CAST(N'2019-10-21 21:03:37.460' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1058, N'1910000025', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:12:36.190' AS DateTime), 0, 0, CAST(N'2019-10-21 21:12:36.190' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1059, N'1910000026', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:13:29.453' AS DateTime), 0, 0, CAST(N'2019-10-21 21:13:29.453' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1060, N'1910000027', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:14:22.617' AS DateTime), 0, 0, CAST(N'2019-10-21 21:14:22.617' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1061, N'1910000028', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:15:33.247' AS DateTime), 0, 0, CAST(N'2019-10-21 21:15:33.247' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1062, N'1910000029', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:16:13.243' AS DateTime), 0, 0, CAST(N'2019-10-21 21:16:13.243' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1063, N'1910000030', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:17:39.457' AS DateTime), 0, 0, CAST(N'2019-10-21 21:17:39.457' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1064, N'1910000031', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:19:14.407' AS DateTime), 0, 0, CAST(N'2019-10-21 21:19:14.407' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1065, N'1910000032', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:19:45.687' AS DateTime), 0, 0, CAST(N'2019-10-21 21:19:45.687' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1066, N'1910000033', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:22:49.973' AS DateTime), 0, 0, CAST(N'2019-10-21 21:22:49.973' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1067, N'1910000034', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:26:10.853' AS DateTime), 0, 0, CAST(N'2019-10-21 21:26:10.853' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1068, N'1910000035', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:27:46.223' AS DateTime), 0, 0, CAST(N'2019-10-21 21:27:46.223' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1069, N'1910000036', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:29:04.507' AS DateTime), 0, 0, CAST(N'2019-10-21 21:29:04.507' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1070, N'1910000037', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:29:47.280' AS DateTime), 0, 0, CAST(N'2019-10-21 21:29:47.280' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1071, N'1910000038', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:30:55.150' AS DateTime), 0, 0, CAST(N'2019-10-21 21:30:55.150' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1072, N'1910000039', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:32:27.637' AS DateTime), 0, 0, CAST(N'2019-10-21 21:32:27.637' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1073, N'1910000040', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:37:35.687' AS DateTime), 0, 0, CAST(N'2019-10-21 21:37:35.687' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1074, N'1910000041', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 21:38:50.670' AS DateTime), 0, 0, CAST(N'2019-10-21 21:38:50.670' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1075, N'1910000042', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:01:21.793' AS DateTime), 0, 0, CAST(N'2019-10-21 22:01:21.793' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1076, N'1910000043', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:02:27.937' AS DateTime), 0, 0, CAST(N'2019-10-21 22:02:27.937' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1077, N'1910000044', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:03:34.047' AS DateTime), 0, 0, CAST(N'2019-10-21 22:03:34.047' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1078, N'1910000045', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:05:42.507' AS DateTime), 0, 0, CAST(N'2019-10-21 22:05:42.507' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1079, N'1910000046', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:06:40.647' AS DateTime), 0, 0, CAST(N'2019-10-21 22:06:40.647' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1080, N'1910000047', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:10:20.117' AS DateTime), 0, 0, CAST(N'2019-10-21 22:10:20.117' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1081, N'1910000048', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:10:53.070' AS DateTime), 0, 0, CAST(N'2019-10-21 22:10:53.070' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1082, N'1910000049', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:16:04.953' AS DateTime), 0, 0, CAST(N'2019-10-21 22:16:04.953' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1083, N'1910000050', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 22:45:03.690' AS DateTime), 0, 0, CAST(N'2019-10-21 22:45:03.690' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1084, N'1910000051', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 23:23:21.203' AS DateTime), 1, 1, CAST(N'2019-10-21 23:02:11.980' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1085, N'1910000052', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 23:14:21.360' AS DateTime), 0, 0, CAST(N'2019-10-21 23:14:21.360' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1086, N'1910000053', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-21 23:27:20.867' AS DateTime), 0, 0, CAST(N'2019-10-21 23:15:22.453' AS DateTime), NULL, CAST(N'2019-10-21 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1087, N'1910000054', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:39:49.187' AS DateTime), 0, 0, CAST(N'2019-10-23 21:39:49.187' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1088, N'1910000055', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:40:41.790' AS DateTime), 0, 0, CAST(N'2019-10-23 21:40:41.790' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1089, N'1910000056', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:41:29.017' AS DateTime), 0, 0, CAST(N'2019-10-23 21:41:29.017' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1090, N'1910000057', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:42:04.897' AS DateTime), 0, 0, CAST(N'2019-10-23 21:42:04.897' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1091, N'1910000058', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:42:34.863' AS DateTime), 0, 0, CAST(N'2019-10-23 21:42:34.863' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1092, N'1910000059', NULL, N'FFFFFFFFFFFFFF', N'', N'', 0, N'', N'', 1, CAST(N'2019-10-23 21:43:32.923' AS DateTime), 0, 0, CAST(N'2019-10-23 21:43:05.340' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1093, N'1910000060', NULL, N'XXXXXXXXXXXXX', N'', N'', 0, N'', N'', 1, CAST(N'2019-10-23 21:44:08.070' AS DateTime), 0, 0, CAST(N'2019-10-23 21:44:02.207' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, NULL, 0, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1094, N'1910000061', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'01698899999', N'', 1, CAST(N'2019-10-23 21:46:18.417' AS DateTime), 1, 1, CAST(N'2019-10-23 21:46:13.700' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 0, 1, 18, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1095, N'1910000062', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 39, N'0169889999', N'', 1, CAST(N'2019-10-23 21:52:52.640' AS DateTime), 1, 1, CAST(N'2019-11-09 23:48:49.497' AS DateTime), 1, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 6, 1, 16, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1096, N'1910000063', NULL, N'ĐINH QUANG PHÁT', N'232433433', N'26 lê duy nhuận,P12, Tân bình', 37, N'0698899999', N'', 1, CAST(N'2019-10-23 22:58:04.760' AS DateTime), 1, 1, CAST(N'2019-10-23 22:58:04.767' AS DateTime), 0, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1097, N'1910000064', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'26 lê duy nhuận', 2, N'0169889999', N'', 1, CAST(N'2019-10-23 23:23:23.410' AS DateTime), 1, 1, CAST(N'2019-10-23 23:23:23.410' AS DateTime), NULL, CAST(N'2019-10-23 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1098, N'1910000065', NULL, N'ĐINH QUANG PHÁT', N'123212132', N'222222', 2, N'016989999', N'', 1, CAST(N'2019-10-27 14:18:21.597' AS DateTime), 0, 0, CAST(N'2019-10-27 14:18:21.597' AS DateTime), NULL, CAST(N'2019-10-27 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1099, N'1910000066', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'26 lê duy nhuận,P12, Tân bình', 37, N'0169889999', N'', 1, CAST(N'2019-10-27 14:19:06.227' AS DateTime), 1, 1, CAST(N'2019-10-27 14:19:06.227' AS DateTime), NULL, CAST(N'2019-10-27 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(2 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1101, N'1910000539', NULL, N'PHUNG VINH TOAN', N'123456789', N'216 nmh', 18, N'0939672144', N'', 1, CAST(N'2019-10-11 09:52:55.600' AS DateTime), 1, 1, CAST(N'2019-10-11 09:55:08.503' AS DateTime), 1, CAST(N'2019-10-11 00:00:00.000' AS DateTime), 7, 1, 1, N'', 0, CAST(100000000 AS Decimal(18, 0)), 0, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1103, N'1910000645', NULL, N'TEDT', N'341298062', N'Djdjdj', 1587, N'0938068736', N'', 1, CAST(N'2019-10-14 10:35:29.337' AS DateTime), 1, 1, CAST(N'2019-10-14 16:46:08.753' AS DateTime), 1870, CAST(N'2019-10-14 00:00:00.000' AS DateTime), 7, 1, 31, N'', 0, CAST(30000000 AS Decimal(18, 0)), 24, N'', 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1104, N'1911000001', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1111111111', N'2222222222', 1, CAST(N'2019-11-10 22:55:01.867' AS DateTime), 0, 0, CAST(N'2019-11-10 22:55:01.867' AS DateTime), NULL, CAST(N'2019-11-10 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1105, N'1911000002', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-10 22:57:08.033' AS DateTime), 0, 0, CAST(N'2019-11-10 22:57:08.033' AS DateTime), NULL, CAST(N'2019-11-10 00:00:00.000' AS DateTime), 0, NULL, 2, NULL, 0, CAST(7 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1106, N'1911000003', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-10 23:08:19.910' AS DateTime), 0, 0, CAST(N'2019-11-10 23:08:19.910' AS DateTime), NULL, CAST(N'2019-11-10 00:00:00.000' AS DateTime), 0, NULL, 2, NULL, 0, CAST(8 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1107, N'1911000004', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-10 23:09:19.880' AS DateTime), 0, 0, CAST(N'2019-11-10 23:09:19.880' AS DateTime), NULL, CAST(N'2019-11-10 00:00:00.000' AS DateTime), 0, NULL, 2, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1108, N'1911000005', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-10 23:11:15.223' AS DateTime), 0, 0, CAST(N'2019-11-10 23:11:15.223' AS DateTime), NULL, CAST(N'2019-11-10 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1109, N'1911000006', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 21:13:53.810' AS DateTime), 1, 1, CAST(N'2019-11-11 21:13:53.810' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, 1, 2, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1110, N'1911000007', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'182 lê đại hành', 2, N'0398899548', N'', 1, CAST(N'2019-11-16 12:50:25.307' AS DateTime), 1, 1, CAST(N'2019-11-23 23:55:00.950' AS DateTime), 1, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 2, 1, 16, N'', 0, CAST(4 AS Decimal(18, 0)), 0, N'', 0, 0, CAST(N'2019-11-16 00:00:00.000' AS DateTime), CAST(N'2019-11-16 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1111, N'1911000008', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 22:07:01.577' AS DateTime), 0, 0, CAST(N'2019-11-11 22:07:01.577' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1112, N'1911000009', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 22:08:13.820' AS DateTime), 0, 0, CAST(N'2019-11-11 22:08:13.820' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1113, N'1911000010', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 22:10:42.547' AS DateTime), 0, 0, CAST(N'2019-11-11 22:10:42.547' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1114, N'1911000011', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 22:30:31.923' AS DateTime), 0, 0, CAST(N'2019-11-11 22:30:31.923' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1115, N'1911000012', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-11 22:31:15.967' AS DateTime), 0, 0, CAST(N'2019-11-11 22:31:15.967' AS DateTime), NULL, CAST(N'2019-11-11 00:00:00.000' AS DateTime), 0, NULL, 17, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1116, N'1911000013', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 37, N'3333333333', N'2222333333', 1, CAST(N'2019-11-12 22:27:51.727' AS DateTime), 1, 1, CAST(N'2019-11-12 22:27:51.727' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1117, N'1911000014', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 701, N'2222222222', N'3333333333', 1, CAST(N'2019-11-12 22:41:04.120' AS DateTime), 1, 1, CAST(N'2019-11-12 22:41:04.120' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1118, N'1911000015', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'26 lê duy nhuận', 4, N'1111111111', N'', 1, CAST(N'2019-11-12 22:46:24.673' AS DateTime), 1, 1, CAST(N'2019-11-12 22:46:24.673' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1119, N'1911000016', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'26 lê duy nhuận', 38, N'2222222222', N'2222222222', 1, CAST(N'2019-11-12 23:03:45.517' AS DateTime), 1, 1, CAST(N'2019-11-12 23:03:45.517' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1120, N'1911000017', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 38, N'2222222222', N'2222222222', 1, CAST(N'2019-11-12 23:10:11.277' AS DateTime), 1, 1, CAST(N'2019-11-12 23:10:11.277' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1121, N'1911000018', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'2222222222', N'2222222222', 1, CAST(N'2019-11-12 23:24:10.163' AS DateTime), 1, 1, CAST(N'2019-11-12 23:24:10.163' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1122, N'1911000019', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'1698899999', N'2222222222', 1, CAST(N'2019-11-12 23:28:53.707' AS DateTime), 1, 1, CAST(N'2019-11-12 23:28:53.707' AS DateTime), NULL, CAST(N'2019-11-12 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1123, N'1911000020', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'', 1, CAST(N'2019-11-14 19:57:01.203' AS DateTime), 1, 1, CAST(N'2019-11-14 19:57:01.200' AS DateTime), 0, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 30, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1124, N'1911000021', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 37, N'1698899999', N'', 1, CAST(N'2019-11-14 19:59:38.690' AS DateTime), 1, 1, CAST(N'2019-11-14 19:59:38.690' AS DateTime), NULL, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1125, N'1911000022', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'26 lê duy nhuận', 3, N'1698899999', N'1111111111', 1, CAST(N'2019-11-14 22:25:09.197' AS DateTime), 1, 1, CAST(N'2019-11-14 22:25:09.203' AS DateTime), 0, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1126, N'1911000023', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'26 lê duy nhuận', 2, N'1698899999', N'1111111111', 1, CAST(N'2019-11-14 22:27:39.577' AS DateTime), 1, 1, CAST(N'2019-11-14 22:27:39.577' AS DateTime), NULL, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1127, N'1911000024', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 4, N'1698899999', N'', 1, CAST(N'2019-11-14 22:41:14.750' AS DateTime), 1, 1, CAST(N'2019-11-14 22:41:14.750' AS DateTime), NULL, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1128, N'1911000025', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'26 lê duy nhuận', 2, N'1698899999', N'', 1, CAST(N'2019-11-14 22:45:34.507' AS DateTime), 1, 1, CAST(N'2019-11-14 22:45:34.507' AS DateTime), 0, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1129, N'1911000026', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'26 lê duy nhuận', 37, N'1698899999', N'', 1, CAST(N'2019-11-14 22:52:27.603' AS DateTime), 1, 1, CAST(N'2019-11-14 22:52:27.610' AS DateTime), 0, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1130, N'1911000027', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'1698899999', N'', 1, CAST(N'2019-11-14 22:55:41.340' AS DateTime), 1, 1, CAST(N'2019-11-14 22:55:41.340' AS DateTime), NULL, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 16, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1131, N'1911000028', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 37, N'1698899999', N'', 1, CAST(N'2019-11-14 23:09:07.133' AS DateTime), 1, 1, CAST(N'2019-11-14 23:09:07.133' AS DateTime), NULL, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1132, N'1911000029', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899996', N'', 1, CAST(N'2019-11-14 23:35:38.527' AS DateTime), 1, 1, CAST(N'2019-11-14 23:35:38.530' AS DateTime), 0, CAST(N'2019-11-14 00:00:00.000' AS DateTime), 1, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1133, N'1911000030', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'', 1, CAST(N'2019-11-15 00:02:55.527' AS DateTime), 1, 1, CAST(N'2019-11-24 22:04:10.123' AS DateTime), 1, CAST(N'2019-11-15 00:00:00.000' AS DateTime), 2, 2, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, N'', 0, 0, CAST(N'2019-11-24 22:04:10.110' AS DateTime), CAST(N'2019-11-24 22:04:10.110' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1134, N'1911000031', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-16 21:34:21.750' AS DateTime), 0, 0, CAST(N'2019-11-16 21:34:21.750' AS DateTime), NULL, CAST(N'2019-11-16 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-16 21:34:21.753' AS DateTime), CAST(N'2019-11-16 21:34:21.753' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1135, N'1911000032', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-16 21:37:50.940' AS DateTime), 0, 0, CAST(N'2019-11-16 21:37:50.940' AS DateTime), NULL, CAST(N'2019-11-16 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-16 21:37:50.940' AS DateTime), CAST(N'2019-11-16 21:37:50.940' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1136, N'1911000033', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-16 21:39:06.707' AS DateTime), 0, 0, CAST(N'2019-11-16 21:39:06.707' AS DateTime), NULL, CAST(N'2019-11-16 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-16 21:39:06.707' AS DateTime), CAST(N'2019-11-16 21:39:06.707' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1137, N'1911000034', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-16 21:48:53.027' AS DateTime), 0, 0, CAST(N'2019-11-16 21:48:53.027' AS DateTime), NULL, CAST(N'2019-11-16 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-16 21:48:53.030' AS DateTime), CAST(N'2019-11-16 21:48:53.030' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1138, N'1911000035', NULL, N'ĐINH QUANG PHÁT', N'333333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-23 19:07:49.753' AS DateTime), 0, 0, CAST(N'2019-11-23 19:07:49.753' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 17, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:07:49.757' AS DateTime), CAST(N'2019-11-23 19:07:49.757' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1139, N'1911000036', NULL, N'ĐINH QUANG PHÁT', N'333333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-23 19:09:32.900' AS DateTime), 0, 0, CAST(N'2019-11-23 19:09:32.900' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 17, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:09:32.900' AS DateTime), CAST(N'2019-11-23 19:09:32.900' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1140, N'1911000037', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-23 19:10:25.233' AS DateTime), 0, 0, CAST(N'2019-11-23 19:10:25.233' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:10:25.233' AS DateTime), CAST(N'2019-11-23 19:10:25.233' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1141, N'1911000038', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-23 19:12:33.997' AS DateTime), 0, 0, CAST(N'2019-11-23 19:12:33.997' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:12:33.997' AS DateTime), CAST(N'2019-11-23 19:12:33.997' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1142, N'1911000039', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-23 19:13:52.903' AS DateTime), 0, 0, CAST(N'2019-11-23 19:13:52.903' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 16, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:13:52.903' AS DateTime), CAST(N'2019-11-23 19:13:52.903' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1143, N'1911000040', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-23 19:15:21.807' AS DateTime), 0, 0, CAST(N'2019-11-23 19:15:21.807' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:15:21.807' AS DateTime), CAST(N'2019-11-23 19:15:21.807' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1144, N'1911000041', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-23 19:20:25.067' AS DateTime), 0, 0, CAST(N'2019-11-23 19:20:25.067' AS DateTime), NULL, CAST(N'2019-11-23 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-23 19:20:25.070' AS DateTime), CAST(N'2019-11-23 19:20:25.070' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1145, N'1911000042', NULL, N'ĐINH QUANG PHÁT', N'111111111', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 37, N'1698899999', N'5555555555', 1, CAST(N'2019-11-26 12:41:43.297' AS DateTime), 1, 1, CAST(N'2019-11-26 21:59:44.920' AS DateTime), 1, CAST(N'2019-11-24 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-24 00:00:00.000' AS DateTime), CAST(N'2019-11-24 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1146, N'1911000043', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-24 22:26:10.030' AS DateTime), 0, 0, CAST(N'2019-11-24 22:26:10.030' AS DateTime), NULL, CAST(N'2019-11-24 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-24 22:26:10.033' AS DateTime), CAST(N'2019-11-24 22:26:10.033' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1147, N'1911000044', NULL, N'ĐINH QUANG PHÁT', N'433333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-24 23:46:37.593' AS DateTime), 0, 0, CAST(N'2019-11-24 23:46:37.593' AS DateTime), NULL, CAST(N'2019-11-25 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-24 23:46:37.593' AS DateTime), CAST(N'2019-11-24 23:46:37.593' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1148, N'1911000045', NULL, N'ĐINH QUANG PHÁT', N'433333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-24 23:48:18.357' AS DateTime), 0, 0, CAST(N'2019-11-24 23:48:18.357' AS DateTime), NULL, CAST(N'2019-11-25 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-24 23:48:18.357' AS DateTime), CAST(N'2019-11-24 23:48:18.357' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1149, N'1911000046', NULL, N'ĐINH QUANG PHÁT', N'433333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-24 23:55:42.400' AS DateTime), 0, 0, CAST(N'2019-11-24 23:55:42.400' AS DateTime), NULL, CAST(N'2019-11-25 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-24 23:55:50.847' AS DateTime), CAST(N'2019-11-24 23:55:51.063' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1150, N'1911000047', NULL, N'ĐINH QUANG PHÁT', N'433333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-25 00:01:02.840' AS DateTime), 0, 0, CAST(N'2019-11-25 00:01:02.840' AS DateTime), NULL, CAST(N'2019-11-25 00:00:00.000' AS DateTime), 0, NULL, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-25 00:01:03.743' AS DateTime), CAST(N'2019-11-25 00:01:03.743' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1151, N'1911000048', NULL, N'ĐINH QUANG PHÁT', N'433333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'3333333333', 1, CAST(N'2019-11-25 00:02:52.837' AS DateTime), 1, 1, CAST(N'2019-11-25 00:02:52.837' AS DateTime), NULL, CAST(N'2019-11-25 00:00:00.000' AS DateTime), 1, 1, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-25 00:02:54.023' AS DateTime), CAST(N'2019-11-25 00:02:54.023' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1152, N'1911000049', NULL, N'ĐINH QUANG PHÁT', N'444444444', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 38, N'1698899999', N'4444444444', 1, CAST(N'2019-11-25 00:20:07.797' AS DateTime), 1, 1, CAST(N'2019-11-25 00:20:07.797' AS DateTime), NULL, CAST(N'2019-11-26 00:00:00.000' AS DateTime), 1, 1, 39, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-25 00:20:11.983' AS DateTime), CAST(N'2019-11-25 00:20:11.983' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1153, N'1911000050', NULL, N'ĐINH QUANG PHÁT', N'555555555', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 37, N'1698899999', N'5555555555', 1, CAST(N'2019-11-25 00:22:33.223' AS DateTime), 1, 1, CAST(N'2019-11-25 00:22:33.223' AS DateTime), NULL, CAST(N'2019-11-26 00:00:00.000' AS DateTime), 1, 1, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-25 00:22:33.223' AS DateTime), CAST(N'2019-11-25 00:22:33.223' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1154, N'1911000051', NULL, N'ĐINH QUANG PHÁT', N'444444444', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'1698899999', N'4455555555', 1, CAST(N'2019-11-25 00:29:24.113' AS DateTime), 1, 1, CAST(N'2019-11-25 00:29:24.113' AS DateTime), NULL, CAST(N'2019-11-26 00:00:00.000' AS DateTime), 1, 1, 1, NULL, 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-25 00:29:24.113' AS DateTime), CAST(N'2019-11-25 00:29:24.113' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1155, N'1911000052', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, NULL, 1, 1, CAST(N'2019-11-26 20:43:05.470' AS DateTime), 0, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1156, N'1911000053', NULL, N'ĐINH QUANG PHÁT', N'222222222', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'2222222222', 1, NULL, 1, 1, CAST(N'2019-11-26 20:48:46.587' AS DateTime), 0, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1157, N'1911000054', NULL, N'ĐINH QUANG PHÁT', N'333333333', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'1698899999', N'3344445567', 1, CAST(N'2019-11-26 20:52:23.950' AS DateTime), 1, 1, CAST(N'2019-11-26 20:52:23.950' AS DateTime), NULL, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 0, 1, 16, N'', 0, CAST(5 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1158, N'1911000055', NULL, N'ĐINH QUANG PHÁT', N'444444444', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 3, N'1698899999', N'5555555555', 1, NULL, 1, 1, CAST(N'2019-11-26 20:55:18.717' AS DateTime), 0, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 1, 1, 18, N'', 0, CAST(4 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1159, N'1911000056', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-26 20:59:23.620' AS DateTime), 1, 1, CAST(N'2019-11-26 20:59:23.623' AS DateTime), 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), 0, 1, 16, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 20:59:23.620' AS DateTime), CAST(N'2019-11-26 20:59:23.620' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1160, N'1911000057', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, CAST(N'2019-11-26 21:00:11.057' AS DateTime), 1, 1, CAST(N'2019-11-26 21:00:11.057' AS DateTime), NULL, CAST(N'2019-11-26 00:00:00.000' AS DateTime), 0, 1, 18, N'', 0, CAST(10 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 21:00:11.057' AS DateTime), CAST(N'2019-11-26 21:00:11.057' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1161, N'1911000058', NULL, N'ĐINH QUANG PHÁT', N'', N'', 0, N'1698899999', N'', 1, NULL, 1, 1, CAST(N'2019-11-26 21:09:34.230' AS DateTime), 1, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1162, N'1911000059', NULL, N'ĐINH QUANG PHÁT', N'445566674', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 2, N'1698899999', N'', 1, CAST(N'2019-11-26 21:18:30.507' AS DateTime), 1, 1, CAST(N'2019-11-26 21:38:44.613' AS DateTime), 1, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 4, 1, 17, N'', 0, CAST(3 AS Decimal(18, 0)), 0, N'', 0, 0, CAST(N'2019-11-26 21:38:44.610' AS DateTime), CAST(N'2019-11-26 21:38:44.610' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1163, N'1911000060', NULL, N'ĐINH QUANG PHÁT', N'444444444', N'373 Hoàng Hữu Nam,P. Long Bình,Q.9', 870, N'1698899999', N'4444444444', 1, CAST(N'2019-11-26 22:04:54.623' AS DateTime), 1, 1, CAST(N'2019-11-26 22:05:16.733' AS DateTime), 1, CAST(N'2019-11-27 00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-11-26 00:00:00.000' AS DateTime), CAST(N'2019-11-26 00:00:00.000' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1164, N'1912000001', NULL, N'RRS', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:41:45.603' AS DateTime), 1, 1, CAST(N'2019-12-03 15:41:45.603' AS DateTime), NULL, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(4 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 15:41:45.603' AS DateTime), CAST(N'2019-12-03 15:41:45.603' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1165, N'1912000002', NULL, N'43', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:43:56.243' AS DateTime), 1, 1, CAST(N'2019-12-03 15:43:56.243' AS DateTime), NULL, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 15:43:56.243' AS DateTime), CAST(N'2019-12-03 15:43:56.243' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1166, N'1912000003', NULL, N'DD', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:45:19.770' AS DateTime), 1, 1, CAST(N'2019-12-03 15:45:19.770' AS DateTime), NULL, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 16, N'', 0, CAST(23 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 15:45:19.770' AS DateTime), CAST(N'2019-12-03 15:45:19.770' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1167, N'1912000004', NULL, N'Ẻ', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:48:37.493' AS DateTime), 1, 1, CAST(N'2019-12-03 15:48:37.493' AS DateTime), NULL, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 2, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 15:48:36.170' AS DateTime), CAST(N'2019-12-03 15:48:36.407' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1168, N'1912000005', NULL, N'DFFFF', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:52:39.313' AS DateTime), 1, 1, CAST(N'2019-12-03 15:52:39.313' AS DateTime), NULL, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 30, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 15:52:39.313' AS DateTime), CAST(N'2019-12-03 15:52:39.313' AS DateTime), NULL, NULL)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa], [BirthDay], [CMNDDay], [F88Result], [F88Reason]) VALUES (1169, N'1912000006', NULL, N'43R', N'', N'', 0, N'', N'', 1, CAST(N'2019-12-03 15:57:35.780' AS DateTime), 1, 1, CAST(N'2019-12-05 11:28:47.350' AS DateTime), 0, CAST(N'2019-12-03 00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(3 AS Decimal(18, 0)), 0, NULL, 0, 0, CAST(N'2019-12-03 00:00:00.000' AS DateTime), CAST(N'2019-12-03 00:00:00.000' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[HO_SO] OFF
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (4, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (7, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (8, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (9, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (12, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (13, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (14, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (15, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (16, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (17, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (18, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (19, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (20, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (21, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (22, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (23, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (24, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (26, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (27, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (28, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1028, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1029, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1034, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1095, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1096, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1097, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1099, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1110, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1116, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1117, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1118, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1119, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1120, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1121, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1122, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1123, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1124, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1125, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1126, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1127, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1128, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1129, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1130, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1131, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1132, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1133, 1)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1145, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (1163, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (4, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (4, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (8, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (14, 0)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (9, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (12, 0)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (20, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (19, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (22, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (24, 0)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (27, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (1133, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (1110, 1)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (1162, 1)
SET IDENTITY_INSERT [dbo].[KET_QUA_HS] ON 

INSERT [dbo].[KET_QUA_HS] ([ID], [Ten], [Da_Xoa]) VALUES (1, N'', 0)
INSERT [dbo].[KET_QUA_HS] ([ID], [Ten], [Da_Xoa]) VALUES (2, N'Gọi Khách Hàng', 0)
INSERT [dbo].[KET_QUA_HS] ([ID], [Ten], [Da_Xoa]) VALUES (3, N'Thẩm Định Địa Bàn', 0)
INSERT [dbo].[KET_QUA_HS] ([ID], [Ten], [Da_Xoa]) VALUES (4, N'Chốt Khoản Vay', 0)
SET IDENTITY_INSERT [dbo].[KET_QUA_HS] OFF
SET IDENTITY_INSERT [dbo].[KHU_VUC] ON 

INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1, N'TP. Hồ Chí Minh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (2, N'Quận 1', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (3, N'Quận 2', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (4, N'Quận 3', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (5, N'Quận 4', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (6, N'Quận 5', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (7, N'Quận 6', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (8, N'Quận 7', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (9, N'Quận 8', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (10, N'Quận 9', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (11, N'Quận 10', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (12, N'Quận 11', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (13, N'Quận 12', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (14, N'Quận Bình Tân', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (15, N'Quận Bình Thạnh', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (16, N'Quận Gò Vấp', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (17, N'Quận Phú Nhuận', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (18, N'Quận Tân Bình', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (19, N'Quận Tân Phú', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (20, N'Quận Thủ Đức', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (21, N'Huyện Bình Chánh', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (22, N'Huyện Cần Giờ', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (23, N'Huyện Củ Chi', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (24, N'Huyện Hóc Môn', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (25, N'Huyện Nhà Bè', 2, 1)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (26, N'Hà Nội', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (27, N'Đà Nẵng', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (37, N'Quận Đống Đa', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (38, N'Quận Ba Đình', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (39, N'Quận Cầu Giấy', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (40, N'Quận Hai Bà Trưng', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (41, N'Quận Hoàn Kiếm', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (42, N'Quận Hoàng Mai', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (43, N'Quận Long Biên', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (44, N'Quận Tây Hồ', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (45, N'Quận Thanh Xuân', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (46, N'Huyện Đông Anh', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (47, N'Huyện Gia Lâm', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (48, N'Huyện Sóc Sơn', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (49, N'Huyện Thanh Trì', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (81, N'Cà Mau', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (82, N'An Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (101, N'Hải Phòng', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (102, N'Bà Rịa - Vũng Tàu', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (103, N'Bắc Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (104, N'Bắc Kạn', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (105, N'Bạc Liêu', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (106, N'Bắc Ninh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (107, N'Bến Tre', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (108, N'Bình Định', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (109, N'Bình Dương', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (110, N'Bình Phước', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (111, N'Bình Thuận', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (112, N'Cao Bằng', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (113, N'Đắk Nông', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (114, N'Điện Biên', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (115, N'Đồng Tháp', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (116, N'Gia Lai', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (117, N'Hà Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (118, N'Hà Nam', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (120, N'Hà Tĩnh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (121, N'Hải Dương', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (122, N'Hậu Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (123, N'Hòa Bình', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (124, N'Hưng Yên', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (125, N'Khánh Hòa', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (126, N'Kiên Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (127, N'Kon Tum', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (128, N'Lai Châu', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (129, N'Lâm Đồng', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (130, N'Lạng Sơn', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (131, N'Lào Cai', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (132, N'Long An', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (133, N'Nam Định', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (134, N'Nghệ An', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (135, N'Ninh Bình', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (136, N'Ninh Thuận', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (137, N'Phú Thọ', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (138, N'Phú Yên', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (139, N'Quảng Bình', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (140, N'Quảng Nam', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (141, N'Quảng Ngãi', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (142, N'Quảng Ninh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (143, N'Quảng Trị', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (144, N'Sóc Trăng', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (145, N'Sơn La', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (146, N'Tây Ninh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (147, N'Thái Bình', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (148, N'Thái Nguyên', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (149, N'Thanh Hóa', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (150, N'Thừa Thiên Huế', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (151, N'Tiền Giang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (152, N'Trà Vinh', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (153, N'Tuyên Quang', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (154, N'Vĩnh Long', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (155, N'Vĩnh Phúc', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (156, N'Yên Bái', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (701, N'Quận Thanh Khê', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (724, N'Thành Phố Châu Đốc', 2, 82)
GO
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (809, N'Huyện Châu Thành', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (810, N'Huyện Chợ Mới', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (811, N'Huyện Tri Tôn', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (812, N'Huyện Tịnh Biên', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (813, N'Huyện Thoại Sơn', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (814, N'Huyện Châu Phú', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (815, N'Huyện Phú Tân', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (816, N'Huyện An Phú', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (817, N'Thị Xã Tân Châu', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (818, N'Huyện Ba Vì', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (819, N'Huyện Chương Mỹ', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (820, N'Huyện Đan Phượng', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (821, N'Quận Hà Đông', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (822, N'Huyện Hoài Đức', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (823, N'Huyện Mê Linh', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (824, N'Huyện Mỹ Đức', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (825, N'Huyện Phú Xuyên', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (826, N'Huyện Phúc Thọ', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (827, N'Huyện Quốc Oai', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (828, N'Thị Xã Sơn Tây', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (829, N'Huyện Thạch Thất', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (830, N'Huyện Thanh Oai', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (831, N'Huyện Thường Tín', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (832, N'Huyện Ứng Hòa', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (849, N'Quận Cẩm Lệ', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (850, N'Quận Hải Châu', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (851, N'Huyện Hòa Vang', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (852, N'Huyện Đảo Hoàng Sa', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (853, N'Quận Liên Chiểu', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (854, N'Quận Ngũ Hành Sơn', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (855, N'Quận Sơn Trà', 2, 27)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (856, N'TP. Long Xuyên', 2, 82)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (870, N'Huyện An Dương', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (871, N'Huyện An Lão', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (872, N'Huyện Bạch Long Vĩ', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (873, N'Huyện Cát Hải', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (874, N'Quận Dương Kinh', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (875, N'Quận Đồ Sơn', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (876, N'Quận Hải An', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (877, N'Quận Hồng Bàng', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (878, N'Quận Kiến An', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (879, N'Huyện Kiến Thụy', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (880, N'Quận Lê Chân', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (881, N'Quận Ngô Quyền', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (882, N'Huyện Thuỷ Nguyên', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (883, N'Huyện Tiên Lãng', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (884, N'Huyện Vĩnh Bảo', 2, 101)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (885, N'TP. Bà Rịa', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (886, N'Huyện Châu Đức', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (887, N'Huyện Côn Đảo', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (888, N'Huyện Đất Đỏ', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (889, N'Huyện Long Điền', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (890, N'Huyện Tân Thành', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (891, N'TP. Vũng Tàu', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (892, N'Huyện Xuyên Mộc', 2, 102)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (893, N'TP.Bắc Giang', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (894, N'Huyện Hiệp Hòa', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (895, N'Huyện Lạng Giang', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (896, N'Huyện Lục Nam', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (897, N'Huyện Lục Ngạn', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (898, N'Huyện Sơn Động', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (899, N'Huyện Việt Yên', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (900, N'Huyện Yên Dũng', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (901, N'Huyện Yên Thế', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (903, N'Thành Phố Bắc Kạn', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (904, N'Huyện Bạch Thông', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (905, N'Huyện Chợ Đồn', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (906, N'Huyện Chợ Mới', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (907, N'Huyện Na Rì', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (908, N'Huyện Ngân Sơn', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (909, N'Huyện Pác Nặm', 2, 104)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (910, N'Tp.Bạc Liêu', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (911, N'Huyện Đông Hải', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (912, N'Thị Xã Giá Rai', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (913, N'Huyện Hòa Bình', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (914, N'Huyện Hồng Dân', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (915, N'Huyện Phước Long', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (916, N'Huyện Vĩnh Lợi', 2, 105)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (917, N'TP.Bắc Ninh', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (918, N'Huyện Gia Bình', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (919, N'Huyện Lương Tài', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (920, N'Huyện Quế Võ', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (921, N'Huyện Thuận Thành', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (922, N'Huyện Tiên Du', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (923, N'Thị Xã Từ Sơn', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (924, N'Huyện Yên Phong', 2, 106)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1446, N'Huyện Tân Yên', 2, 103)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1454, N'Quận Nam Từ Liêm', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1455, N'Quận Bắc Từ Liêm', 2, 26)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1456, N'Thành phố Thủ Dầu Một
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1457, N' Thị Xã Bến Cát
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1458, N'Thị Xã Dĩ An
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1459, N'Thị Xã Thuận An
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1460, N'Thị Xã Tân Uyên
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1461, N'Huyện Bắc Tân Uyên
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1462, N'Huyện Bàu Bàng
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1463, N'Huyện Phú Giáo
', 2, 109)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1464, N'Đồng Nai', 1, 0)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1465, N'Thành phố Biên Hoà', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1466, N'Thị xã Long Khánh', 2, 1464)
GO
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1467, N'Huyện Vĩnh Cửu', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1468, N'Huyện Tân Phú', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1469, N'Huyện Định Quán', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1470, N'Huyện Thống Nhất', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1471, N'Huyện Xuân Lộc', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1472, N'Huyện Long Thành', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1473, N'Huyện Nhơn Trạch', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1474, N'Huyện Trảng Bom', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1475, N'Huyện Cẩm Mỹ', 2, 1464)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1476, N'Thành phố Tân An', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1477, N'Thị xã Kiến Tường', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1478, N'Huyện Bến Lức', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1479, N'Huyện Cần Đước', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1480, N'Huyện Cần Giuộc', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1481, N'Huyện Châu Thành', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1482, N'Huyện Đức Hòa', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1483, N'Huyện Đức Huệ', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1484, N'Huyện Mộc Hóa', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1485, N'Huyện Thủ Thừa', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1486, N'Huyện Tân Trụ', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1487, N'Huyện Thạnh Hóa', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1488, N'Huyện Tân Thạnh', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1489, N'Huyện Tân Hưng', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1490, N'Huyện Vĩnh Hưng', 2, 132)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1491, N'Thành phố Thanh Hóa', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1492, N'Thị xã Bỉm Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1493, N'Thị xã Sầm Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1494, N'Huyện Đông Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1495, N'Huyện Quảng Xương', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1496, N'Huyện Hoằng Hóa', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1497, N'Huyện Hậu Lộc', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1498, N'Huyện Hà Trung', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1499, N'Huyện Nga Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1500, N'Huyện Thiệu Hóa', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1501, N'Huyện Triệu Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1502, N'Huyện Yên Định', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1503, N'Huyện Tĩnh Gia', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1504, N'Huyện Nông Cống', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1505, N'Huyện Ngọc Lặc', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1506, N'Huyện Cẩm Thủy', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1507, N'Huyện Thạch Thành', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1508, N'Huyện Vĩnh Lộc', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1509, N'Huyện Thọ Xuân', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1510, N'Huyện Như Thanh', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1511, N'Huyện Như Xuân', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1512, N'Huyện Thường Xuân', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1513, N'Huyện Lang Chánh', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1514, N'Huyện Bá Thước', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1515, N'Huyện Quan Hóa', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1516, N'Huyện Quan Sơn', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1517, N'Huyện Mường Lát', 2, 149)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1518, N'Thành phố Lào Cai', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1519, N'Huyện Xi Ma Cai', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1520, N'Huyện Bát Xát', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1521, N'Huyện Bảo Thắng', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1522, N'Huyện Sa Pa', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1523, N'Huyện Văn Bàn', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1524, N'Huyện Bảo Yên', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1525, N'Huyện Bắc Hà', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1526, N'Huyện Mường Khương', 2, 131)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1527, N'Thành Phố Ninh Bình', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1528, N'Thành Phố Tam Điệp', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1529, N'Huyện Nho Quan', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1530, N'Huyện Gia Viễn', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1531, N'Huyện Hoa Lư', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1532, N'Huyện Yên Mô', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1533, N'Huyện Kim Sơn', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1534, N'Huyện Yên Khánh', 2, 135)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1535, N'Thành phổ Rạch Giá', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1536, N'Thị xã Hà Tiên', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1537, N'Huyện Kiên Lương', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1538, N'Huyện Hòn Đất', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1539, N'Huyện Tân Hiệp', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1540, N'Huyện Châu Thành', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1541, N'Huyện Giồng Riềng', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1542, N'Huyện Gò Quao', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1543, N'Huyện An Biên', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1544, N'Huyện An Minh', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1545, N'Huyện Vĩnh Thuận', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1546, N'Huyện Phú Quốc', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1547, N'Huyện Kiên Hải', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1548, N'Huyện U Minh Thượng', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1549, N'Huyện Giang Thành', 2, 126)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1550, N'Thành phố Cà Mau', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1551, N'Huyện Thới Bình', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1552, N'Huyện U Minh', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1553, N'Huyện Trần Văn Thời', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1554, N'Huyện Cái Nước', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1555, N'Huyện Đầm Dơi', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1556, N'Huyện Ngọc Hiển', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1557, N'Huyện Năm Căn', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1558, N'Huyện Phú Tân', 2, 81)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1559, N'Thành phố Vị Thanh', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1560, N'Thị xã Ngã Bảy', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1561, N'Huyện Vị Thuỷ', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1562, N'Huyện Long Mỹ', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1563, N'Huyện Phụng Hiệp', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1564, N'Huyện Châu Thành', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1565, N'Huyện Châu Thành A', 2, 122)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1566, N'Thành phố Cao Lãnh', 2, 115)
GO
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1567, N'Thành phố Sa Đéc', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1568, N'Thị xã Hồng Ngự', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1569, N'Huyện Cao Lãnh', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1570, N'Huyện Châu Thành', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1571, N'Huyện Hồng Ngự', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1572, N'Huyện Lai Vung', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1573, N'Huyện Lấp Vò', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1574, N'Huyện Tam Nông', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1575, N'Huyện Tân Hồng', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1576, N'Huyện Thanh Bình', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1577, N'Huyện Tháp Mười', 2, 115)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1578, N'Thành phố Bến Tre', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1579, N'Huyện Ba Tri', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1580, N'Huyện Bình Đại', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1581, N'Huyện Châu Thành', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1582, N'Huyện Chợ Lách', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1583, N'Huyện Giồng Tôm', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1584, N'Huyện Mỏ Cày Bắc', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1585, N'Huyện Mỏ Cày Nam', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1586, N'Huyện Thạnh Phú', 2, 107)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1587, N'Thành phố Vĩnh Long', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1588, N'Thị xã Bình Minh', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1589, N'Huyện Bình Tân', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1590, N'Huyện Long Hồ', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1591, N'Huyện Mang Thít', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1592, N'Huyện Tam Bình', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1593, N'Huyện Trà Ôn', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1594, N'Huyện Vũng Liêm', 2, 154)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1595, N'Thành phố Trà Vinh', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1596, N'Thị xã Duyên Hải', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1597, N'Huyện Châu thành', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1598, N'Huyện Cầu ngang', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1599, N'Huyện Càng Long', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1600, N'Huyện Duyên Hải', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1601, N'Huyện Tiểu Cần', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1602, N'Huyện Cầu Kè', 2, 152)
INSERT [dbo].[KHU_VUC] ([ID], [Ten], [Loai], [Ma_Cha]) VALUES (1603, N'Huyện Trà Cú', 2, 152)
SET IDENTITY_INSERT [dbo].[KHU_VUC] OFF
SET IDENTITY_INSERT [dbo].[LOAI_TAI_LIEU] ON 

INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (1, N'ĐƠN ĐỀ NGHỊ VAY (ACCA)', 1)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (2, N'CMND/CCCD', 1)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (3, N'SỔ HỘ KHẨU ', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (4, N'ẢNH CHỤP CÙNG KHÁCH HÀNG', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (5, N'CHỮ KÝ MẪU CỦA KHÁCH HÀNG', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (6, N'BẢO HIỂM Y TẾ', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (7, N'CHỨNG MINH THU NHẬP KHÁCH HÀNG', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (8, N'HỢP ĐỒNG LAO ĐỘNG HOẶC CHỨNG TỪ TƯƠNG ĐƯƠNG', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (9, N'HÓA ĐƠN ĐIỆN, NƯỚC, INTERNET, CAB', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (10, N'GIẤY PHÉP KINH DOANH HOẶC CHỨNG TỪ TƯƠNG ĐƯƠNG', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (11, N'HỢP ĐỒNG BẢO HIỂM NHÂN THỌ', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (12, N'CHỨNG TỪ THANH TOÁN PHÍ BẢO HIỂM NHÂN TH', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (13, N'GIẤY XÁC NHẬN CƯ TRÚ', 0)
INSERT [dbo].[LOAI_TAI_LIEU] ([ID], [Ten], [Bat_Buoc]) VALUES (14, N'KHÁC', 0)
SET IDENTITY_INSERT [dbo].[LOAI_TAI_LIEU] OFF
SET IDENTITY_INSERT [dbo].[NHAN_VIEN] ON 

INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (1, N'VBF0001', N'ThaiNM', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Minh Thái', NULL, N'0901812024', N'minhtai.nguyen@vietbankfc.vn', 1, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (2, N'VBF0002', N'NhuTH', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Huỳnh Như', NULL, N'0901812024', N'support@vietbankkfc.vn', 1, 0, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (3, N'VBF0003', N'TungND', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Ngô Duy Tùng', NULL, N'0901812024', N'tung.ngo@vietbankfc.vn', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (4, N'VBF0004', N'AnhNN', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Ngọc Anh
', 0, N'0901812024', N'anh.nguyen.5@vietbankfc.vn
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (5, N'VBF0005', N'TungDA', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Đào Anh Tùng
', 0, N'0901812024', N'tung.dao@vietbankfc.vn
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (6, N'VBF0006', N'HoaNTN', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Như Hoa
', 0, N'0901812024', N'hoa.nguyen@vietbankfc.vn
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (7, N'VBF0007', N'LinhNTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Thùy Linh
', 0, N'0901812024', N'nguyenlinh08121992@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (8, N'VBF0008', N'OanhNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Oanh 
', 0, N'0901812024', N'nguyenoanhnd2608@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (9, N'VBF0009', N'ChienTD', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Tống Đình Chiến
', 0, N'0901812024', N'tuanchien68@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (10, N'VBF0010', N'ThuyLTD', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Lưu Thị Diệu Thúy
', NULL, N'0901812024', N'dieuthuy.bhhyyen@mail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (11, N'VBF0011', N'HungDQ', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Đặng Quang Hưng 
', NULL, N'0901812024', N'dangquanghung88nd@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (12, N'VBF0012', N'NgaHT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Hoàng Thúy Nga
', NULL, N'0901812024', N'ngahoang266@mail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (13, N'VBF0013', N'HuyenLTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Lã Thị Thu Huyền
', NULL, N'0901812024', N'Hoavioletnamdinh@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (14, N'VBF0014', N'DuNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Du', NULL, N'0901812024', N'buianphuong2nd@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (15, N'VBF0015', N'BonTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Thị Bốn', NULL, N'0901812024', N'tbon1982@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (16, N'VBF0016', N'HangDT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Dương Thị Hằng
', NULL, N'0901812024', N'duonghang2412@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (17, N'VBF0017', N'LanNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Lan', NULL, N'0901812024', N'nguyenlan1980@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (18, N'VBF0018', N'HuongTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Thu Hường
', NULL, N'0901812024', N'tranthuhuonghuong40@gmail.com
', 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (21, N'VBF0021', N'TEO', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'TEO', NULL, NULL, NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (22, N'VBF0022', N'SALE', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'SALE', NULL, NULL, NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (23, N'VBF0023', N'Teo2', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'teo2', NULL, N'', N'teo2@gmail.com', 1, 0, 2, 1, 3, CAST(N'2001-01-01 00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-11-07 21:09:38.650' AS DateTime), 27)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (24, N'VBF0023', N'sale2', N'43f5c90612d381db7c03ee43880afea1', N'sale2', NULL, N'0169889999', N'sale2@gmail.com', 1, 0, 2, 26, 38, CAST(N'2019-11-03 00:00:00.000' AS DateTime), NULL, NULL, CAST(N'2019-11-04 21:37:10.410' AS DateTime), 27)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (27, NULL, N'quangphat', N'9d480b0068558b93ec8cd7f07de18562', N'Đinh Quang Phát', NULL, N'0169889999', N'quangphatitu@gmail.com', 1, 0, 1, 1, 3, CAST(N'2019-11-02 00:00:00.000' AS DateTime), CAST(N'2019-11-02 17:40:49.647' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa], [RoleId], [ProvinceId], [DistrictId], [WorkDate], [CreatedTime], [CreatedBy], [UpdatedTime], [UpdatedBy]) VALUES (28, NULL, N'KS001888', N'43f5c90612d381db7c03ee43880afea1', N'LÊ QUANG TÈO', NULL, N'0987654321', N'teo@gmail.com', 1, 0, 2, 1, 5, CAST(N'2019-11-11 00:00:00.000' AS DateTime), CAST(N'2019-11-07 20:57:57.493' AS DateTime), 27, NULL, NULL)
SET IDENTITY_INSERT [dbo].[NHAN_VIEN] OFF
INSERT [dbo].[NHAN_VIEN_CF] ([Ma_Nhan_Vien], [Quyen], [Ma_Nhom]) VALUES (1, 0, 1)
INSERT [dbo].[NHAN_VIEN_CF] ([Ma_Nhan_Vien], [Quyen], [Ma_Nhom]) VALUES (19, 0, 7)
INSERT [dbo].[NHAN_VIEN_CF] ([Ma_Nhan_Vien], [Quyen], [Ma_Nhom]) VALUES (19, 0, 9)
INSERT [dbo].[NHAN_VIEN_CF] ([Ma_Nhan_Vien], [Quyen], [Ma_Nhom]) VALUES (19, 0, 10)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 1)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 7)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 14)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 16)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 17)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (2, 8)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 1)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 2)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 9)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 15)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (4, 3)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (4, 8)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (4, 10)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (4, 17)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (5, 4)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (5, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (6, 5)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (6, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (7, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (8, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (9, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (10, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (11, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (12, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (13, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (14, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (14, 14)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (15, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (16, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (17, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (18, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (18, 16)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (18, 17)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (21, 11)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (22, 12)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (22, 15)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (24, 13)
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (1, N'ffffff')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (2, N'fffffffff33')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (5, N'ffffff')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (19, N'ffffffff')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (20, N'fffffffff')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (23, N'fffffffff')
SET IDENTITY_INSERT [dbo].[NHOM] ON 

INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (1, 0, N'0', 1, N'Director', N'Director', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (2, 1, N'0.1', 3, N'Head North1', N'Head North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (3, 2, N'0.1.2', 4, N'RSM North1', N'RSM North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (4, 3, N'0.1.2.3', 5, N'ASM North1', N'ASM North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (5, 4, N'0.1.2.3.4', 6, N'SS North1', N'SS North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (6, 5, N'0.1.2.3.4.5', 6, N'NAMDINH1', N'NAMDINH1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (7, 1, N'0.1', 3, N'test', N'test', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (8, 6, N'0.1.2.3.4.5.6', 3, N'namdinh1.2', N'namdinh1.2', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (9, 1, N'0.1', 20, N'test2', N'test2', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (10, 9, N'0.1.9', 19, N'test3', N'test3', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (11, 4, N'0.1.2.3.4', 5, N'TEO', N'SS NORTH2', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (12, 11, N'0.1.2.3.4.11', 21, N'SG1', N'SG1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (13, 1, N'0.1', 23, N'teamlead1', N'teamlead1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (14, 0, N'0', 1, N'tttt', N'test4', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (15, 0, N'0', 2, N'eee', N'ff', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (16, 0, N'0', 27, N'test6', N'test6', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (17, 0, N'0', 27, N'test6', N'test6', NULL)
SET IDENTITY_INSERT [dbo].[NHOM] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name], [Code], [Deleted], [CreatedBy], [CreatedTime], [UpdatedBy], [UpdatedTime]) VALUES (1, N'Admin', N'admin', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Role] ([Id], [Name], [Code], [Deleted], [CreatedBy], [CreatedTime], [UpdatedBy], [UpdatedTime]) VALUES (2, N'Sale', N'sale', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Role] ([Id], [Name], [Code], [Deleted], [CreatedBy], [CreatedTime], [UpdatedBy], [UpdatedTime]) VALUES (3, N'Courier', N'sale', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Role] ([Id], [Name], [Code], [Deleted], [CreatedBy], [CreatedTime], [UpdatedBy], [UpdatedTime]) VALUES (4, N'Data Entry', N'admin', 0, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[RolePermission] ON 

INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission]) VALUES (2, 2, N'sale', N'hoso.all')
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission]) VALUES (3, 1, N'admin', N'admin')
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission]) VALUES (4, 2, N'hoso.read', N'hoso.read')
INSERT [dbo].[RolePermission] ([Id], [RoleId], [RoleCode], [Permission]) VALUES (5, 2, N'sale', N'hoso.approve')
SET IDENTITY_INSERT [dbo].[RolePermission] OFF
SET IDENTITY_INSERT [dbo].[SAN_PHAM_VAY] ON 

INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (1, 1, NULL, N'GOLD
', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (2, 1, NULL, N'TITANIUM
', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (3, 6, N'', N'FC UP PL CAT A PJICO NEW- 608', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (4, 6, N'', N'FC UP PL CAT B PJICO NEW- 610', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (5, 6, N'', N'FC UP PL CAT C PJICO - 306', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (6, 6, N'', N'FC UP SUR INS 37 NEW- 612', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (7, 6, N'', N'FC PL UP BIKE SUR NEW VNP - 903', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (8, 6, N'', N'FC PL UP BIKE SUR EX NEW- 904', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (9, 6, N'', N'FC PL UP BIKE SUR PLUS NEW- 905', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (10, 6, N'', N'FC UP SUR CF - 285', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (11, 6, N'', N'FC UP SUR CF PLUS - 555', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (12, 6, N'', N'FC UP BANK SUR - 556', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (13, 6, N'', N'FC UP EVN VIP NEW - 622', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (14, 6, N'', N'FC UP EVN STD NEW - 629', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (15, 6, N'', N'FC PL UP EVN CLASSIC NEW - 915', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (16, 2, N'CS BHYT 35', N'CS BHYT 35', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (17, 2, N'CS BHYT 37', N'CS BHYT 37', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (18, 2, N'CS BHYT 47', N'CS BHYT 47', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (19, 2, N'CS INSURANCE 37', N'CS INSURANCE 37', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (20, 2, N'CS INSURANCE 45', N'CS INSURANCE 45', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (21, 2, N'CS CAT A 37', N'CS CAT A 37', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (22, 2, N'CS SY B 45', N'CS SY B 45', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (23, 2, N'CS SY C 60', N'CS SY C 60', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (24, 2, N'CS EVN VIP 37', N'CS EVN VIP 37', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (25, 2, N'CS EVN STANDARD 45', N'CS EVN STANDARD 45', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (26, 2, N'CS EVN CLASSIC 60', N'CS EVN CLASSIC 60', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (27, 2, N'CS EVN FAST 50', N'CS EVN FAST 50', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (28, 2, N'CS BANK SURROGATE VIP 37', N'CS BANK SURROGATE VIP 37', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (29, 2, N'CS BANK SURROGATE STANDARD 45', N'CS BANK SURROGATE STANDARD 45', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (30, 1, N'SILVER', N'SILVER', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (31, 1, N'GREY', N'GREY', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (32, 1, N'CTV', N'CTV', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (33, 1, N'HĐ1', N'HĐ1', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (34, 1, N'HĐ2', N'HĐ2', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (35, 1, N'HĐ3', N'HĐ3', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (36, 1, N'VAY+', N'VAY+', NULL, NULL, NULL)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (51, 3, N'APPL00085184', N'APPL00085184', CAST(N'2019-11-29 00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (52, 3, N'APPL00083863', N'APPL00083863', CAST(N'2019-11-29 00:00:00.000' AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[SAN_PHAM_VAY] OFF
SET IDENTITY_INSERT [dbo].[TAI_LIEU_HS] ON 

INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (294, 1, N'20190519143150291_02016Thg226163814555_Penguins.jpg', N'/Upload/HoSo/2019/5/19/20190519143150291_02016Thg226163814555_Penguins.jpg', 11, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (295, 2, N'20190519143237085_02016Thg226163814555_Penguins.jpg', N'/Upload/HoSo/2019/5/19/20190519143237085_02016Thg226163814555_Penguins.jpg', 11, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (298, 1, N'20190621104417394_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/21/20190621104417394_19260408_1692556414108083_4110998579350839397_n.png', 13, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (299, 2, N'20190621104419392_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/21/20190621104419392_19959157_788535194640385_2653266257468029984_n.jpg', 13, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (302, 1, N'20190622170752643_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622170752643_19260408_1692556414108083_4110998579350839397_n.png', 15, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (303, 2, N'20190622170754119_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622170754119_19959157_788535194640385_2653266257468029984_n.jpg', 15, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (304, 1, N'20190622171059918_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622171059918_19260408_1692556414108083_4110998579350839397_n.png', 16, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (305, 2, N'20190622171101453_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622171101453_19959157_788535194640385_2653266257468029984_n.jpg', 16, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (308, 1, N'20190622141356365_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622141356365_19260408_1692556414108083_4110998579350839397_n.png', 14, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (309, 2, N'20190622141357814_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622141357814_19959157_788535194640385_2653266257468029984_n.jpg', 14, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (310, 1, N'20190622182652163_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622182652163_19260408_1692556414108083_4110998579350839397_n.png', 9, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (311, 2, N'20190622182653617_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622182653617_19959157_788535194640385_2653266257468029984_n.jpg', 9, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (324, 1, N'20190622182520625_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622182520625_19260408_1692556414108083_4110998579350839397_n.png', 8, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (325, 2, N'20190622182522245_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622182522245_19959157_788535194640385_2653266257468029984_n.jpg', 8, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (326, 1, N'20190622223910141_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622223910141_19260408_1692556414108083_4110998579350839397_n.png', 17, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (327, 2, N'20190622223911946_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622223911946_19959157_788535194640385_2653266257468029984_n.jpg', 17, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (328, 1, N'20190622224525379_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622224525379_19260408_1692556414108083_4110998579350839397_n.png', 18, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (329, 2, N'20190622224527675_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622224527675_19959157_788535194640385_2653266257468029984_n.jpg', 18, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (336, 1, N'20190623162410493_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/6/23/20190623162410493_53226262_246257156318354_8760365182137401344_n.jpg', 20, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (337, 2, N'20190623162412625_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/23/20190623162412625_19959157_788535194640385_2653266257468029984_n.jpg', 20, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (376, 1, N'20190624162520633_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/24/20190624162520633_19260408_1692556414108083_4110998579350839397_n.png', 21, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (377, 2, N'20190624162522502_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/24/20190624162522502_19959157_788535194640385_2653266257468029984_n.jpg', 21, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (380, 1, N'20190624162548108_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/24/20190624162548108_19959157_788535194640385_2653266257468029984_n.jpg', 22, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (381, 2, N'20190624162549669_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/24/20190624162549669_19959157_788535194640385_2653266257468029984_n.jpg', 22, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (382, 1, N'20190623154709076_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/6/23/20190623154709076_53226262_246257156318354_8760365182137401344_n.jpg', 19, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (383, 2, N'20190623154714400_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/6/23/20190623154714400_53226262_246257156318354_8760365182137401344_n.jpg', 19, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (384, 2, N'20190620171045345_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/20/20190620171045345_19959157_788535194640385_2653266257468029984_n.jpg', 12, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (385, 1, N'20190620171051533_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/20/20190620171051533_19959157_788535194640385_2653266257468029984_n.jpg', 12, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (394, 1, N'20190624200054003_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/6/24/20190624200054003_53226262_246257156318354_8760365182137401344_n.jpg', 23, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (395, 2, N'20190624200057090_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/6/24/20190624200057090_53226262_246257156318354_8760365182137401344_n.jpg', 23, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (402, 1, N'20190625215854595_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/25/20190625215854595_19260408_1692556414108083_4110998579350839397_n.png', 25, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (403, 2, N'20190625215857178_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/25/20190625215857178_19260408_1692556414108083_4110998579350839397_n.png', 25, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (414, 1, N'20190622184927610_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/22/20190622184927610_19260408_1692556414108083_4110998579350839397_n.png', 4, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (415, 2, N'20190622184929468_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/22/20190622184929468_19959157_788535194640385_2653266257468029984_n.jpg', 4, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (416, 1, N'20190625185326212_19260408_1692556414108083_4110998579350839397_n.png', N'/Upload/HoSo/2019/6/25/20190625185326212_19260408_1692556414108083_4110998579350839397_n.png', 24, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (417, 2, N'20190625185328199_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/25/20190625185328199_19959157_788535194640385_2653266257468029984_n.jpg', 24, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (428, 1, N'20190626193754697_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/26/20190626193754697_19959157_788535194640385_2653266257468029984_n.jpg', 26, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (429, 2, N'20190626193758623_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/6/26/20190626193758623_19959157_788535194640385_2653266257468029984_n.jpg', 26, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (432, 1, N'20190901213445989_19959157_788535194640385_2653266257468029984_n.jpg', N'/Upload/HoSo/2019/9/1/20190901213445989_19959157_788535194640385_2653266257468029984_n.jpg', 27, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (433, 2, N'20190901213449464_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/9/1/20190901213449464_53226262_246257156318354_8760365182137401344_n.jpg', 27, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1434, 1, N'20191013235832952_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/10/13/20191013235832952_56226055_2441636429202124_5126384707222634496_n.jpg', 28, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1435, 2, N'20191013235835805_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/10/13/20191013235835805_56811293_2397744143623591_659462958098677760_n.jpg', 28, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1436, 1, N'20191019160022912_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/19/20191019160022912_53226262_246257156318354_8760365182137401344_n.jpg', 1028, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1437, 2, N'20191019160025856_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/10/19/20191019160025856_56811293_2397744143623591_659462958098677760_n.jpg', 1028, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1440, 1, N'20191019160433305_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/10/19/20191019160433305_60468641_2644069809212998_5188156361392783360_n.png', 1029, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1441, 2, N'20191019160445766_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/10/19/20191019160445766_56226055_2441636429202124_5126384707222634496_n.jpg', 1029, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1444, 1, N'20191019161655315_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/19/20191019161655315_53226262_246257156318354_8760365182137401344_n.jpg', 1030, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1445, 2, N'20191019161727574_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/10/19/20191019161727574_56226055_2441636429202124_5126384707222634496_n.jpg', 1030, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1446, 1, N'20191020214501815_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/20/20191020214501815_53226262_246257156318354_8760365182137401344_n.jpg', 1051, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1447, 2, N'20191020214553937_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/20/20191020214553937_53226262_246257156318354_8760365182137401344_n.jpg', 1051, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1448, 2, N'20191020214631690_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/10/20/20191020214631690_56811293_2397744143623591_659462958098677760_n.jpg', 1052, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1455, 1, N'20191021230206672_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/21/20191021230206672_53678158_1990595077736930_4994092537615482880_n.jpg', 1084, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1456, 2, N'20191021230209227_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/10/21/20191021230209227_56811293_2397744143623591_659462958098677760_n.jpg', 1084, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1460, 1, N'20191021231504364_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/21/20191021231504364_53226262_246257156318354_8760365182137401344_n.jpg', 1086, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1461, 3, N'20191021231646142_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/10/21/20191021231646142_56811293_2397744143623591_659462958098677760_n.jpg', 1086, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1462, 1, N'20191023225757024_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/23/20191023225757024_53678158_1990595077736930_4994092537615482880_n.jpg', 1096, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1463, 2, N'20191023225800907_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/10/23/20191023225800907_56226055_2441636429202124_5126384707222634496_n.jpg', 1096, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1464, 9, N'20191023232312693_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/23/20191023232312693_53678158_1990595077736930_4994092537615482880_n.jpg', 1097, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1465, 1, N'20191023232317921_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/23/20191023232317921_53226262_246257156318354_8760365182137401344_n.jpg', 1097, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1466, 2, N'20191023232320468_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/23/20191023232320468_53678158_1990595077736930_4994092537615482880_n.jpg', 1097, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1467, 1, N'20191027141807867_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/27/20191027141807867_53226262_246257156318354_8760365182137401344_n.jpg', 1098, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1468, 2, N'20191027141810475_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/27/20191027141810475_53678158_1990595077736930_4994092537615482880_n.jpg', 1098, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1469, 1, N'20191027141859774_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/10/27/20191027141859774_53226262_246257156318354_8760365182137401344_n.jpg', 1099, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1470, 2, N'20191027141902515_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/10/27/20191027141902515_53678158_1990595077736930_4994092537615482880_n.jpg', 1099, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1471, 1, N'20191109234842108_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/9/20191109234842108_53226262_246257156318354_8760365182137401344_n.jpg', 1095, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1472, 2, N'20191109234845231_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/9/20191109234845231_55708876_2179766748765962_4578422690720776192_n.png', 1095, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1473, 1, N'20191109235240305_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/9/20191109235240305_53226262_246257156318354_8760365182137401344_n.jpg', 1034, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1474, 2, N'20191109235244760_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/9/20191109235244760_55708876_2179766748765962_4578422690720776192_n.png', 1034, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1475, 1, N'53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/11/20191111221135805_53226262_246257156318354_8760365182137401344_n.jpg', 1113, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1476, 2, N'55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/11/20191111221139799_55708876_2179766748765962_4578422690720776192_n.png', 1113, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1477, 1, N'53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/11/20191111221157629_53678158_1990595077736930_4994092537615482880_n.jpg', 1113, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1478, 1, N'20191112222700217_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/12/20191112222700217_60468641_2644069809212998_5188156361392783360_n.png', 1116, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1479, 2, N'20191112222703087_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/12/20191112222703087_56226055_2441636429202124_5126384707222634496_n.jpg', 1116, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1480, 1, N'20191112224041993_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112224041993_53226262_246257156318354_8760365182137401344_n.jpg', 1117, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1481, 2, N'20191112224044536_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/12/20191112224044536_55708876_2179766748765962_4578422690720776192_n.png', 1117, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1482, 1, N'20191112224551142_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112224551142_53226262_246257156318354_8760365182137401344_n.jpg', 1118, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1483, 2, N'20191112224554183_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/12/20191112224554183_55708876_2179766748765962_4578422690720776192_n.png', 1118, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1484, 1, N'20191112230335982_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112230335982_53226262_246257156318354_8760365182137401344_n.jpg', 1119, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1485, 2, N'20191112230339229_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/12/20191112230339229_55708876_2179766748765962_4578422690720776192_n.png', 1119, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1486, 1, N'20191112230959382_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112230959382_53226262_246257156318354_8760365182137401344_n.jpg', 1120, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1487, 2, N'20191112231002587_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/12/20191112231002587_55708876_2179766748765962_4578422690720776192_n.png', 1120, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1488, 1, N'20191112232403655_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112232403655_53226262_246257156318354_8760365182137401344_n.jpg', 1121, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1489, 2, N'20191112232406542_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/12/20191112232406542_56226055_2441636429202124_5126384707222634496_n.jpg', 1121, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1490, 1, N'20191112232845798_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/12/20191112232845798_53226262_246257156318354_8760365182137401344_n.jpg', 1122, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1491, 2, N'20191112232848627_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/12/20191112232848627_53678158_1990595077736930_4994092537615482880_n.jpg', 1122, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1496, 1, N'20191114195553699_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114195553699_53226262_246257156318354_8760365182137401344_n.jpg', 1123, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1497, 2, N'20191114195556592_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/14/20191114195556592_53678158_1990595077736930_4994092537615482880_n.jpg', 1123, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1498, 1, N'20191114195934287_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114195934287_53226262_246257156318354_8760365182137401344_n.jpg', 1124, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1499, 2, N'20191114195937272_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114195937272_53226262_246257156318354_8760365182137401344_n.jpg', 1124, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1502, 1, N'20191114222449275_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114222449275_53226262_246257156318354_8760365182137401344_n.jpg', 1125, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1503, 2, N'20191114222452267_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/14/20191114222452267_53678158_1990595077736930_4994092537615482880_n.jpg', 1125, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1504, 1, N'20191114222731913_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/14/20191114222731913_60339401_645961089264743_3587526765771227136_n.jpg', 1126, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1505, 2, N'20191114222734460_60523035_645961315931387_321405403625684992_n.jpg', N'/Upload/HoSo/2019/11/14/20191114222734460_60523035_645961315931387_321405403625684992_n.jpg', 1126, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1506, 1, N'20191114224107624_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114224107624_53226262_246257156318354_8760365182137401344_n.jpg', 1127, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1507, 2, N'20191114224110748_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/14/20191114224110748_53678158_1990595077736930_4994092537615482880_n.jpg', 1127, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1514, 1, N'20191114224421576_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114224421576_53226262_246257156318354_8760365182137401344_n.jpg', 1128, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1515, 2, N'20191114224424790_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/14/20191114224424790_60339401_645961089264743_3587526765771227136_n.jpg', 1128, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1518, 1, N'20191114225158654_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/14/20191114225158654_60468641_2644069809212998_5188156361392783360_n.png', 1129, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1519, 2, N'20191114225201455_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/14/20191114225201455_60339401_645961089264743_3587526765771227136_n.jpg', 1129, NULL)
GO
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1520, 1, N'20191114225535737_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/14/20191114225535737_60468641_2644069809212998_5188156361392783360_n.png', 1130, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1521, 2, N'20191114225538482_60523035_645961315931387_321405403625684992_n.jpg', N'/Upload/HoSo/2019/11/14/20191114225538482_60523035_645961315931387_321405403625684992_n.jpg', 1130, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1522, 1, N'20191114230901182_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/14/20191114230901182_60339401_645961089264743_3587526765771227136_n.jpg', 1131, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1523, 2, N'20191114230904026_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/14/20191114230904026_60468641_2644069809212998_5188156361392783360_n.png', 1131, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1528, 1, N'20191114233408090_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/14/20191114233408090_53678158_1990595077736930_4994092537615482880_n.jpg', 1132, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1529, 2, N'20191114233410663_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/14/20191114233410663_53226262_246257156318354_8760365182137401344_n.jpg', 1132, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1538, 1, N'20191115000222300_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/15/20191115000222300_60339401_645961089264743_3587526765771227136_n.jpg', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1539, 2, N'20191124182341458_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/24/20191124182341458_55708876_2179766748765962_4578422690720776192_n.png', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1542, 1, N'20191116212912770_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116212912770_53226262_246257156318354_8760365182137401344_n.jpg', 1134, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1543, 1, N'20191116212916479_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/16/20191116212916479_56226055_2441636429202124_5126384707222634496_n.jpg', 1134, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1544, 2, N'20191116212919890_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116212919890_53226262_246257156318354_8760365182137401344_n.jpg', 1134, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1545, 1, N'20191116213742548_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116213742548_53226262_246257156318354_8760365182137401344_n.jpg', 1135, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1546, 1, N'20191116213745090_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/16/20191116213745090_53678158_1990595077736930_4994092537615482880_n.jpg', 1135, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1547, 1, N'20191116213853566_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116213853566_53226262_246257156318354_8760365182137401344_n.jpg', 1136, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1548, 1, N'20191116213902890_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/16/20191116213902890_56226055_2441636429202124_5126384707222634496_n.jpg', 1136, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1549, 2, N'20191116213858626_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/16/20191116213858626_60468641_2644069809212998_5188156361392783360_n.png', 1136, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1550, 1, N'20191116214757971_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/16/20191116214757971_53678158_1990595077736930_4994092537615482880_n.jpg', 1137, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1551, 2, N'20191116214801027_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116214801027_53226262_246257156318354_8760365182137401344_n.jpg', 1137, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1552, 2, N'20191116214819049_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/16/20191116214819049_53226262_246257156318354_8760365182137401344_n.jpg', 1137, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1553, 2, N'20191116214827738_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/16/20191116214827738_55708876_2179766748765962_4578422690720776192_n.png', 1137, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1554, 2, N'20191116214827738_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/16/20191116214827738_55708876_2179766748765962_4578422690720776192_n.png', 1137, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1574, 1, N'20191123235445580_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/23/20191123235445580_55708876_2179766748765962_4578422690720776192_n.png', 1110, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1575, 1, N'20191123192007631_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/23/20191123192007631_53226262_246257156318354_8760365182137401344_n.jpg', 1110, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1576, 1, N'20191123192010276_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/23/20191123192010276_56226055_2441636429202124_5126384707222634496_n.jpg', 1110, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1577, 2, N'20191123235405634_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/23/20191123235405634_55708876_2179766748765962_4578422690720776192_n.png', 1110, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1578, 3, N'20191123235456799_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/11/23/20191123235456799_56811293_2397744143623591_659462958098677760_n.jpg', 1110, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1579, 1, N'20191124213554751_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/11/24/20191124213554751_56811293_2397744143623591_659462958098677760_n.jpg', 1133, 0)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1580, 2, N'20191124214129770_60339401_645961089264743_3587526765771227136_n.jpg', N'/Upload/HoSo/2019/11/24/20191124214129770_60339401_645961089264743_3587526765771227136_n.jpg', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1581, 2, N'20191124215004958_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/24/20191124215004958_55708876_2179766748765962_4578422690720776192_n.png', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1582, 2, N'20191124215542296_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/24/20191124215542296_60468641_2644069809212998_5188156361392783360_n.png', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1583, 2, N'20191124215604136_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/24/20191124215604136_56226055_2441636429202124_5126384707222634496_n.jpg', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1584, 2, N'20191124215716865_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/24/20191124215716865_53226262_246257156318354_8760365182137401344_n.jpg', 1133, 0)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1585, 2, N'20191124215611324_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/24/20191124215611324_55708876_2179766748765962_4578422690720776192_n.png', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1586, 2, N'20191124215614730_60742236_645961052598080_3697703959792713728_n.jpg', N'/Upload/HoSo/2019/11/24/20191124215614730_60742236_645961052598080_3697703959792713728_n.jpg', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1587, 2, N'20191124215954387_58379111_2263027070428178_2210568582910181376_n.jpg', N'/Upload/HoSo/2019/11/24/20191124215954387_58379111_2263027070428178_2210568582910181376_n.jpg', 1133, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1588, 2, N'20191124215958231_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/24/20191124215958231_60468641_2644069809212998_5188156361392783360_n.png', 1133, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1589, 2, N'20191124220019408_63381726_2046521995652981_308630487260200960_n.png', N'/Upload/HoSo/2019/11/24/20191124220019408_63381726_2046521995652981_308630487260200960_n.png', 1133, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1590, 2, N'20191124220022270_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/24/20191124220022270_56226055_2441636429202124_5126384707222634496_n.jpg', 1133, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1591, 5, N'20191124220224275_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/24/20191124220224275_55708876_2179766748765962_4578422690720776192_n.png', 1133, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1592, 5, N'20191124220227437_60643267_1594088384057690_6813531428270112768_n.jpg', N'/Upload/HoSo/2019/11/24/20191124220227437_60643267_1594088384057690_6813531428270112768_n.jpg', 1133, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1595, 1, N'20191124222543194_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/24/20191124222543194_60468641_2644069809212998_5188156361392783360_n.png', 1146, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1596, 1, N'20191124222545954_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/24/20191124222545954_56226055_2441636429202124_5126384707222634496_n.jpg', 1146, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1597, 2, N'20191124222556663_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/24/20191124222556663_56226055_2441636429202124_5126384707222634496_n.jpg', 1146, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1598, 2, N'20191124222602125_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/24/20191124222602125_55708876_2179766748765962_4578422690720776192_n.png', 1146, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1599, 13, N'20191125002858324_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/25/20191125002858324_53226262_246257156318354_8760365182137401344_n.jpg', 1154, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1600, 14, N'20191125002902581_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/25/20191125002902581_55708876_2179766748765962_4578422690720776192_n.png', 1154, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1601, 1, N'20191125002916278_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/25/20191125002916278_53678158_1990595077736930_4994092537615482880_n.jpg', 1154, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1602, 2, N'20191125002920411_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/11/25/20191125002920411_56811293_2397744143623591_659462958098677760_n.jpg', 1154, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1605, 1, N'20191126130450230_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/26/20191126130450230_53226262_246257156318354_8760365182137401344_n.jpg', 1145, 0)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1606, 2, N'20191126130455043_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/26/20191126130455043_53678158_1990595077736930_4994092537615482880_n.jpg', 1145, 0)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1607, 1, N'20191126205155805_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/26/20191126205155805_53226262_246257156318354_8760365182137401344_n.jpg', 1157, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1608, 2, N'20191126205159287_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/26/20191126205159287_55708876_2179766748765962_4578422690720776192_n.png', 1157, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1611, 1, N'20191126205448823_53226262_246257156318354_8760365182137401344_n.jpg', N'/Upload/HoSo/2019/11/26/20191126205448823_53226262_246257156318354_8760365182137401344_n.jpg', 1158, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1612, 2, N'20191126205452543_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/26/20191126205452543_55708876_2179766748765962_4578422690720776192_n.png', 1158, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1613, 1, N'20191126212217212_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/26/20191126212217212_55708876_2179766748765962_4578422690720776192_n.png', 1162, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1614, 1, N'20191126212220255_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/26/20191126212220255_56226055_2441636429202124_5126384707222634496_n.jpg', 1162, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1615, 2, N'20191126212223608_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/11/26/20191126212223608_56811293_2397744143623591_659462958098677760_n.jpg', 1162, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1616, 4, N'20191126212228542_60742236_645961052598080_3697703959792713728_n.jpg', N'/Upload/HoSo/2019/11/26/20191126212228542_60742236_645961052598080_3697703959792713728_n.jpg', 1162, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1617, 1, N'20191126215118398_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/26/20191126215118398_55708876_2179766748765962_4578422690720776192_n.png', 1145, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1618, 1, N'20191126215922183_56811293_2397744143623591_659462958098677760_n.jpg', N'/Upload/HoSo/2019/11/26/20191126215922183_56811293_2397744143623591_659462958098677760_n.jpg', 1145, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1619, 4, N'20191126215926224_53678158_1990595077736930_4994092537615482880_n.jpg', N'/Upload/HoSo/2019/11/26/20191126215926224_53678158_1990595077736930_4994092537615482880_n.jpg', 1145, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1620, 13, N'20191126215942825_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/26/20191126215942825_60468641_2644069809212998_5188156361392783360_n.png', 1145, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1626, 1, N'20191126220429259_55708876_2179766748765962_4578422690720776192_n.png', N'/Upload/HoSo/2019/11/26/20191126220429259_55708876_2179766748765962_4578422690720776192_n.png', 1163, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1627, 2, N'20191205113517566_Capture.PNG', N'/Upload/HoSo/2019/12/5/20191205113517566_Capture.PNG', 1163, 0)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1628, 2, N'20191126220436300_60468641_2644069809212998_5188156361392783360_n.png', N'/Upload/HoSo/2019/11/26/20191126220436300_60468641_2644069809212998_5188156361392783360_n.png', 1163, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1629, 2, N'20191126220439608_56226055_2441636429202124_5126384707222634496_n.jpg', N'/Upload/HoSo/2019/11/26/20191126220439608_56226055_2441636429202124_5126384707222634496_n.jpg', 1163, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1630, 4, N'20191126220444054_60523035_645961315931387_321405403625684992_n.jpg', N'/Upload/HoSo/2019/11/26/20191126220444054_60523035_645961315931387_321405403625684992_n.jpg', 1163, NULL)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1631, 1, N'20191205112800283_Capture.PNG', N'/Upload/HoSo/2019/12/5/20191205112800283_Capture.PNG', 1169, 1)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So], [Deleted]) VALUES (1632, 1, N'20191205112839291_Capture.PNG', N'/Upload/HoSo/2019/12/5/20191205112839291_Capture.PNG', 1169, NULL)
SET IDENTITY_INSERT [dbo].[TAI_LIEU_HS] OFF
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (0, N'Lưu tạm', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (1, N'Nhập liệu', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (2, N'Thẩm định', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (3, N'Từ chối', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (4, N'Bổ sung hồ sơ', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (5, N'Giải ngân', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (6, N'Đã đối chiếu', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (7, N'Cancel', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (8, N'Lỗi PCB', 0)
SET IDENTITY_INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ON 

INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (1, 2)
INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (2, 3)
INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (3, 5)
INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (4, 7)
INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (5, 8)
INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] ([Id], [IdTrangThaiHoso]) VALUES (6, 1)
SET IDENTITY_INSERT [dbo].[TRANGTHAI_HS_IGNORE_TEAMLEAD] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([Id], [RoleId], [UserId], [Deleted]) VALUES (1, 1, 1, NULL)
INSERT [dbo].[UserRole] ([Id], [RoleId], [UserId], [Deleted]) VALUES (2, 2, 2, NULL)
INSERT [dbo].[UserRole] ([Id], [RoleId], [UserId], [Deleted]) VALUES (3, 1, 27, NULL)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
SET IDENTITY_INSERT [dbo].[UserRoleMenu] ON 

INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1, 1, N'admin', 1, N'Trang chủ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (2, 2, N'admin', 1, N'Giới thiệu')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (3, 3, N'sale', 2, N'Phiên bản')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (4, 4, N'admin', 1, N'Tạo hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (5, 5, N'sale', 2, N'Quản lý hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (6, 5, N'admin', 1, N'Quản lý hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (7, 6, N'admin', 1, N'Danh sách hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (8, 7, N'admin', 1, N'Duyệt hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (9, 3, N'admin', 1, N'Phiên bản')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1008, 4, N'sale', 2, N'Tạo hồ sơ')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1009, 8, N'admin', 1, N'Tổ  nhóm')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1010, 9, N'admin', 1, N'Tạo mới nhóm')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1011, 10, N'admin', 1, N'Quản lý nhóm')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1012, 11, N'admin', 1, N'Cấu hình duyệt')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1013, 12, N'admin', 1, N'Mã AP')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1014, 13, N'admin', 1, N'Quản lý mã AP')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1015, 14, N'admin', 1, N'Import')
INSERT [dbo].[UserRoleMenu] ([Id], [MenuId], [RoleCode], [RoleId], [MenuName]) VALUES (1016, 15, N'admin', 1, N'Nhân sự')
SET IDENTITY_INSERT [dbo].[UserRoleMenu] OFF
SET IDENTITY_INSERT [dbo].[UserScope] ON 

INSERT [dbo].[UserScope] ([Id], [UserId], [Role]) VALUES (1, 2, N'sale')
INSERT [dbo].[UserScope] ([Id], [UserId], [Role]) VALUES (2, 1, N'admin')
SET IDENTITY_INSERT [dbo].[UserScope] OFF
/****** Object:  StoredProcedure [dbo].[getTailieuByHosoId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getTailieuByHosoId](@hosoId int)
as
begin
select tl.Id as FileId, tl.Ma_Loai as [Key], tl.Ten as FileName
, tl.Duong_Dan_File as FileUrl,ltl.Ten as KeyName, ltl.Bat_Buoc as IsRequire  from TAI_LIEU_HS tl
inner join LOAI_TAI_LIEU ltl on tl.Ma_Loai = ltl.ID
where tl.Ma_Ho_So = @hosoId and ISNULL(tl.Deleted,0) = 0
order by ltl.Bat_Buoc desc
end

GO
/****** Object:  StoredProcedure [dbo].[removeTailieu]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[removeTailieu](@hosoId int, @tailieuId int)
as
begin
update TAI_LIEU_HS set Deleted = 1 where ID = @tailieuId;
end

GO
/****** Object:  StoredProcedure [dbo].[sp_AddMemberToTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_AddMemberToTeam](@teamId int, @userId int)
as
begin
Insert Into NHAN_VIEN_NHOM (Ma_Nhan_Vien, Ma_Nhom) Values (@userId, @teamId)
end


GO
/****** Object:  StoredProcedure [dbo].[sp_AllUserNotInTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
--sp_NHAN_VIEN_NHOM_LayDSKhongThanhVienNhom
CREATE PROCEDURE [dbo].[sp_AllUserNotInTeam](@id int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID as Id, NHAN_VIEN.Ten_Dang_Nhap + ' - ' + NHAN_VIEN.Ho_Ten as Name 
	From NHAN_VIEN Where NHAN_VIEN.ID not in 
	(Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = @id)
END











GO
/****** Object:  StoredProcedure [dbo].[sp_AUTOID_GetID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AUTOID_GetID]
	-- Add the parameters for the stored procedure here
	@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
select ID, Name_ID as NameID, Prefix, Suffixes,[Value] from AUTOID where ID=@ID
END












GO
/****** Object:  StoredProcedure [dbo].[sp_AUTOID_Update]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AUTOID_Update]
	-- Add the parameters for the stored procedure here
	@ID int,
	@Prefix nvarchar(10),
	@Suffixes nvarchar(10),
	@Value int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	update AUTOID set Prefix=@Prefix,Suffixes=@Suffixes,[Value]=@Value where ID=@ID
END












GO
/****** Object:  StoredProcedure [dbo].[sp_CheckIsAdmin]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_CheckIsAdmin] 
@userId as int
as 
begin
if exists(select top(1) * from NHAN_VIEN_QUYEN where Ma_NV = @userId)
 select isAdmin = 1;
else select isAdmin = 0
end

GO
/****** Object:  StoredProcedure [dbo].[sp_CheckIsTeamlead]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_CheckIsTeamlead] 
@userId as int
as 
begin
--select ID,Ma_Nguoi_QL as MaNguoiQL,
--Chuoi_Ma_Cha as ChuoiMaCha,
--Ten
-- from NHOM where Ma_Nhom_Cha in (select id from NHOM where Ma_Nguoi_QL = @userId)
--select ID,Ma_Nguoi_QL as MaNguoiQL,
--Chuoi_Ma_Cha as ChuoiMaCha,
--Ten
-- from NHOM where Ma_Nguoi_QL = @userId
if exists(select * from NHAN_VIEN_NHOM where Ma_Nhom in (select ID from NHOM where Ma_Nguoi_QL = @userId))
	select isTeamLead = 1
else
select isTeamlead = 0
end

GO
/****** Object:  StoredProcedure [dbo].[sp_CountAllMemberByTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--sp_NHAN_VIEN_NHOM_LayDSChiTietThanhVienNhom
create PROCEDURE [dbo].[sp_CountAllMemberByTeam](@id int)

AS
BEGIN
	Select count( nv.ID )
	 From NHAN_VIEN nv inner join  NHAN_VIEN_NHOM  nvn
	 on nv.ID = nvn.Ma_Nhan_Vien
	Where nvn.Ma_Nhom = @id
END














GO
/****** Object:  StoredProcedure [dbo].[sp_CountChildTeamByParentId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sp_CountChildTeamByParentId](@parentId int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select count(n.ID)
	From NHOM n inner join NHAN_VIEN nv 
	on n.Ma_Nguoi_QL = nv.ID
	 Where n.Ma_Nhom_Cha = @parentId
END


GO
/****** Object:  StoredProcedure [dbo].[sp_CountCustomer]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_CountCustomer]
(
@freeText nvarchar(30)
)
as
begin
if @freeText = '' begin set @freeText = null end;
Select count(*) from Customer n
where (@freeText is null or n.FullName like N'%' + @freeText + '%'
		or n.Cmnd like '%' + @freeText + '%'
		)
end

GO
/****** Object:  StoredProcedure [dbo].[sp_CountNhanvien]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from NHAN_VIEN

CREATE procedure [dbo].[sp_CountNhanvien]
(
@workFromDate datetime,
@workToDate datetime,
@roleId int,
@freeText nvarchar(30)
)
as
begin
if @freeText = '' begin set @freeText = null end;
Select count(*) from NHAN_VIEN n
where (convert(varchar(10),n.WorkDate,121) >= (convert(varchar(10),@workFromDate,121))
and convert(varchar(10),n.WorkDate,121) <= (convert(varchar(10),@workToDate,121)) or n.WorkDate is null)
	and (@freeText is null or n.Ho_Ten like N'%' + @freeText + '%'
		or n.Ten_Dang_Nhap like N'%' + @freeText + '%'
		or n.Dien_Thoai like N'%' + @freeText + '%'
		or n.Email like N'%' + @freeText + '%')
		and ((@roleId <> 0 and n.RoleId = @roleId) or (@roleId = 0 and (n.RoleId <> 0 or n.RoleId is null)))
		and n.Xoa = 0
end

GO
/****** Object:  StoredProcedure [dbo].[sp_Create_NhanvienQuyen]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Create_NhanvienQuyen] 
@userId as int,
 @maquyen as nvarchar(50)
as
begin
insert into NHAN_VIEN_QUYEN values (@userId,'fffffffff')
end

GO
/****** Object:  StoredProcedure [dbo].[sp_CreateTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_CreateTeam](@id int output,@name nvarchar(400), @shortName nvarchar(50),
@manageUserId int,@parentId int, @parentCode varchar(50))
as
begin
Insert Into NHOM (Ma_Nhom_Cha, Ma_Nguoi_QL, Ten_Viet_Tat, Ten, Chuoi_Ma_Cha) 
Values (@parentId, @manageUserId, @shortName, @name, @parentCode)
	Set @id = @@IDENTITY
end


GO
/****** Object:  StoredProcedure [dbo].[sp_DOI_TAC_LayDS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DOI_TAC_LayDS]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten,Isnull( F88Value,0) as F88Value from DOI_TAC
END












GO
/****** Object:  StoredProcedure [dbo].[sp_DOI_TAC_LayIDByMaSanPham]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DOI_TAC_LayIDByMaSanPham]
	-- Add the parameters for the stored procedure here
@MaSanPham int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select Ma_Doi_Tac as ID from SAN_PHAM_VAY where ID=@MaSanPham
END












GO
/****** Object:  StoredProcedure [dbo].[sp_getAllEmployeeSimpleList]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_getAllEmployeeSimpleList]
as
begin
	select Id,Ho_Ten as Name, Ten_Dang_Nhap as Code from NHAN_VIEN
end

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllMemberByTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--sp_NHAN_VIEN_NHOM_LayDSChiTietThanhVienNhom
create PROCEDURE [dbo].[sp_GetAllMemberByTeam](@id int,
@page int,
@limit int)

AS
BEGIN
	declare @offset int = (@page-1)*@limit;
	Select nv.ID as Id,
	 nv.Ten_Dang_Nhap as UserName,
	 nv.Ho_Ten as FullName, 
	 nv.Email, 
	 nv.Dien_Thoai as Phone 
	 From NHAN_VIEN nv inner join  NHAN_VIEN_NHOM  nvn
	 on nv.ID = nvn.Ma_Nhan_Vien
	Where nvn.Ma_Nhom = @id
	order by nv.Id asc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END














GO
/****** Object:  StoredProcedure [dbo].[sp_getAllNhomSimpleList]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_getAllNhomSimpleList]
as
begin
	Select NHOM.ID as Id, NHOM.Ten as Name, NHOM.Chuoi_Ma_Cha as Code From NHOM
end


Select * From NHOM

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllProduct]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_GetAllProduct](@partnerId int =0)
as
begin
if(@partnerId <=0)
begin
select ID as Id, Ten as Name, Ma as Code from SAN_PHAM_VAY;
end;
else
begin
select ID as Id, Ten as Name, Ma as Code from SAN_PHAM_VAY where Ma_Doi_Tac = @partnerId;
end;
end
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllUserInTeam]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
--sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhom
create PROCEDURE [dbo].[sp_GetAllUserInTeam] (@id int)
AS
BEGIN
	Select NHAN_VIEN.ID as Id, 
	NHAN_VIEN.Ten_Dang_Nhap + ' - ' + NHAN_VIEN.Ho_Ten as Name 
	From NHAN_VIEN, NHAN_VIEN_NHOM 
	Where NHAN_VIEN.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @id
END













GO
/****** Object:  StoredProcedure [dbo].[sp_GetChildTeamByParentId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetChildTeamByParentId](@parentId int,
@page int,
@limit int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare @offset int = (@page-1)*@limit;
	Select n.Id as Id, n.Ten as Name, 
	n.Ten_Viet_Tat as ShortName, 
	nv.Ho_Ten as ManageUser
	From NHOM n inner join NHAN_VIEN nv 
	on n.Ma_Nguoi_QL = nv.ID
	 Where n.Ma_Nhom_Cha = @parentId
	 order by n.Id asc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END


GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomer]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetCustomer]
(
@freeText nvarchar(30),
@offset int,
@limit int
)
as
begin
if @freeText = '' begin set @freeText = null end;
Select *
 from Customer n 
where(@freeText is null or n.FullName like N'%' + @freeText + '%'
		
		or n.Cmnd like '%' + @freeText + '%'
		)
order by n.Id desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
end


GO
/****** Object:  StoredProcedure [dbo].[sp_GetGhichuByHosoId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_GetGhichuByHosoId] 
@hosoId as int
as 
begin
select g.*,  Concat(CONCAT(n.Ma, ' - '),n.Ho_Ten) as Commentator from Ghichu g left join NHAN_VIEN n
on g.UserId = n.ID where g.HosoId= @hosoId
order by Id desc
end

GO
/****** Object:  StoredProcedure [dbo].[sp_getListPartnerForCustomerCheck]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_getListPartnerForCustomerCheck]
as
begin
select ID as Id, Ten as Name from DOI_TAC where AllowCheckDup =1;	
end

GO
/****** Object:  StoredProcedure [dbo].[sp_GetNhanvien]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from NHAN_VIEN

CREATE procedure [dbo].[sp_GetNhanvien]
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
n.WorkDate,CONCAT(k.Ten + ' - ',k2.Ten) as Location
 from NHAN_VIEN n left join KHU_VUC k on n.DistrictId = k.ID
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



GO
/****** Object:  StoredProcedure [dbo].[sp_GetNotesByCustomerId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_GetNotesByCustomerId] 
@customerId as int
as 
begin
select c.*,  Concat(CONCAT(n.Ma, ' - '),n.Ho_Ten) as Commentator from CustomerNote c left join NHAN_VIEN n
on c.CreatedBy = n.ID where c.CustomerId= @customerId
order by Id desc
end

GO
/****** Object:  StoredProcedure [dbo].[sp_getPermissionByUserId]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_getPermissionByUserId]
(@userId int)
as
begin
	select Permission from RolePermission where roleId in
		(select RoleId from userrole where userId =@userId) 
end

GO
/****** Object:  StoredProcedure [dbo].[sp_GetTeamById]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_GetTeamById](@id int,@isGetForDetail bit = 0)
AS
BEGIN
--sp_NHOM_LayThongTinTheoMa
	
	if(@isGetForDetail = 1)
	begin
		Select NHOM.ID as Id, 
		NHOM.Ten as Name, NHOM.Ten_Viet_Tat as ShortName, 
		NHOM.Ma_Nhom_Cha as ParentTeamCode, 
		NHOM.Ma_Nguoi_QL as ManageUserId, 
		NHOMCHA.Ten as ParentName, 
		NHAN_VIEN.Ho_Ten as LeaderName
		From NHAN_VIEN, NHOM
		left join NHOM as NHOMCHA on NHOM.Ma_Nhom_Cha = NHOMCHA.ID
		Where NHOM.ID = @id and NHAN_VIEN.ID = NHOM.Ma_Nguoi_QL
	end
	else
	begin
		Select NHOM.ID as Id, 
	NHOM.Ten as Name, NHOM.Ten_Viet_Tat as ShortName, 
	NHOM.Ma_Nhom_Cha as ParentTeamCode, 
	NHOM.Ma_Nguoi_QL as ManageUserId
	From NHOM Where NHOM.ID = @id
	end
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhat]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_CapNhat]
@ID int,
@TenKhachHang nvarchar(50),
@CMND nvarchar(50),
@DiaChi ntext,
@CourierCode int,
@MaKhuVuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@GioiTinh int,

@NgayNhanDon datetime,
@MaTrangThai int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@KetQuaHS int,
@TenCuaHang nvarchar(200),
@UpdatedUserId int =0,
@UpdatedDate datetime = null,
@birthDay datetime,
@cmndDay datetime,
@HoSoCuaAi int = 0
AS
BEGIN
if(@UpdatedDate is null) begin set @UpdatedDate = GETDATE() end;
	UPDATE HO_SO SET Ten_Khach_Hang=Upper(@TenKhachHang),
		CMND=@CMND,Dia_Chi=@DiaChi, Courier_Code=@CourierCode,Ma_Khu_Vuc=@MaKhuVuc,SDT=@SDT,SDT2=@SDT2,Gioi_Tinh=@GioiTinh,
		Ngay_Nhan_Don=@NgayNhanDon,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ngay_Cap_Nhat = @UpdatedDate,Ma_Nguoi_Cap_Nhat = @UpdatedUserId,
		Ma_Ket_Qua=@KetQuaHS,
		BirthDay = @birthDay,
		CMNDDay= @cmndDay
	WHERE ID=@ID
END













GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatDaXoa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_CapNhatDaXoa] 
	-- Add the parameters for the stored procedure here
	@ID int,
	@NhanVienID int,
	@NgayThaoTac datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Update HO_SO Set HO_SO.Ma_Nguoi_Cap_Nhat = @NhanVienID, HO_SO.Ngay_Cap_Nhat = @NgayThaoTac, HO_SO.Da_Xoa = 1 
	Where HO_SO.ID = @ID
END













GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatHoSo]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_CapNhatHoSo]
@ID int,
@TenKhachHang nvarchar(50),
@CMND nvarchar(50),
@DiaChi ntext,
@CourierCode int,
@MaKhuVuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@GioiTinh int,
@MaNguoiCapNhat int,
@HoSoCuaAi int,
@NgayNhanDon datetime,
@NgayCapNhat datetime,
@MaTrangThai int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@KetQuaHS int,
@TenCuaHang nvarchar(200) = '',
@birthDay datetime,
@cmndDay datetime
AS
BEGIN
	UPDATE HO_SO SET Ten_Khach_Hang=Upper(@TenKhachHang),
		CMND=@CMND,Dia_Chi=@DiaChi, Courier_Code=@CourierCode,Ma_Khu_Vuc=@MaKhuVuc,SDT=@SDT,SDT2=@SDT2,Gioi_Tinh=@GioiTinh,
		Ngay_Nhan_Don=@NgayNhanDon, Ma_Nguoi_Cap_Nhat=@MaNguoiCapNhat,Ho_So_Cua_Ai=@HoSoCuaAi,Ngay_Cap_Nhat=@NgayCapNhat,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ma_Ket_Qua=@KetQuaHS,
		BirthDay = @birthDay,
    CMNDDay= @cmndDay
	WHERE ID=@ID
END













GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatTrangThaiHS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_CapNhatTrangThaiHS] 
	-- Add the parameters for the stored procedure here
	@ID int,
	@MaNguoiThaoTac int,
	@NgayThaoTac datetime,
	@MaTrangThai int,
	@MaKetQua int,
	@GhiChu nvarchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Update HO_SO Set Ma_Nguoi_Cap_Nhat = @MaNguoiThaoTac, Ngay_Cap_Nhat = @NgayThaoTac, Ma_Trang_Thai = @MaTrangThai, Ma_Ket_Qua = @MaKetQua, Ghi_Chu = @GhiChu
	Where HO_SO.ID = @ID
END













GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_Count_TimHoSoDuyet]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_Count_TimHoSoDuyet] 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@freeText as nvarchar(50) = '',
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
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
	Select count(*) as TotalRecord
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM
		 Where NHAN_VIEN_NHOM.Ma_Nhom 
		 in (Select NHOM.ID From NHOM
		  Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID))
		  like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		  or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
		  like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao 
		in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) 
	and CONVERT(date, @DenNgay) 
	and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ',')) 
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
END







GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_Count_TimHoSoQuanLy]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_Count_TimHoSoQuanLy] 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@TrangThai nvarchar(50),
	@freeText as nvarchar(50) = '',
	@LoaiNgay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	if @MaHS is null begin set @MaHS ='' end;
	if @CMND is null begin set @CMND ='' end;
	set @TrangThai = @TrangThai + ',6,7,8'
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	-- Lay nhom la thanh vien
	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @MaNVDangNhap
	-- Lay nhom con truc tiep (nv la nguoi quan ly)
	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
	(
		Select * From @DSNhomQL
	)) > 0
	End
	Select count(*) as TotalRecord
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) 
			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
	and (HO_SO.CMND like '%'+@CMND+'%')
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) 
	or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
END




GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_DUYET_XEM_DaXem]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_DUYET_XEM_DaXem]
@ID int
AS
BEGIN
	update   HO_SO_DUYET_XEM set Xem=1 where Ma_Ho_So=@ID

END








GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_DUYET_XEM_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_DUYET_XEM_Them]
@ID int
AS
BEGIN
	delete  from HO_SO_DUYET_XEM where Ma_Ho_So=@ID
	INSERT HO_SO_DUYET_XEM(Ma_Ho_So,Xem)values(@ID,0)
END









GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_LayChiTiet]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_LayChiTiet]
	-- Add the parameters for the stored procedure here
	@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select hs.ID
	,Ma_Ho_So as MaHoSo
	,Ten_Khach_Hang as TenKhachHang
	,CMND,Dia_Chi as DiaChi
	,Courier_Code as CourierCode,
	SDT,SDT2
	,Gioi_Tinh as GioiTinh
	,hs.Ngay_Tao as NgayTao
	,hs.Ma_Nguoi_Tao as MaNguoiTao,
	Ma_Khu_Vuc as MaKhuVuc
	, tt.Ten as TenTrangThai
	, kq.Ten as KetQuaText,
	Ho_So_Cua_Ai as HoSoCuaAi
	,Ngay_Cap_Nhat as NgayCapNhat
	,Ma_Nguoi_Cap_Nhat as MaNguoiCapNhat,
	Ngay_Nhan_Don as NgayNhanDon
	,Ma_Trang_Thai as MaTrangThai
	,Ma_Ket_Qua as MaKetQua,San_Pham_Vay as SanPhamVay
	,sv.Ten as ProductName
	,Ten_Cua_Hang as TenCuaHang
	,Co_Bao_Hiem as CoBaoHiem,So_Tien_Vay as SoTienVay,Han_Vay as HanVay,Ghi_Chu as GhiChu
	,BirthDay,CmndDay,
	dbo.getDoitacBySanphamId(sv.ID,0) as Doitac,
	dbo.getDoitacBySanphamId(sv.ID,1) as PartnerId,
	sv.Ten as Sanpham
	,dbo.getProvinceNameFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceName
	,dbo.getProvinceIdFromDistrictId(hs.Ma_Khu_Vuc) as ProvinceId
	,kv.Ten as DistrictName,
	nv.Ten_Dang_Nhap as SaleCode,
	nv.Ho_Ten as EmployeeName,
	nv.Dien_Thoai as EmployeePhone
	from HO_SO hs
	left join TRANG_THAI_HS tt on tt.ID=hs.Ma_Trang_Thai
	left join KET_QUA_HS kq on kq.ID=hs.Ma_Ket_Qua
	left join SAN_PHAM_VAY sv on hs.San_Pham_Vay = sv.ID
	left join KHU_VUC kv on hs.Ma_Khu_Vuc = kv.ID
	left join NHAN_VIEN nv on hs.Ho_So_Cua_Ai = nv.ID
	 where hs.ID=@ID
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_Them]
@ID int out,
@MaHoSo nvarchar(50),
@TenKhachHang nvarchar(50),
@CMND nvarchar(50),
@DiaChi ntext,
@CourierCode int,
@MaKhuVuc int,
@SDT nvarchar(50),
@SDT2 nvarchar(50),
@GioiTinh int,
@NgayTao datetime,
@MaNguoiTao int,
@HoSoCuaAi int,
@NgayNhanDon datetime,
@MaTrangThai int,
@KetQuaHS int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@TenCuaHang nvarchar(200),
@birthDay datetime,
@cmndDay datetime
AS
BEGIN
 INSERT HO_SO
 (Ma_Ho_So, Ten_Khach_Hang,CMND,Dia_Chi, Courier_Code,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,Ngay_Tao,Ma_Nguoi_Tao,Ho_So_Cua_Ai
 ,Ngay_Nhan_Don, Ma_Trang_Thai, Ma_Ket_Qua,San_Pham_Vay,Ten_Cua_Hang,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Da_Xoa, Ngay_Cap_Nhat,
 BirthDay,CMNDDay)
 VALUES(@MaHoSo,Upper(@TenKhachHang),@CMND,@DiaChi,@CourierCode,@MaKhuVuc,@SDT,@SDT2,@GioiTinh,@NgayTao,@MaNguoiTao,
 @HoSoCuaAi,@NgayNhanDon,@MaTrangThai,@KetQuaHS,@SanPhamVay,@TenCuaHang,@CoBaoHiem,@SoTienVay,@HanVay,0, @NgayTao,
 @birthDay,@cmndDay) 
	SET @ID=@@IDENTITY
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoCuaToi]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoCuaToi] 
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
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left  join SAN_PHAM_VAY on HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
		left join DOI_TAC on SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
		left join TRANG_THAI_HS on  HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	    left join KET_QUA_HS on HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	Where 
	 HO_SO.Ma_Nguoi_Tao = @MaNhanVien
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@SDT+'%'
	and (HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay))
	and HO_SO.Da_Xoa = 0 
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.Ngay_Tao desc
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoCuaToiChuaXem]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoCuaToiChuaXem] 
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
	From HO_SO_XEM,HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
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











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoDuyet]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoDuyet] 
	-- Add the parameters for the stored procedure here
	@offset int,
	@limit int,
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@LoaiNgay int,
	@freeText as nvarchar(50)='',
	@TrangThai nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
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
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao,HO_SO.F88Result,
	 DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,
	 HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS,
	  KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, 
	  NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua,
	   HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, dbo.fn_getGhichuByHosoId(HO_SO.ID) as GhiChu, 
	   NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM
	 Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0)
	  THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	  Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao)
	   END 
	   as DoiNguBanHang, SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM
		 Where NHAN_VIEN_NHOM.Ma_Nhom 
		 in (Select NHOM.ID From NHOM
		  Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID))
		  like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
		  or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) 
		  like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao 
		in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) 
	and CONVERT(date, @DenNgay) 
	and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or TRANG_THAI_HS.Ten like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
	order by HO_SO.Ngay_Cap_Nhat desc 
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END




GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem] 
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
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
	(
		Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap
	)) > 0
	or NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where Ma_Nhan_Vien = @MaNVDangNhap)
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, kvt.Ten as DiaChiKH, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang
	From HO_SO_DUYET_XEM,HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where
	HO_SO_DUYET_XEM.Xem=0
	and HO_SO_DUYET_XEM.Ma_Ho_So=HO_SO.ID
	and HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and ((@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)))
		or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)))
		)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.SDT like '%'+@CMND+'%'
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
END








GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoQuanLy]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoQuanLy] 
	-- Add the parameters for the stored procedure here
	@offset int,
	@limit int,
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@TrangThai nvarchar(50),
	@freeText as nvarchar(50) = '',
	@LoaiNgay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if @freeText = '' begin set @freeText = null end;
	if @MaHS is null begin set @MaHS ='' end;
	if @CMND is null begin set @CMND ='' end;
	set @TrangThai = @TrangThai + ',6,7,8'
	Declare @DSNhomQL TABLE(ID int)
	if(@MaNhom = 0)
	Begin
	-- Lay nhom la thanh vien
	Insert Into @DSNhomQL Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @MaNVDangNhap
	-- Lay nhom con truc tiep (nv la nguoi quan ly)
	Insert Into @DSNhomQL Select NHOM.ID From NHOM Where NHOM.Ma_Nhom_Cha in (select * from @DSNhomQL)
	-- Lay ds nhom tu nhom quan ly truc tiep tro xuong
	Insert Into @DSNhomQL Select distinct NHOM.ID From NHOM Where (Select COUNT(*) From fn_SplitStringToTable(NHOM.Chuoi_Ma_Cha, '.') Where CONVERT(int, value) in
	(
		Select * From @DSNhomQL
	)) > 0
	End
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND,
	 HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai ,TRANG_THAI_HS.Ten as TrangThaiHS,
	 KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang,
	  NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,
	  kvt.Ten as KhuVucText, dbo.fn_getGhichuByHosoId(HO_SO.ID) as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) 
	THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM 
	Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao)
	 END as DoiNguBanHang, SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY,TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
	and SAN_PHAM_VAY.Ma_Doi_Tac = DOI_TAC.ID
	and (
			(@MaThanhVien > 0 and HO_SO.Ma_Nguoi_Tao = @MaThanhVien) 
			or (@MaNhom > 0 and @MaThanhVien = 0 and HO_SO.Ma_Nguoi_Tao in (
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien 
			From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in 
			(Select NHOM.ID From NHOM Where 
			((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') 
			or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 
			Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and (HO_SO.Ma_Ho_So like '%'+@MaHS+'%')
	and (HO_SO.CMND like '%'+@CMND+'%')
	and ((convert(varchar(10), HO_SO.Ngay_Tao,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 1) 
	or ( convert(varchar(10),HO_SO.Ngay_Cap_Nhat,121) between CONVERT(varchar(10), @TuNgay,121) and CONVERT(varchar(10), @DenNgay,121) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	and (@freeText is null or HO_SO.Ma_Ho_So like N'%' + @freeText + '%'
		or DOI_TAC.Ten like N'%' + @freeText + '%'
		or HO_SO.CMND like N'%' + @freeText + '%'
		or HO_SO.Ten_Khach_Hang like N'%' + @freeText + '%'
		or KET_QUA_HS.Ten like N'%' + @freeText + '%'
		or NV1.Ma like N'%' + @freeText + '%'
		or NV1.Ho_Ten like N'%' + @freeText + '%'
		or NV2.Ma like N'%' + @freeText + '%'
		or NV3.Ma like N'%' + @freeText + '%'
		or kvt.Ten like N'%' + @freeText + '%'
		or SAN_PHAM_VAY.Ten like N'%' + @freeText + '%'
	)
	order by HO_SO.Ngay_Cap_Nhat desc
	offset @offset ROWS FETCH NEXT @limit ROWS ONLY
END




GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_XEM_DaXem]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_XEM_DaXem]
@ID int
AS
BEGIN
	update   HO_SO_XEM set Xem=1 where Ma_Ho_So=@ID

END









GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_XEM_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HO_SO_XEM_Them]
@ID int
AS
BEGIN
	delete  from HO_SO_XEM where Ma_Ho_So=@ID
	INSERT HO_SO_XEM(Ma_Ho_So,Xem)values(@ID,0)
END









GO
/****** Object:  StoredProcedure [dbo].[sp_InserCustomerCheck]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_InserCustomerCheck]
(
@id int out,
@customerId int,
@partnerId int,
@status int,
@createdTime datetime,
@createdBy int
)
as
begin
insert into CustomerCheck(CustomerId, PartnerId,Status,CreatedTime,CreatedBy)
values (@customerId,@partnerId,@status,@createdTime,@createdBy);
SET @id=@@IDENTITY;

end

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCustomer]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE procedure [dbo].[sp_InsertCustomer]
  (
  @id int out,
  @fullname nvarchar(200),
  @checkdate datetime,
  @cmnd varchar(15),
  @gender bit,
  @status int,
  @note nvarchar(200),
  @createdtime datetime,
  @createdby int
  )
  as
  begin
	insert into Customer(FullName,CheckDate,Cmnd,CICStatus,LastNote,Gender,CreatedTime,CreatedBy)
	values
	(@fullname,@checkdate,@cmnd,0,@note,@gender,@createdtime,@createdby);
	SET @id=@@IDENTITY;

  end

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertUser]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_InsertUser]
(
@id int out,
@userName varchar(50),
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
insert into NHAN_VIEN (Ten_Dang_Nhap,Mat_Khau,Ho_Ten,Dien_Thoai,Email,RoleId,WorkDate,ProvinceId,DistrictId,Trang_Thai,Xoa,CreatedTime,CreatedBy)
values (@userName,@password,@fullName,@phone,@email,@roleId,@workDate,@provinceId,@districtId,1,0,@createdtime,@createdby);
SET @id=@@IDENTITY
end



GO
/****** Object:  StoredProcedure [dbo].[sp_KET_QUA_HS_LayDSKetQua]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_KET_QUA_HS_LayDSKetQua] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select ID, Ten From KET_QUA_HS Where Da_Xoa = 0
END













GO
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayDSHuyen]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_KHU_VUC_LayDSHuyen]
	-- Add the parameters for the stored procedure here
	@MaTinh int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten from KHU_VUC where Ma_Cha=@MaTinh and Ma_Cha <>0
END












GO
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayDSTinh]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_KHU_VUC_LayDSTinh]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten from KHU_VUC where Loai=1
END












GO
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayMaTinhByMaHuyen]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_KHU_VUC_LayMaTinhByMaHuyen]
	-- Add the parameters for the stored procedure here
@MaHuyen int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select Ma_Cha as ID from KHU_VUC where ID=@MaHuyen 
END










GO
/****** Object:  StoredProcedure [dbo].[sp_LOAI_TAI_LIEU_LayDS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LOAI_TAI_LIEU_LayDS]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ten,Bat_Buoc as BatBuoc from LOAI_TAI_LIEU
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_CF_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_CF_Them] 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Insert Into NHAN_VIEN_CF (Ma_Nhan_Vien, Ma_Nhom, Quyen) Values (@MaNhanVien, @MaNhom, 0)
END








GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_CF_Xoa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_CF_Xoa]
	-- Add the parameters for the stored procedure here
	@MaNhanVien int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Delete From NHAN_VIEN_CF Where NHAN_VIEN_CF.Ma_Nhan_Vien = @MaNhanVien
END








GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_GetUserByID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_GetUserByID]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma as Code,Ho_Ten as FullName,Ten_Dang_Nhap as UserName,
	Mat_Khau as [Password],Dien_Thoai as Phone,Email as Email,'1' as [Type]  
	from NHAN_VIEN where ID=@UserID
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSByMaQL]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_LayDSByMaQL]
	-- Add the parameters for the stored procedure here
	@MaQL int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma as Code, NHAN_VIEN.Ma + ' - ' + Ho_Ten as HoTen from NHAN_VIEN where ID=@MaQL
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSByRule]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_LayDSByRule]
	-- Add the parameters for the stored procedure here
	@UserID int,
	@Rule int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select distinct NHAN_VIEN.ID,Ma as Code,Ho_Ten as FullName,Ten_Dang_Nhap as UserName,Dien_Thoai as Phone,Email as Email 
	from NHAN_VIEN,NHAN_VIEN_CF
	where --ID=@UserID
	 NHAN_VIEN.ID=NHAN_VIEN_CF.Ma_Nhan_Vien
	and NHAN_VIEN_CF.Quyen=@Rule
	and NHAN_VIEN_CF.Ma_Nhom in (Select Ma_Nhom from NHAN_VIEN_NHOM where Ma_Nhan_Vien=@UserID)

END








GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSCourierCode]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_LayDSCourierCode]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select ID,Ma+'-'+Ho_Ten as [FullText],Ma+'-'+Ho_Ten as Ten from NHAN_VIEN where Loai=10
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_Login]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_Login]
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50),
	@Password nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select ID as ID,Ma as Code,Ten_Dang_Nhap as UserName, Ho_Ten as FullName,Email, Dien_Thoai as Phone
	 From NHAN_VIEN where Ten_Dang_Nhap=@UserName and Mat_Khau=@Password and [Trang_Thai]=1
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChiTietThanhVienNhom]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_LayDSChiTietThanhVienNhom]
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID, NHAN_VIEN.Ma, NHAN_VIEN.Ma + ' - ' + NHAN_VIEN.Ho_Ten as HoTen, NHAN_VIEN.Email, NHAN_VIEN.Dien_Thoai as SDT From NHAN_VIEN, NHAN_VIEN_NHOM 
	Where NHAN_VIEN.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @MaNhom
END














GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhom]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhom] 
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID, NHAN_VIEN.Ma + ' - ' + NHAN_VIEN.Ho_Ten as Ten From NHAN_VIEN, NHAN_VIEN_NHOM Where NHAN_VIEN.ID = NHAN_VIEN_NHOM.Ma_Nhan_Vien and NHAN_VIEN_NHOM.Ma_Nhom = @MaNhom
END













GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhomCaCon]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhomCaCon]
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID, NHAN_VIEN.Ma + ' - ' + NHAN_VIEN.Ho_Ten as Ten, NHAN_VIEN.Ma as Code From NHAN_VIEN
	Where NHAN_VIEN.ID in (
		Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM Where((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
	)
END













GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSKhongThanhVienNhom]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_LayDSKhongThanhVienNhom]
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID, NHAN_VIEN.Ho_Ten as Ten From NHAN_VIEN Where NHAN_VIEN.ID not in (Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = @MaNhom)
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_Them] 
	-- Add the parameters for the stored procedure here
	@MaNhanVien int,
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Insert Into NHAN_VIEN_NHOM (Ma_Nhan_Vien, Ma_Nhom) Values (@MaNhanVien, @MaNhom)
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_Xoa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHAN_VIEN_NHOM_Xoa] 
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Delete From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = @MaNhom
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHANVIEN_ChangePass]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NHANVIEN_ChangePass]
	-- Add the parameters for the stored procedure here
	@ID int,
	@Pass nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	 update NHAN_VIEN set Mat_Khau=@Pass where ID=@ID
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHANVIEN_LayDS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHANVIEN_LayDS] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHAN_VIEN.ID, NHAN_VIEN.Ma + ' - ' + NHAN_VIEN.Ho_Ten as HoTen From NHAN_VIEN
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayCayNhomCon]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayCayNhomCon] 
	-- Add the parameters for the stored procedure here
	@MaNhomCha int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nguoi_QL as MaNguoiQL, 
	NHAN_VIEN.Ho_Ten as TenQL From NHOM, NHAN_VIEN 
	Where NHOM.Ma_Nguoi_QL = NHAN_VIEN.ID 
	and ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhomCha) + '.%') 
	or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhomCha))
END








GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayChuoiMaChaCuaMaNhom]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayChuoiMaChaCuaMaNhom] 
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select isnull(NHOM.Chuoi_Ma_Cha,'') as ChuoiMaCha From NHOM Where NHOM.ID = @MaNhom
END








GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSChonTheoNhanVien]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayDSChonTheoNhanVien]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--Select NHOM.ID, NHOM.Ten From NHAN_VIEN_CF, NHOM Where NHAN_VIEN_CF.Ma_Nhom = NHOM.ID and NHAN_VIEN_CF.Ma_Nhan_Vien = @UserID
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nhom_Cha as MaNhomCha, NHAN_VIEN.Ho_Ten as TenQL
	 From NHOM, NHAN_VIEN 
	Where NHOM.Ma_Nguoi_QL = NHAN_VIEN.ID and NHOM.ID in 
	(Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @UserID)
END













GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhom]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayDSNhom]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha From NHOM
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhomCon]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayDSNhomCon] 
	-- Add the parameters for the stored procedure here
	@MaNhomCha int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Ten_Viet_Tat as TenNgan, NHAN_VIEN.Ho_Ten as NguoiQuanLy, NHOM.Chuoi_Ma_Cha as ChuoiMaCha From NHOM, NHAN_VIEN Where NHOM.Ma_Nguoi_QL = NHAN_VIEN.ID and NHOM.Ma_Nhom_Cha = @MaNhomCha
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhomDuyetChonTheoNhanVien]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayDSNhomDuyetChonTheoNhanVien]
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--Select NHOM.ID, NHOM.Ten From NHAN_VIEN_CF, NHOM Where NHAN_VIEN_CF.Ma_Nhom = NHOM.ID and NHAN_VIEN_CF.Ma_Nhan_Vien = @UserID
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nhom_Cha as MaNhomCha, NHAN_VIEN.Ho_Ten as TenQL From NHOM, NHAN_VIEN 
	Where NHAN_VIEN.ID = NHOM.Ma_Nguoi_QL and NHOM.ID in (Select NHAN_VIEN_CF.Ma_Nhom From NHAN_VIEN_CF Where NHAN_VIEN_CF.Ma_Nhan_Vien = @UserID)
END









GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayThongTinChiTietTheoMa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayThongTinChiTietTheoMa] 
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Ten_Viet_Tat as TenNgan, NHOMCHA.Ten as TenNhomCha, NHAN_VIEN.Ho_Ten as NguoiQuanLy
	From NHAN_VIEN, NHOM
	left join NHOM as NHOMCHA on NHOM.Ma_Nhom_Cha = NHOMCHA.ID
	Where NHOM.ID = @MaNhom and NHAN_VIEN.ID = NHOM.Ma_Nguoi_QL
END












GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayThongTinTheoMa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_LayThongTinTheoMa] 
	-- Add the parameters for the stored procedure here
	@MaNhom int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select NHOM.ID, NHOM.Ten, NHOM.Ten_Viet_Tat as TenNgan, NHOM.Ma_Nhom_Cha as MaNhomCha, NHOM.Ma_Nguoi_QL as MaNguoiQuanLy From NHOM Where NHOM.ID = @MaNhom
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_Sua]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_Sua]
	-- Add the parameters for the stored procedure here
	@ID int,
	@MaNhomCha int,
	@MaNguoiQL int,
	@TenVietTat nvarchar(50),
	@Ten nvarchar(200),
	@ChuoiMaCha nvarchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Update NHOM Set NHOM.Ma_Nhom_Cha = @MaNhomCha, NHOM.Ma_Nguoi_QL = @MaNguoiQL, NHOM.Ten = @Ten, NHOM.Ten_Viet_Tat = @TenVietTat, Chuoi_Ma_Cha = @ChuoiMaCha Where NHOM.ID = @ID
END










GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sang Nguyen>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NHOM_Them]
	-- Add the parameters for the stored procedure here
	@ID int output,
	@MaNhomCha int,
	@MaNguoiQL int,
	@TenVietTat nvarchar(50),
	@Ten nvarchar(200),
	@ChuoiMaCha nvarchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Insert Into NHOM (Ma_Nhom_Cha, Ma_Nguoi_QL, Ten_Viet_Tat, Ten, Chuoi_Ma_Cha) Values (@MaNhomCha, @MaNguoiQL, @TenVietTat, @Ten, @ChuoiMaCha)
	Set @ID = @@IDENTITY
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Remove_NhanvienQuyen]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Remove_NhanvienQuyen] 
@userId as int
as
begin
delete from NHAN_VIEN_QUYEN  where Ma_NV = @userId;
end

GO
/****** Object:  StoredProcedure [dbo].[sp_RULE_GetWidthID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_RULE_GetWidthID]
@ID nvarchar(50)
AS
BEGIN
 SELECT Quyen as [Rule] FROM NHAN_VIEN_QUYEN WHERE Ma_NV=@ID
END














GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_CapNhatSuDung]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_CapNhatSuDung]
	-- Add the parameters for the stored procedure here
	@ID int,
	@SanPhamVay int
AS
BEGIN
	Declare @SanPhamVayOld int
	select  @SanPhamVayOld=San_Pham_Vay from HO_SO where ID=@ID
	if(@SanPhamVayOld<>@SanPhamVay)
		begin
		Declare @IDDoiTacOld int
		select @IDDoiTacOld=Ma_Doi_Tac from SAN_PHAM_VAY where ID=@SanPhamVayOld
		if(@IDDoiTacOld=3)
		begin
			 if((select COUNT(*) from SAN_PHAM_VAY where ID=@SanPhamVayOld and Loai=2)>0)
			 begin
				update SAN_PHAM_VAY set Loai=1  where ID=@SanPhamVayOld
			 end

		end
	end
	Declare @IDDoiTac int
	select @IDDoiTac=Ma_Doi_Tac from SAN_PHAM_VAY where ID=@SanPhamVay
	if(@IDDoiTac=3)
	begin
		 if((select COUNT(*) from SAN_PHAM_VAY where ID=@SanPhamVay and Loai=1)>0)
		 begin
			update SAN_PHAM_VAY set Loai=2  where ID=@SanPhamVay
		 end

	end
END






GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_CheckExist]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_CheckExist]
	-- Add the parameters for the stored procedure here
	@ID int,
	@SanPhamVay int,
	@Exist int output
AS
BEGIN
	set @Exist=0;
	Declare @IDDoiTac int
	select @IDDoiTac=Ma_Doi_Tac from SAN_PHAM_VAY where ID=@SanPhamVay
	if(@IDDoiTac=3)
	begin
		 if((select COUNT(*) from SAN_PHAM_VAY where ID=@SanPhamVay and Loai=2)>0)
		 begin
			Declare @IDHoSo int
			select @IDHoSo=ID from HO_SO where San_Pham_Vay=@SanPhamVay
			if(@IDHoSo<>0 and @ID<>@IDHoSo)
			begin
				set @Exist=1
			end
		 end

	end
	
END







GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_DemTrungMa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_DemTrungMa]
	-- Add the parameters for the stored procedure here
	@Ma nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select COUNT(*) From SAN_PHAM_VAY Where N''+@Ma+'' = SAN_PHAM_VAY.Ma
END






GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSByID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_LayDSByID]
	-- Add the parameters for the stored procedure here
	@MaDoiTac int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@MaDoiTac=3)
	begin
		select ID,Ten from SAN_PHAM_VAY where Ma_Doi_Tac=@MaDoiTac and Loai=1
	end
	else
		begin
			select ID,Ten from SAN_PHAM_VAY where Ma_Doi_Tac=@MaDoiTac
		end
	
END






GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSByIDAndMaHS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_LayDSByIDAndMaHS]
	-- Add the parameters for the stored procedure here
	@MaDoiTac int,
	@MaHS int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@MaDoiTac=3)
	begin
		Declare @SanPhamVay int
		select @SanPhamVay=San_Pham_Vay from HO_SO,SAN_PHAM_VAY where HO_SO.ID=@MaHS and HO_SO.San_Pham_Vay=SAN_PHAM_VAY.ID and Loai=2 and SAN_PHAM_VAY.Ma_Doi_Tac=3
		select ID,Ten from SAN_PHAM_VAY where Ma_Doi_Tac=@MaDoiTac and (SAN_PHAM_VAY.Loai=1 or ID=@SanPhamVay)
	end
	else
		begin
			select ID,Ten from SAN_PHAM_VAY where Ma_Doi_Tac=@MaDoiTac
		end
	
END






GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSThongTinByID]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_LayDSThongTinByID]
	-- Add the parameters for the stored procedure here
	@MaDoiTac int,
	@NgayTao datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select SAN_PHAM_VAY.ID, SAN_PHAM_VAY.Ten, SAN_PHAM_VAY.Ma, SAN_PHAM_VAY.Ngay_Tao as NgayTao, NHAN_VIEN.Ho_Ten as NguoiTao
	from SAN_PHAM_VAY left join NHAN_VIEN on NHAN_VIEN.ID = SAN_PHAM_VAY.Ma_Nguoi_Tao
	where Ma_Doi_Tac=@MaDoiTac and SAN_PHAM_VAY.Ngay_Tao = @NgayTao
END










GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sang Nguyen>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_Them]
	-- Add the parameters for the stored procedure here
	@ID int output,
	@Ma nvarchar(50),
	@MaDoiTac int,
	@Ten nvarchar(200),
	@NgayTao datetime,
	@MaNguoiTao int,
	@Loai int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Insert Into SAN_PHAM_VAY(Ma, Ma_Doi_Tac, Ten, Ngay_Tao, Ma_Nguoi_Tao, Loai) Values (@Ma, @MaDoiTac, @Ten, @NgayTao, @MaNguoiTao, @Loai)
	Set @ID = @@IDENTITY
END









GO
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_Xoa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SAN_PHAM_VAY_Xoa]
	-- Add the parameters for the stored procedure here
	@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Delete from SAN_PHAM_VAY Where ID = @ID
END






GO
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_LayDS]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TAI_LIEU_HS_LayDS]
	-- Add the parameters for the stored procedure here
	@MaHS int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	select Ma_Loai as MaLoai,Duong_Dan_File as DuongDanFile,Ten,ID from TAI_LIEU_HS where Ma_Ho_So=@MaHS
END











GO
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_Them]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TAI_LIEU_HS_Them]
	-- Add the parameters for the stored procedure here
	@Maloai int,
	@Ten nvarchar(500),
	@DuongDan ntext,
	@MaHS int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	insert TAI_LIEU_HS(Ma_Loai,Ten,Duong_Dan_File,Ma_Ho_So)
	values(@MaLoai,@Ten,@DuongDan,@MaHS)
END














GO
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_XoaTatCa]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TAI_LIEU_HS_XoaTatCa]
	-- Add the parameters for the stored procedure here
	@MaHS int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	delete from TAI_LIEU_HS where Ma_Ho_So=@MaHS
END














GO
/****** Object:  StoredProcedure [dbo].[sp_TRANG_THAI_HS_LayDSTrangThai]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_TRANG_THAI_HS_LayDSTrangThai] 
@isTeamlead as bit = 0
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	if(@isTeamlead=0)
		Select ID, Ten From TRANG_THAI_HS Where Da_Xoa = 0
	else 
	select ID,Ten from TRANG_THAI_HS where Da_Xoa = 0 and ID not in (select IdTrangThaiHoso from TRANGTHAI_HS_IGNORE_TEAMLEAD)
END











GO
/****** Object:  StoredProcedure [dbo].[sp_Update_NhanvienQuyen]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_Update_NhanvienQuyen] 
@userId as int,
 @maquyen as nvarchar(50)
as
begin
update NHAN_VIEN_QUYEN set Quyen = 'ffffff' where Ma_NV = @userId;
end

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCustomer]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE procedure [dbo].[sp_UpdateCustomer]
  (
  @id int,
  @fullname nvarchar(200),
  @checkdate datetime,
  @cmnd varchar(15),
  @gender bit,
  @status int,
  @match nvarchar(500),
  @notmatch nvarchar(500),
  @updatedtime datetime,
  @updatedby int,
  @note nvarchar(200) = null
  )
  as
  begin
	update Customer 
		set FullName = @fullname,
		CheckDate = @checkdate,
		Cmnd = @cmnd,
		Gender = @gender,
		UpdatedTime = @updatedtime,
		UpdatedBy = @updatedby,
		MatchCondition = @match,
		NotMatch = @notmatch,
		CICStatus = @status
		where Id = @id
	  if(@note is not null)
		update Customer set LastNote = @note where Id = @id
  end

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_UpdateUser]
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
update NHAN_VIEN set 
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



GO
/****** Object:  StoredProcedure [dbo].[updateExistingFile]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[updateExistingFile] (@fileId int, @name nvarchar(200),@url nvarchar(max))
as
begin
update TAI_LIEU_HS set Ten = @name,
	Duong_Dan_File = @url,
	Deleted = 0
	where Id = @fileId
end

GO
/****** Object:  StoredProcedure [dbo].[updateF88Result]    Script Date: 09/01/2020 5:30:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[updateF88Result](@hosoId int, @result int, @reason nvarchar(200))
as 
begin
update HO_SO set F88Result = @result,
F88Reason = @reason
where ID = @hosoId
end
GO
USE [master]
GO
ALTER DATABASE [Test13] SET  READ_WRITE 
GO
