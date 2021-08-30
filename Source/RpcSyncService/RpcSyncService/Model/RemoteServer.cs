namespace RpcSyncService.Model
{
        internal class RemoteServer
        {
                public long Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 区域Id
                /// </summary>
                public int RegionId { get; set; }
                public long SystemType
                {
                        get;
                        set;
                }
                public long GroupId
                {
                        get;
                        set;
                }
        }
}
