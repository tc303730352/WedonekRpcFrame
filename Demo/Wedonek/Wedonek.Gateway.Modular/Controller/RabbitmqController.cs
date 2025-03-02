
using Wedonek.Gateway.Modular.Interface;
using WeDonekRpc.HttpApiGateway;

namespace Wedonek.Gateway.Modular.Controller
{
    /// <summary>
    /// RabbitmqDemo
    /// </summary>
    internal class RabbitmqController : ApiController
    {
        private readonly IRabbitmqDemo _Rabbitmq;

        public RabbitmqController (IRabbitmqDemo demo)
        {
            this._Rabbitmq = demo;
        }
        /// <summary>
        /// 发布
        /// </summary>
        public void Public ()
        {
            this._Rabbitmq.Public();
        }
        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe ()
        {
            this._Rabbitmq.Subscribe();
        }
    }
}
