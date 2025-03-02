USE [RpcService]
GO
INSERT [dbo].[ContainerGroup] ([Id], [RegionId], [HostMac], [ContainerType], [Name], [Remark], [HostIp], [CreateTime]) VALUES (1, 1, N'7c:b2:7d:ea:fd:df', 1, N'本地物理机', N'测试容器', N'192.168.65.254', CAST(N'2023-09-20T21:55:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerGroup] ([Id], [TypeVal], [GroupName]) VALUES (1, N'sys', N'系统服务节点')
GO
INSERT [dbo].[ServerGroup] ([Id], [TypeVal], [GroupName]) VALUES (2, N'demo', N'演示服务')
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (1, 1, N'sys.sync', N'RPC基础服务', 835, 1, CAST(N'2021-01-26T12:01:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (2, 1, N'sys.store', N'后台API网关服务', 834, 2, CAST(N'2021-05-24T14:01:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (3, 1, N'sys.task', N'自动任务服务', 839, 1, CAST(N'2021-08-11T15:29:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (4, 2, N'demo.gateway', N'演示网关服务', 836, 2, CAST(N'2021-05-24T13:58:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (5, 2, N'demo.user', N'演示用户服务', 838, 1, CAST(N'2021-05-24T14:02:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (6, 2, N'demo.order', N'演示订单服务', 837, 1, CAST(N'2021-05-24T13:59:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (7, 1, N'sys.extend', N'RPC基础扩展服务', 840, 1, CAST(N'2022-01-16T12:28:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerType] ([Id], [GroupId], [TypeVal], [SystemName], [DefPort], [ServiceType], [AddTime]) VALUES (10, 1, N'sys.store.service', N'后台管理服务', 843, 1, CAST(N'2023-05-27T16:52:00' AS SmallDateTime))
GO
SET IDENTITY_INSERT [dbo].[ServerRegion] ON 
GO
INSERT [dbo].[ServerRegion] ([Id], [RegionName], [CountryId], [ProId], [CityId], [DistrictId], [Address], [Contacts], [Phone], [IsDrop]) VALUES (1, N'默认', 1, 510000, 510100, 510122, N'成都市', N'圈圈', NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[ServerRegion] OFF
GO
SET IDENTITY_INSERT [dbo].[RpcControlList] ON 
GO
INSERT [dbo].[RpcControlList] ([Id], [ServerIp], [Port], [RegionId], [Show]) VALUES (1, N'127.0.0.1', 983, 1, N'测试')
GO
SET IDENTITY_INSERT [dbo].[RpcControlList] OFF
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (1, N'RB01', N'Rpr同步服务', N'127.0.0.1', N'127.0.0.1', 835, 835, 1, 1, N'7c:b2:7d:ea:fd:df', 0, 1, 1, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 1, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-05' AS Date), NULL, CAST(N'2023-12-24T20:07:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (2, N'RS01', N'RPC后台管理服务', N'127.0.0.1', N'127.0.0.1', 834, 834, 1, 2, N'7c:b2:7d:ea:fd:df', 0, 2, 1, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 1, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-05' AS Date), NULL, CAST(N'2021-05-08T18:05:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (3, N'RT01', N'自动任务服务', N'127.0.0.1', N'127.0.0.1', 839, 839, 1, 3, N'7c:b2:7d:ea:fd:df', 0, 1, 1, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 0, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-03' AS Date), NULL, CAST(N'2021-08-11T15:31:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (4, N'D01', N'演示订单服务', N'127.0.0.1', N'127.0.0.1', 837, 837, 2, 6, N'7c:b2:7d:ea:fd:df', 0, 1, 2, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 0, 1, 11, 0, 0, N'127.0.0.1', 10003, 1, CAST(N'2024-05-03' AS Date), NULL, CAST(N'2021-03-25T18:43:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (5, N'D02', N'演示用户服务', N'127.0.0.1', N'127.0.0.1', 838, 838, 2, 5, N'7c:b2:7d:ea:fd:df', 0, 1, 2, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 0, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-03' AS Date), NULL, CAST(N'2021-04-18T11:05:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (6, N'D03', N'演示网关', N'127.0.0.1', N'127.0.0.1', 836, 836, 2, 4, N'7c:b2:7d:ea:fd:df', 0, 2, 2, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 0, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-03' AS Date), NULL, CAST(N'2021-03-25T18:42:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (7, N'RB02', N'RPC基础扩展服务', N'127.0.0.1', N'127.0.0.1', 840, 840, 1, 7, N'7c:b2:7d:ea:fd:df', 0, 1, 1, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 1, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-05' AS Date), NULL, CAST(N'2022-01-16T12:29:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerConfig] ([Id], [ServerCode], [ServerName], [ServerIp], [RemoteIp], [ServerPort], [RemotePort], [GroupId], [SystemType], [ServerMac], [ServerIndex], [ServiceType], [HoldRpcMerId], [ContainerId], [ContainerGroupId], [IsContainer], [PublicKey], [ServiceState], [RegionId], [IsOnline], [BindIndex], [ConfigPrower], [RecoveryLimit], [RecoveryTime], [ConIp], [ApiVer], [VerNum], [LastOffliceDate], [Remark], [AddTime]) VALUES (11, N'RS02', N'Rpc后台服务', N'127.0.0.1', N'127.0.0.1', 843, 843, 1, 10, N'7c:b2:7d:ea:fd:df', 0, 1, 1, NULL, NULL, 0, N'6xy3#7a%ad', 0, 1, 1, 1, 11, 0, 0, N'127.0.0.1', 10003, 0, CAST(N'2024-05-05' AS Date), NULL, CAST(N'2023-05-27T16:54:00' AS SmallDateTime))
GO
INSERT [dbo].[RpcMer] ([Id], [SystemName], [AppId], [AppSecret], [AllowServerIp], [AddTime]) VALUES (1, N'Rpc组件服务', N'4a96a64fa89e4fd387d9a881a6a539f9', N'c429d30e8ab14464b70ce926802ba7dd', N'["*"]', CAST(N'2021-03-25T16:58:00' AS SmallDateTime))
GO
INSERT [dbo].[RpcMer] ([Id], [SystemName], [AppId], [AppSecret], [AllowServerIp], [AddTime]) VALUES (2, N'Demo演示服务', N'9cc83d72ba2a4b9f94493325c3acf4ee', N'a375982219934630aabb646e41e0ef14', N'["*"]', CAST(N'2021-05-24T14:04:00' AS SmallDateTime))
GO
INSERT [dbo].[RpcMerConfig] ([Id], [RpcMerId], [SystemTypeId], [IsRegionIsolate], [IsolateLevel], [BalancedType]) VALUES (1, 1, 1, 0, 0, 5)
GO
INSERT [dbo].[ServerTransmitScheme] ([Id], [Scheme], [RpcMerId], [SystemTypeId], [TransmitType], [VerNum], [Show], [IsEnable], [AddTime]) VALUES (1, N'RemoteLock', 1, 1, 1, 0, N'', 1, CAST(N'2024-02-15T11:35:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerTransmitScheme] ([Id], [Scheme], [RpcMerId], [SystemTypeId], [TransmitType], [VerNum], [Show], [IsEnable], [AddTime]) VALUES (2, N'RpcTran', 1, 1, 1, 0, N'', 1, CAST(N'2024-02-15T11:35:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerTransmitScheme] ([Id], [Scheme], [RpcMerId], [SystemTypeId], [TransmitType], [VerNum], [Show], [IsEnable], [AddTime]) VALUES (3, N'Accredit', 1, 1, 1, 0, N'', 1, CAST(N'2024-02-15T11:35:00' AS SmallDateTime))
GO
INSERT [dbo].[ServerTransmitScheme] ([Id], [Scheme], [RpcMerId], [SystemTypeId], [TransmitType], [VerNum], [Show], [IsEnable], [AddTime]) VALUES (2751265257292821, N'asssetAdd', 1, 1, 1, 0, N'测试', 0, CAST(N'2024-05-01T19:21:00' AS SmallDateTime))
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (1, 1, 1, 1, 1, N'sys.sync', 1, 2, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (2, 1, 2, 1, 2, N'sys.store', 2, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (3, 2, 1, 1, 1, N'sys.sync', 1, 1, 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (4, 2, 6, 1, 4, N'demo.gateway', 2, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (5, 2, 4, 1, 6, N'demo.order', 1, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (6, 2, 5, 1, 5, N'demo.user', 1, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (8, 1, 7, 1, 7, N'sys.extend', 1, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (9, 2, 7, 1, 7, N'sys.extend', 1, 1, 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (12, 1, 3, 1, 3, N'sys.task', 1, 1, 1)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (13, 2, 3, 1, 3, N'sys.task', 1, 1, 0)
GO
INSERT [dbo].[RemoteServerGroup] ([Id], [RpcMerId], [ServerId], [RegionId], [SystemType], [TypeVal], [ServiceType], [Weight], [IsHold]) VALUES (18, 1, 11, 1, 10, N'sys.store.service', 1, 1, 1)
GO
INSERT [dbo].[ServerTransmitConfig] ([Id], [SchemeId], [ServerCode], [TransmitConfig]) VALUES (2502866296374293, 3, N'RB01', N'[{"Range":[{"BeginRange":48,"EndRange":127,"IsFixed":false,"Key":"0_48_127"}]}]')
GO
INSERT [dbo].[ServerTransmitConfig] ([Id], [SchemeId], [ServerCode], [TransmitConfig]) VALUES (2502950597690389, 2, N'RB01', N'[{"Range":[{"BeginRange":48,"EndRange":127,"IsFixed":false,"Key":"0_48_127"}]}]')
GO
INSERT [dbo].[ServerTransmitConfig] ([Id], [SchemeId], [ServerCode], [TransmitConfig]) VALUES (2502956559631381, 1, N'RB01', N'[{"Range":[{"BeginRange":48,"EndRange":127,"IsFixed":false,"Key":"0_48_127"}]}]')
GO
INSERT [dbo].[ServerTransmitConfig] ([Id], [SchemeId], [ServerCode], [TransmitConfig]) VALUES (2751272739669013, 2751265257292821, N'RB01', N'[{"Range":[{"BeginRange":48,"EndRange":127,"IsFixed":false,"Key":"0_48_127"}]}]')
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (1, 0, 0, 0, 0, 0, 0, N'', N'rpc:cache', 1, N'{"CacheType":1,"Redis":{"ConConfig":{"Connection":"127.0.0.1"}},"Memcached":{"ServerIp":["127.0.0.1"]}}', N'缓存配置', 10, 2, 1, CAST(N'2024-02-23T20:56:56.000' AS DateTime), N'cacheFrom')
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (2, 0, 0, 0, 0, 0, 0, N'', N'local:log', 1, N'{"IsConsole":true,"LogGrade":3,"IsWriteFile":true,"SaveDir":"Log"}', N'本地日志', 10, 1, 1, CAST(N'2024-05-01T10:15:00.000' AS DateTime), N'localLogFrom')
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (3, 0, 0, 0, 0, 0, 0, N'sys.store', N'admin', 1, N'{"Pwd":"6xy3#7a%ad","Head":"http://150.138.77.150:10089/file/images/defhead.jpg","Prower":["rpc.store.admin"]}', N'后台管理员信息', 0, 2, 1, CAST(N'2024-04-23T21:52:10.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (4, 0, 0, 0, 0, 0, 0, N'', N'queue:QueueType', 0, N'0', N'订阅和广播使用的队列类型', 0, 2, 1, CAST(N'2022-02-09T15:35:00.477' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (5, 0, 0, 0, 0, 0, 0, N'', N'queue:Kafka', 1, N'{"Servers":[{"ServerIp":"127.0.0.1"}]}', N'Kafka配置', 0, 2, 1, CAST(N'2024-02-23T21:03:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (6, 0, 0, 0, 0, 0, 0, N'', N'queue:Rabbitmq', 1, N'{"UserName":"guest","Pwd":"guest","Servers":[{"ServerIp":"127.0.0.1"}]}', N'Rabbitmq配置', 0, 2, 1, CAST(N'2021-09-09T09:22:20.653' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (7, 0, 0, 0, 0, 0, 0, N'', N'rpcassembly:Track', 1, N'{"IsEnable":true,"Trace128Bits":true,"TraceType":0,"TrackDepth":30,"TrackRange":14,"SamplingRate":10000,"Local":{"Dictate":"SysTrace","SystemType":"sys.extend"},"ZipkinTack":{"PostUri":"http://127.0.0.1:9411/api/v1/spans"}}', N'链路跟踪配置', 10, 1, 1, CAST(N'2024-02-14T19:26:48.713' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (8, 0, 0, 0, 0, 0, 0, N'', N'rpcassembly:Resource', 1, N'{"UpRange":6}', N'API资源收集配置', 0, 2, 1, CAST(N'2021-09-09T09:54:28.040' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (9, 0, 0, 0, 0, 0, 0, N'', N'rpc:Log', 1, N'{"IsUpLog":true,"ExcludeLogGroup":[],"LogGradeLimit":3}', N'RPC日志配置', 10, 2, 1, CAST(N'2024-02-23T21:11:07.000' AS DateTime), N'logFrom')
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (10, 0, 0, 0, 0, 0, 0, N'', N'SqlSugar:LogGrade', 0, N'1', N'Sugar日志等级', 0, 2, 1, CAST(N'2024-04-13T21:02:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (11, 0, 0, 0, 0, 0, 0, N'', N'rpcassembly:Visit', 1, N'{"IsEnable":true,"Interval":60}', N'Visit访问配置', 0, 2, 1, CAST(N'2024-01-22T11:16:18.693' AS DateTime), N'visitFrom')
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (12, 0, 0, 0, 0, 0, 0, N'sys.store', N'user', 1, N'{"Pwd":"123456","Head":"http://150.138.77.150:10089/file/images/defhead.jpg","Prower":["rpc.store.user"]}', N'后台用户', 0, 2, 1, CAST(N'2024-04-23T21:52:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (13, 2, 0, 0, 0, 1, 0, N'', N'queue:Rabbitmq', 1, N'{"UserName":"guest","Pwd":"guest","Servers":[{"ServerUri":"http://host.docker.internal"}]}', N'容器中的Rabbitmq配置', 0, 2, 1, CAST(N'2024-04-29T21:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (14, 2, 0, 0, 0, 1, 0, N'', N'rpc:cache', 1, N'{"CacheType":1,"Redis":{"ConConfig":{"Connection":"host.docker.internal"}},"Memcached":{"ServerIp":["host.docker.internal"]}}', N'容器中的缓存配置', 0, 2, 1, CAST(N'2024-04-29T21:00:00.000' AS DateTime), NULL)
GO
INSERT [dbo].[SysConfig] ([Id], [RpcMerId], [ServiceType], [ServerId], [RegionId], [ContainerGroup], [VerNum], [SystemType], [Name], [ValueType], [Value], [Show], [Prower], [ConfigType], [IsEnable], [ToUpdateTime], [TemplateKey]) VALUES (2817250777629717, 1, 2, 0, 0, 0, 0, N'', N'gateway:urlRewrite', 1, N'[]', N'', 10, 1, 1, CAST(N'2024-05-04T18:30:30.000' AS DateTime), N'urlRewriteFrom')
GO
INSERT [dbo].[ErrorCollect] ([Id], [ErrorCode], [IsPerfect]) VALUES (509777198850181, N'accredit.Invalid', 0)
GO
INSERT [dbo].[DictCollect] ([DictCode], [DictName]) VALUES (N'001', N'配置项说明')
GO
INSERT [dbo].[DictCollect] ([DictCode], [DictName]) VALUES (N'002', N'日志组')
GO
INSERT [dbo].[DictCollect] ([DictCode], [DictName]) VALUES (N'003', N'配置摸版')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Basic', N'基础日志组', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Cache', N'缓存日志组', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'DataQueue', N'数据队列日志', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'DataSyncClass', N'同步类日志', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Def', N'默认日志组', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'DelayDataSave', N'数据延迟保存日志', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Gateway', N'网关日志', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'http', N'Http日志', N'Gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'HttpGateway', N'Http网关日志', N'Gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Json', N'JSON序列化和反序列化日志', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'LocalTask', N'本地任务日志', N'Basic')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Memcached', N'Memcached日志', N'Cache')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Pipe_Client', N'Pipe客户端日志', N'Socket')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Redis', N'Redis日志', N'Cache')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'RedisQueue', N'Redis队列日志', N'Cache')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc', N'Rpc日志组', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_CentralMsg', N'中控消息日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Config', N'配置日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_ConLog', N'链接日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Kafka', N'Kafka日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Log', N'普通日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Msg', N'消息日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Rabbitmq', N'Rabbitmq日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_RouteLog', N'路由日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Rpc_Trace', N'链路日志', N'Rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Socket', N'通信组件日志', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Socket_Client', N'Socket客户端日志', N'Socket')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'Socket_Server', N'Socket服务端日志', N'Socket')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'SqlSugar', N'SqlSugar日志', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'SqlSugar_DELETE', N'删除日志', N'SqlSugar')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'SqlSugar_INSERT', N'新增数据日志', N'SqlSugar')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'SqlSugar_SELECT', N'查询日志', N'SqlSugar')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'002', N'SqlSugar_UPDATE', N'修改日志', N'SqlSugar')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'accreditFrom', N'登陆授权', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'apiShieldFrom', N'Api接口屏蔽', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'cacheFrom', N'缓存配置', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'cacheTimeFrom', N'缓存时间', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'crossFrom', N'跨域配置', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'gateway', N'网关配置', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'gatewayIdentityFrom', N'网关租户配置', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'http', N'Http网关配置项', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'httpBasicFrom', N'Http基础配置', N'http')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'httpFileFrom', N'文件服务', N'http')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'httpGzipFrom', N'Gzip压缩', N'http')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'httpLogFrom', N'请求日志', N'http')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'IdempotentFrom', N'重复请求', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'identityFrom', N'租户服务', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'idgeneratorFrom', N'雪花算法', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'ipBlackFrom', N'IP黑名单', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'ipLimitFrom', N'IP限流', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'loacalConfigFrom', N'本地配置管理', N'local')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'local', N'节点本地配置项', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'localLogFrom', N'本地日志管理', N'local')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'logFrom', N'日志服务', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'message', N'通信配置', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'nodeLimitFrom', N'Api接口限流', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'pipeFrom', N'Pipe隧道', N'message')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'resourceFrom', N'资源上传配置', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'rpc', N'基础配置项', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'rpcassembly', N'组件配置项', N'root')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'rpcBasicFrom', N'Rpc基础配置', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'shieldForm', N'远程方法屏蔽', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'socketTcpFrom', N'Tcp配置', N'message')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'threadPoolFrom', N'线程池配置', N'rpc')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'trackFrom', N'链路配置', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'upConfigFrom', N'文件上传', N'gateway')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'visitFrom', N'访问统计', N'rpcassembly')
GO
INSERT [dbo].[DictItem] ([DictCode], [ItemCode], [ItemText], [PrtItemCode]) VALUES (N'003', N'wholeLimItFrom', N'全局限流', N'gateway')
GO
INSERT [dbo].[RpcMerServerVer] ([Id], [RpcMerId], [SystemTypeId], [CurrentVer]) VALUES (2572145440588821, 2, 6, 1)
GO
