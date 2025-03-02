namespace WeDonekRpc.HttpService.Model
{
    internal class FilePage
    {
        public string type;
        public string boundary;
        public BranchPage[] pages;

        public byte[] end;
        internal long len;

        public long Size { get; internal set; }
    }
}
