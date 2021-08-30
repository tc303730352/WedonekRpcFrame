using System;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Config
{
        internal class ConfigDic
        {
                public static readonly string ErrorParamName = "error";

                public static readonly string CountParamName = "count";
                public static readonly string LocalIp = "127.0.0.1";
                public static readonly Type BroadcastType = typeof(IRemoteBroadcast);
                public static readonly Type RemoteConfigType = typeof(RpcModel.IRemoteConfig);
                public static readonly Type MsgSourceType = typeof(MsgSource);
                public static readonly Type ReturnType = typeof(IBasicRes);
                public static readonly Type ParamType = typeof(IMsg);
                public static readonly Type RpcApiService = typeof(IRpcApiService);
                public static readonly Type RpcSubscribeService = typeof(IRpcSubscribeService);
                public static readonly Type VoidType = typeof(void);
                public static readonly Type IStreamAllot = typeof(IStreamAllot);

                public static readonly Type IExtendService = typeof(IExtendService);
                internal static BasicRes SuccessRes = new BasicRes();
        }
}
