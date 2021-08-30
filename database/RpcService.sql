USE [master]
GO
/****** Object:  Database [RpcService]    Script Date: 2021/8/11 15:34:16 ******/
CREATE DATABASE [RpcService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RpcService', FILENAME = N'E:\数据库\RpcService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RpcService_log', FILENAME = N'E:\数据库\RpcService.ldf' , SIZE = 265344KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RpcService] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RpcService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RpcService] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RpcService] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RpcService] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RpcService] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RpcService] SET ARITHABORT OFF 
GO
ALTER DATABASE [RpcService] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RpcService] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RpcService] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RpcService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RpcService] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RpcService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RpcService] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RpcService] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RpcService] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RpcService] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RpcService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RpcService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RpcService] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RpcService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RpcService] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RpcService] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RpcService] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RpcService] SET RECOVERY FULL 
GO
ALTER DATABASE [RpcService] SET  MULTI_USER 
GO
ALTER DATABASE [RpcService] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RpcService] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RpcService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RpcService] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [RpcService] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'RpcService', N'ON'
GO
ALTER DATABASE [RpcService] SET QUERY_STORE = OFF
GO
USE [RpcService]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [RpcService]
GO
/****** Object:  Table [dbo].[AutoTaskList]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTaskList](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[TaskName] [nvarchar](50) NOT NULL,
	[TaskType] [smallint] NOT NULL,
	[TaskTimeSpan] [int] NOT NULL,
	[TaskPriority] [int] NOT NULL,
	[SendType] [smallint] NOT NULL,
	[SendParam] [ntext] NOT NULL,
	[RegionId] [int] NOT NULL,
	[VerNum] [int] NOT NULL,
 CONSTRAINT [PK_AutoTaskList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorCollect]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorCollect](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ErrorCode] [varchar](50) NOT NULL,
	[IsPerfect] [bit] NOT NULL,
 CONSTRAINT [PK_ErrorCollect] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLangMsg]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLangMsg](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ErrorId] [bigint] NOT NULL,
	[Lang] [varchar](20) NOT NULL,
	[Msg] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_ErrorLangMsg]    Script Date: 2021/8/11 15:34:16 ******/
CREATE CLUSTERED INDEX [IX_ErrorLangMsg] ON [dbo].[ErrorLangMsg]
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReduceInRankConfig]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReduceInRankConfig](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[LimitNum] [int] NOT NULL,
	[FusingErrorNum] [int] NOT NULL,
	[RefreshTime] [int] NOT NULL,
	[BeginDuration] [int] NOT NULL,
	[EndDuration] [int] NOT NULL,
 CONSTRAINT [PK_ReduceInRankConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerConfig]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerConfig](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerIp] [varchar](15) NOT NULL,
	[RemoteIp] [varchar](15) NOT NULL,
	[ServerPort] [int] NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[SystemType] [bigint] NOT NULL,
	[ServerMac] [varchar](17) NOT NULL,
	[ServerIndex] [int] NOT NULL,
	[PublicKey] [varchar](32) NOT NULL,
	[ServiceState] [smallint] NOT NULL,
	[TransmitConfig] [varchar](max) NOT NULL,
	[Weight] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[BindIndex] [int] NOT NULL,
	[ConfigPrower] [smallint] NOT NULL,
	[ConIp] [varchar](15) NOT NULL,
	[ApiVer] [varchar](20) NOT NULL,
	[LastOffliceDate] [date] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RemoteServerConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerGroup]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[SystemType] [bigint] NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[Weight] [int] NOT NULL,
 CONSTRAINT [PK_RemoteServerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerType]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerType](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[SystemName] [nvarchar](50) NOT NULL,
	[BalancedType] [smallint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RemoteServerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcControlList]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcControlList](
	[Id] [int] NOT NULL,
	[ServerIp] [varchar](15) NOT NULL,
	[Port] [smallint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[IsDrop] [bit] NOT NULL,
 CONSTRAINT [PK_RpcControlList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcMer]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcMer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SystemName] [nvarchar](50) NOT NULL,
	[AppId] [varchar](32) NOT NULL,
	[AppSecret] [varchar](32) NOT NULL,
	[AllowServerIp] [varchar](max) NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RpcMer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcMerConfig]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcMerConfig](
	[Id] [uniqueidentifier] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[IsRegionIsolate] [bit] NOT NULL,
	[IsolateLevel] [bit] NOT NULL,
 CONSTRAINT [PK_RpcMerConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerClientLimit]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerClientLimit](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[LimitType] [smallint] NOT NULL,
	[LimitNum] [int] NOT NULL,
	[LimitTime] [smallint] NOT NULL,
	[TokenNum] [smallint] NOT NULL,
	[TokenInNum] [smallint] NOT NULL,
 CONSTRAINT [PK_ServerClientLimit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerDictateLimit]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerDictateLimit](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[Dictate] [varchar](50) NOT NULL,
	[LimitType] [smallint] NOT NULL,
	[LimitNum] [int] NOT NULL,
	[LimitTime] [smallint] NOT NULL,
	[BucketSize] [smallint] NOT NULL,
	[BucketOutNum] [smallint] NOT NULL,
	[TokenNum] [smallint] NOT NULL,
	[TokenInNum] [smallint] NOT NULL,
 CONSTRAINT [PK_ServerDictateLimit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerGroup]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ServerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerLimitConfig]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerLimitConfig](
	[ServerId] [bigint] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[LimitType] [smallint] NOT NULL,
	[LimitNum] [int] NOT NULL,
	[LimitTime] [smallint] NOT NULL,
	[IsEnableBucket] [bit] NOT NULL,
	[BucketSize] [smallint] NOT NULL,
	[BucketOutNum] [smallint] NOT NULL,
	[TokenNum] [smallint] NOT NULL,
	[TokenInNum] [smallint] NOT NULL,
 CONSTRAINT [PK_ServerLimitConfig_1] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerRegion]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerRegion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionName] [nvarchar](50) NOT NULL,
	[IsDrop] [bit] NOT NULL,
 CONSTRAINT [PK_ServerRegion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerRunState]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerRunState](
	[ServerId] [bigint] NOT NULL,
	[Pid] [int] NOT NULL,
	[PName] [nvarchar](50) NOT NULL,
	[ConNum] [int] NOT NULL,
	[WorkMemory] [bigint] NOT NULL,
	[CpuRunTime] [int] NOT NULL,
	[SyncTime] [smalldatetime] NOT NULL,
	[StartTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerRunState] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerSignalState]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerSignalState](
	[ServerId] [bigint] NOT NULL,
	[RemoteId] [bigint] NOT NULL,
	[ConNum] [int] NOT NULL,
	[AvgTime] [int] NOT NULL,
	[SendNum] [int] NOT NULL,
	[ErrorNum] [int] NOT NULL,
	[UsableState] [smallint] NOT NULL,
	[SyncTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerSignalState_1] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC,
	[RemoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysConfig]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysConfig](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ValueType] [bit] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[ToUpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SysConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AutoTaskList] ON 
GO
INSERT [dbo].[AutoTaskList] ([Id], [RpcMerId], [TaskName], [TaskType], [TaskTimeSpan], [TaskPriority], [SendType], [SendParam], [RegionId], [VerNum]) VALUES (1, 1, N'检查超时事务', 1, 10, 10, 0, N'{"RpcConfig":{"SendConfig":{"SysDictate":"CheckOverTimeTran","IsRetry":true,"TransmitId":0,"TransmitType":0,"LockType":0,"LockColumn":[]},"SystemType":"sys.sync","ServerId":0,"MsgBody":{}}}', 1, 0)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RpcMerId], [TaskName], [TaskType], [TaskTimeSpan], [TaskPriority], [SendType], [SendParam], [RegionId], [VerNum]) VALUES (2, 1, N'事务回滚重试', 1, 10, 10, 0, N'{"RpcConfig":{"SendConfig":{"SysDictate":"CheckRetryTran","IsRetry":true,"TransmitId":0,"TransmitType":0,"LockType":0,"LockColumn":[]},"SystemType":"sys.sync","ServerId":0}}', 1, 0)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RpcMerId], [TaskName], [TaskType], [TaskTimeSpan], [TaskPriority], [SendType], [SendParam], [RegionId], [VerNum]) VALUES (3, 1, N'标记待删除的资源', 0, 60, 10, 0, N'{"RpcConfig":{"SendConfig":{"SysDictate":"InvalidResource","IsRetry":true,"IsReply":false,"TransmitId":0,"TransmitType":0,"LockType":0,"LockColumn":[]},"SystemType":"sys.sync","ServerId":0}}', 1, 0)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RpcMerId], [TaskName], [TaskType], [TaskTimeSpan], [TaskPriority], [SendType], [SendParam], [RegionId], [VerNum]) VALUES (4, 1, N'清理已删除的资源', 0, 120, 10, 0, N'{"RpcConfig":{"SendConfig":{"SysDictate":"ClearResource","IsRetry":true,"IsReply":false,"TransmitId":0,"TransmitType":0,"LockType":0,"LockColumn":[]},"SystemType":"sys.sync","ServerId":0}}', 1, 0)
GO
SET IDENTITY_INSERT [dbo].[AutoTaskList] OFF
GO
SET IDENTITY_INSERT [dbo].[ErrorCollect] ON 
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10119, N'demo.user.phone.null', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10120, N'demo.user.phone.error', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10121, N'public.system.error', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10122, N'demo.order.title.null', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10123, N'demo.order.title.len', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10124, N'demo.order.price.error', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10125, N'lock.not.find', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10126, N'public.no.server[demo.user,GetUser]', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10127, N'demo.orderNo.repeat', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10128, N'rpc.tran.already.rollback', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10129, N'rpc.tran.wait.overtime', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10130, N'rpc.tran.status.change', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10131, N'demo.user.no.reg', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10132, N'public.no.server', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (10133, N'rpc.tran.rollback', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (20130, N'socket.receive.overtime', 0)
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (20131, N'rpc.tran.log.get.error', 0)
GO
SET IDENTITY_INSERT [dbo].[ErrorCollect] OFF
GO
SET IDENTITY_INSERT [dbo].[RemoteServerConfig] ON 
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (1, N'Rpr同步服务', N'127.0.0.1', N'127.0.0.1', 835, 1, 1, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'', 1, 1, 0, 1, 11, N'127.0.0.1', N'1.0.2', CAST(N'2021-08-10' AS Date), CAST(N'2021-01-26T12:02:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (2, N'RPC后台管理服务', N'127.0.0.1', N'127.0.0.1', 834, 1, 2, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'', 1, 1, 1, 1, 11, N'127.0.0.1', N'1.0.1', CAST(N'2021-05-29' AS Date), CAST(N'2021-05-08T18:05:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (3, N'自动任务服务', N'127.0.0.1', N'127.0.0.1', 839, 1, 3, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'[]', 1, 1, 0, 1, 11, N'127.0.0.1', N'1.0.2', CAST(N'2021-08-11' AS Date), CAST(N'2021-08-11T15:31:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (4, N'演示订单服务', N'127.0.0.1', N'127.0.0.1', 837, 2, 6, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'', 1, 1, 1, 1, 11, N'127.0.0.1', N'1.0.2', CAST(N'2021-08-10' AS Date), CAST(N'2021-03-25T18:43:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (5, N'演示用户服务', N'127.0.0.1', N'127.0.0.1', 838, 2, 5, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'[]', 1, 1, 1, 1, 11, N'127.0.0.1', N'1.0.2', CAST(N'2021-08-10' AS Date), CAST(N'2021-04-18T11:05:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [PublicKey], [ServiceState], [TransmitConfig], [Weight], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [ConIp], [ApiVer], [LastOffliceDate], [AddTime]) VALUES (6, N'演示网关', N'127.0.0.1', N'127.0.0.1', 836, 2, 4, N'7c:b2:7d:ea:fd:df', 0, N'6xy3#7a%ad', 0, N'[]', 1, 1, 1, 1, 11, N'127.0.0.1', N'1.0.2', CAST(N'2021-08-10' AS Date), CAST(N'2021-03-25T18:42:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[RemoteServerConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[RemoteServerGroup] ON 
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (1, 1, 1, 1, 1, N'sys.sync', 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (2, 1, 2, 1, 2, N'sys.store', 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (3, 2, 1, 1, 1, N'sys.sync', 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (4, 2, 3, 1, 4, N'demo.gateway', 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (5, 2, 4, 1, 3, N'demo.order', 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [Weight]) VALUES (6, 2, 5, 1, 5, N'demo.user', 0)
GO
SET IDENTITY_INSERT [dbo].[RemoteServerGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[RemoteServerType] ON 
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (1, 1, N'sys.sync', N'RPC基础服务', 4, CAST(N'2021-01-26T12:01:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (2, 1, N'sys.store', N'后台API网关服务', 4, CAST(N'2021-05-24T14:01:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (3, 1, N'sys.task', N'自动任务服务', 4, CAST(N'2021-08-11T15:29:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (4, 2, N'demo.gateway', N'演示网关服务', 4, CAST(N'2021-05-24T13:58:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (5, 2, N'demo.user', N'演示用户服务', 4, CAST(N'2021-05-24T14:02:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [BalancedType], [AddTime]) VALUES (6, 2, N'demo.order', N'演示订单服务', 4, CAST(N'2021-05-24T13:59:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[RemoteServerType] OFF
GO
INSERT [dbo].[RpcControlList] ([Id], [ServerIp], [Port], [RegionId], [IsDrop]) VALUES (1, N'127.0.0.1', 983, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[RpcMer] ON 
GO
INSERT [dbo].[RpcMer] ([Id], [SystemName], [AppId], [AppSecret], [AllowServerIp], [AddTime]) VALUES (1, N'Rpc组件服务', N'4a96a64fa89e4fd387d9a881a6a539f9', N'c429d30e8ab14464b70ce926802ba7dd', N'["*"]', CAST(N'2021-03-25T16:58:00' AS SmallDateTime))
GO
INSERT [dbo].[RpcMer] ([Id], [SystemName], [AppId], [AppSecret], [AllowServerIp], [AddTime]) VALUES (2, N'Demo演示服务', N'9cc83d72ba2a4b9f94493325c3acf4ee', N'a375982219934630aabb646e41e0ef14', N'["*"]', CAST(N'2021-05-24T14:04:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[RpcMer] OFF
GO
SET IDENTITY_INSERT [dbo].[ServerGroup] ON 
GO
INSERT [dbo].[ServerGroup] ([Id], [TypeVal], [GroupName]) VALUES (1, N'sys', N'系统服务节点')
GO
INSERT [dbo].[ServerGroup] ([Id], [TypeVal], [GroupName]) VALUES (2, N'demo', N'演示服务')
GO
SET IDENTITY_INSERT [dbo].[ServerGroup] OFF
GO
SET IDENTITY_INSERT [dbo].[ServerRegion] ON 
GO
INSERT [dbo].[ServerRegion] ([Id], [RegionName], [IsDrop]) VALUES (1, N'默认', 0)
GO
SET IDENTITY_INSERT [dbo].[ServerRegion] OFF
GO
INSERT [dbo].[ServerRunState] ([ServerId], [Pid], [PName], [ConNum], [WorkMemory], [CpuRunTime], [SyncTime], [StartTime]) VALUES (1, 39080, N'ConsoleApp', 5, 84119552, 6015, CAST(N'2021-08-10T17:58:00' AS SmallDateTime), CAST(N'2021-08-10T17:58:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerRunState] ([ServerId], [Pid], [PName], [ConNum], [WorkMemory], [CpuRunTime], [SyncTime], [StartTime]) VALUES (3, 660, N'Wedonek.Gateway', 1, 79495168, 3468, CAST(N'2021-08-10T18:12:00' AS SmallDateTime), CAST(N'2021-08-10T17:50:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerRunState] ([ServerId], [Pid], [PName], [ConNum], [WorkMemory], [CpuRunTime], [SyncTime], [StartTime]) VALUES (4, 37320, N'Wedonek.OrderService', 3, 78909440, 3421, CAST(N'2021-08-10T18:13:00' AS SmallDateTime), CAST(N'2021-08-10T17:50:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerRunState] ([ServerId], [Pid], [PName], [ConNum], [WorkMemory], [CpuRunTime], [SyncTime], [StartTime]) VALUES (5, 26488, N'Wedonek.UserService', 5, 77422592, 3484, CAST(N'2021-08-10T18:12:00' AS SmallDateTime), CAST(N'2021-08-10T17:49:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (1, 4, 2, 14, 2921, 0, 0, CAST(N'2021-08-08T17:55:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (1, 5, 2, 15, 2921, 0, 0, CAST(N'2021-08-08T17:55:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (3, 1, 0, 10, 254, 0, 1, CAST(N'2021-08-10T18:08:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (3, 4, 2, 10, 625, 0, 0, CAST(N'2021-08-10T18:12:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (3, 5, 2, 10, 625, 0, 0, CAST(N'2021-08-10T18:12:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (4, 1, 0, 10, 135, 0, 1, CAST(N'2021-08-10T18:09:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (4, 5, 2, 10, 335, 0, 0, CAST(N'2021-08-10T18:13:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerSignalState] ([ServerId], [RemoteId], [ConNum], [AvgTime], [SendNum], [ErrorNum], [UsableState], [SyncTime]) VALUES (5, 1, 0, 10, 140, 0, 1, CAST(N'2021-08-10T18:08:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[SysConfig] ON 
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (1, 0, 0, 0, N'redis', 1, N'{"ServerIp":["127.0.0.1"]}', CAST(N'2021-03-25T21:33:02.653' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (2, 0, 0, 0, N'rpc_config', 1, N'{"ErrorGradeLimit":0,"CacheType":1,"IsDebug":true,"IsEnableQueue":true,"ErrorGradeLimit":3,"LogGrade":1,"IsUpError":true}', CAST(N'2021-06-20T15:53:05.250' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (3, 0, 0, 0, N'accredit', 1, N'{"HeartbeatTime":600,"ErrorVaildTime":10,"MinCheckTime":5,"MaxCheckime":60,"MinCacheTime":180,"MaxCacheTime":300}', CAST(N'2021-05-28T17:20:49.930' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (4, 0, 0, 0, N'memcached', 1, N'{"ServerIp":["127.0.0.1"]}', CAST(N'2021-05-24T14:10:30.073' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (5, 0, 0, 2, N'RpcAdmin_admin', 0, N'060fbb505780234e5744f9bd98a65502', CAST(N'2021-05-08T18:11:10.920' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (6, 0, 0, 0, N'queue', 1, N'{"UserName":"demo","Pwd":"123456","Servers":[{"ServerIp":"127.0.0.1"}]}', CAST(N'2021-05-24T14:14:19.160' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (7, 0, 0, 0, N'track', 1, N'{"IsEnable":false,"Trace128Bits":true,"TrackDepth":30,"TrackRange":2,"SamplingRate":1000000,"ZipkinTack":{"PostUri":"http://127.0.0.1:9411/api/v1/spans"}}', CAST(N'2021-06-05T11:12:06.630' AS DateTime))
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServerId], [SystemTypeId], [Name], [ValueType], [Value], [ToUpdateTime]) VALUES (9, 0, 0, 0, N'identity', 1, N'{"IsEnable":false}', CAST(N'2021-08-03T17:39:07.703' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SysConfig] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ErrorCollect]    Script Date: 2021/8/11 15:34:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ErrorCollect] ON [dbo].[ErrorCollect]
(
	[ErrorCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK_ErrorLangMsg_1]    Script Date: 2021/8/11 15:34:16 ******/
ALTER TABLE [dbo].[ErrorLangMsg] ADD  CONSTRAINT [PK_ErrorLangMsg_1] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ErrorLangMsg_1]    Script Date: 2021/8/11 15:34:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ErrorLangMsg_1] ON [dbo].[ErrorLangMsg]
(
	[ErrorId] ASC,
	[Lang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RemoteServerType]    Script Date: 2021/8/11 15:34:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RemoteServerType] ON [dbo].[RemoteServerType]
(
	[TypeVal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RpcMerConfig]    Script Date: 2021/8/11 15:34:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RpcMerConfig] ON [dbo].[RpcMerConfig]
(
	[RpcMerId] ASC,
	[SystemTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_SysConfig]    Script Date: 2021/8/11 15:34:16 ******/
CREATE NONCLUSTERED INDEX [IX_SysConfig] ON [dbo].[SysConfig]
(
	[SystemTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_RpcMerId]  DEFAULT ((0)) FOR [RpcMerId]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_TaskPriority]  DEFAULT ((1)) FOR [TaskPriority]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_RegionId]  DEFAULT ((1)) FOR [RegionId]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_VerNum]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[ErrorCollect] ADD  CONSTRAINT [DF_ErrorCollect_IsPerfect]  DEFAULT ((0)) FOR [IsPerfect]
GO
ALTER TABLE [dbo].[ErrorLangMsg] ADD  CONSTRAINT [DF_ErrorLangMsg_Lang]  DEFAULT ('zh') FOR [Lang]
GO
ALTER TABLE [dbo].[ReduceInRankConfig] ADD  CONSTRAINT [DF_ReduceInRankConfig_IsEnable]  DEFAULT ((0)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[ReduceInRankConfig] ADD  CONSTRAINT [DF_ReduceInRankConfig_FusingErrorNum]  DEFAULT ((1)) FOR [FusingErrorNum]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ServerIndex]  DEFAULT ((0)) FOR [ServerIndex]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ServiceState]  DEFAULT ((0)) FOR [ServiceState]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_Weight]  DEFAULT ((1)) FOR [Weight]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_IsOnline]  DEFAULT ((0)) FOR [IsOnline]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_BindIndex]  DEFAULT ((0)) FOR [BindIndex]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ConfigPrower]  DEFAULT ((0)) FOR [ConfigPrower]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ApiVer]  DEFAULT ('0.0.0') FOR [ApiVer]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_LastOffliceDate]  DEFAULT (getdate()) FOR [LastOffliceDate]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RemoteServerGroup] ADD  CONSTRAINT [DF_RemoteServerGroup_RegionId]  DEFAULT ((1)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RemoteServerGroup] ADD  CONSTRAINT [DF_RemoteServerGroup_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_GroupId]  DEFAULT ((0)) FOR [GroupId]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_BalancedType]  DEFAULT ((4)) FOR [BalancedType]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RpcControlList] ADD  CONSTRAINT [DF_RpcControlList_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RpcMer] ADD  CONSTRAINT [DF_RpcMer_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RpcMerConfig] ADD  CONSTRAINT [DF_RpcMerConfig_IsolateLevel]  DEFAULT ((1)) FOR [IsolateLevel]
GO
ALTER TABLE [dbo].[ServerLimitConfig] ADD  CONSTRAINT [DF_ServerLimitConfig_IsEnableBucket]  DEFAULT ((1)) FOR [IsEnableBucket]
GO
ALTER TABLE [dbo].[ServerLimitConfig] ADD  CONSTRAINT [DF_ServerLimitConfig_BucketSize]  DEFAULT ((100)) FOR [BucketSize]
GO
ALTER TABLE [dbo].[ServerLimitConfig] ADD  CONSTRAINT [DF_ServerLimitConfig_BucketOutNum]  DEFAULT ((2)) FOR [BucketOutNum]
GO
ALTER TABLE [dbo].[ServerRegion] ADD  CONSTRAINT [DF_ServerRegion_IsDrop]  DEFAULT ((0)) FOR [IsDrop]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_Pid]  DEFAULT ((0)) FOR [Pid]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_ConNum]  DEFAULT ((0)) FOR [ConNum]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_WorkMemory]  DEFAULT ((0)) FOR [WorkMemory]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_CpuRunTime]  DEFAULT ((0)) FOR [CpuRunTime]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_SyncTime]  DEFAULT (getdate()) FOR [SyncTime]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_ConNum]  DEFAULT ((0)) FOR [ConNum]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_AvgTime]  DEFAULT ((0)) FOR [AvgTime]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_SendNum]  DEFAULT ((0)) FOR [SendNum]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_ErrorNum]  DEFAULT ((0)) FOR [ErrorNum]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_UsableState]  DEFAULT ((0)) FOR [UsableState]
GO
ALTER TABLE [dbo].[ServerSignalState] ADD  CONSTRAINT [DF_ServerSignalState_SyncTime]  DEFAULT (getdate()) FOR [SyncTime]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_RpcMerId]  DEFAULT ((0)) FOR [RpcMerId]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ServerId]  DEFAULT ((0)) FOR [ServerId]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ValueType]  DEFAULT ((0)) FOR [ValueType]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ToUpdateTime]  DEFAULT (getdate()) FOR [ToUpdateTime]
GO
ALTER TABLE [dbo].[AutoTaskList]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskList_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskList] NOCHECK CONSTRAINT [FK_AutoTaskList_RpcMer]
GO
ALTER TABLE [dbo].[AutoTaskList]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskList_ServerRegion] FOREIGN KEY([RegionId])
REFERENCES [dbo].[ServerRegion] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskList] NOCHECK CONSTRAINT [FK_AutoTaskList_ServerRegion]
GO
ALTER TABLE [dbo].[ErrorLangMsg]  WITH NOCHECK ADD  CONSTRAINT [FK_ErrorLangMsg_ErrorCollect] FOREIGN KEY([ErrorId])
REFERENCES [dbo].[ErrorCollect] ([Id])
GO
ALTER TABLE [dbo].[ErrorLangMsg] NOCHECK CONSTRAINT [FK_ErrorLangMsg_ErrorCollect]
GO
ALTER TABLE [dbo].[ReduceInRankConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_ReduceInRankConfig_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ReduceInRankConfig] NOCHECK CONSTRAINT [FK_ReduceInRankConfig_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ReduceInRankConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_ReduceInRankConfig_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[ReduceInRankConfig] NOCHECK CONSTRAINT [FK_ReduceInRankConfig_RpcMer]
GO
ALTER TABLE [dbo].[RemoteServerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerConfig_RemoteServerType] FOREIGN KEY([SystemType])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerConfig] NOCHECK CONSTRAINT [FK_RemoteServerConfig_RemoteServerType]
GO
ALTER TABLE [dbo].[RemoteServerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerConfig_RpcControlList] FOREIGN KEY([BindIndex])
REFERENCES [dbo].[RpcControlList] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerConfig] NOCHECK CONSTRAINT [FK_RemoteServerConfig_RpcControlList]
GO
ALTER TABLE [dbo].[RemoteServerConfig]  WITH CHECK ADD  CONSTRAINT [FK_RemoteServerConfig_ServerGroup] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ServerGroup] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerConfig] CHECK CONSTRAINT [FK_RemoteServerConfig_ServerGroup]
GO
ALTER TABLE [dbo].[RemoteServerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerConfig_ServerRegion] FOREIGN KEY([RegionId])
REFERENCES [dbo].[ServerRegion] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerConfig] NOCHECK CONSTRAINT [FK_RemoteServerConfig_ServerRegion]
GO
ALTER TABLE [dbo].[RemoteServerGroup]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerGroup_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerGroup] NOCHECK CONSTRAINT [FK_RemoteServerGroup_RemoteServerConfig]
GO
ALTER TABLE [dbo].[RemoteServerGroup]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerGroup_RemoteServerType] FOREIGN KEY([SystemType])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerGroup] NOCHECK CONSTRAINT [FK_RemoteServerGroup_RemoteServerType]
GO
ALTER TABLE [dbo].[RemoteServerGroup]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerGroup_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerGroup] NOCHECK CONSTRAINT [FK_RemoteServerGroup_RpcMer]
GO
ALTER TABLE [dbo].[RemoteServerType]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerType_ServerGroup] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ServerGroup] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerType] NOCHECK CONSTRAINT [FK_RemoteServerType_ServerGroup]
GO
ALTER TABLE [dbo].[RpcControlList]  WITH NOCHECK ADD  CONSTRAINT [FK_RpcControlList_ServerRegion] FOREIGN KEY([RegionId])
REFERENCES [dbo].[ServerRegion] ([Id])
GO
ALTER TABLE [dbo].[RpcControlList] NOCHECK CONSTRAINT [FK_RpcControlList_ServerRegion]
GO
ALTER TABLE [dbo].[RpcMerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RpcMerConfig_RemoteServerType] FOREIGN KEY([SystemTypeId])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[RpcMerConfig] NOCHECK CONSTRAINT [FK_RpcMerConfig_RemoteServerType]
GO
ALTER TABLE [dbo].[RpcMerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RpcMerConfig_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[RpcMerConfig] NOCHECK CONSTRAINT [FK_RpcMerConfig_RpcMer]
GO
ALTER TABLE [dbo].[ServerClientLimit]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerClientLimit_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerClientLimit] NOCHECK CONSTRAINT [FK_ServerClientLimit_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ServerClientLimit]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerClientLimit_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[ServerClientLimit] NOCHECK CONSTRAINT [FK_ServerClientLimit_RpcMer]
GO
ALTER TABLE [dbo].[ServerDictateLimit]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerDictateLimit_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerDictateLimit] NOCHECK CONSTRAINT [FK_ServerDictateLimit_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ServerLimitConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerLimitConfig_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerLimitConfig] NOCHECK CONSTRAINT [FK_ServerLimitConfig_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ServerRunState]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerRunState_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerRunState] NOCHECK CONSTRAINT [FK_ServerRunState_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ServerSignalState]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerSignalState_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerSignalState] NOCHECK CONSTRAINT [FK_ServerSignalState_RemoteServerConfig]
GO
ALTER TABLE [dbo].[ServerSignalState]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerSignalState_RemoteServerConfig1] FOREIGN KEY([RemoteId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerSignalState] NOCHECK CONSTRAINT [FK_ServerSignalState_RemoteServerConfig1]
GO
ALTER TABLE [dbo].[SysConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_SysConfig_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[SysConfig] NOCHECK CONSTRAINT [FK_SysConfig_RemoteServerConfig]
GO
ALTER TABLE [dbo].[SysConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_SysConfig_RemoteServerType] FOREIGN KEY([SystemTypeId])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[SysConfig] NOCHECK CONSTRAINT [FK_SysConfig_RemoteServerType]
GO
ALTER TABLE [dbo].[SysConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_SysConfig_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[SysConfig] NOCHECK CONSTRAINT [FK_SysConfig_RpcMer]
GO
/****** Object:  StoredProcedure [dbo].[RegError]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[RegError]
@Code varchar(50)
as
	declare @ErrorId bigint=0,@IsPerfect bit=0;
	select @ErrorId=Id,@IsPerfect=IsPerfect from ErrorCollect(nolock) where ErrorCode=@Code
	if(@@rowcount !=0)
	begin
		select @ErrorId as ErrorId,@IsPerfect as IsPerfect
	end
	else
	begin
		insert into ErrorCollect(ErrorCode)values(@Code)
		select SCOPE_IDENTITY() as ErrorId,@IsPerfect as IsPerfect
	end
GO
/****** Object:  StoredProcedure [dbo].[ServerOffline]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ServerOffline]
@Id bigint,
@IsOffline bit output,
@Res bit output
as
	set @IsOffline=0;
	update RemoteServerConfig with(rowlock) set IsOnline=0 where Id=@Id and IsOnline=1
	if(@@rowcount >0)
	begin
		set @IsOffline=1;
		set @Res=1;
	end 
GO
/****** Object:  StoredProcedure [dbo].[ServerOnline]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ServerOnline]
@Id bigint,
@Index int,
@IsOneOnline bit output,
@Res bit output
as
	set @Res=0;
	set @IsOneOnline=0;
	update RemoteServerConfig with(rowlock) set IsOnline=1,BindIndex=@Index where Id=@Id and IsOnline=0
	if(@@rowcount >0)
	begin
		set @Res=1;
		set @IsOneOnline=1;
	end 
GO
/****** Object:  StoredProcedure [dbo].[SyncError]    Script Date: 2021/8/11 15:34:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SyncError]
@Code varchar(50),
@ErrorId bigint output,
@Res bit output
as
	set @Res=0;
	set @ErrorId=0;
	select @ErrorId=Id from ErrorCollect(nolock) where ErrorCode=@Code
	if(@@rowcount !=0)
	begin
		set @Res=1;
		select Lang,Msg from ErrorLangMsg(nolock) where ErrorId=@ErrorId
	end
	else
	begin
		insert into ErrorCollect(ErrorCode)values(@Code)
		set @ErrorId=SCOPE_IDENTITY();
		if(@ErrorId !=0)
		begin
			set @Res=1;
		end
	end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务执行间隔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskTimeSpan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务优先级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskPriority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'SendType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'SendParam'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'VerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动任务信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorCollect', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorCollect', @level2type=N'COLUMN',@level2name=N'ErrorCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已完善' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorCollect', @level2type=N'COLUMN',@level2name=N'IsPerfect'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误码表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorCollect'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLangMsg', @level2type=N'COLUMN',@level2name=N'ErrorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属语言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLangMsg', @level2type=N'COLUMN',@level2name=N'Lang'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提示信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLangMsg', @level2type=N'COLUMN',@level2name=N'Msg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLangMsg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'触发限制错误数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'LimitNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接失败触发熔断次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'FusingErrorNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'刷新统计数的时间(秒)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'RefreshTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最短融断时长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'BeginDuration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最长熔断时长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig', @level2type=N'COLUMN',@level2name=N'EndDuration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点降级配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReduceInRankConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServerName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认链接Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServerIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'远端链接IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'RemoteIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点端口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServerPort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点分组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器mac地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServerMac'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServerIndex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公共key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'PublicKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServiceState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点负载均衡配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'TransmitConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点负载权重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否在线' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'IsOnline'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接的服务中心Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'BindIndex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置权限值大于10 远程优先' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ConfigPrower'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际链接Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ConIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ApiVer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后离线日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'LastOffliceDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点服务配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'TypeVal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'负载权重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup', @level2type=N'COLUMN',@level2name=N'Weight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群与服务节点关系表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'远程服务器类型表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'TypeVal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'SystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全局负载方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'BalancedType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点类型表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList', @level2type=N'COLUMN',@level2name=N'ServerIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所在区域Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList', @level2type=N'COLUMN',@level2name=N'IsDrop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务中心地址表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'认证商家表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'SystemName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群AppId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群链接密钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'AppSecret'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群授权链接的IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'AllowServerIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务集群表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMerConfig', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMerConfig', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用区域隔离' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMerConfig', @level2type=N'COLUMN',@level2name=N'IsRegionIsolate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'隔离级别 1 完全隔离 0 区域隔离' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMerConfig', @level2type=N'COLUMN',@level2name=N'IsolateLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcMerConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限制类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'LimitType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定请求量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'LimitNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定的时间窗口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'LimitTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定令牌数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'TokenNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每秒写入令牌数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit', @level2type=N'COLUMN',@level2name=N'TokenInNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点客户端限流配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerClientLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指令值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'Dictate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限制类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'LimitType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大流量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'LimitNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'窗口大小（秒）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'LimitTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'桶大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'BucketSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'桶溢出速度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'BucketOutNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定令牌数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'TokenNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每秒写入令牌数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit', @level2type=N'COLUMN',@level2name=N'TokenInNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务消息指令限流配置表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerDictateLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组别值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerGroup', @level2type=N'COLUMN',@level2name=N'TypeVal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组别名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerGroup', @level2type=N'COLUMN',@level2name=N'GroupName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务组别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定方式 0 无 1固定时间窗 2 流动时间窗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'LimitType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定请求量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'LimitNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定的时间窗口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'LimitTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'超过限定流量是否启用桶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'IsEnableBucket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'桶大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'BucketSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'桶固定出量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'BucketOutNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'限定令牌数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'TokenNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每秒写入令牌数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig', @level2type=N'COLUMN',@level2name=N'TokenInNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点限流配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerLimitConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRegion', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRegion', @level2type=N'COLUMN',@level2name=N'RegionName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRegion', @level2type=N'COLUMN',@level2name=N'IsDrop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务区域表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRegion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'基础Pid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'Pid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'进程名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'PName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'ConNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作内存占用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'WorkMemory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU占用时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'CpuRunTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后同步时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'SyncTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启动时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'StartTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点运行状态表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接的服务节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'RemoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'ConNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均响应时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'AvgTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送的包数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'SendNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'ErrorNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可用状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'UsableState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同步时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState', @level2type=N'COLUMN',@level2name=N'SyncTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点链接状态表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerSignalState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ValueType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ToUpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统配置信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig'
GO
USE [master]
GO
ALTER DATABASE [RpcService] SET  READ_WRITE 
GO
