using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class BlockUp : IBlockUp
    {
        private readonly IUpTask _Task;
        public BlockUp ( IUpTask task, UpFileData data )
        {
            this.PostData = data;
            this._Task = task;
        }

        public UpFileData PostData
        {
            get;
        }
        public void BeginUp ( string taskKey, object extend )
        {
            this._Task.BeginUp(new UpBasicFile
            {
                Extend = extend,
                TaskKey = taskKey,
                FileMd5 = this.PostData.FileMd5,
                FileSize = this.PostData.FileSize,
                FileName = this.PostData.FileName
            });
        }

        public void UpComplate<Result> ( Result result )
        {
            this._Task.UpComplate(result);
        }

        public void UpError ( string error )
        {
            this._Task.UpError(error);
        }
    }
    internal class BlockUp<T> : IBlockUp<T>
    {
        private readonly IUpTask _Task;

        public BlockUp ( IUpTask task, UpFileData<T> file )
        {
            this.PostData = file;
            this._Task = task;
        }

        public UpFileData<T> PostData { get; }

        public void BeginUp ( string taskKey, object extend )
        {
            this._Task.BeginUp(new UpBasicFile
            {
                TaskKey = taskKey,
                Extend = extend,
                FileMd5 = this.PostData.FileMd5,
                FileSize = this.PostData.FileSize,
                FileName = this.PostData.FileName,
                Form = this.PostData.Form
            });
        }

        public void UpComplate<Result> ( Result result )
        {
            this._Task.UpComplate(result);
        }
    }
}
