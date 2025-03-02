using System.IO;
using WeDonekRpc.IOSendInterface;
namespace WeDonekRpc.TcpClient.UpFile.Model
{
    internal class FileUpResult : IFileUpResult
    {
        internal FileUpResult (byte[] myByte)
        {
            this._Result = myByte;
        }
        public FileInfo File { get; internal set; }
        public object UpParam { get; internal set; }
        public int ConsumeTime { get; internal set; }
        public bool IsSuccess { get; internal set; }
        public string Error { get; internal set; }
        private readonly byte[] _Result = null;

        public byte[] GetByte ()
        {
            return this._Result;
        }
        public T GetObject<T> ()
        {
            return ToolsHelper.DeserializeData<T>(this._Result);
        }
        public string GetString ()
        {
            return ToolsHelper.DeserializeStringData(this._Result);
        }
        protected T GetValue<T> () where T : struct
        {
            return this._Result == null ? default : (T)ToolsHelper.GetValue(typeof(T), this._Result);
        }
    }
}
