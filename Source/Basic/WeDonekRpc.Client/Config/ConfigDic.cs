using System;
using System.Threading.Tasks;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Model;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.Client.Config
{
    internal class ConfigDic
    {

        public static readonly string DefTransmitScheme = "Def";
        public static readonly string LocalIp = "127.0.0.1";
        public static readonly Type BroadcastType = typeof(IRemoteBroadcast);
        public static readonly Type RemoteConfigType = typeof(IRemoteConfig);
        public static readonly Type MsgSourceType = typeof(MsgSource);
        public static readonly Type ReturnType = typeof(IBasicRes);
        public static readonly Type ParamType = typeof(IMsg);
        public static readonly Type RpcApiService = typeof(IRpcApiService);
        public static readonly Type RpcSubscribeService = typeof(IRpcSubscribeService);
        public static readonly Type VoidType = typeof(void);
        public static readonly Type TaskType = typeof(Task);
        public static readonly Type TaskReturnType = typeof(Task<IBasicRes>);
        public static readonly Type IStreamAllot = typeof(IStreamAllot);
        public static readonly Type InitModularType = typeof(IRpcInitModular);
        public static readonly Type IExtendService = typeof(IRpcExtendService);
        public static readonly Type IMapper = typeof(ISchemeMapper);
        public static readonly string TaskFuncType = "Task`1";
        public static BasicRes SuccessRes = new BasicRes();
    }
}
