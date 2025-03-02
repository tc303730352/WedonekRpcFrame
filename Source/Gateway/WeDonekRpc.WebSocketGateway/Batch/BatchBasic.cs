using System.Threading;
using System.Threading.Tasks;

using WeDonekRpc.HttpWebSocket.Interface;

using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Batch
{
    internal class BatchBasic
        {
                protected readonly IService _Service;
                protected readonly IResponseTemplate _Template = null;
                public BatchBasic(IService service, IResponseTemplate template)
                {
                        this._Template = template;
                        this._Service = service;
                }
                private int _Send(IClientSession session, string text)
                {
                        if (session.Send(text))
                        {
                                return 1;
                        }
                        return 0;
                }
                protected int _Send(IClientSession[] session, string text)
                {
                        if (session.Length == 1)
                        {
                                return this._Send(session[0], text);
                        }
                        int num = 0;
                        Parallel.ForEach(session, a =>
                        {
                                if (a.Send(text))
                                {
                                        Interlocked.Increment(ref num);
                                }
                        });
                        return Interlocked.CompareExchange(ref num, 0, 0);
                }
        }
}
