using System.Reflection;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.Helper
{
    public class ApiParamHelper
    {
        private static readonly string _DefMethod = "GET";
        public static string GetReceiveMethod (ParameterInfo type, string def, ref string name)
        {
            object[] list = type.GetCustomAttributes(false);
            if (list.IsNull())
            {
                return def ?? _DefMethod;
            }
            foreach (object i in list)
            {
                if (i is ApiPostForm post)
                {
                    if (post.Name != null)
                    {
                        name = post.Name;
                    }
                    return "PostForm";
                }
                else if (i is ApiGet get)
                {
                    if (get.Name != null)
                    {
                        name = get.Name;
                    }
                    return "GET";
                }
                else if (i is ApiHeadParam head)
                {
                    if (head.Name != null)
                    {
                        name = head.Name;
                    }
                    return "Head";
                }
                else if (i is ApiRouteParam route)
                {
                    if (route.Name != null)
                    {
                        name = route.Name;
                    }
                    return "Route";
                }
                else if (i is ApiPathParam path)
                {
                    if (path.Name != null)
                    {
                        name = path.Name;
                    }
                    return "Path";
                }
            }
            return def;
        }

    }
}
