using RpcModel;
using System;

namespace Wedonek.Demo.RemoteModel.Order
{
        /// <summary>
        /// 添加订单
        /// </summary>
        [IRemoteConfig("demo.order",LockType = RemoteLockType.同步锁)]
        public class AddOrder : RpcClient.RpcRemote<Guid>
        {
                /// <summary>
                /// 订单号
                /// </summary>
                [RemoteLockAttr]
                public string OrderNo
                {
                        get;
                        set;
                }
                /// <summary>
                /// 下单用户ID
                /// </summary>
                public long UserId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 订单标题
                /// </summary>
                public string OrderTitle
                {
                        get;
                        set;
                }
                /// <summary>
                /// 价格
                /// </summary>
                public int OrderPrice
                {
                        get;
                        set;
                }
        }
}
