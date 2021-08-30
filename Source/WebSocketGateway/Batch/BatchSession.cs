using System;

using HttpWebSocket.Interface;
using HttpWebSocket.Model;

using RpcHelper;

using WebSocketGateway.Interface;

using WebSocketGateway.Model;

namespace WebSocketGateway.Batch
{
        internal class BatchSession : BatchBasic, IBatchSend
        {
                private readonly Guid[] _SessionId = null;
                public BatchSession(Guid[] sessionId, IService service, IResponseTemplate template) : base(service, template)
                {
                        this._SessionId = sessionId;
                }

                public int Send(string direct, object result)
                {
                        IClientSession[] session = this._Service.GetSession(this._SessionId);
                        if (session.Length == 0)
                        {
                                return 0;
                        }
                        string text = this._Template.GetResponse(new UserPage(direct), result);
                        return this._Send(session, text);
                }
                public int Send(string direct, ErrorException error)
                {
                        IClientSession[] session = this._Service.GetSession(this._SessionId);
                        if (session.Length == 0)
                        {
                                return 0;
                        }
                        string text = this._Template.GetErrorResponse(new UserPage(direct), error);
                        return this._Send(session, text);
                }
                public int Send(ErrorException error)
                {
                        IClientSession[] session = this._Service.GetSession(this._SessionId);
                        if (session.Length == 0)
                        {
                                return 0;
                        }
                        string text = this._Template.GetErrorResponse(error);
                        return this._Send(session, text);
                }
        }
}
