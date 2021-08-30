using System.IO;

namespace SocketTcpClient.UpFile.Model
{
        public class FileUpResult
        {
                internal FileUpResult(byte[] myByte)
                {
                        this._Result = myByte;
                }
                public FileInfo File { get; internal set; }
                public object UpParam { get; internal set; }
                public int ConsumeTime { get; internal set; }
                public bool IsSuccess { get; internal set; }
                public string Error { get; internal set; }
                private readonly byte[] _Result = null;

                public byte[] GetByte()
                {
                        return this._Result;
                }
                public T GetObject<T>()
                {
                        return SocketTools.DeserializeData<T>(this._Result);
                }
                public string GetString()
                {
                        return SocketTools.DeserializeStringData(this._Result);
                }
                protected T GetValue<T>() where T : struct
                {
                        return this._Result == null ? default : (T)SocketTools.GetValue(typeof(T), this._Result);
                }
        }
}
