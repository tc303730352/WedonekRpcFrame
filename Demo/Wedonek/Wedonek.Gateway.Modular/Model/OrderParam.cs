using WeDonekRpc.Helper.Validate;

namespace Wedonek.Gateway.Modular.Model
{
    public class OrderParam
    {
        /// <summary>
        /// 订单标题
        /// </summary>
        [NullValidate("demo.order.title.null")]
        [LenValidate("demo.order.title.len", 2, 100)]
        public string OrderTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 订单价格
        /// </summary>
        [NumValidate("demo.order.price.error", 1, int.MaxValue)]
        public int OrderPrice
        {
            get;
            set;
        }

    }
}
