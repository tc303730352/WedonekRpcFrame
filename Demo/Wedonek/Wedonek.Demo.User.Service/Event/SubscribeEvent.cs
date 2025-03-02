using Wedonek.Demo.RemoteModel.Subscribe;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace Wedonek.Demo.User.Service.Event
{
    /// <summary>
    /// 订阅演示
    /// </summary>
    internal class SubscribeEvent : IRpcSubscribeService
    {
        /// <summary>
        /// 订阅用户上线事件
        /// </summary>
        /// <param name="obj"></param>
        public void UserGoOnline (UserGoOnline obj)
        {
            System.Console.WriteLine("用户上线了! ");
            System.Console.WriteLine(string.Concat("user:", obj.ToJson()));
        }
    }
}
