using WeDonekRpc.Client;
using WeDonekRpc.Modular;

namespace RpcSync.Service
{
    public class RpcSyncService
    {
        public static void Start ()
        {
            //注册RPC事件处理类
            RpcClient.RpcEvent = new RpcSyncEvent();
            //启动服务
            RpcClient.Start();
        }
        internal class RpcSyncEvent : RpcEvent
        {
            /// <summary>
            /// 启动前加载事件
            /// </summary>
            /// <param name="option"></param>
            public override void Load (RpcInitOption option)
            {
                //加载扩展模块
                option.LoadModular<ExtendModular>();
                //加载入口文件命名空间(注册IOC，加载远程事件方法，加载本地消息事件等)
                option.Load("RpcSync.Service");
            }
        }
    }
}
