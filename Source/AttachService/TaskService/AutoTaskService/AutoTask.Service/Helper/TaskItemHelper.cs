using System.Text;
using AutoTask.Collect.Model;
using AutoTask.Service.Model;
using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model;

namespace AutoTask.Service.Helper
{
    internal class TaskItemHelper
    {
        public static TaskExecLog[] ExecTask ( TaskItemDto item, int? regionId, long rpcMerId )
        {
            List<TaskExecLog> logs = [];
            if ( item.SendType == TaskSendType.指令 )
            {
                _SendDictate(item, rpcMerId, regionId, logs);
            }
            else if ( item.SendType == TaskSendType.Http )
            {
                _SendHttp(item, logs);
            }
            else if ( item.SendType == TaskSendType.广播 )
            {
                _SendBroadcast(item, rpcMerId, regionId, logs);
            }
            return logs.ToArray();
        }
        private static void _SendDictate ( TaskItemDto item, long rpcMerId, int? regionId, List<TaskExecLog> logs )
        {
            RpcParamConfig config = item.SendParam.RpcConfig;
            IRemoteConfig send = new IRemoteConfig(config.SysDictate, item.RetryNum.HasValue)
            {
                TimeOut = item.TimeOut,
                IsAllowRetry = item.RetryNum.HasValue,
                RetryNum = item.RetryNum,
                RpcMerId = rpcMerId,
                RegionId = regionId,
                SystemType = config.SystemType,
            };
            if ( config.RemoteSet != null )
            {
                send = send.ConvertInto(config.RemoteSet);
                send.IsEnableLock = !send.LockColumn.IsNull();
            }
            else
            {
                send.IsProhibitTrace = true;
            }
            TaskExecLog result = new TaskExecLog
            {
                begin = DateTime.Now,
                logRange = item.LogRange
            };
            if ( !RemoteCollect.Send(send, config.MsgBody, out string error) )
            {
                result.end = DateTime.Now;
                result.isFail = true;
                result.error = error;
            }
            else
            {
                result.end = DateTime.Now;
            }
            logs.Add(result);
        }
        private static TaskExecLog _SendHttp ( TaskItemDto item )
        {
            HttpParamConfig param = item.SendParam.HttpConfig;
            RequestSet request = new RequestSet();
            if ( item.TimeOut.HasValue )
            {
                request.Timeout = new TimeSpan(0, 0, item.TimeOut.Value);
            }
            TaskExecLog result = new TaskExecLog
            {
                begin = DateTime.Now
            };
            HttpCompletionOption option = param.ResponseHeadersRead ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;
            try
            {
                using ( HttpClient client = HttpClientFactory.Create(item.ClientKey) )
                {
                    using ( HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Parse(param.RequestMethod), param.Uri) )
                    {
                        if ( param.PostParam.IsNotNull() )
                        {
                            req.Content = new StringContent(param.PostParam, Encoding.UTF8, param.ContentType);
                        }
                        HttpResult res = client.SendRequest(req, option);
                        result.end = DateTime.Now;
                        result.isFail = false;
                        if ( option == HttpCompletionOption.ResponseContentRead )
                        {
                            result.result = res.GetString();
                        }
                    }
                }
            }
            catch ( ErrorException e )
            {
                result.isFail = true;
                result.error = e.ErrorCode;
            }
            catch ( Exception e )
            {
                result.end = DateTime.Now;
                ErrorException error = ErrorException.FormatError(e);
                error.SaveLog("Task");
                result.error = error.ErrorCode;
            }
            return result;
        }
        private static void _SendHttp ( TaskItemDto item, List<TaskExecLog> logs )
        {
            TaskExecLog log = _SendHttp(item);
            log.logRange = item.LogRange;
            logs.Add(log);
            if ( log.isFail && item.RetryNum.HasValue )
            {
                for ( int i = 0 ; i < item.RetryNum.Value ; i++ )
                {
                    log = _SendHttp(item);
                    logs.Add(log);
                    if ( !log.isFail )
                    {
                        break;
                    }
                }
            }

        }
        private static RemoteSet _GetRemoteSet ( TaskItemDto item, RpcRemoteSet remoteSet )
        {
            return new RemoteSet
            {
                AppIdentity = remoteSet.AppIdentity,
                IdentityColumn = remoteSet.IdentityColumn,
                IsAllowRetry = item.RetryNum.HasValue,
                IsEnableLock = remoteSet.IsEnableLock,
                LockColumn = remoteSet.LockColumn,
                IsProhibitTrace = remoteSet.IsProhibitTrace,
                LockType = remoteSet.LockType,
                RetryNum = item.RetryNum,
                TimeOut = item.TimeOut,
                Transmit = remoteSet.Transmit,
                TransmitType = remoteSet.TransmitType,
                ZIndexBit = remoteSet.ZIndexBit
            };
        }
        private static void _SendBroadcast ( TaskItemDto item, long rpcMerId, int? regionId, List<TaskExecLog> logs )
        {
            RpcBroadcastConfig broadcast = item.SendParam.BroadcastConfig;
            IRemoteBroadcast send = new IRemoteBroadcast(broadcast.SysDictate, broadcast.IsOnly, broadcast.TypeVal)
            {
                IsCrossGroup = false,
                RpcMerId = rpcMerId,
                ServerId = broadcast.ServerId,
                BroadcastType = broadcast.BroadcastType,
                IsExclude = false,
                RegionId = regionId,
                IsOnly = broadcast.IsOnly,
                RemoteConfig = _GetRemoteSet(item, broadcast.RemoteSet)
            };
            if ( send.RemoteConfig == null )
            {
                send.IsProhibitTrace = true;
            }
            else
            {
                send.IsProhibitTrace = send.RemoteConfig.IsProhibitTrace;
            }
            TaskExecLog result = new TaskExecLog
            {
                begin = DateTime.Now,
                logRange = item.LogRange
            };
            try
            {
                RemoteCollect.BroadcastMsg(send, broadcast.MsgBody);
                result.end = DateTime.Now;
            }
            catch ( Exception e )
            {
                result.end = DateTime.Now;
                ErrorException error = ErrorException.FormatError(e);
                error.SaveLog("Task");
                result.error = error.ErrorCode;
            }
            logs.Add(result);
        }
    }
}
