using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Interface;

namespace AutoTask.Gateway
{
    public class AutoTaskApiModular : BasicApiModular
    {
        public AutoTaskApiModular () : base("AutoTask_Modular", "自动任务服务")
        {
        }
        /// <summary>
        /// 模块加载事件(启动前)
        /// </summary>
        /// <param name="option">加载项</param>
        /// <param name="config">配置信息</param>
        protected override void Load (IHttpGatewayOption option, IModularConfig config)
        {
            //设置默认生命周期的事件方法修改即将注册的关系结构
            option.IocBuffer.SetDefLifetimeType((body) =>
            {
                if (body.To.FullName.StartsWith("AutoTask.Gateway.Interface."))
                {
                    body.SetLifetimeType(ClassLifetimeType.SingleInstance);
                }
            });
        }
    }
}
