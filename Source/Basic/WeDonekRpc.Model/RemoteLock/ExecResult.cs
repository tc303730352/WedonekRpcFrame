namespace WeDonekRpc.Model.RemoteLock
{
    public class ExecResult : BasicRes
    {
        public string Extend
        {
            get;
            set;
        }
        public bool IsReset
        {
            get;
            set;
        }
    }
}
