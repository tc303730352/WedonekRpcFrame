
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway
{
    internal class GatewayLog
    {
        private static readonly string _DefGroup = "WebSocketGateway";
        internal static void AddErrorLog ( ErrorException error )
        {
            if ( error.IsSystemError )
            {
                error.SaveLog(_DefGroup);
            }
        }

        internal static void AddErrorLog ( ErrorException error, IApiModular modular )
        {
            new ErrorLog(error, _DefGroup)
             {
            { "Name",modular.ServiceName},
            { "Modular",modular.GetType().FullName}
             }.Save();
        }
    }
}
