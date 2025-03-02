using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.FileUp;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.Handler;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;
namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class BlockUpCollect
    {
        private static readonly ConcurrentDictionary<string, IBlockUpTask> _UpTaskList = new ConcurrentDictionary<string, IBlockUpTask>();
        private static readonly ConcurrentDictionary<string, string> _UpTaskKey = new ConcurrentDictionary<string, string>();
        private static IBasicHandler _upHandler;
        private static string _RouteId;
        private static readonly UpTaskState[] _RemoveState = new UpTaskState[]
            {
                UpTaskState.上传失败,
                UpTaskState.上传完成
            };
        private static Timer _Timer = null;
        private static IApiModular _ApiModular;
        public static void Init ( IApiModular modular)
        {
            _ApiModular = modular;
            IGatewayUpConfig config = RpcClient.Ioc.Resolve<IGatewayUpConfig>();
            config.RefreshEvent(_Refresh);
            _Timer = new Timer(_Clear, null, 10000, 10000);
        }
        private static void _Clear ( object state )
        {
            if ( _UpTaskList.IsEmpty )
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime - 10;
            string[] keys = _UpTaskList.Where(c => c.Value.TimeOut <= time && _RemoveState.Contains(c.Value.UpState)).Select(c => c.Key).ToArray();
            if ( keys.IsNull() )
            {
                return;
            }
            keys.ForEach(a =>
            {
                if ( _UpTaskList.TryRemove(a, out IBlockUpTask task) && task.TaskKey.IsNotNull() )
                {
                    string id = _UpTaskKey.GetValueOrDefault(task.TaskKey);
                    if ( id == a )
                    {
                        _ = _UpTaskKey.TryRemove(task.TaskKey, out _);
                    }
                }
            });
        }
        private static void _Init ( IGatewayUpConfig config )
        {
            if ( _RouteId == null && config.BlockUpState != null )
            {
                MethodInfo method = typeof(BlockUpRouteApi).GetMethod("GetState");
                BlockUpCollect._RouteId = _ApiModular.Route.RegApi(method, config.BlockUpState);
            }
            if (_upHandler == null && config.BlockUpUri.IsNotNull())
            {
                _upHandler = new HttpBlockUpHandler(config.BlockUpUri, config.BlockUpUriIsRegex);
            }
        }
        private static void _Refresh ( IGatewayUpConfig config )
        {
            _Init(config);
            if ( config.EnableBlock && _upHandler != null && _RouteId.IsNotNull())
            {
                HttpService.HttpService.AddRoute(_upHandler);
                _ApiModular.Route.Enable(_RouteId);
            }
            else if ( !config.EnableBlock && _upHandler != null && _RouteId.IsNotNull() )
            {
                _ApiModular.Route.Disable(_RouteId);
                HttpService.HttpService.RemoveRoute(_upHandler.RequestPath, _upHandler.IsRegex);
            }
        }


        public static bool GetTask ( string taskId, out IBlockUpTask task )
        {
            return _UpTaskList.TryGetValue(taskId, out task);
        }
        public static IBlockUpTask Add ( IService service, string apiName )
        {
            BlockUpTask task = new BlockUpTask(service.ServiceName, apiName);
            if ( _UpTaskList.TryAdd(task.TaskId, task) )
            {
                return task;
            }
            throw new ErrorException("http.gateway.block.up.task.add.fail");
        }

        internal static void Sync ( string taskKey, string taskId )
        {
            if ( _UpTaskKey.TryGetValue(taskKey, out string id) )
            {
                if ( id == taskId )
                {
                    return;
                }
                _ToVoid(id);
            }
            else if ( !_UpTaskKey.TryAdd(taskKey, taskId) )
            {
                Sync(taskKey, taskId);
            }
        }
        private static void _ToVoid ( string id )
        {
            if ( _UpTaskList.TryGetValue(id, out IBlockUpTask task) )
            {
                task.UpError("gateway.http.up.task.tovoid");
            }
        }
    }
}
