USE [master]
GO
/****** Object:  Database [ServiceManager]    Script Date: 25-07-2025 09:00:48 PM ******/
CREATE DATABASE [ServiceManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ServiceManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ServiceManager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ServiceManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ServiceManager_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ServiceManager] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ServiceManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ServiceManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ServiceManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ServiceManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ServiceManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ServiceManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [ServiceManager] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ServiceManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ServiceManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ServiceManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ServiceManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ServiceManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ServiceManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ServiceManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ServiceManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ServiceManager] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ServiceManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ServiceManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ServiceManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ServiceManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ServiceManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ServiceManager] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ServiceManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ServiceManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ServiceManager] SET  MULTI_USER 
GO
ALTER DATABASE [ServiceManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ServiceManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ServiceManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ServiceManager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ServiceManager] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ServiceManager] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ServiceManager] SET QUERY_STORE = ON
GO
ALTER DATABASE [ServiceManager] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ServiceManager]
GO
/****** Object:  Table [dbo].[ServiceRequests]    Script Date: 25-07-2025 09:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ServiceRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ServiceRequests] ON 

INSERT [dbo].[ServiceRequests] ([Id], [Title], [Description], [CreatedDate], [Status], [CreatedBy]) VALUES (2, N'Mobile Repair', N'Samsung S23 Ultra Display Not Working', CAST(N'2025-07-24T14:26:57.4380861' AS DateTime2), N'New', N'Akash Kataria')
INSERT [dbo].[ServiceRequests] ([Id], [Title], [Description], [CreatedDate], [Status], [CreatedBy]) VALUES (3, N'Mobile Repair', N'Samsung S23 Ultra Battery Issue is on working status', CAST(N'2025-07-24T14:28:17.0031939' AS DateTime2), N'In Progress', N'Akash Kataria')
INSERT [dbo].[ServiceRequests] ([Id], [Title], [Description], [CreatedDate], [Status], [CreatedBy]) VALUES (4, N'Laptop Issue', N'Laptop Battery has been replaced successfully', CAST(N'2025-07-24T14:31:56.6668974' AS DateTime2), N'Closed', N'Shivansh Parganiha')
SET IDENTITY_INSERT [dbo].[ServiceRequests] OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_createServiceRequest]    Script Date: 25-07-2025 09:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_createServiceRequest]
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @Status NVARCHAR(50),
    @CreatedBy NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF;

    INSERT INTO [dbo].[ServiceRequests] (
        Title,
        Description,
        CreatedDate,
        Status,
        CreatedBy
    )
    VALUES (
        @Title,
        @Description,
        SYSDATETIME(),
        @Status,
        @CreatedBy
    );

    RETURN @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_updateServiceRequest]    Script Date: 25-07-2025 09:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_updateServiceRequest]
    @Id INT,
    @Description NVARCHAR(MAX),
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE [dbo].[ServiceRequests]
    SET
        Description = @Description,
        Status = @Status
    WHERE Id = @Id;

    RETURN @@ROWCOUNT;
END

GO
USE [master]
GO
ALTER DATABASE [ServiceManager] SET  READ_WRITE 
GO
