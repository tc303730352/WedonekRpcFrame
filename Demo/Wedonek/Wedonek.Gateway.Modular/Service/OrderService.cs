using System;

using ApiGateway.Attr;
using ApiGateway.Interface;

using HttpApiGateway.Interface;

using RpcClient.Interface;
using RpcClient.Tran;

using RpcModel;

using RpcHelper;

using Wedonek.Demo.RemoteModel.Msg;
using Wedonek.Demo.RemoteModel.Order;
using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Demo.RemoteModel.User;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;

namespace Wedonek.Gateway.Modular.Service
{
        /// <summary>
        /// 订单接口
        /// </summary>
        [ApiPrower("demo.order")]
        internal class OrderService : IOrderService, IApiGateway
        {
                /// <summary>
                /// 添加订单
                /// </summary>
                /// <param name="state">用户登陆状态</param>
                /// <param name="add">添加订单参数</param>
                /// <param name="identity">客户端标识</param>
                /// <returns>订单Id</returns>
                public Guid AddOrder(UserLoginState state, OrderParam add, IClientIdentity identity)
                {
                        string orderNo = RpcHelper.Tools.GetSerialNo("DE01");
                        Guid id;
                        using (IRpcTransaction tran = new RpcTransaction(TranLevel.全部))
                        {
                                ICurTran tranDatum = tran.Tran; 
                                id = new AddOrder
                                {
                                        OrderNo = orderNo,
                                        OrderPrice = add.OrderPrice,
                                        OrderTitle = add.OrderTitle,
                                        UserId = state.UserId
                                }.Send();
                                new AddOrderNum
                                {
                                        OrderNum = 1,
                                        UserId = state.UserId
                                }.Send();
                                new PlaceAnOrder
                                {
                                        OrderNo = orderNo,
                                        UserId = state.UserId
                                }.Send();
                                //tran.Complate();//提交事务，无需等待订阅和广播消息确认
                                //等待事务结束
                                TranStatus status= tran.Complate(a =>
                                {
                                        //检查事务日志 (当前完成整个订单流程 需要6步)
                                        if (a.TranLogs != null && a.TranLogs.Length >= 6)
                                        {
                                                a.Commit();
                                        }
                                },10);
                                if(status == TranStatus.提交)
                                {
                                        return id;
                                }
                                throw new ErrorException("事务执行："+status.ToString());
                        }
                }
                /// <summary>
                /// 删除订单
                /// </summary>
                /// <param name="orderId">订单Id</param>
                public void DropOrder(Guid orderId)
                {
                        new DropOrder { OrderId = orderId }.Send();
                }
                /// <summary>
                /// 获取订单
                /// </summary>
                /// <param name="orderId">订单Id</param>
                /// <returns>订单信息</returns>
                public OrderData GetOrder(Guid orderId)
                {
                        return new GetOrder { OrderId = orderId }.Send();
                }
                /// <summary>
                /// 查询订单
                /// </summary>
                /// <param name="state">订单状态</param>
                /// <param name="paging">分页信息</param>
                /// <param name="count">订单数</param>
                /// <returns>订单信息</returns>
                public OrderData[] Query(UserLoginState state, BasicPage paging, out long count)
                {
                        return new QueryOrder
                        {
                                Index = paging.Index,
                                Size = paging.Size,
                                NextId = paging.NextId,
                                UserId = state.UserId
                        }.Send(out count);
                }
        }
}
