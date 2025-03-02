using System;

namespace Wedonek.Demo.Service.DB
{
    [SqlSugar.SugarTable("DemoOrder")]
    public class OrderDB
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

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
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public int OrderPrice
        {
            get;
            set;
        }
        public DateTime AddTime { get; set; }

    }
}
