namespace WeDonekRpc.HttpService.Model
{
        internal class BranchPage
        {
                public long Begin;
                public long End;

                public long Len { get; set; }
                public byte[] Head { get; set; }
        }
}
