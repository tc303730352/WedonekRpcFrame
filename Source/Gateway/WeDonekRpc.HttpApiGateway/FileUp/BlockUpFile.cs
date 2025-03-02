using System.IO;
using System.Threading.Tasks;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    public enum BlockUpState
    {
        初始化 = 0,
        待上传 = 2,
        准备中 = 1,
        校验中 = 3,
        上传完成 = 4,
        上传失败 = 5
    }
    public delegate void UpComplete ( BlockUpState state, string error );
    internal class BlockUpFile : IBlockUpFile
    {
        private string _SaveDir;

        private FileInfo _StateFile;

        private FileStream _StateFileStream;

        private event UpComplete _UpEv;

        private readonly IGatewayUpConfig _Config;

        private FileInfo _UpFile;

        private volatile BlockUpState _UpState = BlockUpState.初始化;

        private readonly UpFileForm _File;

        public BlockUpFile ( UpBasicFile file, IGatewayUpConfig config )
        {
            this.FileMd5 = file.FileMd5;
            this._Config = config;
            this._File = new UpFileForm
            {
                extension = Path.GetExtension(file.FileName),
                alreadyUp = 0,
                blockSize = this._Config.BlockUpSize,
                fileSize = file.FileSize
            };
        }

        public string FileMd5
        {
            get;
        }

        public BlockUpState UpState => this._UpState;

        public int TimeOut { get; private set; } = HeartbeatTimeHelper.HeartbeatTime;

        public BlockDatum GetBlock ()
        {
            return new BlockDatum
            {
                FileSize = this._File.fileSize,
                AlreadyUp = this._File.alreadyUp,
                BlockSize = this._File.blockSize,
                NoUpIndex = this._File.GetUpIndex(this._Config.UpBlockNum),
            };
        }
        public void Load ( UpComplete complete )
        {
            _UpEv += complete;
            if ( this._UpState != BlockUpState.初始化 )
            {
                return;
            }
            this._UpState = BlockUpState.准备中;
            _ = Task.Factory.StartNew(this._Load);
        }
        private void _Load ()
        {
            string zIndex = "ZoneIndex_" + Tools.GetZoneIndex(this.FileMd5);
            this._SaveDir = Path.Combine(HttpService.HttpService.Config.File.TempDirPath, "BlockUp", zIndex);
            if ( !Directory.Exists(this._SaveDir) )
            {
                _ = Directory.CreateDirectory(this._SaveDir);
            }
            this._LoadStateFile();
            this._InitFile();
            if ( this._File.CheckIsComplate() )
            {
                this._UpComplate();
            }
            else
            {
                this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
                this._UpState = BlockUpState.待上传;
            }
        }

        /// <summary>
        /// 初始化临时文件
        /// </summary>
        private void _InitFile ()
        {
            string name = string.Concat(this.FileMd5, this._File.extension + ".tmp");
            string path = Path.Combine(this._SaveDir, name);
            this._UpFile = new FileInfo(path);
            if ( !this._UpFile.Exists )
            {
                using ( FileStream stream = this._UpFile.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite) )
                {
                    stream.SetLength(this._File.fileSize);
                }
                this._UpFile.Refresh();
                if ( this._File.alreadyUp != 0 )
                {
                    this._File.Reset(this._StateFileStream);
                }
            }
        }

        public Stream GetStream ( int index )
        {
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
            return new AgentStream(this._UpFile, index * this._File.blockSize);
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="index"></param>
        public bool WriteUpFile ( IUpFile file, int index )
        {
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
            bool? res = this._File.WriteAlreadyUp(this._StateFileStream, index, (int)file.FileSize);
            if ( res.HasValue == false )
            {
                return false;
            }
            else if ( res.Value )
            {
                this._UpComplate();
            }
            return true;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        private void _UpComplate ()
        {
            this._UpState = BlockUpState.校验中;
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime;
            this._UpEv(this._UpState, null);
            _ = Task.Factory.StartNew(this._UpCheck);
            this.Dispose();
        }
        private void _UpCheck ()
        {
            string md5 = Tools.GetFileMd5(this._UpFile).ToLower();
            if ( md5 != this.FileMd5 )
            {
                this._UpError("gateway.http.up.file.error");
                return;
            }
            this._UpState = BlockUpState.上传完成;
            this._UpEv(this._UpState, null);
        }
        /// <summary>
        /// 加载状态文件
        /// </summary>
        private void _LoadStateFile ()
        {
            string name = string.Concat(this.FileMd5, this._File.extension + ".upState");
            this._StateFile = new FileInfo(Path.Combine(this._SaveDir, name));
            if ( !this._StateFile.Exists )
            {
                this._createTempFile();
            }
            else
            {
                this._initTempFile();
            }
        }
        private void _initTempFile ()
        {
            this._StateFileStream = this._StateFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            this._File.Init(this._StateFileStream);
        }
        private void _createTempFile ()
        {
            if ( !this._StateFile.Directory.Exists )
            {
                this._StateFile.Directory.Create();
            }
            this._StateFileStream = this._StateFile.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            this._File.Create(this._StateFileStream);
        }

        private void _UpError ( string error )
        {
            this._UpState = BlockUpState.上传失败;
            this._UpEv(this._UpState, error);
            this.Dispose();
        }

        public bool CheckIsUp ( int index )
        {
            return this._File.CheckIsUp(index);
        }

        public void Dispose ()
        {
            if ( this._StateFileStream != null )
            {
                this._StateFileStream.Close();
                this._StateFileStream.Dispose();
            }
        }

        public void UpTimeOut ()
        {
            this._UpError("gateway.file.up.timeout");
        }

        public string Save ( string path, bool overwrite = true )
        {
            if ( !this._UpFile.Exists )
            {
                throw new ErrorException("gateway.file.not.exists");
            }
            path = HttpService.HttpService.GetFileSavePath(path);
            FileInfo file = new FileInfo(path);
            if ( file.Exists && overwrite == false )
            {
                throw new ErrorException("http.up.file.already.exists");
            }
            else if ( !file.Directory.Exists )
            {
                file.Directory.Create();
            }
            this._UpFile.MoveTo(path, overwrite);
            this._UpFile.Refresh();
            return path;
        }


        public Stream GetStream ()
        {
            if ( !_UpFile.Exists )
            {
                throw new ErrorException("http.up.file.not.exists");
            }
            return _UpFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        public byte[] GetBytes ()
        {
            if ( !_UpFile.Exists )
            {
                throw new ErrorException("http.up.file.not.exists");
            }
            using ( Stream file = this.GetStream() )
            {
                return file.ToBytes();
            }
        }
        public long CopyStream ( Stream stream, int offset )
        {
            using ( Stream file = this.GetStream() )
            {
                file.Position = 0;
                stream.Position = offset;
                file.CopyTo(stream);
            }
            return this._UpFile.Length;
        }
    }
}
