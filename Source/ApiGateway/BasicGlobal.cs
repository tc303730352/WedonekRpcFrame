using ApiGateway.Interface;

namespace ApiGateway
{
        /// <summary>
        /// 全局事件器
        /// </summary>
        public class BasicGlobal : IGlobal
        {
                public virtual void LoadModular(IGatewayService service)
                {
                }

                /// <summary>
                /// 服务关闭事件
                /// </summary>
                public virtual void ServiceClose()
                {
                }
                /// <summary>
                /// 服务启动完成
                /// </summary>
                public virtual void ServiceStarted(IGatewayService service)
                {

                }
                /// <summary>
                /// 服务正在启动
                /// </summary>
                public virtual void ServiceStarting(IGatewayService service)
                {
                }
        }
}
