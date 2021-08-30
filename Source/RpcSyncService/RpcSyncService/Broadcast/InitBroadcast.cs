using RpcClient.Interface;

using RpcModel;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        /// <summary>
        /// 初始化广播
        /// </summary>
        internal class InitBroadcast
        {
                private static readonly IInitBroadcast[] _InitList = null;
                private static readonly ITrackCollect _Track = null;
                static InitBroadcast()
                {
                        _Track = RpcClient.RpcClient.Unity.Resolve<ITrackCollect>();
                        _InitList = new IInitBroadcast[]
                        {
                                new LoadALLMerNode(),
                                new LoadALLMerOnlyNode(),
                                new LoadALLNode(),
                                new LoadALLOnlyNode(),
                                new LoadMerNode(),
                                new LoadMerOnlyNode(),
                                new LoadNode(),
                                new LoadOnlyNode(),
                                new LoadServerNode(),
                        };
                }

                public static bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, out BroadcastBody body, out string error)
                {
                        body = new BroadcastBody
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
                        IInitBroadcast init = _InitList.Find(a => a.CheckIsUsable(msg));
                        return init.InitBroadcastBody(msg, source, ref body, out error);
                }
        }
}
