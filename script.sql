USE [master]
GO
/****** Object:  Database [Test13]    Script Date: 6/2/2019 5:59:59 PM ******/
CREATE DATABASE [Test13]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Test13', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Test13.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Test13_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\Test13_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Test13] SET COMPATIBILITY_LEVEL = 130
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
ALTER DATABASE [Test13] SET QUERY_STORE = OFF
GO
USE [Test13]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Test13]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_SplitStringToTable]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[AUTOID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[DOI_TAC]    Script Date: 6/2/2019 5:59:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOI_TAC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](200) NULL,
 CONSTRAINT [PK_DOI_TAC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HO_SO]    Script Date: 6/2/2019 5:59:59 PM ******/
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
 CONSTRAINT [PK_HO_SO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HO_SO_DUYET_XEM]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[HO_SO_XEM]    Script Date: 6/2/2019 5:59:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HO_SO_XEM](
	[Ma_Ho_So] [int] NULL,
	[Xem] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KET_QUA_HS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[KHACH_HANG]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[KHU_VUC]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[LOAI_TAI_LIEU]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[NHAN_VIEN]    Script Date: 6/2/2019 5:59:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHAN_VIEN](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ma] [nvarchar](50) NULL,
	[Ten_Dang_Nhap] [nvarchar](50) NULL,
	[Mat_Khau] [nvarchar](50) NULL,
	[Ho_Ten] [nvarchar](100) NULL,
	[Loai] [int] NULL,
	[Dien_Thoai] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Trang_Thai] [int] NULL,
	[Xoa] [int] NULL,
 CONSTRAINT [PK_NHAN_VIEN] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NHAN_VIEN_CF]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[NHAN_VIEN_NHOM]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[NHAN_VIEN_QUYEN]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[NHOM]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[SAN_PHAM_VAY]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  Table [dbo].[TAI_LIEU_HS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
 CONSTRAINT [PK_TAI_LIEU] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TRANG_THAI_HS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
INSERT [dbo].[AUTOID] ([ID], [Name_ID], [Prefix], [Suffixes], [Value]) VALUES (1, NULL, N'19', N'05', 33)
SET IDENTITY_INSERT [dbo].[DOI_TAC] ON 

INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (1, N'TP Bank')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (2, N'Mcredit')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (3, N'Việt Credit')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (4, N'Lotte Finance')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (5, N'Mirae')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (6, N'FE Credit')
INSERT [dbo].[DOI_TAC] ([ID], [Ten]) VALUES (7, N'Giới thiệu khách hàng')
SET IDENTITY_INSERT [dbo].[DOI_TAC] OFF
SET IDENTITY_INSERT [dbo].[HO_SO] ON 

INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (4, N'1904000005', NULL, N'test 1', N'121', N'Bến Nghé', 2, N'0349149240', N'012244336', 1, CAST(N'2019-04-07T14:46:00.037' AS DateTime), 1, 1, CAST(N'2019-05-08T23:12:01.280' AS DateTime), 1, CAST(N'2019-05-05T00:00:00.000' AS DateTime), 3, 3, 1, N'BBC', 0, CAST(5500000 AS Decimal(18, 0)), 6, N'', 0, 0)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (5, N'1904000006', NULL, N'Đỗ Trung Vinh', N'215148870', N'30/20', 2, N'0349149240', N'', 1, CAST(N'2019-04-07T15:00:38.310' AS DateTime), 1, 1, CAST(N'2019-05-10T23:12:01.280' AS DateTime), NULL, NULL, 0, 1, 1, N'', 1, CAST(2 AS Decimal(18, 0)), 0, NULL, NULL, 0)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (8, N'1905000030', NULL, N'test', N'123456789', N'123', 2, N'1234567890', N'', 1, CAST(N'2019-05-18T15:53:56.623' AS DateTime), 11, 11, CAST(N'2019-05-18T15:53:56.623' AS DateTime), NULL, CAST(N'2019-05-18T00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(12 AS Decimal(18, 0)), 0, NULL, 0, 0)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (9, N'1905000031', NULL, N'test', N'123456789', N'12345', 2, N'1234567890', N'', 1, CAST(N'2019-05-18T17:06:39.067' AS DateTime), 1, 1, CAST(N'2019-05-18T17:06:39.067' AS DateTime), NULL, CAST(N'2019-05-18T00:00:00.000' AS DateTime), 1, 1, 1, N'', 0, CAST(1235666 AS Decimal(18, 0)), 0, NULL, 0, 0)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (10, N'1905000032', NULL, N'Test1', N'', N'', 0, N'', N'', 1, CAST(N'2019-05-19T14:11:56.103' AS DateTime), 2, 2, CAST(N'2019-05-19T14:11:56.103' AS DateTime), NULL, CAST(N'2019-05-19T00:00:00.000' AS DateTime), 0, 1, 0, N'', 0, CAST(0 AS Decimal(18, 0)), 0, NULL, 0, 0)
INSERT [dbo].[HO_SO] ([ID], [Ma_Ho_So], [Ma_Khach_Hang], [Ten_Khach_Hang], [CMND], [Dia_Chi], [Ma_Khu_Vuc], [SDT], [SDT2], [Gioi_Tinh], [Ngay_Tao], [Ma_Nguoi_Tao], [Ho_So_Cua_Ai], [Ngay_Cap_Nhat], [Ma_Nguoi_Cap_Nhat], [Ngay_Nhan_Don], [Ma_Trang_Thai], [Ma_Ket_Qua], [San_Pham_Vay], [Ten_Cua_Hang], [Co_Bao_Hiem], [So_Tien_Vay], [Han_Vay], [Ghi_Chu], [Courier_Code], [Da_Xoa]) VALUES (11, N'1905000033', NULL, N'Test1', N'222', N'', 0, N'222', N'222', 1, CAST(N'2019-05-19T14:32:43.103' AS DateTime), 2, 2, CAST(N'2019-05-19T14:29:12.803' AS DateTime), NULL, CAST(N'2019-05-19T00:00:00.000' AS DateTime), 0, 1, 1, N'', 0, CAST(1000000 AS Decimal(18, 0)), 3, NULL, 0, 0)
SET IDENTITY_INSERT [dbo].[HO_SO] OFF
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (7, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (8, 0)
INSERT [dbo].[HO_SO_DUYET_XEM] ([Ma_Ho_So], [Xem]) VALUES (9, 0)
INSERT [dbo].[HO_SO_XEM] ([Ma_Ho_So], [Xem]) VALUES (4, 1)
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

INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (1, N'VBF0001', N'ThaiNM', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Minh Thái', NULL, N'0901812024', N'minhtai.nguyen@vietbankfc.vn', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (2, N'VBF0002', N'NhuTH', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Huỳnh Như', NULL, N'0901812024', N'support@vietbankkfc.vn', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (3, N'VBF0003', N'TungND', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Ngô Duy Tùng', NULL, N'0901812024', N'tung.ngo@vietbankfc.vn', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (4, N'VBF0004', N'AnhNN', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Ngọc Anh
', 0, N'0901812024', N'anh.nguyen.5@vietbankfc.vn
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (5, N'VBF0005', N'TungDA', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Đào Anh Tùng
', 0, N'0901812024', N'tung.dao@vietbankfc.vn
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (6, N'VBF0006', N'HoaNTN', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Như Hoa
', 0, N'0901812024', N'hoa.nguyen@vietbankfc.vn
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (7, N'VBF0007', N'LinhNTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Thùy Linh
', 0, N'0901812024', N'nguyenlinh08121992@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (8, N'VBF0008', N'OanhNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Oanh 
', 0, N'0901812024', N'nguyenoanhnd2608@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (9, N'VBF0009', N'ChienTD', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Tống Đình Chiến
', 0, N'0901812024', N'tuanchien68@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (10, N'VBF0010', N'ThuyLTD', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Lưu Thị Diệu Thúy
', NULL, N'0901812024', N'dieuthuy.bhhyyen@mail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (11, N'VBF0011', N'HungDQ', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Đặng Quang Hưng 
', NULL, N'0901812024', N'dangquanghung88nd@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (12, N'VBF0012', N'NgaHT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Hoàng Thúy Nga
', NULL, N'0901812024', N'ngahoang266@mail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (13, N'VBF0013', N'HuyenLTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Lã Thị Thu Huyền
', NULL, N'0901812024', N'Hoavioletnamdinh@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (14, N'VBF0014', N'DuNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Du', NULL, N'0901812024', N'buianphuong2nd@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (15, N'VBF0015', N'BonTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Thị Bốn', NULL, N'0901812024', N'tbon1982@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (16, N'VBF0016', N'HangDT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Dương Thị Hằng
', NULL, N'0901812024', N'duonghang2412@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (17, N'VBF0017', N'LanNT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Nguyễn Thị Lan', NULL, N'0901812024', N'nguyenlan1980@gmail.com
', 1, 0)
INSERT [dbo].[NHAN_VIEN] ([ID], [Ma], [Ten_Dang_Nhap], [Mat_Khau], [Ho_Ten], [Loai], [Dien_Thoai], [Email], [Trang_Thai], [Xoa]) VALUES (18, N'VBF0018', N'HuongTT', N'2f090c96c895cca18b1bcaa36ed5ab0b', N'Trần Thu Hường
', NULL, N'0901812024', N'tranthuhuonghuong40@gmail.com
', 1, 0)
SET IDENTITY_INSERT [dbo].[NHAN_VIEN] OFF
INSERT [dbo].[NHAN_VIEN_CF] ([Ma_Nhan_Vien], [Quyen], [Ma_Nhom]) VALUES (2, 0, 1)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (1, 1)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (3, 2)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (4, 3)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (5, 4)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (6, 5)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (7, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (8, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (9, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (10, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (11, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (12, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (13, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (14, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (15, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (16, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (17, 6)
INSERT [dbo].[NHAN_VIEN_NHOM] ([Ma_Nhan_Vien], [Ma_Nhom]) VALUES (18, 6)
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (1, N'fffffffff')
INSERT [dbo].[NHAN_VIEN_QUYEN] ([Ma_NV], [Quyen]) VALUES (2, N'fffffffff')
SET IDENTITY_INSERT [dbo].[NHOM] ON 

INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (1, 0, N'0', 1, N'Director', N'Director', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (2, 1, N'0.1', 3, N'Head North1', N'Head North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (3, 2, N'0.1.2', 4, N'RSM North1', N'RSM North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (4, 3, N'0.1.2.3', 5, N'ASM North1', N'ASM North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (5, 4, N'0.1.2.3.4', 6, N'SS North1', N'SS North1', NULL)
INSERT [dbo].[NHOM] ([ID], [Ma_Nhom_Cha], [Chuoi_Ma_Cha], [Ma_Nguoi_QL], [Ten_Viet_Tat], [Ten], [Loai]) VALUES (6, 5, N'0.1.2.3.4.5', 6, N'NAMDINH1', N'NAMDINH1', NULL)
SET IDENTITY_INSERT [dbo].[NHOM] OFF
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
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (39, 3, N'APPL00085184', N'APPL00085184', CAST(N'2019-05-19T00:00:00.000' AS DateTime), 1, 1)
INSERT [dbo].[SAN_PHAM_VAY] ([ID], [Ma_Doi_Tac], [Ma], [Ten], [Ngay_Tao], [Ma_Nguoi_Tao], [Loai]) VALUES (40, 3, N'APPL00083863', N'APPL00083863', CAST(N'2019-05-19T00:00:00.000' AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[SAN_PHAM_VAY] OFF
SET IDENTITY_INSERT [dbo].[TAI_LIEU_HS] ON 

INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So]) VALUES (294, 1, N'20190519143150291_02016Thg226163814555_Penguins.jpg', N'/Upload/HoSo/2019/5/19/20190519143150291_02016Thg226163814555_Penguins.jpg', 11)
INSERT [dbo].[TAI_LIEU_HS] ([ID], [Ma_Loai], [Ten], [Duong_Dan_File], [Ma_Ho_So]) VALUES (295, 2, N'20190519143237085_02016Thg226163814555_Penguins.jpg', N'/Upload/HoSo/2019/5/19/20190519143237085_02016Thg226163814555_Penguins.jpg', 11)
SET IDENTITY_INSERT [dbo].[TAI_LIEU_HS] OFF
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (0, N'Lưu tạm', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (1, N'Nhập liệu', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (2, N'Thẩm định', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (3, N'Từ chối', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (4, N'Bổ sung hồ sơ', 0)
INSERT [dbo].[TRANG_THAI_HS] ([ID], [Ten], [Da_Xoa]) VALUES (5, N'Giải ngân', 0)
/****** Object:  StoredProcedure [dbo].[sp_AUTOID_GetID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_AUTOID_Update]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_DOI_TAC_LayDS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	select ID,Ten from DOI_TAC
END










GO
/****** Object:  StoredProcedure [dbo].[sp_DOI_TAC_LayIDByMaSanPham]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhat]    Script Date: 6/2/2019 5:59:59 PM ******/
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
@NgayTao datetime,
@MaNguoiTao int,
@HoSoCuaAi int,
@NgayNhanDon datetime,
@MaTrangThai int,
@SanPhamVay int,
@CoBaoHiem int,
@SoTienVay decimal,
@HanVay int,
@KetQuaHS int,
@TenCuaHang nvarchar(200)
AS
BEGIN
	UPDATE HO_SO SET Ten_Khach_Hang=Upper(@TenKhachHang),
		CMND=@CMND,Dia_Chi=@DiaChi, Courier_Code=@CourierCode,Ma_Khu_Vuc=@MaKhuVuc,SDT=@SDT,SDT2=@SDT2,Gioi_Tinh=@GioiTinh,
		Ngay_Tao=@NgayTao,Ma_Nguoi_Tao=@MaNguoiTao,Ho_So_Cua_Ai=@HoSoCuaAi,Ngay_Nhan_Don=@NgayNhanDon,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ma_Ket_Qua=@KetQuaHS
	WHERE ID=@ID
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatDaXoa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatHoSo]    Script Date: 6/2/2019 5:59:59 PM ******/
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
@TenCuaHang nvarchar(200)
AS
BEGIN
	UPDATE HO_SO SET Ten_Khach_Hang=Upper(@TenKhachHang),
		CMND=@CMND,Dia_Chi=@DiaChi, Courier_Code=@CourierCode,Ma_Khu_Vuc=@MaKhuVuc,SDT=@SDT,SDT2=@SDT2,Gioi_Tinh=@GioiTinh,
		Ngay_Nhan_Don=@NgayNhanDon, Ma_Nguoi_Cap_Nhat=@MaNguoiCapNhat,Ho_So_Cua_Ai=@HoSoCuaAi,Ngay_Cap_Nhat=@NgayCapNhat,
		Ma_Trang_Thai=@MaTrangThai,San_Pham_Vay=@SanPhamVay,Co_Bao_Hiem=@CoBaoHiem,
		So_Tien_Vay=@SoTienVay,Han_Vay=@HanVay,Ten_Cua_Hang=@TenCuaHang,
		Ma_Ket_Qua=@KetQuaHS
	WHERE ID=@ID
END











GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_CapNhatTrangThaiHS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_DUYET_XEM_DaXem]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_DUYET_XEM_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_LayChiTiet]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	select hs.ID,Ma_Ho_So as MaHoSo,Ten_Khach_Hang as TenKhachHang,CMND,Dia_Chi as DiaChi,Courier_Code as CourierCode,
	SDT,SDT2,Gioi_Tinh as GioiTinh,Ngay_Tao as NgayTao,Ma_Nguoi_Tao as MaNguoiTao,
	Ma_Khu_Vuc as MaKhuVuc, tt.Ten as TenTrangThai, kq.Ten as KetQuaText,
	Ho_So_Cua_Ai as HoSoCuaAi,Ngay_Cap_Nhat as NgayCapNhat,Ma_Nguoi_Cap_Nhat as MaNguoiCapNhat,
	Ngay_Nhan_Don as NgayNhanDon,Ma_Trang_Thai as MaTrangThai,Ma_Ket_Qua as MaKetQua,San_Pham_Vay as SanPhamVay,
	Ten_Cua_Hang as TenCuaHang,Co_Bao_Hiem as CoBaoHiem,So_Tien_Vay as SoTienVay,Han_Vay as HanVay,Ghi_Chu as GhiChu
	from HO_SO hs
	left join TRANG_THAI_HS tt on tt.ID=hs.Ma_Trang_Thai
	left join KET_QUA_HS kq on kq.ID=hs.Ma_Ket_Qua
	 where hs.ID=@ID
END









GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
@TenCuaHang nvarchar(200)
AS
BEGIN
 INSERT HO_SO
 (Ma_Ho_So, Ten_Khach_Hang,CMND,Dia_Chi, Courier_Code,Ma_Khu_Vuc,SDT,SDT2,Gioi_Tinh,Ngay_Tao,Ma_Nguoi_Tao,Ho_So_Cua_Ai
 ,Ngay_Nhan_Don, Ma_Trang_Thai, Ma_Ket_Qua,San_Pham_Vay,Ten_Cua_Hang,Co_Bao_Hiem,So_Tien_Vay,Han_Vay,Da_Xoa, Ngay_Cap_Nhat)
 VALUES(@MaHoSo,Upper(@TenKhachHang),@CMND,@DiaChi,@CourierCode,@MaKhuVuc,@SDT,@SDT2,@GioiTinh,@NgayTao,@MaNguoiTao,
 @HoSoCuaAi,@NgayNhanDon,@MaTrangThai,@KetQuaHS,@SanPhamVay,@TenCuaHang,@CoBaoHiem,@SoTienVay,@HanVay,0, @NgayTao) 
	SET @ID=@@IDENTITY
END









GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoCuaToi]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoCuaToiChuaXem]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoDuyet]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang, SAN_PHAM_VAY.Ten as TenSanPham
	From HO_SO left join NHAN_VIEN as NV1 on HO_SO.Ma_Nguoi_Tao = NV1.ID
		left join NHAN_VIEN as NV2 on HO_SO.Ma_Nguoi_Cap_Nhat = NV2.ID
		left join NHAN_VIEN as NV3 on HO_SO.Courier_Code = NV3.ID
		left join KHU_VUC kvh on kvh.ID=HO_SO.Ma_Khu_Vuc
		left join KHU_VUC kvt on kvt.ID=kvh.Ma_Cha
	, DOI_TAC, SAN_PHAM_VAY, TRANG_THAI_HS, KET_QUA_HS
	Where HO_SO.San_Pham_Vay = SAN_PHAM_VAY.ID
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
	order by HO_SO.Ngay_Cap_Nhat desc 
END





GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoDuyetChuaXem]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_TimHoSoQuanLy]    Script Date: 6/2/2019 5:59:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[sp_HO_SO_TimHoSoQuanLy] 
	-- Add the parameters for the stored procedure here
	@MaNVDangNhap int,
	@MaNhom int,
	@MaThanhVien int,
	@TuNgay datetime,
	@DenNgay datetime,
	@MaHS nvarchar(50),
	@CMND nvarchar(50),
	@TrangThai nvarchar(50),
	@LoaiNgay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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
	Select HO_SO.ID, HO_SO.Ma_Ho_So as MaHoSo, HO_SO.Ngay_Tao as NgayTao, DOI_TAC.Ten as DoiTac, HO_SO.CMND, HO_SO.Ten_Khach_Hang as TenKH,HO_SO.Ma_Trang_Thai as MaTrangThai, TRANG_THAI_HS.Ten as TrangThaiHS, KET_QUA_HS.Ten as KetQuaHS, HO_SO.Ngay_Cap_Nhat as NgayCapNhat, NV1.Ma as MaNV, NV1.Ho_Ten as NhanVienBanHang, NV2.Ma as MaNVSua, HO_SO.Co_Bao_Hiem as CoBaoHiem, HO_SO.Dia_Chi as DiaChiKH,kvt.Ten as KhuVucText, HO_SO.Ghi_Chu as GhiChu, NV3.Ma as MaNVLayHS,
	CASE WHEN ((Select COUNT(*) From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) = 0) THEN '' ELSE (Select Top(1) NHOM.Ten From NHOM, NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom = NHOM.ID and NHAN_VIEN_NHOM.Ma_Nhan_Vien = HO_SO.Ma_Nguoi_Tao) END as DoiNguBanHang, SAN_PHAM_VAY.Ten as TenSanPham
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
			Select NHAN_VIEN_NHOM.Ma_Nhan_Vien From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhom in (Select NHOM.ID From NHOM Where ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhom)) or NHOM.ID = @MaNhom)
			))
			or (@MaNhom = 0 and HO_SO.Ma_Nguoi_Tao in (Select NVN1.Ma_Nhan_Vien From NHAN_VIEN_NHOM as NVN1 Where NVN1.Ma_Nhom in (Select * From @DSNhomQL)) and @MaThanhVien = 0)
	)
	and HO_SO.Ma_Trang_Thai = TRANG_THAI_HS.ID
	and HO_SO.Ma_Ket_Qua = KET_QUA_HS.ID
	and HO_SO.Ma_Ho_So like '%'+@MaHS+'%'
	and HO_SO.CMND like '%'+@CMND+'%'
	and ((HO_SO.Ngay_Tao between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 1) or (HO_SO.Ngay_Cap_Nhat between CONVERT(date, @TuNgay) and CONVERT(date, @DenNgay) and @LoaiNgay = 2))
	and HO_SO.Da_Xoa = 0
	and HO_SO.Ma_Trang_Thai in  (Select CONVERT(int,Value) From dbo.fn_SplitStringToTable(@TrangThai, ','))
	order by HO_SO.Ngay_Cap_Nhat desc
END


GO
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_XEM_DaXem]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_HO_SO_XEM_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_KET_QUA_HS_LayDSKetQua]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayDSHuyen]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayDSTinh]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_KHU_VUC_LayMaTinhByMaHuyen]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_LOAI_TAI_LIEU_LayDS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_CF_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_CF_Xoa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_GetUserByID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	select ID,Ma as Code,Ho_Ten as FullName,Ten_Dang_Nhap as UserName,Mat_Khau as [Password],Dien_Thoai as Phone,Email as Email,'1' as [Type]  from NHAN_VIEN where ID=@UserID
END










GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSByMaQL]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSByRule]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_LayDSCourierCode]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	select ID,Ma+'-'+Ho_Ten as [FullText] from NHAN_VIEN where Loai=10
END









GO
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_Login]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChiTietThanhVienNhom]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhom]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSChonThanhVienNhomCaCon]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_LayDSKhongThanhVienNhom]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHAN_VIEN_NHOM_Xoa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHANVIEN_ChangePass]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHANVIEN_LayDS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayCayNhomCon]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nguoi_QL as MaNguoiQL, NHAN_VIEN.Ho_Ten as TenQL From NHOM, NHAN_VIEN 
	Where NHOM.Ma_Nguoi_QL = NHAN_VIEN.ID and ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhomCha) + '.%') or ((NHOM.Chuoi_Ma_Cha + '.' + Convert(nvarchar, NHOM.ID)) like '%.' + Convert(nvarchar, @MaNhomCha))
END






GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayChuoiMaChaCuaMaNhom]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	Select NHOM.Chuoi_Ma_Cha as ChuoiMaCha From NHOM Where NHOM.ID = @MaNhom
END






GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSChonTheoNhanVien]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	Select NHOM.ID, NHOM.Ten, NHOM.Chuoi_Ma_Cha as ChuoiMaCha, NHOM.Ma_Nhom_Cha as MaNhomCha, NHAN_VIEN.Ho_Ten as TenQL From NHOM, NHAN_VIEN 
	Where NHOM.Ma_Nguoi_QL = NHAN_VIEN.ID and NHOM.ID in (Select NHAN_VIEN_NHOM.Ma_Nhom From NHAN_VIEN_NHOM Where NHAN_VIEN_NHOM.Ma_Nhan_Vien = @UserID)
END











GO
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhom]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhomCon]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayDSNhomDuyetChonTheoNhanVien]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayThongTinChiTietTheoMa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_LayThongTinTheoMa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_Sua]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_NHOM_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_RULE_GetWidthID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_CapNhatSuDung]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_CheckExist]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_DemTrungMa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSByID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSByIDAndMaHS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_LayDSThongTinByID]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_SAN_PHAM_VAY_Xoa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_LayDS]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_Them]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_TAI_LIEU_HS_XoaTatCa]    Script Date: 6/2/2019 5:59:59 PM ******/
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
/****** Object:  StoredProcedure [dbo].[sp_TRANG_THAI_HS_LayDSTrangThai]    Script Date: 6/2/2019 5:59:59 PM ******/
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
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	Select ID, Ten From TRANG_THAI_HS Where Da_Xoa = 0
END









GO
USE [master]
GO
ALTER DATABASE [Test13] SET  READ_WRITE 
GO
