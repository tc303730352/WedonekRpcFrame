using System.Reflection;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Helper
{
    internal class FuncHelper
    {
        public static FuncParam[] InitMethod (MethodInfo method)
        {
            return method.GetParameters().ConvertAll(a => ApiHelper.GetParamType(a));
        }
    }
}
