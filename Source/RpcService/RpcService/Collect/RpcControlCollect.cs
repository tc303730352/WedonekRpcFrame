using RpcModel.Model;

namespace RpcService.Collect
{
        internal class RpcControlCollect
        {
                public static bool GetControlServer(out RpcControlServer[] servers, out string error)
                {
                        string key = "ControlServer";
                        if (RpcService.Cache.TryGet(key, out servers))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.RpcControlListDAL().GetControlServer(out servers))
                        {
                                error = "rpc.control.get.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                RpcService.Cache.Set(key, servers, new System.TimeSpan(0, 10, 0));
                                return true;
                        }
                }
        }
}
