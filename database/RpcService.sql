USE [RpcService]
GO
/****** Object:  Table [dbo].[ContainerGroup]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContainerGroup](
	[Id] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[HostMac] [varchar](17) NOT NULL,
	[ContainerType] [tinyint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Remark] [nvarchar](150) NULL,
	[HostIp] [varchar](36) NULL,
	[CreateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ContainerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContainerList]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContainerList](
	[Id] [bigint] NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[ContrainerId] [varchar](100) NOT NULL,
	[InternalIp] [varchar](36) NOT NULL,
	[InternalPort] [int] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ContainerList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictCollect]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictCollect](
	[DictCode] [varchar](10) NOT NULL,
	[DictName] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_DictCollect] PRIMARY KEY CLUSTERED 
(
	[DictCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictItem]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictItem](
	[DictCode] [varchar](10) NOT NULL,
	[ItemCode] [varchar](50) NOT NULL,
	[ItemText] [nvarchar](50) NOT NULL,
	[PrtItemCode] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DictItem] PRIMARY KEY CLUSTERED 
(
	[DictCode] ASC,
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorCollect]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorCollect](
	[Id] [bigint] NOT NULL,
	[ErrorCode] [varchar](100) NOT NULL,
	[IsPerfect] [bit] NOT NULL,
 CONSTRAINT [PK_ErrorCollect] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLangMsg]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLangMsg](
	[Id] [bigint] NOT NULL,
	[ErrorId] [bigint] NOT NULL,
	[Lang] [varchar](20) NOT NULL,
	[Msg] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ErrorLangMsg_1] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KafkaExchange]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KafkaExchange](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Exchange] [varchar](100) NOT NULL,
 CONSTRAINT [PK_KafkaExchange] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KafkaRouteKey]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KafkaRouteKey](
	[Id] [bigint] NOT NULL,
	[ExchangeId] [int] NOT NULL,
	[RouteKey] [varchar](100) NOT NULL,
	[Queue] [varchar](100) NOT NULL,
 CONSTRAINT [PK_KafkaRouteKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReduceInRankConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReduceInRankConfig](
	[Id] [bigint] NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerConfig](
	[Id] [bigint] NOT NULL,
	[ServerCode] [varchar](50) NULL,
	[ServerName] [nvarchar](50) NOT NULL,
	[ServerIp] [varchar](36) NOT NULL,
	[RemoteIp] [varchar](36) NOT NULL,
	[ServerPort] [int] NOT NULL,
	[RemotePort] [int] NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[SystemType] [bigint] NOT NULL,
	[ServerMac] [varchar](17) NOT NULL,
	[ServerIndex] [int] NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
	[HoldRpcMerId] [bigint] NOT NULL,
	[ContainerId] [bigint] NULL,
	[ContainerGroupId] [bigint] NULL,
	[IsContainer] [bit] NOT NULL,
	[PublicKey] [varchar](32) NOT NULL,
	[ServiceState] [smallint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[IsOnline] [bit] NOT NULL,
	[BindIndex] [int] NOT NULL,
	[ConfigPrower] [smallint] NOT NULL,
	[RecoveryLimit] [int] NOT NULL,
	[RecoveryTime] [int] NOT NULL,
	[ConIp] [varchar](36) NOT NULL,
	[ApiVer] [int] NOT NULL,
	[VerNum] [int] NOT NULL,
	[LastOffliceDate] [date] NOT NULL,
	[Remark] [nvarchar](150) NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RemoteServerConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerGroup]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerGroup](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[SystemType] [bigint] NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
	[Weight] [int] NOT NULL,
	[IsHold] [bit] NOT NULL,
 CONSTRAINT [PK_RemoteServerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteServerType]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteServerType](
	[Id] [bigint] NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[SystemName] [nvarchar](50) NOT NULL,
	[DefPort] [int] NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RemoteServerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcControlList]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcControlList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServerIp] [varchar](15) NOT NULL,
	[Port] [int] NOT NULL,
	[RegionId] [int] NOT NULL,
	[Show] [nvarchar](255) NULL,
 CONSTRAINT [PK_RpcControlList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcMer]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcMer](
	[Id] [bigint] NOT NULL,
	[SystemName] [nvarchar](50) NOT NULL,
	[AppId] [varchar](32) NOT NULL,
	[AppSecret] [varchar](32) NOT NULL,
	[AllowServerIp] [varchar](max) NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_RpcMer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcMerConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcMerConfig](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[IsRegionIsolate] [bit] NOT NULL,
	[IsolateLevel] [bit] NOT NULL,
	[BalancedType] [tinyint] NOT NULL,
 CONSTRAINT [PK_RpcMerConfig_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcMerServerVer]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcMerServerVer](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[CurrentVer] [int] NOT NULL,
 CONSTRAINT [PK_RpcMerServerVer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerClientLimit]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerClientLimit](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[LimitType] [smallint] NOT NULL,
	[LimitNum] [int] NOT NULL,
	[LimitTime] [smallint] NOT NULL,
	[TokenNum] [smallint] NOT NULL,
	[TokenInNum] [smallint] NOT NULL,
 CONSTRAINT [PK_ServerClientLimit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerCurConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerCurConfig](
	[ServerId] [bigint] NOT NULL,
	[CurConfig] [ntext] NOT NULL,
	[UpTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ServerCurConfig] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerDictateLimit]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerDictateLimit](
	[Id] [bigint] NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerEnvironment]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerEnvironment](
	[Id] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[Is64BitOperatingSystem] [bit] NOT NULL,
	[IsPrivilegedProcess] [bit] NOT NULL,
	[Is64BitProcess] [bit] NOT NULL,
	[OSVersion] [varchar](100) NOT NULL,
	[ExitCode] [int] NOT NULL,
	[CommandLine] [varchar](1000) NOT NULL,
	[SystemPageSize] [int] NOT NULL,
	[UserDomainName] [nvarchar](50) NOT NULL,
	[UserInteractive] [bit] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Version] [varchar](50) NOT NULL,
	[LogicalDrives] [nvarchar](3000) NOT NULL,
	[EnvironmentVariables] [varchar](max) NOT NULL,
	[MainModule] [varchar](max) NULL,
	[Modules] [text] NULL,
	[SyncTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerEnvironment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerGroup]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerGroup](
	[Id] [bigint] NOT NULL,
	[TypeVal] [varchar](50) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ServerGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerLimitConfig]    Script Date: 2024/5/5 16:03:04 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerPublicScheme]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerPublicScheme](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SchemeName] [nvarchar](50) NOT NULL,
	[SchemeShow] [nvarchar](100) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[LastTime] [smalldatetime] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerPublicScheme] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerRegion]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerRegion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionName] [nvarchar](50) NOT NULL,
	[CountryId] [int] NOT NULL,
	[ProId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[DistrictId] [int] NULL,
	[Address] [nvarchar](100) NULL,
	[Contacts] [nvarchar](20) NULL,
	[Phone] [varchar](30) NULL,
	[IsDrop] [bit] NOT NULL,
 CONSTRAINT [PK_ServerRegion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerRunState]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerRunState](
	[ServerId] [bigint] NOT NULL,
	[Pid] [int] NOT NULL,
	[PName] [nvarchar](50) NOT NULL,
	[MachineName] [nvarchar](50) NOT NULL,
	[Framework] [varchar](50) NOT NULL,
	[OSType] [tinyint] NOT NULL,
	[OSArchitecture] [tinyint] NOT NULL,
	[OSDescription] [varchar](150) NOT NULL,
	[ProcessArchitecture] [tinyint] NOT NULL,
	[ProcessorCount] [int] NOT NULL,
	[RuntimeIdentifier] [varchar](50) NOT NULL,
	[RunUserIdentity] [varchar](50) NOT NULL,
	[RunUserGroups] [varchar](200) NOT NULL,
	[RunIsAdmin] [bit] NOT NULL,
	[IsLittleEndian] [bit] NOT NULL,
	[SystemStartTime] [datetime] NOT NULL,
	[ConNum] [int] NOT NULL,
	[WorkMemory] [bigint] NOT NULL,
	[CpuRunTime] [bigint] NOT NULL,
	[CpuRate] [smallint] NOT NULL,
	[LockContentionCount] [bigint] NOT NULL,
	[ThreadNum] [int] NOT NULL,
	[TimerNum] [bigint] NOT NULL,
	[GCBody] [varchar](max) NULL,
	[ThreadPool] [varchar](max) NULL,
	[SyncTime] [smalldatetime] NOT NULL,
	[StartTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerRunState] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerSignalState]    Script Date: 2024/5/5 16:03:04 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerTransmitConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerTransmitConfig](
	[Id] [bigint] NOT NULL,
	[SchemeId] [bigint] NOT NULL,
	[ServerCode] [varchar](50) NOT NULL,
	[TransmitConfig] [varchar](max) NOT NULL,
 CONSTRAINT [PK_ServerTransmitConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerTransmitScheme]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerTransmitScheme](
	[Id] [bigint] NOT NULL,
	[Scheme] [nvarchar](50) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[TransmitType] [tinyint] NOT NULL,
	[VerNum] [int] NOT NULL,
	[Show] [nvarchar](50) NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ServerTransmitScheme] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerVisitCensus]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerVisitCensus](
	[Id] [bigint] NOT NULL,
	[ServiceId] [bigint] NOT NULL,
	[Dictate] [varchar](50) NOT NULL,
	[Show] [nvarchar](255) NOT NULL,
	[VisitNum] [bigint] NOT NULL,
	[SuccessNum] [bigint] NOT NULL,
	[FailNum] [bigint] NOT NULL,
	[TodayVisit] [int] NOT NULL,
	[TodayFail] [int] NOT NULL,
	[TodaySuccess] [int] NOT NULL,
 CONSTRAINT [PK_ServerVisitCensus_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceVerConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceVerConfig](
	[Id] [bigint] NOT NULL,
	[SchemeId] [bigint] NOT NULL,
	[VerNum] [int] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
 CONSTRAINT [PK_ServiceVerConfig_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceVerRoute]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceVerRoute](
	[Id] [bigint] NOT NULL,
	[SchemeId] [bigint] NOT NULL,
	[VerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[ToVerId] [int] NOT NULL,
 CONSTRAINT [PK_ServiceVerRoute_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysConfig]    Script Date: 2024/5/5 16:03:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysConfig](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServiceType] [tinyint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[ContainerGroup] [bigint] NOT NULL,
	[VerNum] [int] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ValueType] [bit] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Show] [nvarchar](50) NULL,
	[Prower] [int] NOT NULL,
	[ConfigType] [tinyint] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[ToUpdateTime] [datetime] NOT NULL,
	[TemplateKey] [varchar](50) NULL,
 CONSTRAINT [PK_SysConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContainerGroup] ADD  CONSTRAINT [DF_ContainerGroup_RegionId]  DEFAULT ((1)) FOR [RegionId]
GO
ALTER TABLE [dbo].[ContainerGroup] ADD  CONSTRAINT [DF_ContainerGroup_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ContainerList] ADD  CONSTRAINT [DF_ContainerList_InternalPort]  DEFAULT ((0)) FOR [InternalPort]
GO
ALTER TABLE [dbo].[DictItem] ADD  CONSTRAINT [DF_DictItem_PrtItemCode]  DEFAULT ('root') FOR [PrtItemCode]
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
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ServerType]  DEFAULT ((0)) FOR [ServiceType]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_HoldRpcMerId]  DEFAULT ((0)) FOR [HoldRpcMerId]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_IsContainer]  DEFAULT ((0)) FOR [IsContainer]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ServiceState]  DEFAULT ((0)) FOR [ServiceState]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_IsOnline]  DEFAULT ((0)) FOR [IsOnline]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_BindIndex]  DEFAULT ((0)) FOR [BindIndex]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_ConfigPrower]  DEFAULT ((0)) FOR [ConfigPrower]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_RecoveryLimit]  DEFAULT ((0)) FOR [RecoveryLimit]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_RecoveryTime]  DEFAULT ((0)) FOR [RecoveryTime]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_VerNum]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_LastOffliceDate]  DEFAULT (getdate()) FOR [LastOffliceDate]
GO
ALTER TABLE [dbo].[RemoteServerConfig] ADD  CONSTRAINT [DF_RemoteServerConfig_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RemoteServerGroup] ADD  CONSTRAINT [DF_RemoteServerGroup_RegionId]  DEFAULT ((1)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RemoteServerGroup] ADD  CONSTRAINT [DF_RemoteServerGroup_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[RemoteServerGroup] ADD  CONSTRAINT [DF_RemoteServerGroup_IsHold]  DEFAULT ((0)) FOR [IsHold]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_GroupId]  DEFAULT ((0)) FOR [GroupId]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_ServerType]  DEFAULT ((0)) FOR [ServiceType]
GO
ALTER TABLE [dbo].[RemoteServerType] ADD  CONSTRAINT [DF_RemoteServerType_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RpcControlList] ADD  CONSTRAINT [DF_RpcControlList_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[RpcMer] ADD  CONSTRAINT [DF_RpcMer_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[RpcMerConfig] ADD  CONSTRAINT [DF_RpcMerConfig_IsRegionIsolate]  DEFAULT ((0)) FOR [IsRegionIsolate]
GO
ALTER TABLE [dbo].[RpcMerConfig] ADD  CONSTRAINT [DF_RpcMerConfig_IsolateLevel]  DEFAULT ((1)) FOR [IsolateLevel]
GO
ALTER TABLE [dbo].[RpcMerConfig] ADD  CONSTRAINT [DF_RpcMerConfig_BalancedType]  DEFAULT ((4)) FOR [BalancedType]
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
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_ProcessorCount]  DEFAULT ((0)) FOR [ProcessorCount]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_RunIsAdmin]  DEFAULT ((0)) FOR [RunIsAdmin]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_IsLittleEndian]  DEFAULT ((1)) FOR [IsLittleEndian]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_ConNum]  DEFAULT ((0)) FOR [ConNum]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_WorkMemory]  DEFAULT ((0)) FOR [WorkMemory]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_CpuRunTime]  DEFAULT ((0)) FOR [CpuRunTime]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_CpuRate]  DEFAULT ((0)) FOR [CpuRate]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_ThreadNum]  DEFAULT ((0)) FOR [ThreadNum]
GO
ALTER TABLE [dbo].[ServerRunState] ADD  CONSTRAINT [DF_ServerRunState_TimerNum]  DEFAULT ((0)) FOR [TimerNum]
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
ALTER TABLE [dbo].[ServerTransmitScheme] ADD  CONSTRAINT [DF_ServerTransmitScheme_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_VisitNum]  DEFAULT ((0)) FOR [VisitNum]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_SuccessNum]  DEFAULT ((0)) FOR [SuccessNum]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_FailNum]  DEFAULT ((0)) FOR [FailNum]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_TodayVisit]  DEFAULT ((0)) FOR [TodayVisit]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_TodayFail]  DEFAULT ((0)) FOR [TodayFail]
GO
ALTER TABLE [dbo].[ServerVisitCensus] ADD  CONSTRAINT [DF_RpcServicesCensus_TodaySuccess]  DEFAULT ((0)) FOR [TodaySuccess]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_RpcMerId]  DEFAULT ((0)) FOR [RpcMerId]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ServiceType]  DEFAULT ((0)) FOR [ServiceType]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ServerId]  DEFAULT ((0)) FOR [ServerId]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ContainerType]  DEFAULT ((0)) FOR [ContainerGroup]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ApiVer]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ValueType]  DEFAULT ((0)) FOR [ValueType]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_Prower]  DEFAULT ((0)) FOR [Prower]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_IsBasicConfig]  DEFAULT ((0)) FOR [ConfigType]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_IsEnable]  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[SysConfig] ADD  CONSTRAINT [DF_SysConfig_ToUpdateTime]  DEFAULT (getdate()) FOR [ToUpdateTime]
GO
ALTER TABLE [dbo].[ContainerList]  WITH NOCHECK ADD  CONSTRAINT [FK_ContainerList_ContainerGroup] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ContainerGroup] ([Id])
GO
ALTER TABLE [dbo].[ContainerList] NOCHECK CONSTRAINT [FK_ContainerList_ContainerGroup]
GO
ALTER TABLE [dbo].[ErrorLangMsg]  WITH NOCHECK ADD  CONSTRAINT [FK_ErrorLangMsg_ErrorCollect] FOREIGN KEY([ErrorId])
REFERENCES [dbo].[ErrorCollect] ([Id])
GO
ALTER TABLE [dbo].[ErrorLangMsg] NOCHECK CONSTRAINT [FK_ErrorLangMsg_ErrorCollect]
GO
ALTER TABLE [dbo].[KafkaRouteKey]  WITH NOCHECK ADD  CONSTRAINT [FK_KafkaRouteKey_KafkaExchange] FOREIGN KEY([ExchangeId])
REFERENCES [dbo].[KafkaExchange] ([Id])
GO
ALTER TABLE [dbo].[KafkaRouteKey] NOCHECK CONSTRAINT [FK_KafkaRouteKey_KafkaExchange]
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
ALTER TABLE [dbo].[RemoteServerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_RemoteServerConfig_ContainerList] FOREIGN KEY([ContainerId])
REFERENCES [dbo].[ContainerList] ([Id])
GO
ALTER TABLE [dbo].[RemoteServerConfig] NOCHECK CONSTRAINT [FK_RemoteServerConfig_ContainerList]
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
ALTER TABLE [dbo].[ServerTransmitConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerTransmitConfig_ServerTransmitScheme] FOREIGN KEY([SchemeId])
REFERENCES [dbo].[ServerTransmitScheme] ([Id])
GO
ALTER TABLE [dbo].[ServerTransmitConfig] NOCHECK CONSTRAINT [FK_ServerTransmitConfig_ServerTransmitScheme]
GO
ALTER TABLE [dbo].[ServerTransmitScheme]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerTransmitScheme_RemoteServerType] FOREIGN KEY([SystemTypeId])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[ServerTransmitScheme] NOCHECK CONSTRAINT [FK_ServerTransmitScheme_RemoteServerType]
GO
ALTER TABLE [dbo].[ServerTransmitScheme]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerTransmitScheme_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[ServerTransmitScheme] NOCHECK CONSTRAINT [FK_ServerTransmitScheme_RpcMer]
GO
ALTER TABLE [dbo].[ServiceVerConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_ServiceVerConfig_RemoteServerType] FOREIGN KEY([SystemTypeId])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[ServiceVerConfig] NOCHECK CONSTRAINT [FK_ServiceVerConfig_RemoteServerType]
GO
ALTER TABLE [dbo].[ServiceVerRoute]  WITH NOCHECK ADD  CONSTRAINT [FK_ServiceVerRoute_RemoteServerType] FOREIGN KEY([SystemTypeId])
REFERENCES [dbo].[RemoteServerType] ([Id])
GO
ALTER TABLE [dbo].[ServiceVerRoute] NOCHECK CONSTRAINT [FK_ServiceVerRoute_RemoteServerType]
GO
ALTER TABLE [dbo].[ServiceVerRoute]  WITH NOCHECK ADD  CONSTRAINT [FK_ServiceVerRoute_ServiceVerConfig] FOREIGN KEY([VerId])
REFERENCES [dbo].[ServiceVerConfig] ([Id])
GO
ALTER TABLE [dbo].[ServiceVerRoute] NOCHECK CONSTRAINT [FK_ServiceVerRoute_ServiceVerConfig]
GO
ALTER TABLE [dbo].[SysConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_SysConfig_RemoteServerConfig] FOREIGN KEY([ServerId])
REFERENCES [dbo].[RemoteServerConfig] ([Id])
GO
ALTER TABLE [dbo].[SysConfig] NOCHECK CONSTRAINT [FK_SysConfig_RemoteServerConfig]
GO
ALTER TABLE [dbo].[SysConfig]  WITH NOCHECK ADD  CONSTRAINT [FK_SysConfig_RpcMer] FOREIGN KEY([RpcMerId])
REFERENCES [dbo].[RpcMer] ([Id])
GO
ALTER TABLE [dbo].[SysConfig] NOCHECK CONSTRAINT [FK_SysConfig_RpcMer]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'容器类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ContainerGroup', @level2type=N'COLUMN',@level2name=N'ContainerType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ContainerGroup', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'容器Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ContainerList', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'容器唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ContainerList', @level2type=N'COLUMN',@level2name=N'ContrainerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ContainerList', @level2type=N'COLUMN',@level2name=N'AddTime'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交换机名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaExchange', @level2type=N'COLUMN',@level2name=N'Exchange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kafka交换机字典' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaExchange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交换机ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaRouteKey', @level2type=N'COLUMN',@level2name=N'ExchangeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路由Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaRouteKey', @level2type=N'COLUMN',@level2name=N'RouteKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对应的队列' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaRouteKey', @level2type=N'COLUMN',@level2name=N'Queue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Kafka交换机的路由表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'KafkaRouteKey'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'容器Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ContainerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否在容器中运行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'IsContainer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公共key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'PublicKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点状态 0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ServiceState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属区域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否在线' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'IsOnline'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接的服务中心Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'BindIndex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置权限值大于10 远程优先' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ConfigPrower'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点熔断恢复后临时限流量(客户端限流)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'RecoveryLimit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点熔断恢复后临时限流时长(秒)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'RecoveryTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实际链接Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ConIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'ApiVer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点资料的版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerConfig', @level2type=N'COLUMN',@level2name=N'VerNum'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认端口号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RemoteServerType', @level2type=N'COLUMN',@level2name=N'DefPort'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcControlList', @level2type=N'COLUMN',@level2name=N'Show'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机器名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'MachineName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Framework版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'Framework'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作系统类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'OSType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平台架構x86' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'OSArchitecture'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述應用程式執行所在的作業系統' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'OSDescription'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'当前运行的应用程序的进程体系结构' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'ProcessArchitecture'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'執行應用程式的平台' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'RuntimeIdentifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行程序的用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'RunUserIdentity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行程序用户所在组别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'RunUserGroups'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否已管理员身份运行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'RunIsAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'ConNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作内存占用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'WorkMemory'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU占用时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'CpuRunTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尝试锁定监视器时出现争用的次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'LockContentionCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线程数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'ThreadNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GC信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerRunState', @level2type=N'COLUMN',@level2name=N'GCBody'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'ServiceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指令名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'Dictate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'方法说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'Show'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'调用总量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'VisitNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成功调用总量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'SuccessNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败调用总量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'FailNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'今日调用数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'TodayVisit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'今日失败调用数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'TodayFail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'今日成功调用数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus', @level2type=N'COLUMN',@level2name=N'TodaySuccess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点访问统计表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerVisitCensus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerConfig', @level2type=N'COLUMN',@level2name=N'VerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerConfig', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群下 单一版本的服务节点 限定连接的各服务节点版本范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerConfig'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点版本ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerRoute', @level2type=N'COLUMN',@level2name=N'VerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerRoute', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'可用起始版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerRoute', @level2type=N'COLUMN',@level2name=N'ToVerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'控制节点 可连接的版本范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServiceVerRoute'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ValueType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Show'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配置优先级 本地配置优先级默认等于10 prower>10 则覆盖' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'Prower'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig', @level2type=N'COLUMN',@level2name=N'ToUpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统配置信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysConfig'
GO
