USE [RpcExtendService]
GO
SET IDENTITY_INSERT [dbo].[SystemEventConfig] ON 
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (1, N'内存占用事件', N'MemoryOccupation', 1, 2, N'服务节点({server_name})已使用内存：{memory}，超过内存限制：{threshold}。', N'{"Threshold":10240,"SustainTime":5,"Interval":30}', 1)
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (2, N'CPU占用事件', N'CpuBehavior', 1, 2, N'服务节点({server_name})当前CPU使用率：{rate}%，持续：{duration}。', N'{"Threshold":100,"SustainTime":10,"Interval":30}', 1)
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (3, N'回复超时事件', N'RpcReplyBehavior', 1, 2, N'服务节点({server_name})回复({remote_name})指令{dicate}超时，用时：{duration}。', N'{"Threshold":5000,"RecordRange":0}', 1)
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (4, N'发送超时事件', N'RpcSendBehavior', 1, 2, N'服务节点({server_name})到({remote_name})发送指令{dicate}超时，用时：{duration}。', N'{"Threshold":5000,"RecordRange":0}', 1)
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (5, N'节点状态变更事件', N'RemoteStateChange', 1, 1, N'服务节点({remote_name})链接状态由：{sourcestate}变更为{newstate}。', N'{"CurState":1,"OldState":0}', 1)
GO
INSERT [dbo].[SystemEventConfig] ([Id], [EventName], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (7, N'无服务事件', N'NoServerError', 1, 1, N'服务节点({server_name})向目标类型({systemtypename})发送指令{dictate}失败未找到目标服务。', N'{"SystemType":null,"Interval":10}', 1)
GO
SET IDENTITY_INSERT [dbo].[SystemEventConfig] OFF
GO
INSERT [dbo].[ServerEventSwitch] ([Id], [RpcMerId], [ServerId], [SysEventId], [EventKey], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (523899016024197, 1, 0, 2, N'E6EADD183B24CEE7B39BA25CD44E8324', N'CpuBehavior', 2, 2, N'服务节点({server_name})当前CPU使用率：{rate}%，持续：{duration}。', N'{"Threshold":100,"SustainTime":5,"Interval":30}', 1)
GO
INSERT [dbo].[ServerEventSwitch] ([Id], [RpcMerId], [ServerId], [SysEventId], [EventKey], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (524628742393989, 1, 0, 2, N'DB6056C78285E7EA84D0AD49B67D7A41', N'CpuBehavior', 2, 2, N'服务节点({server_name})当前CPU使用率：{rate}%，持续：{duration}。', N'{"Threshold":60,"SustainTime":5,"Interval":30}', 1)
GO
INSERT [dbo].[ServerEventSwitch] ([Id], [RpcMerId], [ServerId], [SysEventId], [EventKey], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (525340538937477, 1, 0, 5, N'B6D4526DC5EF6410CD5D0B40188E1273', N'RemoteStateChange', 1, 1, N'服务节点({remote_name})链接状态由：{sourcestate}变更为{newstate}。', N'{"CurState":1,"OldState":0}', 1)
GO
INSERT [dbo].[ServerEventSwitch] ([Id], [RpcMerId], [ServerId], [SysEventId], [EventKey], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (525696133484677, 1, 0, 7, N'066D9327B4261A6742BAC5FF36166E24', N'NoServerError', 1, 1, N'服务节点({server_name})向目标类型({systemtypename})发送指令{dictate}失败未找到目标服务。', N'{"SystemType":[],"Interval":10}', 1)
GO
INSERT [dbo].[ServerEventSwitch] ([Id], [RpcMerId], [ServerId], [SysEventId], [EventKey], [Module], [EventLevel], [EventType], [MsgTemplate], [EventConfig], [IsEnable]) VALUES (533347606663301, 1, 0, 4, N'04A6542B19B12266A78FA754993E138F', N'RpcSendBehavior', 1, 0, N'服务节点({server_name})到({remote_name})发送指令{dicate}超时，用时：{duration}。', N'{"Threshold":5000,"RecordRange":2}', 1)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RegionId], [RpcMerId], [TaskName], [TaskShow], [TaskPriority], [BeginStep], [FailEmall], [VerNum], [IsExec], [ExecVerNum], [LastExecTime], [LastExecEndTime], [NextExecTime], [TaskStatus], [StopTime]) VALUES (1, NULL, 1, N'分布式事务', N'1，检查超时事务 2，事务回滚重试', 0, 1, NULL, 0, 0, 1368005963, CAST(N'2024-05-03T11:19:12.000' AS DateTime), CAST(N'2024-05-03T11:19:12.000' AS DateTime), CAST(N'2024-05-03T11:19:22.000' AS DateTime), 1, NULL)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RegionId], [RpcMerId], [TaskName], [TaskShow], [TaskPriority], [BeginStep], [FailEmall], [VerNum], [IsExec], [ExecVerNum], [LastExecTime], [LastExecEndTime], [NextExecTime], [TaskStatus], [StopTime]) VALUES (2, NULL, 1, N'接口资源管理', N'1，标记待删除的资源 2，清理已删除的资源', 1, 1, NULL, 0, 0, 69164886, CAST(N'2024-02-29T00:01:00.000' AS DateTime), CAST(N'2024-02-29T00:01:00.000' AS DateTime), CAST(N'2024-05-04T00:01:00.000' AS DateTime), 1, NULL)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RegionId], [RpcMerId], [TaskName], [TaskShow], [TaskPriority], [BeginStep], [FailEmall], [VerNum], [IsExec], [ExecVerNum], [LastExecTime], [LastExecEndTime], [NextExecTime], [TaskStatus], [StopTime]) VALUES (3, NULL, 1, N'刷新系统配置', N'1, 触发配置系统配置项更新检查', 0, 1, NULL, 1053474441, 0, 631370108, CAST(N'2024-05-03T11:19:12.000' AS DateTime), CAST(N'2024-05-03T11:19:12.000' AS DateTime), CAST(N'2024-05-03T11:19:22.000' AS DateTime), 1, NULL)
GO
INSERT [dbo].[AutoTaskList] ([Id], [RegionId], [RpcMerId], [TaskName], [TaskShow], [TaskPriority], [BeginStep], [FailEmall], [VerNum], [IsExec], [ExecVerNum], [LastExecTime], [LastExecEndTime], [NextExecTime], [TaskStatus], [StopTime]) VALUES (4, NULL, 1, N'清空今日访问统计', N'将今日指令访问统计数据清零', 1, 1, NULL, 597511964, 0, 0, NULL, NULL, CAST(N'2024-05-04T00:00:00.000' AS DateTime), 1, NULL)
GO
INSERT [dbo].[AutoTaskPlan] ([Id], [TaskId], [PlanTitle], [PlanShow], [PlanType], [ExecTime], [ExecRate], [ExecSpace], [SpaceType], [SpaceDay], [SpeceNum], [SpaceWeek], [DayRate], [DayTimeSpan], [DaySpaceType], [DaySpaceNum], [DayBeginSpan], [DayEndSpan], [BeginDate], [EndDate], [PlanOnlyNo], [IsEnable]) VALUES (1, 1, N'每隔2秒钟执行', NULL, 0, NULL, 0, 1, 0, NULL, NULL, NULL, 1, 10, 2, 2, 0, 86399, CAST(N'2022-01-16' AS Date), NULL, NULL, 1)
GO
INSERT [dbo].[AutoTaskPlan] ([Id], [TaskId], [PlanTitle], [PlanShow], [PlanType], [ExecTime], [ExecRate], [ExecSpace], [SpaceType], [SpaceDay], [SpeceNum], [SpaceWeek], [DayRate], [DayTimeSpan], [DaySpaceType], [DaySpaceNum], [DayBeginSpan], [DayEndSpan], [BeginDate], [EndDate], [PlanOnlyNo], [IsEnable]) VALUES (2, 2, N'每天0点1分钟执行一次', NULL, 0, NULL, 0, 1, 0, NULL, NULL, NULL, 0, 60, 2, NULL, NULL, NULL, CAST(N'2022-01-16' AS Date), NULL, NULL, 1)
GO
INSERT [dbo].[AutoTaskPlan] ([Id], [TaskId], [PlanTitle], [PlanShow], [PlanType], [ExecTime], [ExecRate], [ExecSpace], [SpaceType], [SpaceDay], [SpeceNum], [SpaceWeek], [DayRate], [DayTimeSpan], [DaySpaceType], [DaySpaceNum], [DayBeginSpan], [DayEndSpan], [BeginDate], [EndDate], [PlanOnlyNo], [IsEnable]) VALUES (3, 3, N'每隔10秒执行', NULL, 0, NULL, 0, 1, 0, NULL, NULL, NULL, 1, 10, 2, 10, 0, 86399, CAST(N'2022-10-15' AS Date), NULL, NULL, 1)
GO
INSERT [dbo].[AutoTaskPlan] ([Id], [TaskId], [PlanTitle], [PlanShow], [PlanType], [ExecTime], [ExecRate], [ExecSpace], [SpaceType], [SpaceDay], [SpeceNum], [SpaceWeek], [DayRate], [DayTimeSpan], [DaySpaceType], [DaySpaceNum], [DayBeginSpan], [DayEndSpan], [BeginDate], [EndDate], [PlanOnlyNo], [IsEnable]) VALUES (4, 4, N'每天0点执行', NULL, 0, NULL, 0, 1, 0, NULL, NULL, NULL, 0, 0, 2, NULL, NULL, NULL, CAST(N'2024-03-17' AS Date), NULL, NULL, 1)
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (1, 1, N'检查超时事务', 1, 0, N'{"RpcConfig":{"SysDictate":"CheckOverTimeTran","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 1, NULL, 1, NULL, 2, 1, NULL, CAST(N'2024-05-03T11:19:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (2, 1, N'事务回滚超时检查', 2, 0, N'{"RpcConfig":{"SysDictate":"TranLockOverTime","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 1, NULL, 1, NULL, 2, 1, NULL, CAST(N'2024-05-03T11:19:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (3, 1, N'重启回滚失败的事务', 3, 0, N'{"RpcConfig":{"SysDictate":"RestartRetryTran","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 0, NULL, 0, NULL, 2, 1, NULL, CAST(N'2024-05-03T11:19:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (4, 2, N'清理已删除的资源', 2, 0, N'{"RpcConfig":{"SysDictate":"ClearResource","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 0, NULL, 0, NULL, 2, 1, NULL, CAST(N'2024-02-29T00:01:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (5, 2, N'标记待删除的资源', 1, 0, N'{"RpcConfig":{"SysDictate":"InvalidResource","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 1, NULL, 1, NULL, 2, 1, NULL, CAST(N'2024-02-29T00:01:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (6, 3, N'检查配置更新项', 1, 0, N'{"RpcConfig":{"SysDictate":"RefreshSysConfig","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 0, NULL, 0, NULL, 2, 1, NULL, CAST(N'2024-05-03T11:19:00' AS SmallDateTime))
GO
INSERT [dbo].[AutoTaskItem] ([Id], [TaskId], [ItemTitle], [ItemSort], [SendType], [SendParam], [TimeOut], [RetryNum], [FailStep], [FailNextStep], [SuccessStep], [NextStep], [LogRange], [IsSuccess], [Error], [LastExecTime]) VALUES (7, 4, N'清空今日访问统计', 1, 0, N'{"RpcConfig":{"SysDictate":"ClearVisit","IsReply":false,"SystemType":"sys.sync"}}', 10, 2, 0, NULL, 0, NULL, 2, 1, NULL, NULL)
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788549181047445, N'rpc.server.no.usable[2773939419086933]', N'Rpc_RefreshConfig', 1, N'{"ConfigMd5":"41ED261F26D6DE366353911F5EE33ADF"}', 2773939419086933, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T10:51:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788551836828309, N'rpc.server.no.usable[6]', N'Rpc_RefreshConfig', 1, N'{"ConfigMd5":"41ED261F26D6DE366353911F5EE33ADF"}', 6, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T10:51:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788551844692629, N'rpc.server.no.usable[4]', N'Rpc_RefreshConfig', 1, N'{"ConfigMd5":"41ED261F26D6DE366353911F5EE33ADF"}', 4, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T10:51:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807508755093, N'rpc.queue.dead.msg', N'UserGoOnline', 6, N'{"MsgBody":{"UserId":1,"Phone":"18615750368"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 2, N'UserGoOnline', 2, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807690945173, N'rpc.server.no.usable[2764805026349141]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2764805026349141, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807695401621, N'rpc.server.no.usable[2766175004524629]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766175004524629, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807717421717, N'rpc.server.no.usable[2766238220288085]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766238220288085, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807722140309, N'rpc.server.no.usable[2766262018506838]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766262018506838, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807733150357, N'rpc.server.no.usable[2766274259845206]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766274259845206, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807757529749, N'rpc.server.no.usable[2766432326123606]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766432326123606, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807759889045, N'rpc.server.no.usable[2766464184746070]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766464184746070, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807787414165, N'rpc.server.no.usable[2766778738671702]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2766778738671702, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807793967765, N'rpc.server.no.usable[2768438758998101]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2768438758998101, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807800783509, N'rpc.server.no.usable[2768472259952726]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2768472259952726, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807819657877, N'rpc.server.no.usable[2768486934249558]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2768486934249558, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807831192213, N'rpc.server.no.usable[2768564576845910]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2768564576845910, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807846920853, N'rpc.server.no.usable[2773939419086933]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2773939419086933, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788807848493717, N'rpc.server.no.usable[2768575970148438]', N'AccreditRefresh', 1, N'{"AccreditId":"78e278fd04084a9794cf70675234ae2a","IsInvalid":true}', 2768575970148438, N'', N'{"SystemTypeId":1,"SystemType":"sys.sync","SysGroup":"sys","ServerId":1,"RpcMerId":1,"RegionId":1,"VerNum":0}', 0, N'', 0, CAST(N'2024-05-03T11:08:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788912597566101, N'rpc.queue.dead.msg', N'AddOrderMsg', 4, N'{"MsgBody":{"Order":{"Id":"d317733b-990b-7d5c-d91a-3a124e9ee015","OrderNo":"DE012024050303132726886421808523","UserId":1,"UserName":"0o圈圈o0","UserPhone":"18615750368","OrderTitle":"购买了个EVA","OrderPrice":10,"AddTime":"2024-05-03T11:14:17.2372281+08:00"}},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":6,"SystemType":"demo.order","SysGroup":"demo","ServerId":4,"RpcMerId":2,"RegionId":1,"VerNum":1}', 2, N'AddOrderMsg', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788912622207637, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243123349, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243123350, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243123351, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385493, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385494, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385495, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385496, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385497, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385498, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385499, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385500, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385501, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385502, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385503, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385504, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385505, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915243385506, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915247317653, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[BroadcastErrorLog] ([Id], [Error], [MsgKey], [SourceId], [MsgBody], [ServerId], [SysTypeVal], [MsgSource], [BroadcastType], [RouteKey], [RpcMerId], [AddTime]) VALUES (2788915247317654, N'rpc.queue.dead.msg', N'PlaceAnOrder', 6, N'{"MsgBody":{"UserId":1,"OrderNo":"DE012024050303132726886421808523"},"Extend":{"AccreditId":"f124465be1a049b89d8f1b2c0e601dac"}}', 0, N'', N'{"SystemTypeId":4,"SystemType":"demo.gateway","SysGroup":"demo","ServerId":6,"RpcMerId":2,"RegionId":1,"VerNum":0}', 1, N'demo', 2, CAST(N'2024-05-03T11:14:00' AS SmallDateTime))
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (10, 1, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (17, 2, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (12, 3, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (13, 4, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (14, 5, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (15, 6, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (11, 7, N'7c:b2:7d:ea:fd:df', 0)
GO
INSERT [dbo].[Idgenerator] ([WorkId], [SystemTypeId], [Mac], [ServerIndex]) VALUES (16, 10, N'7c:b2:7d:ea:fd:df', 0)
GO
