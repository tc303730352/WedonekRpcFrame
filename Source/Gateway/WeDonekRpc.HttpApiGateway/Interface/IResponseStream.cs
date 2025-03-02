using System.IO;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IResponseStream
    {
        bool IsExists { get; }
        FileInfo File { get; }
        public string Extension { get; }
        string FileName { get; }
        Stream Open();
    }
}