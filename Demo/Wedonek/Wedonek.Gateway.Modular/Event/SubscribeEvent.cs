using RpcClient.Interface;
using RpcHelper;
using Wedonek.Demo.RemoteModel.Subscribe;

namespace Wedonek.Gateway.Modular.Event
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
                public void UserGoOnline(UserGoOnline obj)
                {
                        System.Console.WriteLine("用户上线了! ");
                        System.Console.WriteLine(string.Concat("user:", obj.ToJson()));
                }
        }
}
