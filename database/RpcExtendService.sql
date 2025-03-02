USE [RpcExtendService]
GO
/****** Object:  Table [dbo].[AccreditToken]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccreditToken](
	[AccreditId] [varchar](32) NOT NULL,
	[PAccreditId] [varchar](32) NULL,
	[AccreditRole] [varchar](1000) NOT NULL,
	[ApplyId] [varchar](50) NOT NULL,
	[CheckKey] [varchar](50) NOT NULL,
	[RoleType] [varchar](50) NOT NULL,
	[State] [nvarchar](1000) NOT NULL,
	[StateVer] [int] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SysGroup] [varchar](50) NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[Expire] [smalldatetime] NULL,
	[OverTime] [smalldatetime] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_AccreditToken] PRIMARY KEY CLUSTERED 
(
	[AccreditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoTaskItem]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTaskItem](
	[Id] [bigint] NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[ItemTitle] [nvarchar](50) NOT NULL,
	[ItemSort] [int] NOT NULL,
	[SendType] [tinyint] NOT NULL,
	[SendParam] [nvarchar](max) NOT NULL,
	[TimeOut] [int] NULL,
	[RetryNum] [smallint] NULL,
	[FailStep] [tinyint] NOT NULL,
	[FailNextStep] [int] NULL,
	[SuccessStep] [tinyint] NOT NULL,
	[NextStep] [int] NULL,
	[LogRange] [tinyint] NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[Error] [varchar](100) NULL,
	[LastExecTime] [smalldatetime] NULL,
 CONSTRAINT [PK_AutoTaskItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoTaskList]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTaskList](
	[Id] [bigint] NOT NULL,
	[RegionId] [int] NULL,
	[RpcMerId] [bigint] NOT NULL,
	[TaskName] [nvarchar](50) NOT NULL,
	[TaskShow] [nvarchar](255) NOT NULL,
	[TaskPriority] [int] NOT NULL,
	[BeginStep] [smallint] NOT NULL,
	[FailEmall] [varchar](1000) NULL,
	[VerNum] [int] NOT NULL,
	[IsExec] [bit] NOT NULL,
	[ExecVerNum] [int] NOT NULL,
	[LastExecTime] [datetime] NULL,
	[LastExecEndTime] [datetime] NULL,
	[NextExecTime] [datetime] NULL,
	[TaskStatus] [tinyint] NOT NULL,
	[StopTime] [datetime] NULL,
 CONSTRAINT [PK_AutoTaskList_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoTaskLog]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTaskLog](
	[Id] [bigint] NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[ItemId] [bigint] NOT NULL,
	[ItemTitle] [nvarchar](50) NOT NULL,
	[IsFail] [bit] NOT NULL,
	[Error] [varchar](100) NULL,
	[BeginTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Result] [nvarchar](1000) NULL,
 CONSTRAINT [PK_AutoTaskLog_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoTaskPlan]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTaskPlan](
	[Id] [bigint] NOT NULL,
	[TaskId] [bigint] NOT NULL,
	[PlanTitle] [nvarchar](50) NOT NULL,
	[PlanShow] [nvarchar](255) NULL,
	[PlanType] [tinyint] NOT NULL,
	[ExecTime] [datetime] NULL,
	[ExecRate] [tinyint] NOT NULL,
	[ExecSpace] [smallint] NULL,
	[SpaceType] [tinyint] NOT NULL,
	[SpaceDay] [smallint] NULL,
	[SpeceNum] [tinyint] NULL,
	[SpaceWeek] [smallint] NULL,
	[DayRate] [tinyint] NOT NULL,
	[DayTimeSpan] [int] NULL,
	[DaySpaceType] [tinyint] NOT NULL,
	[DaySpaceNum] [int] NULL,
	[DayBeginSpan] [int] NULL,
	[DayEndSpan] [int] NULL,
	[BeginDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[PlanOnlyNo] [varchar](32) NULL,
	[IsEnable] [bit] NOT NULL,
 CONSTRAINT [PK_AutoTaskPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BroadcastErrorLog]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BroadcastErrorLog](
	[Id] [bigint] NOT NULL,
	[Error] [varchar](100) NOT NULL,
	[MsgKey] [varchar](50) NOT NULL,
	[SourceId] [bigint] NOT NULL,
	[MsgBody] [ntext] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SysTypeVal] [varchar](100) NOT NULL,
	[MsgSource] [varchar](max) NOT NULL,
	[BroadcastType] [smallint] NOT NULL,
	[RouteKey] [varchar](50) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_BroadcastErrorLog_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictateNode]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictateNode](
	[Id] [bigint] NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[Dictate] [varchar](50) NOT NULL,
	[DictateName] [nvarchar](50) NOT NULL,
	[IsEndpoint] [bit] NOT NULL,
 CONSTRAINT [PK_DictateNode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictateNodeRelation]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictateNodeRelation](
	[SubId] [bigint] NOT NULL,
	[ParentId] [bigint] NOT NULL,
 CONSTRAINT [PK_DictateNodeRelation] PRIMARY KEY CLUSTERED 
(
	[SubId] ASC,
	[ParentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityApp]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityApp](
	[Id] [bigint] NOT NULL,
	[AppId] [varchar](32) NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppShow] [nvarchar](255) NULL,
	[AppExtend] [nvarchar](max) NULL,
	[EffectiveDate] [date] NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_IdentityApp_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Idgenerator]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idgenerator](
	[WorkId] [int] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[Mac] [varchar](17) NOT NULL,
	[ServerIndex] [int] NOT NULL,
 CONSTRAINT [PK_Idgenerator] PRIMARY KEY CLUSTERED 
(
	[WorkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IpBlackList]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IpBlackList](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[IpType] [tinyint] NOT NULL,
	[Ip6] [varchar](30) NOT NULL,
	[Ip] [bigint] NOT NULL,
	[EndIp] [bigint] NULL,
	[IsDrop] [bit] NOT NULL,
	[Remark] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_IpBlackList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceList]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceList](
	[Id] [bigint] NOT NULL,
	[ModularId] [bigint] NOT NULL,
	[ResourcePath] [varchar](100) NOT NULL,
	[FullPath] [varchar](300) NOT NULL,
	[ResourceShow] [nvarchar](100) NOT NULL,
	[ResourceState] [smallint] NOT NULL,
	[VerNum] [int] NOT NULL,
	[ResourceVer] [int] NOT NULL,
	[LastTime] [date] NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ResourceList_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceModular]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceModular](
	[Id] [bigint] NOT NULL,
	[ModularKey] [varchar](32) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[ModularName] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](50) NULL,
	[ResourceType] [smallint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ResourceModular] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceShield]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceShield](
	[Id] [bigint] NOT NULL,
	[ResourceId] [bigint] NOT NULL,
	[ShieIdKey] [varchar](36) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[VerNum] [int] NOT NULL,
	[SortNum] [smallint] NOT NULL,
	[ShieldType] [tinyint] NOT NULL,
	[ResourcePath] [varchar](100) NOT NULL,
	[BeOverdueTime] [bigint] NOT NULL,
	[ShieIdShow] [nvarchar](100) NULL,
 CONSTRAINT [PK_ResourceShield] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcTraceList]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcTraceList](
	[Id] [bigint] NOT NULL,
	[TraceId] [varchar](32) NOT NULL,
	[Dictate] [varchar](255) NOT NULL,
	[Show] [nvarchar](50) NULL,
	[BeginTime] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[RegionId] [int] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
 CONSTRAINT [PK_RpcTraceList_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RpcTraceLog]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RpcTraceLog](
	[Id] [bigint] NOT NULL,
	[TraceId] [varchar](32) NOT NULL,
	[SpanId] [bigint] NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[Dictate] [varchar](255) NOT NULL,
	[Show] [nvarchar](50) NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[RemoteId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[ReturnRes] [nvarchar](max) NOT NULL,
	[Args] [nvarchar](max) NOT NULL,
	[MsgType] [varchar](10) NULL,
	[StageType] [tinyint] NOT NULL,
	[BeginTime] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_RpcTraceLog_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerEventSwitch]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerEventSwitch](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SysEventId] [int] NOT NULL,
	[EventKey] [varchar](36) NOT NULL,
	[Module] [varchar](50) NOT NULL,
	[EventLevel] [tinyint] NOT NULL,
	[EventType] [tinyint] NOT NULL,
	[MsgTemplate] [nvarchar](300) NOT NULL,
	[EventConfig] [varchar](1000) NOT NULL,
	[IsEnable] [bit] NOT NULL,
 CONSTRAINT [PK_ServerEventSwitch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemErrorLog]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemErrorLog](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[TraceId] [varchar](64) NOT NULL,
	[LogTitle] [nvarchar](200) NOT NULL,
	[LogShow] [nvarchar](255) NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[LogGroup] [varchar](50) NOT NULL,
	[LogType] [tinyint] NOT NULL,
	[LogGrade] [tinyint] NOT NULL,
	[ErrorCode] [varchar](200) NOT NULL,
	[Exception] [ntext] NULL,
	[AttrList] [ntext] NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemErrorLog_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemEventConfig]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemEventConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [nvarchar](50) NOT NULL,
	[Module] [varchar](50) NOT NULL,
	[EventLevel] [tinyint] NOT NULL,
	[EventType] [tinyint] NOT NULL,
	[MsgTemplate] [nvarchar](300) NOT NULL,
	[EventConfig] [varchar](1000) NOT NULL,
	[IsEnable] [bit] NOT NULL,
 CONSTRAINT [PK_SystemEventConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemEventLog]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemEventLog](
	[Id] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[EventSourceId] [int] NOT NULL,
	[EventName] [nvarchar](50) NOT NULL,
	[EventLevel] [tinyint] NOT NULL,
	[EventType] [tinyint] NOT NULL,
	[EventShow] [nvarchar](500) NOT NULL,
	[EventAttr] [nvarchar](max) NOT NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemEventLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionList]    Script Date: 2024/5/5 16:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionList](
	[Id] [bigint] NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[RegionId] [int] NOT NULL,
	[TranName] [varchar](50) NOT NULL,
	[SubmitJson] [nvarchar](max) NOT NULL,
	[Extend] [nvarchar](500) NULL,
	[TranStatus] [tinyint] NOT NULL,
	[TranMode] [tinyint] NOT NULL,
	[IsRoot] [bit] NOT NULL,
	[IsLock] [bit] NOT NULL,
	[LockTime] [datetime] NULL,
	[CommitStatus] [tinyint] NOT NULL,
	[OverTime] [datetime] NOT NULL,
	[SubmitTime] [datetime] NULL,
	[FailTime] [datetime] NULL,
	[RetryNum] [smallint] NOT NULL,
	[Error] [varchar](100) NOT NULL,
	[EndTime] [datetime] NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TransactionList_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccreditToken] ADD  CONSTRAINT [DF_AccreditToken_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[AutoTaskItem] ADD  CONSTRAINT [DF_AutoTaskItem_TaskId]  DEFAULT ((0)) FOR [TaskId]
GO
ALTER TABLE [dbo].[AutoTaskItem] ADD  CONSTRAINT [DF_AutoTaskItem_IsWriteLog]  DEFAULT ((2)) FOR [LogRange]
GO
ALTER TABLE [dbo].[AutoTaskItem] ADD  CONSTRAINT [DF_AutoTaskItem_IsSuccess]  DEFAULT ((0)) FOR [IsSuccess]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_TaskPriority]  DEFAULT ((0)) FOR [TaskPriority]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_BeginItem]  DEFAULT ((0)) FOR [BeginStep]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_VerNum]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_ExecStatus]  DEFAULT ((0)) FOR [IsExec]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_ExecVerNum]  DEFAULT ((0)) FOR [ExecVerNum]
GO
ALTER TABLE [dbo].[AutoTaskList] ADD  CONSTRAINT [DF_AutoTaskList_IsEnable]  DEFAULT ((0)) FOR [TaskStatus]
GO
ALTER TABLE [dbo].[AutoTaskPlan] ADD  CONSTRAINT [DF_AutoTaskPlan_ExecRate]  DEFAULT ((0)) FOR [ExecRate]
GO
ALTER TABLE [dbo].[AutoTaskPlan] ADD  CONSTRAINT [DF_AutoTaskPlan_SpaceType]  DEFAULT ((0)) FOR [SpaceType]
GO
ALTER TABLE [dbo].[AutoTaskPlan] ADD  CONSTRAINT [DF_AutoTaskPlan_DayRate]  DEFAULT ((0)) FOR [DayRate]
GO
ALTER TABLE [dbo].[AutoTaskPlan] ADD  CONSTRAINT [DF_AutoTaskPlan_DaySpaceType]  DEFAULT ((0)) FOR [DaySpaceType]
GO
ALTER TABLE [dbo].[BroadcastErrorLog] ADD  CONSTRAINT [DF_BroadcastErrorLog_RpcMerId]  DEFAULT ((0)) FOR [RpcMerId]
GO
ALTER TABLE [dbo].[IdentityApp] ADD  CONSTRAINT [DF_IdentityApp_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[IpBlackList] ADD  CONSTRAINT [DF_IpBlackList_IpType]  DEFAULT ((0)) FOR [IpType]
GO
ALTER TABLE [dbo].[IpBlackList] ADD  CONSTRAINT [DF_IpBlackList_Ip6]  DEFAULT ((0)) FOR [Ip6]
GO
ALTER TABLE [dbo].[IpBlackList] ADD  CONSTRAINT [DF_IpBlackList_IsDrop]  DEFAULT ((0)) FOR [IsDrop]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_ResourceState]  DEFAULT ((1)) FOR [ResourceState]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_VerNum]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_ResourceVer]  DEFAULT ((0)) FOR [ResourceVer]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ResourceModular] ADD  CONSTRAINT [DF_ResourceModular_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ResourceShield] ADD  CONSTRAINT [DF_ResourceShield_ServerId]  DEFAULT ((0)) FOR [ServerId]
GO
ALTER TABLE [dbo].[ResourceShield] ADD  CONSTRAINT [DF_ResourceShield_VerNum]  DEFAULT ((0)) FOR [VerNum]
GO
ALTER TABLE [dbo].[SystemEventConfig] ADD  CONSTRAINT [DF_SystemEventConfig_IsEnable]  DEFAULT ((1)) FOR [IsEnable]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_IsLock]  DEFAULT ((0)) FOR [IsLock]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_IsCommit]  DEFAULT ((0)) FOR [CommitStatus]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_RetryNum]  DEFAULT ((0)) FOR [RetryNum]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_ErrorCode]  DEFAULT ((0)) FOR [Error]
GO
ALTER TABLE [dbo].[AutoTaskItem]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskItem_AutoTaskList] FOREIGN KEY([TaskId])
REFERENCES [dbo].[AutoTaskList] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskItem] NOCHECK CONSTRAINT [FK_AutoTaskItem_AutoTaskList]
GO
ALTER TABLE [dbo].[AutoTaskLog]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskLog_AutoTaskItem] FOREIGN KEY([ItemId])
REFERENCES [dbo].[AutoTaskItem] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskLog] NOCHECK CONSTRAINT [FK_AutoTaskLog_AutoTaskItem]
GO
ALTER TABLE [dbo].[AutoTaskLog]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskLog_AutoTaskList] FOREIGN KEY([TaskId])
REFERENCES [dbo].[AutoTaskList] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskLog] NOCHECK CONSTRAINT [FK_AutoTaskLog_AutoTaskList]
GO
ALTER TABLE [dbo].[AutoTaskPlan]  WITH NOCHECK ADD  CONSTRAINT [FK_AutoTaskPlan_AutoTaskList] FOREIGN KEY([TaskId])
REFERENCES [dbo].[AutoTaskList] ([Id])
GO
ALTER TABLE [dbo].[AutoTaskPlan] NOCHECK CONSTRAINT [FK_AutoTaskPlan_AutoTaskList]
GO
ALTER TABLE [dbo].[DictateNodeRelation]  WITH NOCHECK ADD  CONSTRAINT [FK_DictateNodeRelation_DictateNode] FOREIGN KEY([SubId])
REFERENCES [dbo].[DictateNode] ([Id])
GO
ALTER TABLE [dbo].[DictateNodeRelation] NOCHECK CONSTRAINT [FK_DictateNodeRelation_DictateNode]
GO
ALTER TABLE [dbo].[DictateNodeRelation]  WITH NOCHECK ADD  CONSTRAINT [FK_DictateNodeRelation_DictateNode1] FOREIGN KEY([ParentId])
REFERENCES [dbo].[DictateNode] ([Id])
GO
ALTER TABLE [dbo].[DictateNodeRelation] NOCHECK CONSTRAINT [FK_DictateNodeRelation_DictateNode1]
GO
ALTER TABLE [dbo].[ResourceShield]  WITH NOCHECK ADD  CONSTRAINT [FK_ResourceShield_ResourceList] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[ResourceList] ([Id])
GO
ALTER TABLE [dbo].[ResourceShield] NOCHECK CONSTRAINT [FK_ResourceShield_ResourceList]
GO
ALTER TABLE [dbo].[ServerEventSwitch]  WITH NOCHECK ADD  CONSTRAINT [FK_ServerEventSwitch_SystemEventConfig] FOREIGN KEY([SysEventId])
REFERENCES [dbo].[SystemEventConfig] ([Id])
GO
ALTER TABLE [dbo].[ServerEventSwitch] NOCHECK CONSTRAINT [FK_ServerEventSwitch_SystemEventConfig]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'AccreditId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级授权码（）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'PAccreditId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权的角色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'AccreditRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请唯一键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'ApplyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'检验Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'CheckKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务系统类别-默认使用发起节点的系统组别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'RoleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权状态值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存储的状态值版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'StateVer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请集群ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请的节点组别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'SysGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请的系统类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设定的有效时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'Expire'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权过期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'OverTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登陆认证Token存储表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AccreditToken'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'TaskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务項标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'ItemTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顺序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'ItemSort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'SendType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'SendParam'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'超时时间(秒)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'TimeOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允许的重试次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'RetryNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败时执行方案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'FailStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败时执行的步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'FailNextStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成功时执行方案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'SuccessStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成功时执行方案步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'NextStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志记录范围' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'LogRange'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后次是否执行成功' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'IsSuccess'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次错误码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem', @level2type=N'COLUMN',@level2name=N'LastExecTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务执行项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskItem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行的区域ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行的集群ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务优先级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskPriority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始步骤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'BeginStep'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败时发送的EMAIL地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'FailEmall'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'VerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否执行中' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'IsExec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'ExecVerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次执行开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'LastExecTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次执行结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'LastExecEndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下次执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'NextExecTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'TaskStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'停止时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList', @level2type=N'COLUMN',@level2name=N'StopTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动任务信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'TaskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务項Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务項标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'ItemTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'IsFail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败的错误码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回的结果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskLog', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'TaskId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计划标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'PlanTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计划说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'PlanShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'PlanType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'ExecTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行周期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'ExecRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行间隔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'ExecSpace'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月间隔类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'SpaceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'间隔天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'SpaceDay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'间隔数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'SpeceNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'间隔周期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'SpaceWeek'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每天频率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DayRate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'间隔的秒数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DayTimeSpan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行间隔类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DaySpaceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每天间隔数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DaySpaceNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天开始时间（秒）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DayBeginSpan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'天结束时间（秒）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'DayEndSpan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持续开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'持续截止时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'EndDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'计划的唯一建' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'PlanOnlyNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务执行计划（仿照Sql server的代理作业中的计划）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AutoTaskPlan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息指令' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'MsgKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息体' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'MsgBody'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'SysTypeVal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'MsgSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广播方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'BroadcastType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'转发的路由地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'RouteKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送的集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'广播错误日志表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父指令Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictateNode', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指令' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictateNode', @level2type=N'COLUMN',@level2name=N'Dictate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指令名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictateNode', @level2type=N'COLUMN',@level2name=N'DictateName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为终节点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictateNode', @level2type=N'COLUMN',@level2name=N'IsEndpoint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'指令路由节点表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DictateNode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'AppName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'AppShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'有效期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'EffectiveDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityApp', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机器码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Idgenerator', @level2type=N'COLUMN',@level2name=N'WorkId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Idgenerator', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'雪花标识机器码分配表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Idgenerator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'IpType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'Ip6'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP4地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'Ip'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束IP4地址(不为空则是IP范围)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'EndIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'IsDrop'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IP黑名单(网关用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IpBlackList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'ModularId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'ResourcePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完整路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'FullPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'ResourceShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'ResourceState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该行数据的版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'VerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'ResourceVer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后更新日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'LastTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceList', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块Key 唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'ModularKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'ModularName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源类型 2 网关API 4 RPC消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'ResourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源模块' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'ResourceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'屏蔽Key(用于去重)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'ShieIdKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'屏蔽的集群ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Api版本号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'VerNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'SortNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'ResourcePath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'屏蔽规则的过期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield', @level2type=N'COLUMN',@level2name=N'BeOverdueTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源屏蔽信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceShield'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代码生成的链路ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'TraceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'Dictate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始访问时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'耗时' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'Duration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链路日志主表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'代码生成的链路ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'TraceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SpanId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'SpanId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级SpanId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行的指令' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'Dictate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'连接的节点ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'RemoteId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点所在区域ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'返回值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'ReturnRes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'传递的参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'Args'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'MsgType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'传递方向' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'StageType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'BeginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微秒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog', @level2type=N'COLUMN',@level2name=N'Duration'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链路日志详细记录表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RpcTraceLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链路Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'TraceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'Exception'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志属性列表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'AttrList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统错误日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级事务Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发起集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属机房Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'RegionId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'TranName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提交的数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'SubmitJson'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扩展数据' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'Extend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'TranStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是注册的事务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'TranMode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是根事务发起事务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsRoot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsLock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'锁定发起时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'LockTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提交状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'CommitStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'过期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'OverTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'SubmitTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'FailTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重试次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'RetryNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList'
GO
