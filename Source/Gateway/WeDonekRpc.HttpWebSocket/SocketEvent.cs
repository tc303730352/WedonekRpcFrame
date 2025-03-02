using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket
{
    public class SocketEvent : ISocketEvent
    {
        public virtual bool Authorize (RequestBody head)
        {
            return true;
        }

        public virtual void AuthorizeComplate (IApiService service)
        {

        }

        public virtual void CheckSession (IClientSession session)
        {
        }


        public virtual void Receive (IApiService service)
        {
        }

        public virtual void ReplyError (IApiService service, ErrorException error, string source)
        {

        }

        public virtual void SessionOffline (IClientSession session)
        {
        }

    }
}
