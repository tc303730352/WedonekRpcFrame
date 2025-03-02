namespace RpcSync.Service.Model
{
    public struct SyncAccredit
    {
        public string ApplyKey
        {
            set;
            get;
        }

        public string AccreditId
        {
            get;
            set;
        }
        public DateTime? Expire { get; set; }
        public int StateVer { get; set; }
    }
}
