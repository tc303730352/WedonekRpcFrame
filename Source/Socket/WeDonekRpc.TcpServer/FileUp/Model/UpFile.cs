using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpServer.FileUp.Model
{
        public class UpFile
        {
                internal UpFile(UpFileInfo file)
                {
                        this.FileName = file.FileName;
                        this.FileSize = file.FileSize;
                        this.FileSign = file.FileSign;
                        this.IsMd5 = file.IsMd5;
                        this._Param = file.UpParam;
                }

                /// <summary>
                /// 文件名
                /// </summary>
                public string FileName
                {
                        get;
                }

                /// <summary>
                /// 文件大小
                /// </summary>
                public long FileSize
                {
                        get;
                }
                /// <summary>
                /// 文件校验码
                /// </summary>
                internal string FileSign { get; }
                /// <summary>
                /// 是否是MD5
                /// </summary>
                internal bool IsMd5 { get; }
                private readonly byte[] _Param = null;

                public byte[] GetByte()
                {
                        return this._Param;
                }

                public string GetString()
                {
                        return ToolsHelper.DeserializeStringData(this._Param);
                }
                private object _Data = null;
                public T GetData<T>()
                {
                        if (this._Data != null)
                        {
                                return (T)this._Data;
                        }
                        T data = ToolsHelper.DeserializeData<T>(this._Param);
                        this._Data = data;
                        return data;
                }
                public T GetValue<T>() where T : struct
                {
                        if (this._Param == null)
                        {
                                return default;
                        }
                        return (T)ToolsHelper.GetValue(typeof(T), this._Param);
                }
        }
}
