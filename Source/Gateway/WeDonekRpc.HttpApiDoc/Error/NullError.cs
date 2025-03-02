namespace WeDonekRpc.HttpApiDoc.Error
{
    internal class NullError : ProErrorMsg
    {
        public NullError()
        {
            this.IsAllowNull = true;
        }
        public NullError(string errorCode, string show) : base(errorCode)
        {
            this.IsAllowNull = false;
            this.Show = show;
        }
        public bool IsAllowNull
        {
            get;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string Show
        {
            get;
            set;
        }
    }
}
