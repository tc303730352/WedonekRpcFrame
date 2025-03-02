using System;

using Wedonek.Gateway.Modular.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;

namespace Wedonek.Gateway.Modular.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RabbitmqDemo : IRabbitmqDemo
    {
        private readonly IRabbitmqService _Rabbitmq;
        private readonly string _Exchange = "Rabbitmq_Demo";
        private readonly ISubscribe _Subscribe = null;
        public RabbitmqDemo (IRabbitmqService service)
        {
            this._Rabbitmq = service;
            this._Subscribe = this._Rabbitmq.Subscribe(this._Exchange, this._SubscribeMsg);
        }
        public void Public ()
        {
            this._Rabbitmq.Public("Ok", this._Exchange, "Demo_Msg");
        }

        public void Subscribe ()
        {
            this._Subscribe.BindRoute("Demo_Queue", "Demo_Msg");
        }

        private void _SubscribeMsg (ISubscribe sub, ISubEventArgs arg)
        {
            Console.WriteLine("RoutingKey：" + arg.RoutingKey);
            Console.WriteLine("收到订阅消息：" + arg.GetValue());
        }
        public void Dispose ()
        {
            this._Subscribe?.Dispose();
        }

    }
}
