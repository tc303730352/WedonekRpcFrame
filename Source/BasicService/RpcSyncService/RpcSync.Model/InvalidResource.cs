namespace RpcSync.Model
{
    public class InvalidResource
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public long ModularId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 最大版本号
        /// </summary>
        public int MaxVer
        {
            get;
            set;
        }
        /// <summary>
        /// 最小版本号
        /// </summary>
        public int MinVer
        {
            get;
            set;
        }


        public bool CheckIsInvalid ()
        {
            return this.MaxVer - this.MinVer >= 2;
        }
    }
}
