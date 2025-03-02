namespace WeDonekRpc.Model.ErrorManage
{
    [IRemoteConfig("FindError", "sys.sync", IsProhibitTrace = true)]
    public class GetErrorCode
    {
        public long ErrorId
        {
            get;
            set;
        }
    }
}
