using System;
using System.Linq;

using RpcClient;
using RpcClient.Interface;
using RpcClient.Tran;

using RpcModel;

using RpcHelper;

using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Demo.RemoteModel.Order.Msg;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.Service.Interface;
using Wedonek.Demo.Service.LocalEvent.Event;
using Wedonek.Demo.Service.Model;

namespace Wedonek.Demo.Service.Service
{
        internal class OrderService : IOrderService
        {
                private readonly IUserService _User = null;
                public OrderService(IUserService user, IRpcTranCollect tran)
                {
                        this._User = user;
                }
                #region 静态属性和方法
                private static OrderInfo[] _Orders = new OrderInfo[0];
                private static readonly ReadWriteLockHelper _Lock = new ReadWriteLockHelper();

                static OrderService()
                {
                        //手动注册路由
                        RpcClient.RpcClient.Route.AddRoute<DropOrderMsg>("DropOrderMsg", _Drop);
                }

                private static void _Drop(DropOrderMsg obj)
                {
                        using (_Lock.Write)
                        {
                                if (_Lock.Write.GetLock())
                                {
                                        _Orders = _Orders.Remove(a => a.Id == obj.OrderId);
                                }
                        }
                }
                #endregion
                public OrderInfo GetOrder(Guid orderId)
                {
                        OrderInfo order = null;
                        using (_Lock.Read)
                        {
                                if (_Lock.Read.GetLock())
                                {
                                        order = _Orders.Find(a => a.Id == orderId);
                                }
                        }
                        if (order == null)
                        {
                                throw new ErrorException("demo.order.not.find");
                        }
                        return order;
                }
                public OrderInfo[] Query(long userId, IBasicPage paging, out long count)
                {
                        using (_Lock.Read)
                        {
                                if (_Lock.Read.GetLock())
                                {
                                        count = _Orders.LongCount(a => a.UserId == userId);
                                        return _Orders.Where(a => a.UserId == userId).OrderBy(a => a.Id).Skip((paging.Index - 1) * paging.Size).Take(paging.Size).ToArray();
                                }
                                throw new ErrorException("demo.get.lock.overtime");
                        }
                }
                public void DropOrder(Guid id)
                {
                        if (!_Orders.IsExists(a => a.Id == id))
                        {
                                throw new ErrorException("demo.order.not.find");
                        }
                        using (_Lock.Write)
                        {
                                if (_Lock.Write.GetLock())
                                {
                                        _Orders = _Orders.Remove(a => a.Id == id);
                                }
                        }
                        //广播删除订单消息
                        new DropOrderMsg { OrderId = id }.Send();
                }
                public bool AddOrder(OrderAddModel order, out Guid orderId, out string error)
                {
                        if (_Orders.IsExists(a => a.OrderNo == order.OrderNo))
                        {
                                orderId = Guid.Empty;
                                error = "demo.orderNo.repeat";
                                return false;
                        }
                        UserDatum user = this._User.GetUser(order.UserId);
                        OrderInfo add = order.ConvertMap<OrderAddModel, OrderInfo>();
                        add.Id = Tools.NewGuid();
                        add.UserName = user.UserName;
                        add.UserPhone = user.UserPhone;
                        add.AddTime = DateTime.Now;
                        using (IRpcTransaction tran = new RpcTransaction("CreateOrder"))
                        {
                                using (_Lock.Write)
                                {
                                        if (_Lock.Write.GetLock())
                                        {
                                                _Orders = _Orders.Add(add);
                                        }
                                }
                                tran.SetExtend(add.Id.ToString());
                                this._User.SetUserOrderNo(order.UserId, order.OrderNo);
                                tran.Complate();
                        }
                        orderId = add.Id;
                        //发布订单添加事件
                        new AddOrderMsg
                        {
                                Order = add.ConvertMap<OrderInfo, OrderData>()
                        }.Send();
                        new AddOrderEvent
                        {
                                SerialNo = add.OrderNo,
                                UserId = add.UserId
                        }.AsyncSend();
                        error = null;
                        return true;
                }

                public void AddOrder(OrderInfo order)
                {
                        using (_Lock.Write)
                        {
                                if (_Lock.Write.GetLock())
                                {
                                        if (!_Orders.IsExists(a => a.Id == order.Id))
                                        {
                                                _Orders = _Orders.Add(order);
                                        }
                                }
                        }
                }

                public bool DropOrder(string orderNo)
                {
                        OrderInfo order = _Orders.Find(a => a.OrderNo == orderNo);
                        if (order == null)
                        {
                                return false;
                        }
                        using (_Lock.Write)
                        {
                                if (_Lock.Write.GetLock())
                                {
                                        _Orders = _Orders.Remove(a => a.Id == order.Id);
                                }
                        }
                        //广播删除订单消息
                        new DropOrderMsg { OrderId = order.Id }.Send();
                        return true;
                }
        }
}
