using RpcModel;
using System;

namespace Wedonek.Demo.RemoteModel.Order.Msg
{
        /// <summary>
        /// 向其它订单服务广播删除订单
        /// </summary>
        [IRemoteBroadcast("DropOrderMsg", false, "demo.order")]
        public class DropOrderMsg : RpcClient.RpcBroadcast
        {
                public Guid OrderId
                {
                        get;
                        set;
                }
        }
}
