using System;
using System.IO;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;
namespace WeDonekRpc.HttpApiGateway.FileUp
{
    public enum UpTaskState
    {
        初始化 = 0,
        待上传 = 1,
        校验中 = 2,
        上传完成 = 3,
        上传失败 = 4
    }
    /// <summary>
    /// 分块任务
    /// </summary>
    internal class BlockUpTask : IBlockUpTask
    {
        private string _ErrorCode;

        private volatile UpTaskState _UpState = UpTaskState.初始化;

        private object _UpResult;

        private readonly string _ApiName;

        public BlockUpTask ( string serviceName, string apiName )
        {
            this._ApiName = apiName;
            this.TaskId = Guid.NewGuid().ToString("N");
            this.ServerName = serviceName;
        }
        public int TimeOut
        {
            get;
            private set;
        }

        public string TaskId
        {
            get;
        }

        public string TaskKey
        {
            get;
            private set;
        }

        public string ServerName { get; }

        public UpTaskState UpState => this._UpState;

        /// <summary>
        /// 文件名
        /// </summary>
        public UpBasicFile File { get; private set; }

        private IBlockUpFile _UpFile;

        public void BeginUp ( UpBasicFile file )
        {
            this.TaskKey = file.TaskKey;
            this._UpState = UpTaskState.待上传;
            BlockUpCollect.Sync(this.TaskKey, this.TaskId);
            this.File = file;
            this._UpFile = BlockUpFileCollect.Create(file);
            this._UpFile.Load(this._complete);
        }
        private void _complete ( BlockUpState state, string error )
        {
            if ( state == BlockUpState.校验中 )
            {
                this._UpState = UpTaskState.校验中;
                return;
            }
            using ( IocScope scope = RpcClient.Ioc.CreateScore() )
            {;
                IUpBlockFileTask api = scope.Resolve<IUpBlockFileTask>(this._ApiName);
                if ( state == BlockUpState.上传失败 )
                {
                    this._UpFail(api, error);
                }
                else
                {
                    this._UpSuccess(api);
                }
            }
        }
        private void _UpSuccess ( IUpBlockFileTask api )
        {
            IUpFileResult result = new UpFileResult(this);
            IUpFile upFile = new UpBlockFile(this.File, this._UpFile);
            try
            {
                api.Complete(result, upFile);
            }
            catch ( ErrorException ex )
            {
                this.UpError(ex.ErrorCode);
            }
            catch ( Exception e )
            {
                this.UpError("public.system.error");
                ErrorException.FormatError(e).Save("gateway");
            }
        }

        private void _UpFail ( IUpBlockFileTask api, string error )
        {
            this.UpError(error);
            try
            {
                api.UpFail(this.File, error);
            }
            catch ( Exception e )
            {
                ErrorException.FormatError(e).Save("gateway");
            }
        }
       
        /// <summary>
        /// 获取上传状态
        /// </summary>
        /// <returns></returns>
        public BlockUpSate GetUpState ()
        {
            if ( this._UpState == UpTaskState.上传失败 )
            {
                throw new ErrorException(this._ErrorCode);
            }
            else if ( this._UpState == UpTaskState.上传完成 )
            {
                return new BlockUpSate
                {
                    TaskId = this.TaskId,
                    Result = this._UpResult,
                    UpState = BlockUpState.上传完成
                };
            }
            else if ( this._UpState == UpTaskState.待上传 )
            {
                return new BlockUpSate
                {
                    TaskId = this.TaskId,
                    Block = this._UpFile.GetBlock(),
                    UpState = this._UpFile.UpState
                };
            }
            else
            {
                return new BlockUpSate
                {
                    TaskId = this.TaskId,
                    UpState = this._UpFile.UpState
                };
            }
        }
        public Stream GetStream ( int index )
        {
            return this._UpFile.GetStream(index);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="index"></param>
        public bool WriteUpFile ( IUpFile file, int index )
        {
            return this._UpFile.WriteUpFile(file, index);
        }

        public void UpComplate<T> ( T result )
        {
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
            this._UpResult = result;
            this._UpState = UpTaskState.上传完成;
        }

        public void UpError ( string error )
        {
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
            this._ErrorCode = error;
            this._UpState = UpTaskState.上传失败;
        }

        public bool CheckIsUp ( int index )
        {
            return this._UpFile.CheckIsUp(index);
        }
    }
}
