using WeDonekRpc.Client.Attr;
using System.Reflection;

namespace WeDonekRpc.Client.RouteService
{
    internal class RouteHelper
    {
        public static string GetRouteName(MethodInfo method)
        {
            RpcRouteName attr = method.GetCustomAttribute<RpcRouteName>();
            if (attr != null)
            {
                return attr.RouteName;
            }
            else
            {
               return method.Name;
            }
        }
    }
}
