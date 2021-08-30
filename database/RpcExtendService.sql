USE [master]
GO
/****** Object:  Database [RpcExtendService]    Script Date: 2021/8/8 14:21:50 ******/
CREATE DATABASE [RpcExtendService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RpcExtendService', FILENAME = N'E:\数据库\RpcExtendService.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RpcExtendService_log', FILENAME = N'E:\数据库\RpcExtendService_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RpcExtendService] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RpcExtendService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RpcExtendService] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RpcExtendService] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RpcExtendService] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RpcExtendService] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RpcExtendService] SET ARITHABORT OFF 
GO
ALTER DATABASE [RpcExtendService] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RpcExtendService] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RpcExtendService] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RpcExtendService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RpcExtendService] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RpcExtendService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RpcExtendService] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RpcExtendService] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RpcExtendService] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RpcExtendService] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RpcExtendService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RpcExtendService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RpcExtendService] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RpcExtendService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RpcExtendService] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RpcExtendService] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RpcExtendService] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RpcExtendService] SET RECOVERY FULL 
GO
ALTER DATABASE [RpcExtendService] SET  MULTI_USER 
GO
ALTER DATABASE [RpcExtendService] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RpcExtendService] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RpcExtendService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RpcExtendService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RpcExtendService] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'RpcExtendService', N'ON'
GO
ALTER DATABASE [RpcExtendService] SET QUERY_STORE = OFF
GO
USE [RpcExtendService]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [RpcExtendService]
GO
/****** Object:  Table [dbo].[BroadcastErrorLog]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BroadcastErrorLog](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ErrorCode] [bigint] NOT NULL,
	[MsgKey] [varchar](50) NOT NULL,
	[MsgBody] [ntext] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SysTypeVal] [varchar](100) NOT NULL,
	[MsgSource] [varchar](max) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_BroadcastErrorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DictateNode]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DictateNode](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [bigint] NOT NULL,
	[Dictate] [varchar](50) NOT NULL,
	[DictateName] [nvarchar](50) NOT NULL,
	[IsEndpoint] [bit] NOT NULL,
 CONSTRAINT [PK_DictateNode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityApp]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityApp](
	[Id] [uniqueidentifier] NOT NULL,
	[AppName] [nvarchar](50) NOT NULL,
	[AppShow] [nvarchar](100) NULL,
	[EffectiveDate] [date] NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_IdentityApp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityPrower]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityPrower](
	[Id] [uniqueidentifier] NOT NULL,
	[AppId] [uniqueidentifier] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[ResourceType] [smallint] NOT NULL,
	[Value] [varchar](100) NOT NULL,
 CONSTRAINT [PK_IdentityPrower] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceList]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceList](
	[Id] [uniqueidentifier] NOT NULL,
	[ModularId] [uniqueidentifier] NOT NULL,
	[ResourcePath] [varchar](100) NOT NULL,
	[FullPath] [varchar](300) NOT NULL,
	[ResourceShow] [nvarchar](100) NOT NULL,
	[ResourceState] [smallint] NOT NULL,
	[VerNum] [varchar](25) NOT NULL,
	[ResourceVer] [int] NOT NULL,
	[LastTime] [date] NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ResourceList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceModular]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceModular](
	[Id] [uniqueidentifier] NOT NULL,
	[ModularKey] [varchar](32) NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[SysGroupId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[ModularName] [nvarchar](50) NOT NULL,
	[ResourceType] [smallint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ResourceModular] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemErrorLog]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemErrorLog](
	[Id] [uniqueidentifier] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[TraceId] [varchar](64) NOT NULL,
	[LogTitle] [nvarchar](200) NOT NULL,
	[LogShow] [nvarchar](max) NOT NULL,
	[GroupId] [bigint] NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[LogGroup] [varchar](50) NOT NULL,
	[LogType] [smallint] NOT NULL,
	[LogGrade] [smallint] NOT NULL,
	[ErrorCode] [varchar](200) NOT NULL,
	[Exception] [ntext] NOT NULL,
	[AttrList] [ntext] NOT NULL,
	[AddTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemErrorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionList]    Script Date: 2021/8/8 14:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionList](
	[Id] [uniqueidentifier] NOT NULL,
	[MainTranId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[RpcMerId] [bigint] NOT NULL,
	[ServerId] [bigint] NOT NULL,
	[SystemType] [varchar](50) NOT NULL,
	[SystemTypeId] [bigint] NOT NULL,
	[RegionId] [int] NOT NULL,
	[TranName] [varchar](50) NOT NULL,
	[SubmitJson] [nvarchar](max) NOT NULL,
	[Extend] [nvarchar](500) NULL,
	[TranStatus] [smallint] NOT NULL,
	[Level] [smallint] NOT NULL,
	[IsMainTran] [bit] NOT NULL,
	[IsRegTran] [bit] NOT NULL,
	[IsLock] [bit] NOT NULL,
	[IsEnd] [bit] NOT NULL,
	[OverTime] [smalldatetime] NOT NULL,
	[EndTime] [smalldatetime] NULL,
	[FailTime] [smalldatetime] NULL,
	[RetryNum] [smallint] NOT NULL,
	[ErrorCode] [bigint] NOT NULL,
	[AddTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_TransactionList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_IdentityPrower]    Script Date: 2021/8/8 14:21:50 ******/
CREATE NONCLUSTERED INDEX [IX_IdentityPrower] ON [dbo].[IdentityPrower]
(
	[AppId] ASC,
	[SystemTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ResourceList]    Script Date: 2021/8/8 14:21:50 ******/
CREATE NONCLUSTERED INDEX [IX_ResourceList] ON [dbo].[ResourceList]
(
	[ModularId] ASC,
	[VerNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionList]    Script Date: 2021/8/8 14:21:50 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionList] ON [dbo].[TransactionList]
(
	[MainTranId] ASC,
	[TranStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BroadcastErrorLog] ADD  CONSTRAINT [DF_BroadcastErrorLog_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[BroadcastErrorLog] ADD  CONSTRAINT [DF_BroadcastErrorLog_RpcMerId]  DEFAULT ((0)) FOR [RpcMerId]
GO
ALTER TABLE [dbo].[IdentityApp] ADD  CONSTRAINT [DF_IdentityApp_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_ResourceState]  DEFAULT ((1)) FOR [ResourceState]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_ResourceVer]  DEFAULT ((0)) FOR [ResourceVer]
GO
ALTER TABLE [dbo].[ResourceList] ADD  CONSTRAINT [DF_ResourceList_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[ResourceModular] ADD  CONSTRAINT [DF_ResourceModular_AddTime]  DEFAULT (getdate()) FOR [AddTime]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_RegionId]  DEFAULT ((0)) FOR [RegionId]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_IsMainTran]  DEFAULT ((0)) FOR [IsMainTran]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_IsLock]  DEFAULT ((0)) FOR [IsLock]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_RetryNum]  DEFAULT ((0)) FOR [RetryNum]
GO
ALTER TABLE [dbo].[TransactionList] ADD  CONSTRAINT [DF_TransactionList_ErrorCode]  DEFAULT ((0)) FOR [ErrorCode]
GO
/****** Object:  StoredProcedure [dbo].[RegError]    Script Date: 2021/8/8 14:21:50 ******/
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
/****** Object:  StoredProcedure [dbo].[ServerOffline]    Script Date: 2021/8/8 14:21:50 ******/
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
/****** Object:  StoredProcedure [dbo].[ServerOnline]    Script Date: 2021/8/8 14:21:50 ******/
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
/****** Object:  StoredProcedure [dbo].[SyncError]    Script Date: 2021/8/8 14:21:50 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BroadcastErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorCode'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityPrower', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系列类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityPrower', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源类型 2 网关API 4 RPC消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityPrower', @level2type=N'COLUMN',@level2name=N'ResourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IdentityPrower', @level2type=N'COLUMN',@level2name=N'Value'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'SysGroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模块名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'ModularName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'资源类型 2 网关API 4 RPC消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ResourceModular', @level2type=N'COLUMN',@level2name=N'ResourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链路Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'TraceId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'LogShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类别Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemErrorLog', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主事务Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'MainTranId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级事务Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'ParentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发起集群Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'RpcMerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务节点Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'ServerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'SystemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点类型Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'SystemTypeId'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否为主事务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsMainTran'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否是注册的事务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsRegTran'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否锁定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsLock'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否结束' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'IsEnd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'过期时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'OverTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'EndTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失败时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'FailTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'重试次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'RetryNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'ErrorCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList', @level2type=N'COLUMN',@level2name=N'AddTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事务信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransactionList'
GO
USE [master]
GO
ALTER DATABASE [RpcExtendService] SET  READ_WRITE 
GO
