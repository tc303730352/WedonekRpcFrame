use RpcService;
delete from [dbo].[KafkaExchange]
delete from [dbo].[KafkaRouteKey]
delete from [dbo].[ServerEnvironment]
delete from [dbo].[ServerRunState]
delete from [dbo].[ServerSignalState]
delete from [dbo].[ServerVisitCensus]
delete from [dbo].[ErrorCollect] where Id != 509777198850181
delete from [dbo].[ServerCurConfig]
delete from [dbo].[ServerClientLimit]
use RpcExtendService
delete from [dbo].[ResourceList]
delete from [dbo].[ResourceModular]
delete from [dbo].[IpBlackList]
delete from [dbo].[RpcTraceList]
delete from [dbo].[RpcTraceLog]
delete from [dbo].[SystemErrorLog]
delete from [dbo].[SystemEventLog]
delete from [dbo].[TransactionList]
delete from [dbo].[AccreditToken]
delete from [dbo].[AutoTaskLog]