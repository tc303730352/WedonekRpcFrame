namespace WeDonekRpc.HttpApiDoc.Error
{
    internal class OtherError : ProErrorMsg
    {
        public OtherError () { }
        public OtherError (string error, string show) : base(error)
        {
            this.Show = show;
        }

        public string Show
        {
            get;
            set;
        }
    }
}
