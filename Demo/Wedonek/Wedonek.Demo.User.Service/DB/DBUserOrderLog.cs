using System;
using SqlSugar;

namespace Wedonek.Demo.User.Service.DB
{
    [SugarTable("UserOrderLog")]
    public class DBUserOrderLog
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

        public long OrderId
        {
            get;
            set;
        }
        public int OrderPrice
        {
            get;
            set;
        }
        public DateTime AddTime { get; set; }
    }
}
