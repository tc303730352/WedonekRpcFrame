namespace WeDonekRpc.Model.Config
{
    [IRemoteConfig("GetSysConfigVal", "sys.sync")]
    public class GetSysConfigVal
    {
        public string Name
        {
            get;
            set;
        }
    }
}
