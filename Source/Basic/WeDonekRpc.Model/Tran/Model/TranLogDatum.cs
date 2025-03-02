namespace WeDonekRpc.Model.Tran.Model
{
    public class TranLogDatum
    {
        public string Dictate
        {
            get;
            set;
        }
        public RpcTranMode TranMode
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public long ServerId
        {
            get;
            set;
        }
        public string SystemType
        {
            get;
            set;
        }
        public string SubmitJson
        {
            get;
            set;
        }
    }
}
