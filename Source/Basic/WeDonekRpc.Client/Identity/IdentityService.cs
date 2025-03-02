using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Identity
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class IdentityService : IIdentityService
    {
        private static ushort _WorkId;
        private static IdgeneratorConfig _Config;
        private static void _InitWork ()
        {
            if (_WorkId == 0)
            {
                GetIdgeneratorWorkId send = new GetIdgeneratorWorkId
                {
                    Mac = Config.WebConfig.Environment.Mac,
                    Index = Config.WebConfig.BasicConfig.RpcServerIndex
                };
                if (!RemoteCollect.Send(send, out _WorkId, out string error))
                {
                    RpcLogSystem.AddErrorLog("初始化身份标识组件错误!", typeof(IdentityService), new ErrorException(error));
                }
            }
        }
        public static void InitServie ()
        {
            _InitWork();
            _Config = new IdgeneratorConfig(_Init);
        }
        private static void _Init (IdgeneratorConfig config)
        {
            IdentityConfig con = new()
            {
                WorkId = _WorkId,
                SeqBitLength = config.SeqBitLength,
                WorkerIdBitLength = config.WorkerIdBitLength,
                Method = config.Method,
                MaxSeqNumber = config.MaxSeqNumber,
                BaseTime = config.BaseTime,
                MinSeqNumber = config.MinSeqNumber,
                TimestampType = config.TimestampType
            };
            if (config.EnableDataCenter)
            {
                con.DataCenterId = (uint)RpcStateCollect.LocalSource.RegionId;
                con.DataCenterIdBitLength = config.DataCenterIdBitLength;
            }
            IdentityHelper.Init(con);
        }
        public long CreateId ()
        {
            return IdentityHelper.CreateId();
        }

    }
}
