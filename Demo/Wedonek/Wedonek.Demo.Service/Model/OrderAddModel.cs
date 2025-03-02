namespace Wedonek.Demo.Service.Model
{
    public class OrderAddModel
    {
        /// <summary>
        /// 下单用户ID
        /// </summary>
        public long UserId
        {
            get;
            set;
        }

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
    }
}
