namespace WeDonekRpc.Model
{
    public class BasicRes : IBasicRes
    {
        public BasicRes ()
        {

        }
        public BasicRes (string error)
        {
            this.IsError = true;
            this.ErrorMsg = error;
        }
        public bool IsError
        {
            get;
            set;
        }
        /// <summary>
        /// 是否错误
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }
    }
}
