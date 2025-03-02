using Wedonek.Demo.RemoteModel.Order.Msg;
using Wedonek.Demo.RemoteModel.Subscribe;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace Wedonek.Demo.Service.Subscribe
{
    /// <summary>
    /// 订阅演示
    /// </summary>
    internal class SubscribeEvent : IRpcSubscribeService
    {

        /// <summary>
        /// 添加订单事件
        /// </summary>
        /// <param name="add"></param>
        public void AddOrderMsg ( AddOrderMsg add )
        {
            System.Console.WriteLine("收到订单订阅消息! ");
            System.Console.WriteLine("order:" + add.Order.ToJson());
        }

        /// <summary>
        /// 订阅用户上线事件
        /// </summary>
        /// <param name="obj"></param>
        public void UserGoOnline ( UserGoOnline obj )
        {
            System.Console.WriteLine("用户上线了! ");
            System.Console.WriteLine(string.Concat("user:", obj.ToJson()));
        }
    }
}
