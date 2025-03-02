using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using LinqKit;
using RpcStore.Model.DB;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.DictateLimit.Model;
using RpcStore.RemoteModel.Error.Model;
using RpcStore.RemoteModel.Identity.Model;
using RpcStore.RemoteModel.IpBlack.Model;
using RpcStore.RemoteModel.Resource.Model;
using RpcStore.RemoteModel.ResourceModular.Model;
using RpcStore.RemoteModel.ResourceShield.Model;
using RpcStore.RemoteModel.RetryTask.Model;
using RpcStore.RemoteModel.ServerBind.Model;
using RpcStore.RemoteModel.ServerConfig.Model;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using RpcStore.RemoteModel.ServerPublic.Model;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.RemoteModel.SysConfig.Model;
using RpcStore.RemoteModel.SysEventLog.Model;
using RpcStore.RemoteModel.SysLog.Model;
using RpcStore.RemoteModel.Trace.Model;
using RpcStore.RemoteModel.Tran.Model;
using RpcStore.RemoteModel.TransmitScheme.Model;
using RpcStore.RemoteModel.VisitCensus.Model;
using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar.LinqKit;

namespace RpcStore.DAL
{
    internal static partial class WhereLinq
    {
        [GeneratedRegex(@"^([a-zA-Z]|[_])+$")]
        private static partial Regex LogGroupRegex ();
        private static readonly Regex _IpContainsRegex = new Regex(@"(([.]\d{1,3}[.]{0,1})|([.]{0,1}\d{1,3}[.])){1,3}", RegexOptions.IgnoreCase);

        private static readonly Regex _ApiVerRegex = new Regex(@"^\d{3,}([.]\d{2}){2}$");
        private static readonly Regex _ApiVerContainsRegex = new Regex(@"\d{3,}([.]\d{2}){0,1}$");
        public static Expression<Func<RemoteServerGroupModel, RemoteServerConfigModel, bool>> ToWhere (this BindGetParam query)
        {
            ExpressionStarter<RemoteServerGroupModel, RemoteServerConfigModel> where = LinqKitExtend.New<RemoteServerGroupModel, RemoteServerConfigModel>((a, b) => a.RpcMerId == query.RpcMerId);
            if (query.RegionId.HasValue)
            {
                where = where.And((a, b) => b.RegionId == query.RegionId.Value);
            }
            if (query.ServerType.HasValue)
            {
                where = where.And((a, b) => b.ServiceType == query.ServerType);
            }
            if (query.IsHold.HasValue)
            {
                where = where.And((a, b) => a.IsHold == query.IsHold.Value);
            }
            return where;
        }
        public static Expression<Func<SystemEventLogModel, bool>> ToWhere (this SysEventLogQuery query)
        {
            ExpressionStarter<SystemEventLogModel> where = PredicateBuilder.New<SystemEventLogModel>();
            if (query.ServerId.HasValue)
            {
                where = where.And(a => a.ServerId == query.ServerId.Value);
            }
            else
            {
                where = where.And(a => a.RpcMerId == query.RpcMerId);
                if (query.SystemTypeId.HasValue)
                {
                    where = where.And(a => a.SystemTypeId == query.SystemTypeId.Value);
                }
                if (query.RegionId.HasValue)
                {
                    where = where.And(a => a.RegionId == query.RegionId.Value);
                }
            }
            if (query.EventLevel.HasValue)
            {
                where = where.And(a => a.EventLevel == query.EventLevel.Value);
            }
            if (query.EventType.HasValue)
            {
                where = where.And(a => a.EventType == query.EventType.Value);
            }
            return where;
        }
        public static Expression<Func<AutoRetryTaskModel, bool>> ToWhere (this RetryTaskQuery query)
        {
            ExpressionStarter<AutoRetryTaskModel> where = PredicateBuilder.New<AutoRetryTaskModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.ServerId.HasValue)
            {
                where = where.And(a => a.ServerId == query.ServerId.Value);
            }
            if (query.RegionId.HasValue)
            {
                where = where.And(a => a.RegionId == query.RegionId.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(a => a.SystemType == query.SystemType);
            }
            if (!query.Status.IsNull())
            {
                where = where.And(a => query.Status.Contains(a.Status));
            }
            if (query.RetryNum.HasValue)
            {
                where = where.And(a => a.RetryNum > query.RetryNum.Value);
            }
            if (query.Begin.HasValue && query.End.HasValue)
            {
                long btime = query.Begin.Value.ToLong();
                long etime = query.End.Value.ToLong();
                where = where.And(a => SqlFunc.Between(a.NextRetryTime, btime, etime));
            }
            if (query.QueryKey.IsNotNull())
            {
                where = where.And(a => a.IdentityId.StartsWith(query.QueryKey));
            }
            return where;
        }
        public static Expression<Func<ServerEventSwitchModel, bool>> ToWhere (this EventSwitchQuery query)
        {
            ExpressionStarter<ServerEventSwitchModel> where = PredicateBuilder.New<ServerEventSwitchModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.ServerId.HasValue)
            {
                where = where.And(a => a.ServerId == query.ServerId.Value);
            }
            if (query.SysEventId.HasValue)
            {
                where = where.And(a => a.SysEventId == query.SysEventId.Value);
            }
            if (query.EventLevel.HasValue)
            {
                where = where.And(a => a.EventLevel == query.EventLevel.Value);
            }
            if (query.EventType.HasValue)
            {
                where = where.And(a => a.EventType == query.EventType.Value);
            }
            return where;
        }
        public static Expression<Func<ContainerGroupModel, bool>> ToWhere (this ContainerGroupQuery query)
        {
            ExpressionStarter<ContainerGroupModel> where = PredicateBuilder.New<ContainerGroupModel>();
            if (query.QueryKey.IsNotNull())
            {
                where = where.And(a => a.Name.Contains(query.QueryKey));
            }
            if (query.RegionId.HasValue)
            {
                where = where.And(a => a.RegionId == query.RegionId.Value);
            }
            if (query.ContainerType.HasValue)
            {
                where = where.And(a => a.ContainerType == query.ContainerType.Value);
            }
            if (where.IsStarted)
            {
                return where;
            }
            return null;
        }

        public static Expression<Func<RemoteServerGroupModel, bool>> ToBindGetWhere (this BindGetParam query)
        {
            ExpressionStarter<RemoteServerGroupModel> where = PredicateBuilder.New<RemoteServerGroupModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.RegionId.HasValue)
            {
                where = where.And(a => a.RegionId == query.RegionId.Value);
            }
            if (query.ServerType.HasValue)
            {
                where = where.And(a => a.ServiceType == query.ServerType.Value);
            }
            if (query.IsHold.HasValue)
            {
                where = where.And((a) => a.IsHold == query.IsHold.Value);
            }
            return where;
        }
        public static Expression<Func<RemoteServerGroupModel, RemoteServerConfigModel, bool>> ToWhere (this ServerBindQueryParam query)
        {
            ExpressionStarter<RemoteServerGroupModel, RemoteServerConfigModel> where = LinqKitExtend.New<RemoteServerGroupModel, RemoteServerConfigModel>((a, b) => a.RpcMerId == query.RpcMerId);
            if (query.SystemTypeId.HasValue)
            {
                where = where.And((a, b) => b.SystemType == query.SystemTypeId.Value);
            }
            if (query.RegionId.HasValue)
            {
                where = where.And((a, b) => b.RegionId == query.RegionId.Value);
            }
            if (query.ContainerGroup.HasValue)
            {
                where = where.And((a, b) => b.IsContainer && b.ContainerGroupId == query.ContainerGroup.Value);
            }
            if (!query.ServiceState.IsNull())
            {
                where = where.And((a, b) => query.ServiceState.Contains(b.ServiceState));
            }
            if (query.IsHold.HasValue)
            {
                where = where.And((a, b) => a.IsHold == query.IsHold.Value);
            }
            if (query.VerNum.HasValue)
            {
                where = where.And((a, b) => b.VerNum == query.VerNum.Value);
            }
            if (query.ServerType.HasValue)
            {
                where = where.And((a, b) => b.ServiceType == query.ServerType);
            }
            return where;
        }
        public static Expression<Func<RemoteServerGroupModel, RemoteServerConfigModel, bool>> ToWhere (this BindQueryParam query, long rpcMerId)
        {
            ExpressionStarter<RemoteServerGroupModel, RemoteServerConfigModel> where = LinqKitExtend.New<RemoteServerGroupModel, RemoteServerConfigModel>((a, b) => a.RpcMerId == rpcMerId);
            if (query.GroupId.HasValue)
            {
                where = where.And((a, b) => b.GroupId == query.GroupId.Value);
            }
            if (query.SystemTypeId.HasValue)
            {
                where = where.And((a, b) => b.SystemType == query.SystemTypeId.Value);
            }
            if (query.IsContainer.HasValue)
            {
                where = where.And((a, b) => b.IsContainer == query.IsContainer.Value);
            }
            if (!query.ServiceState.IsNull())
            {
                where = where.And((a, b) => query.ServiceState.Contains(b.ServiceState));
            }
            if (query.RegionId.HasValue)
            {
                where = where.And((a, b) => b.RegionId == query.RegionId.Value);
            }
            if (query.ServiceType.HasValue)
            {
                where = where.And((a, b) => b.ServiceType == query.ServiceType.Value);
            }
            if (query.IsOnline.HasValue)
            {
                where = where.And((a, b) => b.IsOnline == query.IsOnline.Value);
            }
            if (query.VerNum.HasValue)
            {
                where = where.And((a, b) => b.VerNum == query.VerNum.Value);
            }
            if (query.IsHold.HasValue)
            {
                where = where.And((a, b) => a.IsHold == query.IsHold.Value);
            }
            if (!query.QueryKey.IsNull())
            {
                if (query.QueryKey.Validate(ValidateFormat.IP))
                {
                    where = where.And((a, b) => b.ConIp == query.QueryKey
                    || b.ServerIp == query.QueryKey
                    || b.RemoteIp == query.QueryKey);
                }
                else if (_IpContainsRegex.IsMatch(query.QueryKey))
                {
                    where = where.And((a, b) => b.ConIp.Contains(query.QueryKey)
                    || b.ServerIp.Contains(query.QueryKey)
                    || b.RemoteIp.Contains(query.QueryKey));
                }
                else if (query.QueryKey.Validate(ValidateFormat.MAC))
                {
                    where = where.And((a, c) => c.ServerMac == query.QueryKey);
                }
                else if (Regex.IsMatch(query.QueryKey, @"^(\w{2,12})|((\w|[-:]){2,17})$"))
                {
                    where = where.And((a, c) => c.ServerMac.Contains(query.QueryKey));
                }
                else if (_ApiVerRegex.IsMatch(query.QueryKey))
                {
                    int ver = int.Parse(query.QueryKey.Replace(".", string.Empty));
                    where = where.And((a, c) => c.VerNum == ver);
                }
                else if (_ApiVerContainsRegex.IsMatch(query.QueryKey))
                {
                    int ver = _GetVer(query.QueryKey);
                    int end = _GetVer(query.QueryKey, 1);
                    where = where.And((a, c) => c.VerNum >= ver && c.VerNum <= end);
                }
                else
                {
                    where = where.And((a, c) => c.ServerName.Contains(query.QueryKey));
                }
            }
            return where;
        }
        private static int _GetVer (string str)
        {
            string[] t = str.Split('.');
            if (t.Length == 1)
            {
                return int.Parse(t[0].PadRight(5, '0'));
            }
            else if (t.Length == 2)
            {
                return int.Parse(t[0] + t[1].PadLeft(2, '0') + "00");
            }
            return int.Parse(t[0] + t[1].PadLeft(2, '0') + t[2].PadLeft(2, '0'));
        }
        private static int _GetVer (string str, int add)
        {
            string[] t = str.Split('.');
            if (t.Length == 1)
            {
                return int.Parse(t[0].PadRight(5, '0')) + ( add * 10000 );
            }
            else if (t.Length == 2)
            {
                return int.Parse(t[0] + t[1].PadLeft(2, '0') + "00") + ( add * 100 );
            }
            return int.Parse(t[0] + t[1].PadLeft(2, '0') + t[2].PadLeft(2, '0')) + add;
        }
        public static Expression<Func<TransactionListModel, bool>> ToWhere (this TransactionQuery query)
        {
            if (query == null)
            {
                return LinqKit.PredicateBuilder.New<TransactionListModel>(a => a.IsRoot);
            }
            ExpressionStarter<TransactionListModel> where = LinqKit.PredicateBuilder.New<TransactionListModel>();
            if (query.TranName.IsNotNull())
            {
                where = where.And(a => a.TranName == query.TranName);
            }
            if (query.RpcMerId.HasValue)
            {
                where = where.And(c => c.RpcMerId == query.RpcMerId.Value);
            }
            if (query.ServerId.HasValue)
            {
                where = where.And(c => c.ServerId == query.ServerId.Value);
            }
            if (query.RegionId.HasValue)
            {
                where = where.And(c => c.RegionId == query.RegionId.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (!query.TranStatus.IsNull())
            {
                where = where.And(c => query.TranStatus.Contains(c.TranStatus));
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.AddTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.AddTime <= query.End.Value);
            }
            where = where.And(c => c.IsRoot);
            return where;
        }
        public static Expression<Func<RpcTraceModel, bool>> ToWhere (this TraceQuery query)
        {
            ExpressionStarter<RpcTraceModel> where = LinqKit.PredicateBuilder.New<RpcTraceModel>();
            if (query.QueryKey.IsNotNull())
            {
                if (query.QueryKey.Validate(ValidateFormat.纯数字))
                {
                    where = where.And(c => c.TraceId == query.QueryKey);
                }
                else
                {
                    where = where.And(c => c.Dictate.Contains(query.QueryKey));
                }
            }
            if (query.RegionId.HasValue)
            {
                where = where.And(c => c.RegionId == query.RegionId.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (query.Begin.HasValue && query.End.HasValue)
            {
                where = where.And(c => SqlFunc.Between(c.BeginTime, query.Begin.Value, query.End.Value));
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<ServerTransmitSchemeModel, bool>> ToWhere (this TransmitSchemeQuery query)
        {
            ExpressionStarter<ServerTransmitSchemeModel> where = LinqKit.PredicateBuilder.New<ServerTransmitSchemeModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.SystemTypeId.HasValue)
            {
                where = where.And(c => c.SystemTypeId == query.SystemTypeId.Value);
            }
            if (query.TransmitType.HasValue)
            {
                where = where.And(c => c.TransmitType == query.TransmitType.Value);
            }
            if (query.IsEnable.HasValue)
            {
                where = where.And(c => c.IsEnable == query.IsEnable.Value);
            }
            if (query.VerNum.HasValue)
            {
                where = where.And(c => c.VerNum == query.VerNum.Value);
            }
            if (query.Scheme.IsNotNull())
            {
                where = where.And(c => c.Scheme.Contains(query.Scheme));
            }
            return where;
        }
        public static Expression<Func<ResourceShieldModel, bool>> ToWhere (this ResourceShieldQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<ResourceShieldModel> where = LinqKit.PredicateBuilder.New<ResourceShieldModel>();
            if (query.RpcMerId.HasValue)
            {
                where = where.And(c => c.RpcMerId == query.RpcMerId.Value);
            }
            if (query.ResourceId.HasValue)
            {
                where = where.And(c => c.ResourceId == query.ResourceId.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (query.Path.IsNotNull())
            {
                where = where.And(c => c.ResourcePath.Contains(query.Path));
            }
            if (query.ShieldType.HasValue)
            {
                where = where.And(c => c.ShieldType == query.ShieldType.Value);
            }
            if (query.IsOverTime.HasValue)
            {
                long now = DateTime.Now.ToLong();
                if (query.IsOverTime.Value)
                {
                    where = where.And(c => c.BeOverdueTime != 0 && c.BeOverdueTime <= now);
                }
                else
                {
                    where = where.And(c => c.BeOverdueTime == 0 || c.BeOverdueTime > now);
                }
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<IpBlackListModel, bool>> ToWhere (this IpBlackQuery query)
        {
            ExpressionStarter<IpBlackListModel> where = LinqKit.PredicateBuilder.New<IpBlackListModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (query.Ip.IsNotNull())
            {
                if (query.Ip.Validate(ValidateFormat.IP))
                {
                    long ip = IPAddress.Parse(query.Ip).Address;
                    where = where.And(c => c.IpType == IpType.Ip4 && c.Ip == ip);
                }
                else
                {
                    where = where.And(c => c.IpType == IpType.Ip6 && c.Ip6 == query.Ip);
                }
            }
            else if (query.IpType.HasValue)
            {
                where = where.And(c => c.IpType == query.IpType.Value);
            }
            where = where.And(c => c.IsDrop == false);
            return where;
        }
        public static Expression<Func<ResourceListModel, bool>> ToWhere (this ResourceQuery query)
        {
            ExpressionStarter<ResourceListModel> where = LinqKit.PredicateBuilder.New<ResourceListModel>(a => a.ModularId == query.ModularId);
            if (query.ResourceState.HasValue)
            {
                where = where.And(c => c.ResourceState == query.ResourceState.Value);
            }
            if (query.QueryKey.IsNotNull())
            {
                where = where.And(c => c.ResourcePath.Contains(query.QueryKey) || c.ResourceShow.Contains(query.QueryKey));
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<ResourceModularModel, bool>> ToWhere (this ModularQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<ResourceModularModel> where = LinqKit.PredicateBuilder.New<ResourceModularModel>();
            if (query.RpcMerId.HasValue)
            {
                where = where.And(c => c.RpcMerId == query.RpcMerId.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (query.ResourceType.HasValue)
            {
                where = where.And(c => c.ResourceType == query.ResourceType.Value);
            }
            if (query.QueryKey.IsNotNull())
            {
                where = where.And(c => c.Remark.Contains(query.QueryKey) || c.ModularName.Contains(query.QueryKey));
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<SystemErrorLogModel, bool>> ToWhere (this SysLogQuery query)
        {
            ExpressionStarter<SystemErrorLogModel> where = LinqKit.PredicateBuilder.New<SystemErrorLogModel>();

            if (query.RpcMerId.HasValue)
            {
                where = where.And(c => c.RpcMerId == query.RpcMerId.Value);
            }
            if (query.LogType.HasValue)
            {
                where = where.And(c => c.LogType == query.LogType.Value);
            }
            if (query.LogGrade.HasValue)
            {
                where = where.And(c => c.LogGrade == query.LogGrade.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(c => c.SystemType == query.SystemType);
            }
            if (query.LogGroup.IsNotNull())
            {
                where = where.And(c => c.LogGroup == query.LogGroup);
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.AddTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.AddTime <= query.End.Value);
            }
            if (!query.QueryKey.IsNull())
            {
                if (query.QueryKey.Validate(ValidateFormat.纯数字))
                {
                    where = where.And(c => c.TraceId == query.QueryKey);
                }
                else
                {
                    where = where.And(a => a.LogTitle.Contains(query.QueryKey));
                }
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<ErrorCollectModel, bool>> ToWhere (this ErrorQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<ErrorCollectModel> where = LinqKit.PredicateBuilder.New<ErrorCollectModel>();
            if (query.QueryKey.IsNotNull())
            {
                if (query.QueryKey.Validate(ValidateFormat.纯数字))
                {
                    long id = long.Parse(query.QueryKey);
                    where = where.And(c => c.Id == id);
                }
                else
                {
                    where = where.And(c => c.ErrorCode.StartsWith(query.QueryKey));
                }
            }
            if (query.IsPerfect.HasValue)
            {
                where = where.And(c => c.IsPerfect == query.IsPerfect.Value);
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<IdentityAppModel, bool>> ToWhere (this IdentityQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<IdentityAppModel> where = LinqKit.PredicateBuilder.New<IdentityAppModel>();
            if (query.AppName.IsNotNull())
            {
                where = where.And(c => c.AppName.Contains(query.AppName));
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.EffectiveDate >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.EffectiveDate <= query.End.Value);
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<RemoteServerConfigModel, bool>> ToWhere (this ServerConfigQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<RemoteServerConfigModel> where = LinqKit.PredicateBuilder.New<RemoteServerConfigModel>();
            if (query.GroupId.HasValue)
            {
                where = where.And(c => c.GroupId == query.GroupId.Value);
            }
            if (!query.SystemTypeId.IsNull())
            {
                where = where.And(c => query.SystemTypeId.Contains(c.SystemType));
            }
            if (!query.ServiceState.IsNull())
            {
                where = where.And(c => query.ServiceState.Contains(c.ServiceState));
            }
            if (!query.RegionId.IsNull())
            {
                where = where.And(c => query.RegionId.Contains(c.RegionId));
            }
            if (query.ServiceType.HasValue)
            {
                where = where.And(c => c.ServiceType == query.ServiceType.Value);
            }
            if (query.ContainerGroup.HasValue)
            {
                where = where.And(c => c.IsContainer && c.ContainerGroupId == query.ContainerGroup.Value);
            }
            else if (query.IsContainer.HasValue)
            {
                where = where.And(c => c.IsContainer == query.IsContainer.Value);
            }
            if (query.IsOnline.HasValue)
            {
                where = where.And(c => c.IsOnline == query.IsOnline.Value);
            }
            if (!query.ControlId.IsNull())
            {
                where = where.And(c => query.ControlId.Contains(c.BindIndex));
            }
            if (!query.QueryKey.IsNull())
            {
                if (query.QueryKey.Validate(ValidateFormat.IP))
                {
                    where = where.And(c => c.ConIp == query.QueryKey
                    || c.ServerIp == query.QueryKey
                    || c.RemoteIp == query.QueryKey);
                }
                else if (_IpContainsRegex.IsMatch(query.QueryKey))
                {
                    where = where.And(c => c.ConIp.Contains(query.QueryKey)
                    || c.ServerIp.Contains(query.QueryKey)
                    || c.RemoteIp.Contains(query.QueryKey));
                }
                else if (query.QueryKey.Validate(ValidateFormat.MAC))
                {
                    where = where.And(c => c.ServerMac == query.QueryKey);
                }
                else if (Regex.IsMatch(query.QueryKey, @"^(\w{2,12})|((\w|[-:]){2,17})$"))
                {
                    where = where.And(c => c.ServerMac.Contains(query.QueryKey));
                }
                else if (_ApiVerRegex.IsMatch(query.QueryKey))
                {
                    int ver = int.Parse(query.QueryKey.Replace(".", string.Empty));
                    where = where.And(c => c.VerNum == ver);
                }
                else if (_ApiVerContainsRegex.IsMatch(query.QueryKey))
                {
                    int ver = _GetVer(query.QueryKey);
                    int end = _GetVer(query.QueryKey, 1);
                    where = where.And(c => c.VerNum >= ver && c.VerNum <= end);
                }
                else
                {
                    where = where.And(c => c.ServerName.Contains(query.QueryKey));
                }
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<ServerVisitCensusModel, bool>> ToWhere (this VisitCensusQuery query)
        {
            ExpressionStarter<ServerVisitCensusModel> where = LinqKit.PredicateBuilder.New<ServerVisitCensusModel>(a => a.ServiceId == query.ServiceId);
            if (query.Dictate.IsNotNull())
            {
                where = where.And(c => c.Dictate.StartsWith(query.Dictate));
            }
            return where;
        }
        public static Expression<Func<BroadcastErrorLogModel, bool>> ToWhere (this BroadcastErrorQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<BroadcastErrorLogModel> where = LinqKit.PredicateBuilder.New<BroadcastErrorLogModel>();
            if (query.MsgKey.IsNotNull())
            {
                where = where.And(c => c.MsgKey == query.MsgKey);
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.AddTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.AddTime <= query.End.Value);
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<ServerPublicSchemeModel, bool>> ToWhere (this PublicSchemeQuery query)
        {
            ExpressionStarter<ServerPublicSchemeModel> where = LinqKit.PredicateBuilder.New<ServerPublicSchemeModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.QueryKey.IsNotNull())
            {
                where = where.And(c => c.SchemeName.Contains(query.QueryKey));
            }
            if (!query.Status.IsNull())
            {
                where = where.And(c => query.Status.Contains(c.Status));
            }
            if (query.Begin.HasValue)
            {
                where = where.And(c => c.AddTime >= query.Begin.Value);
            }
            if (query.End.HasValue)
            {
                where = where.And(c => c.AddTime <= query.End.Value);
            }
            return where;
        }
        public static Expression<Func<ServerDictateLimitModel, bool>> ToWhere (this DictateLimitQuery query)
        {
            ExpressionStarter<ServerDictateLimitModel> where = LinqKit.PredicateBuilder.New<ServerDictateLimitModel>(a => a.ServerId == query.ServerId);
            if (query.Dictate.IsNotNull())
            {
                where = where.And(c => c.Dictate.StartsWith(query.Dictate));
            }
            if (query.LimitType.HasValue)
            {
                where = where.And(c => c.LimitType == query.LimitType.Value);
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<RemoteServerTypeModel, bool>> ToWhere (this ServerTypeQuery query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<RemoteServerTypeModel> where = LinqKit.PredicateBuilder.New<RemoteServerTypeModel>();
            if (query.GroupId.HasValue)
            {
                where = where.And(a => a.GroupId == query.GroupId.Value);
            }
            if (!query.Name.IsNull())
            {
                where = where.And(a => a.SystemName.Contains(query.Name));
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }
        public static Expression<Func<SysConfigModel, bool>> ToWhere (this SysConfigModel query)
        {
            ExpressionStarter<SysConfigModel> where = LinqKit.PredicateBuilder.New<SysConfigModel>(a => a.RpcMerId == query.RpcMerId);
            if (query.ServerId != 0)
            {
                where = where.And(a => a.ServerId == query.ServerId);
            }
            else
            {
                if (query.ContainerGroup != 0)
                {
                    where = where.And(a => a.ContainerGroup == query.ContainerGroup);
                }
                if (query.RegionId != 0)
                {
                    where = where.And(a => a.RegionId == query.RegionId);
                }
                if (query.VerNum != 0)
                {
                    where = where.And(a => a.VerNum == query.VerNum);
                }
                if (!query.SystemType.IsNull())
                {
                    where = where.And(a => a.SystemType == query.SystemType);
                }
            }
            RpcConfigType[] types = new RpcConfigType[]
            {
                RpcConfigType.独立配置,
                RpcConfigType.自定义配置
            };
            where = where.And(a => a.Name == query.Name && types.Contains(a.ConfigType));
            return where;
        }
        public static Expression<Func<SysConfigModel, bool>> ToWhere (this QuerySysParam query)
        {
            if (query == null)
            {
                return null;
            }
            ExpressionStarter<SysConfigModel> where = LinqKit.PredicateBuilder.New<SysConfigModel>();
            if (query.RpcMerId.HasValue)
            {
                where = where.And(a => a.RpcMerId == query.RpcMerId.Value);
            }
            if (query.ServerId.HasValue)
            {
                where = where.And(a => a.ServerId == query.ServerId.Value);
            }
            if (query.RegionId.HasValue)
            {
                where = where.And(a => a.RegionId == query.RegionId.Value);
            }
            if (query.ContainerGroup.HasValue)
            {
                where = where.And(a => a.ContainerGroup == query.ContainerGroup.Value);
            }
            if (query.VerNum.HasValue)
            {
                where = where.And(a => a.VerNum == query.VerNum.Value);
            }
            if (query.SystemType.IsNotNull())
            {
                where = where.And(a => a.SystemType == query.SystemType);
            }
            if (query.ConfigName.IsNotNull())
            {
                where = where.And(c => c.Name.Contains(query.ConfigName));
            }
            if (query.ConfigType.HasValue)
            {
                where = where.And(c => c.ConfigType == query.ConfigType.Value);
            }
            if (!where.IsStarted)
            {
                return null;
            }
            return where;
        }

    }
}
