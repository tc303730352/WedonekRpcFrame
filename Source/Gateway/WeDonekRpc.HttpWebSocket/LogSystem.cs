using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket
{
    internal class LogSystem
    {
        private static readonly string _Group = "WebSocket";

        internal static void AddErrorLog ( string title, string show, string error )
        {
            new WarnLog(error, title, show, _Group).Save();
        }
        internal static void AddErrorLog ( Exception e, IApiService service )
        {
            ErrorException error = ErrorException.FormatError(e);
            AddErrorLog(error, service.Session);
        }
        internal static void AddErrorLog ( Exception error, string source )
        {
            new ErrorLog(error, _Group)
                        {
                                { "Source",source}
                        }.Save();
        }
        internal static void AddErrorLog ( ErrorException error, IClientSession session )
        {
            new ErrorLog(error, _Group)
                        {
                                {"SessionId",session.SessionId }
                        }.Save();
        }
        internal static void AddErrorLog ( ErrorException error, IApiService service, string source )
        {
            if ( error.IsSystemError )
            {
                new ErrorLog(error, _Group)
                                {
                                        {"SessionId",service.Session.SessionId },
                                        { "Source",source}
                                }.Save();
            }
        }
    }
}
