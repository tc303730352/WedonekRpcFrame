using RpcHelper;
using Wedonek.Demo.RemoteModel.Msg;

namespace Wedonek.Demo.Service.Event
{
        /// <summary>
        /// 接收广播消息
        /// </summary>
        internal class MsgEvent : RpcClient.Interface.IRpcApiService
        {

                public void PlaceAnOrder(PlaceAnOrder obj)
                {
                        System.Console.WriteLine("收到了一个下单的消息广播! ");
                        System.Console.WriteLine(string.Concat("order:", obj.ToJson()));
                }
        }
}
