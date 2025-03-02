namespace WeDonekRpc.Model.ErrorManage
{
    [IRemoteConfig("GetErrorMsg", "sys.sync", IsProhibitTrace = true)]
    public class GetErrorMsg
    {
        public string ErrorCode
        {
            get;
            set;
        }
        public string Lang { get; set; }
    }
}
