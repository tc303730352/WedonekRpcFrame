using RpcClient.Attr;

namespace RpcClient.Interface
{
        /// <summary>
        /// 扩展服务
        /// </summary>
        [ClassLifetimeAttr(ClassLifetimeType.单例)]
        public interface IExtendService
        {
                ///<summary>
                /// 名称
                /// </summary>
                string Name
                {
                        get;
                }
                /// <summary>
                /// 加载事件
                /// </summary>
                /// <param name="service"></param>
                void Load(IRpcService service);
        }
}
