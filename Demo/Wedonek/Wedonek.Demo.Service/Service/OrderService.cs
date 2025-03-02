using System;
using Wedonek.Demo.RemoteModel.Order.Model;
using Wedonek.Demo.RemoteModel.Order.Msg;
using Wedonek.Demo.RemoteModel.User.Model;
using Wedonek.Demo.Service.DB;
using Wedonek.Demo.Service.Interface;
using Wedonek.Demo.Service.LocalEvent.Event;
using Wedonek.Demo.Service.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace Wedonek.Demo.Service.Service
{
    internal class OrderService : IOrderService
    {
        private readonly IRepository<DB.OrderDB> _Repository;
        private readonly IUserService _User = null;
        private readonly IRpcTranService _RpcTran;

        public OrderService ( IRepository<OrderDB> repository,
            IUserService user,
            IRpcTranService rpcTran )
        {
            this._RpcTran = rpcTran;
            this._User = user;
            this._Repository = repository;
        }

        public long AddOrder ( OrderAddModel order )
        {
            if ( this._Repository.IsExist(a => a.OrderNo == order.OrderNo) )
            {
                throw new ErrorException("demo.orderNo.repeat");
            }
            UserDatum user = this._User.GetUser(order.UserId);
            OrderDB add = order.ConvertMap<OrderAddModel, OrderDB>();
            add.Id = IdentityHelper.CreateId();
            add.UserName = user.UserName;
            add.UserPhone = user.UserPhone;
            add.AddTime = DateTime.Now;
            this._Repository.Insert(add);
            this._RpcTran.SetTranExtend(add.Id.ToString());
            this._User.SetUserOrderNo(order.UserId, order.OrderNo);
            //发布订单添加事件
            new AddOrderMsg
            {
                Order = add.ConvertMap<OrderDB, OrderData>()
            }.Send();
            //本地消息事件
            new LocalOrderEvent
            {
                SerialNo = add.OrderNo,
                UserId = add.UserId
            }.Send("Add");
            return add.Id;
        }
    }
}
