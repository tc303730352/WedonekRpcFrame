namespace Wedonek.Demo.RemoteModel.User.Model
{
    public class UserDatum
    {
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
        /// 锁定数量
        /// </summary>
        public int LockNum { get; set; }
        /// <summary>
        /// 剩余数
        /// </summary>
        public int SurplusNum { get; set; }

        /// <summary>
        /// 提交的数量
        /// </summary>
        public int SubmitNum
        {
            get;
            set;
        }
        public string OrderNo
        {
            get;
            set;
        }
    }
}
