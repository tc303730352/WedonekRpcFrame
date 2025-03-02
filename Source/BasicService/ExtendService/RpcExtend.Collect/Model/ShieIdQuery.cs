using WeDonekRpc.Model;

namespace RpcExtend.Collect.Model
{
    public class ShieIdQuery
    {
        public long RpcMerId
        {
            get;
            set;
        }
        public string SystemType
        {
            get;
            set;
        }
        public long ServerId
        {
            get;
            set;
        }
        public int VerNum
        {
            get;
            set;
        }
        public ShieldType ShieIdType { get; set; }
    }
}
