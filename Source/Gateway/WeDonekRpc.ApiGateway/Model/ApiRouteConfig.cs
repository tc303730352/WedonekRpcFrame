using System.Reflection;

namespace WeDonekRpc.ApiGateway.Model
{
    public class ApiRouteConfig
    {
        public int ApiId
        {
            get;
            set;
        }
        public string ApiPath
        {
            get;
            set;
        }
        public MethodInfo Method
        {
            get;
            set;
        }
    }
}
