using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        public class ServerRegion
        {
                /// <summary>
                /// 区域Id
                /// </summary>
                [NumValidate("rpc.region.id.error", 1)]
                public int Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 区域名称
                /// </summary>
                [NullValidate("rpc.region.name.null")]
                [LenValidate("rpc.region.name.len", 2, 50)]
                public string RegionName
                {
                        get;
                        set;
                }
        }
}
