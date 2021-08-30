using RpcModel;

namespace RpcSyncService.Model
{
        internal struct SyncAccredit
        {
                public string ApplyKey
                {
                        set;
                        get;
                }

                public string AccreditId
                {
                        get;
                        set;
                }
                public MsgSource Source { get; internal set; }
        }
}
