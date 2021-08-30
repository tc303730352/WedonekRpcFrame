
using RpcClient;

using RpcModel;

using RpcModularModel.Resource.Model;

namespace RpcModularModel.Resource
{
        [IRemoteConfig("sys.sync")]
        public class SyncResource : RpcRemote
        {
                public string ModularName
                {
                        get;
                        set;
                }

                /// <summary>
                /// 资源类型
                /// </summary>
                public ResourceType ResourceType
                {
                        get;
                        set;
                }
                /// <summary>
                /// 资源列表
                /// </summary>
                public ResourceDatum[] Resources
                {
                        get;
                        set;
                }
                public string ModularVer { get; set; }
        }
}
