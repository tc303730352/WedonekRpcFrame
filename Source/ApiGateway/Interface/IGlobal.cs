namespace ApiGateway.Interface
{
        public interface IGlobal
        {
                /// <summary>
                /// 服务启动完成
                /// </summary>
                void ServiceStarted(IGatewayService service);
                /// <summary>
                /// 服务正在启动
                /// </summary>
                void ServiceStarting(IGatewayService service);

                /// <summary>
                /// 服务关闭事件
                /// </summary>
                void ServiceClose();
                void LoadModular(IGatewayService service);
        }
}