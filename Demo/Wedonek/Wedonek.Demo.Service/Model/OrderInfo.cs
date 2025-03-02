using System;

namespace Wedonek.Demo.Service.Model
{
    public class OrderInfo
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
        /// 订单编号
        /// </summary>
        public string OrderNo
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
        public int OrderPrice
        {
            get;
            set;
        }
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
