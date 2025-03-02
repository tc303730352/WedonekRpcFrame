using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.FileUp;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class BlockUpFileCollect
    {
        private static readonly ConcurrentDictionary<string, IBlockUpFile> _UpFileList = new ConcurrentDictionary<string, IBlockUpFile>();
        private static readonly Timer _Timer = null;
        private static readonly IGatewayUpConfig _upConfig;
        static BlockUpFileCollect ()
        {
            _upConfig = RpcClient.Ioc.Resolve<IGatewayUpConfig>();
            _Timer = new Timer(_Clear, null, 10000, 10000);
        }
        private static void _Clear ( object state )
        {
            if ( _UpFileList.IsEmpty )
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime - _upConfig.BlockUpOverTime;
            string[] keys = _UpFileList.Where(c => c.Value.TimeOut <= time).Select(c => c.Key).ToArray();
            if ( keys.IsNull() )
            {
                return;
            }
            keys.ForEach(a =>
            {
                if ( _UpFileList.TryRemove(a, out IBlockUpFile task) )
                {
                    task.UpTimeOut();
                }
            });
        }
        public static IBlockUpFile Create ( UpBasicFile file )
        {
            if ( !_UpFileList.TryGetValue(file.FileMd5, out IBlockUpFile upFile) )
            {
                upFile = _UpFileList.GetOrAdd(file.FileMd5, new BlockUpFile(file, _upConfig));
            }
            return upFile;
        }

    }
}
