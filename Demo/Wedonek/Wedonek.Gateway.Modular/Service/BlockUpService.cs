using System;
using System.IO;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace Wedonek.Gateway.Modular.Service
{
    internal class BlockUpService : IBlockUpService
    {
        private readonly ICacheController _Cache;

        public BlockUpService ( ICacheController cache )
        {
            this._Cache = cache;
        }

        public void InitTask ( IBlockUp<UpFileArg> task, IUserState state )
        {
            //上传的文件信息
            UpFileData<UpFileArg> file = task.PostData;
            string key = file.FileMd5.ToLower();
            //检查文件是否已上传
            if ( this._Cache.TryGet(key, out BlockUpTask upTask) )
            {
                //已上传 将上传任务设为完成
                task.UpComplate(upTask);
                return;
            }
            //创建任务编号 生成规则： 上传用户和上传的文件唯一
            string taskkey = string.Join("_", "UpFile", state.AccreditId, key);
            //开始上传
            task.BeginUp(taskkey, key);
        }
        public void Complete ( IUpFileResult result, IUpFile file )
        {
            string key = file.FileMd5.ToLower();
            //检查是否是重复上传
            if ( this._Cache.TryGet(key, out BlockUpTask upTask) )
            {
                //已上传 将上传任务设为完成
                result.UpComplate(upTask);
                return;
            }
            upTask = new BlockUpTask
            {
                Complete = DateTime.Now,
                FileMd5 = file.FileMd5,
                FileName = file.FileName
            };
            _ = this._Cache.Set(key, upTask);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpFile", @"BlockUp\" + file.FileMd5 + Path.GetExtension(file.FileName).ToLower());
            _ = file.SaveFile(path, true);
            result.UpComplate(upTask);
        }

        public void UpFail ( UpBasicFile file, string error )
        {
            //上传失败
        }
    }
}
