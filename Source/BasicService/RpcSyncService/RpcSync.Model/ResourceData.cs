namespace RpcSync.Model
{
    public class ResourceData
    {
        public long Id { get; set; }

        public string ResourcePath { get; set; }

        public string FullPath { get; set; }

        public string ResourceShow { get; set; }

        public ResourceState ResourceState { get; set; }

        public long VerNum { get; set; }

        public int ResourceVer { get; set; }
    }
}
