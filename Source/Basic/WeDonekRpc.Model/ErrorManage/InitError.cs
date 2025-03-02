namespace WeDonekRpc.Model.ErrorManage
{
    [IRemoteConfig("InitError", "sys.sync", isReply: false, IsProhibitTrace = true)]
    public class InitError
    {
        public long ErrorId
        {
            get;
            set;
        }
        public string ErrorCode { get; set; }
    }
}
