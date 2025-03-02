using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcSync.Collect.Model;
using RpcSync.Service.Broadcast.Filter;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Broadcast
{
    /// <summary>
    /// 初始化广播
    /// </summary>
    internal class LoadBroadcast : ILoadBroadcast
    {
        private static readonly IBroadcastFilter[] _Filters = null;

        static LoadBroadcast()
        {
            _Filters = new IBroadcastFilter[]
            {
                new FilterALLMerNode(),
                new FilterALLMerOnlyNode(),
                new FilterALLNode(),
                new FilterALLOnlyNode(),
                new FilterMerNode(),
                new FilterMerOnlyNode(),
                new FilterRootNode(),
                new FilterRootOnlyNode(),
                new FilterServerNode()
            };
        }
        private readonly ITrackCollect _Track = null;
        private IIocService _Unity;
        public LoadBroadcast(ITrackCollect track, IIocService unity)
        {
            _Track = track;
            _Unity = unity;

        }

        public BroadcastBody InitBroadcastBody(BroadcastMsg msg, MsgSource source)
        {
            BroadcastBody body = new BroadcastBody
            {
                IsExclude = msg.IsExclude,
                Track = _Track.TrackSpan,
                Datum = new BroadcastDatum
                {
                    MsgBody = msg.MsgBody,
                    MsgConfig = msg.MsgConfig,
                    RegionId = msg.RegionId,
                    RpcMerId = msg.RpcMerId,
                    MsgKey = msg.MsgKey
                },
                Source = source
            };
            IBroadcastFilter filter = _Filters.Find(a => a.CheckIsUsable(msg));
            IInitBroadcast init = _Unity.Resolve<IInitBroadcast>(filter.FilterName);
            init.InitBroadcastBody(msg, source, ref body);
            return body;
        }
    }
}
