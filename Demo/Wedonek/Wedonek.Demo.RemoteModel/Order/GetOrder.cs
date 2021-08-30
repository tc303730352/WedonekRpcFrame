using RpcModel;
using System;

namespace Wedonek.Demo.RemoteModel.Order
{
        /// <summary>
        /// 查询订单
        /// </summary>
        [IRemoteConfig("demo.order")]
        public  class GetOrder:RpcClient.RpcRemote<Model.OrderData>
        {
                /// <summary>
                /// 订单号
                /// </summary>
                public Guid OrderId { get; set; }
        }
}
