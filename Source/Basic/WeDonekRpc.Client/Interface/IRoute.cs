using System.Reflection;

namespace WeDonekRpc.Client.Interface
{
    public interface IRoute
    {
        /// <summary>
        /// 路由名
        /// </summary>
        string RouteName
        {
            get;
        }
        /// <summary>
        /// 原方法
        /// </summary>
        MethodInfo Source { get; }
        /// <summary>
        /// 是否是系统路由
        /// </summary>
        bool IsSystemRoute { get; }
        /// <summary>
        /// 路由说明
        /// </summary>
        string RouteShow
        {
            get;
        }
        /// <summary>
        /// 回调
        /// </summary>
        TcpMsgEvent TcpMsgEvent
        {
            get;
        }
    }
}
