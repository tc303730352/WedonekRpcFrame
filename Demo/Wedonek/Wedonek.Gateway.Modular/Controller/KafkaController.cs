using Wedonek.Gateway.Modular.Interface;
using WeDonekRpc.HttpApiGateway;

namespace Wedonek.Gateway.Modular.Controller
{
    internal class KafkaController : ApiController
    {
        private readonly IKafkaDemo _Kafka;

        public KafkaController (IKafkaDemo demo)
        {
            this._Kafka = demo;
        }
        /// <summary>
        /// 发布
        /// </summary>
        public void Public ()
        {
            this._Kafka.Producer();
        }
        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe ()
        {
            this._Kafka.Subscribe();
        }
    }
}
