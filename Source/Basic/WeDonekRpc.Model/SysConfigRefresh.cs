namespace WeDonekRpc.Model
{
    [IRemoteConfig("RefreshSysConfig", false, true)]
    public class SysConfigRefresh
    {
        public string ConfigMd5
        {
            get;
            set;
        }
    }
}
