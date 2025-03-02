namespace WeDonekRpc.Model
{
    [IRemoteConfig("sys.sync")]
    public class GetIdgeneratorWorkId
    {
        public string Mac
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }
    }
}
