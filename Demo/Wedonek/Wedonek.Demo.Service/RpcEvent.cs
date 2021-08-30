using System;

using RpcClient.Interface;

using RpcModel;

using RpcHelper;

using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.Service.Interface;

namespace Wedonek.Demo.Service
{
        internal class RpcEvent : IRpcEvent
        {
                public void RefreshService(long serverId)
                {
                        //刷新服务节点配置事件
                }

                public void ServerClose()
                {
                        //服务关闭事件
                }
                /// <summary>
                /// 服务已启动
                /// </summary>
                public void ServerStarted()
                {
                        //注册事务
                        RpcClient.RpcClient.RpcTran.RegTran<AddOrder>((param, extend) =>
                        {
                                IOrderService order = RpcClient.RpcClient.Unity.Resolve<IOrderService>();
                                if (!order.DropOrder(param.OrderNo))
                                {
                                        throw new ErrorException("demo.order.drop.error");
                                }
                        });
                        RpcClient.RpcClient.RpcTran.RegTran<Guid>("CreateOrder", (orderId) =>
                         {
                                 IOrderService order = RpcClient.RpcClient.Unity.Resolve<IOrderService>();
                                 order.DropOrder(orderId);
                         });
                }

                public void ServerStarting()
                {
                        //服务启动前

                }

                public void ServiceState(RpcServiceState state)
                {

                }
        }
}
