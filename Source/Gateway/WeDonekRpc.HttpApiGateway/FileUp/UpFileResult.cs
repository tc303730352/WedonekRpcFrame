using System;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class UpFileResult : IUpFileResult
    {
        private readonly IBlockUpTask _Task;
        public UpFileResult ( IBlockUpTask task)
        {
            this._Task = task;
        }

        public UpBasicFile File => _Task.File;

        /// <summary>
        /// 上传完成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpComplate<T> ( T result )
        {
            this._Task.UpComplate(result);
        }
    }
}
