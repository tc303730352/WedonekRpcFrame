using System.Threading;
using Wedonek.Demo.RemoteModel.Msg;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace Wedonek.Demo.User.Service.Event
{
    /// <summary>
    /// 接收广播消息
    /// </summary>
    internal class MsgEvent : IRpcApiService
    {
        /// <summary>
        /// 下单的消息广播
        /// </summary>
        /// <param name="obj"></param>
        public void PlaceAnOrder (PlaceAnOrder obj)
        {
            System.Console.WriteLine("收到了一个下单的消息广播! ");
            System.Console.WriteLine(string.Concat("order:", obj.ToJson()));
            Thread.Sleep(3000);
        }
    }
}
