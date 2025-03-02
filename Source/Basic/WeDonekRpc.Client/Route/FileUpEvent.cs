using WeDonekRpc.Client.FileUp;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Route
{
    /// <summary>
    /// 文件上传事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Result"></typeparam>
    [Attr.IgnoreIoc]
    public class FileUpEvent<T, Result> : IFileUpEvent<Result> where T : class
    {

        private UpFileDatum<T> _GetFileDatum(IUpFileInfo file)
        {
            TcpRemoteMsg msg = file.GetData<TcpRemoteMsg>();
            return new UpFileDatum<T>
            {
                FileName = file.FileName,
                FileSize = file.FileSize,
                Param = msg.MsgBody.Json<T>(),
                Source = msg.Source
            };
        }
        public void CheckFile(IUpFileInfo file)
        {
            UpFileDatum<T> datum = this._GetFileDatum(file);
            this.CheckFile(datum);
        }
        public Result UpComplate(IUpFileInfo file, IUpResult result)
        {
            UpFileDatum<T> datum = this._GetFileDatum(file);
            return this.UpComplate(datum, result);
        }

        public string GetFileSaveDir(IUpFileInfo file)
        {
            UpFileDatum<T> datum = this._GetFileDatum(file);
            return this.GetFileSaveDir(datum);
        }
        protected virtual Result UpComplate(UpFileDatum<T> file, IUpResult result)
        {
            return default;
        }

        protected virtual void CheckFile(UpFileDatum<T> file)
        {
        }
        protected virtual string GetFileSaveDir(UpFileDatum<T> file)
        {
            return null;
        }

    }
}
