using System;

namespace Wedonek.Demo.RemoteModel.Order.Model
{
        /// <summary>
        /// 订单信息
        /// </summary>
        public class OrderData
        {
                /// <summary>
                /// 订单ID
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 订单号
                /// </summary>
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
                /// 用户名
                /// </summary>
                public string UserName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 用户手机
                /// </summary>
                public string UserPhone
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
                /// 订单价格
                /// </summary>
                public int OrderPrice
                {
                        get;
                        set;
                }
                /// <summary>
                /// 订单添加时间
                /// </summary>
                public DateTime AddTime
                {
                        get;
                        set;
                }
        }
}
