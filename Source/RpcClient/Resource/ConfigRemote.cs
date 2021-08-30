using RpcClient.Collect;

using RpcModel;

namespace RpcClient.Resource
{
        internal class ConfigRemote
        {
                public static bool GetConfigVal(string name, out string value, out string error)
                {
                        GetSysConfigVal obj = new GetSysConfigVal()
                        {
                                Name = name
                        };
                        return RemoteCollect.Send(obj, out value, out error);
                }
                public static bool GetConfig(out RemoteSysConfig config, out string error)
                {
                        return RemoteCollect.Send(new GetSysConfig(), out config, out error);
                }

        }
}
