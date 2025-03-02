namespace WeDonekRpc.Model.ErrorManage
{
    [IRemoteConfig("GetErrorId", "sys.sync", true, IsProhibitTrace = true)]
    public class GetErrorId
    {
        public string ErrorCode
        {
            get;
            set;
        }
    }
}
