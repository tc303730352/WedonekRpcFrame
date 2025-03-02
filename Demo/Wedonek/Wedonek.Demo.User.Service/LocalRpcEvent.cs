using Wedonek.Demo.RemoteModel.File;
using Wedonek.Demo.RemoteModel.Msg;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.User.Service.Event;
using Wedonek.Demo.User.Service.Interface;
using WeDonekRpc.CacheModular;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Modular;
using WeDonekRpc.SqlSugarDbTran;

namespace Wedonek.Demo.User.Service
{
    internal class LocalRpcEvent : IRpcEvent
    {
        public void Load ( RpcInitOption option )
        {
            option.Route.RegUpFileRoute<UpResult>("UpDemoFile", typeof(UpDemoFileEvent));
            option.LoadModular<ExtendModular>();
            option.LoadModular<CacheModular>();
            option.LoadModular<RpcTranModular>();
            option.Load("Wedonek.Demo.User.Service");
            //注册Tcc事务
            option.Tran.RegTccTran<LockUserMoney, UserMoneyTccTran>();

            option.Tran.RegSagaTran<SetOrderNo>(( cur ) =>
            {
                IUserService user = RpcClient.Ioc.Resolve<IUserService>();
                SetOrderNo data = cur.Body.GetBody<SetOrderNo>();
                user.SetOrderNo(data.UserId, data.OrderNo, cur.Body.Extend);
            });
            option.Tran.RegSagaTran<PlaceAnOrder>(( cur ) =>
            {
                System.Console.WriteLine("PlaceAnOrder回滚了! ");
                System.Console.WriteLine(string.Concat("msg:", cur.Body.Body));
            });
            //注册上传大文件Demo
            option.Route.RegUpFileRoute<UpResult>("UpDemoFile", typeof(UpDemoFileEvent));
        }

        public void RefreshService ( long serverId )
        {
            //服务节点刷新事件
        }

        public void ServerClose ()
        {
            //服务关闭事件
        }

        public void ServerInit ( IIocService ioc )
        {
        }

        public void ServerStarted ()
        {

        }

        public void ServerStarting ()
        {
            //服务启动前

        }

        public void ServiceState ( RpcServiceState state )
        {
            //节点状态变更事件
        }
    }
}
