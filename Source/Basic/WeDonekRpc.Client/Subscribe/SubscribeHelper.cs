using WeDonekRpc.Client.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WeDonekRpc.Client.Subscribe
{
    internal class SubscribeHelper
    {
        public static string GetRouteName(MethodInfo method)
        {
            RpcSubscribeName attr = method.GetCustomAttribute<RpcSubscribeName>();
            if (attr != null)
            {
                return attr.RouteName;
            }
            else
            {
                string name = method.Name;
                if (name.EndsWith("Event"))
                {
                    return name.Substring(0, name.Length - 5);
                }
                return name;
            }
        }
    }
}
