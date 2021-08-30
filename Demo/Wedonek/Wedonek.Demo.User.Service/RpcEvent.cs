using RpcClient.Interface;
using RpcModel;
using SocketTcpServer.FileUp;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Demo.User.Service.Interface;
using Wedonek.Demo.RemoteModel.Msg;
using RpcHelper;

namespace Wedonek.Demo.User.Service
{
        internal class RpcEvent : IRpcEvent
        {
                public void RefreshService(long serverId)
                {
                        //服务节点刷新事件
                }

                public void ServerClose()
                {
                        //服务关闭事件
                }

                public void ServerStarted()
                {
                        //服务以启动
                        RpcClient.RpcClient.RpcTran.RegTran<AddOrderNum>((a, b) =>
                        {
                                IUserService user = RpcClient.RpcClient.Unity.Resolve<IUserService>();
                                user.AddOrderNum(a.UserId, -a.OrderNum);
                        });
                        RpcClient.RpcClient.RpcTran.RegTran<SetOrderNo>((a, b) =>
                         {
                                 IUserService user = RpcClient.RpcClient.Unity.Resolve<IUserService>();
                                 user.SetOrderNo(a.UserId, a.OrderNo, b);
                         });

                        RpcClient.RpcClient.RpcTran.RegTran<PlaceAnOrder>((a, b) =>
                        {
                                System.Console.WriteLine("PlaceAnOrder回滚了! ");
                                System.Console.WriteLine(string.Concat("msg:", a.ToJson()));
                        });
                        FileUpRouteCollect.AddAllot(new Event.UpDemoFileEvent());
                }

                public void ServerStarting()
                {
                        //服务启动前

                }

                public void ServiceState(RpcServiceState state)
                {
                        //节点状态变更事件
                }
        }
}
