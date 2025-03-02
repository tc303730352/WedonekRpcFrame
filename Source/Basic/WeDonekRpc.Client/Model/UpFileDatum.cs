using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
    public class UpFileDatum<T>
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public T Param { get; set; }
        public MsgSource Source { get; set; }
    }
}
