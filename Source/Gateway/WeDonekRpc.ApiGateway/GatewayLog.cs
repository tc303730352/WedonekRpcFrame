
using WeDonekRpc.ApiGateway.Interface;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.ApiGateway
{
    internal class GatewayLog
    {
        private static readonly string _DefGroup = "HttpGateway";
        internal static void AddErrorLog ( ErrorException error )
        {
            if ( error.IsSystemError )
            {
                error.SaveLog(_DefGroup);
            }
        }



        internal static void AddErrorLog ( ErrorException error, IModular modular )
        {
            new ErrorLog(error, _DefGroup)
            {
                    { "ServiceName",modular.ServiceName},
                    { "TypeName",modular.GetType().FullName}
            }.Save();
        }
    }
}
