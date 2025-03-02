using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Modular;

namespace Wedonek.Demo.Service
{
    internal class RpcEvent : IRpcEvent
    {
        public void Load ( RpcInitOption option )
        {
            option.LoadModular<ExtendModular>();
            option.Load("Wedonek.Demo.Service");
        }

        public void RefreshService ( long serverId )
        {
            //刷新服务节点配置事件
        }

        public void ServerClose ()
        {
            //服务关闭事件
        }

        public void ServerInit ( IIocService ioc )
        {
        }

        /// <summary>
        /// 服务已启动
        /// </summary>
        public void ServerStarted ()
        {
        }

        public void ServerStarting ()
        {
            //服务启动前

        }

        public void ServiceState ( RpcServiceState state )
        {

        }
    }
}
