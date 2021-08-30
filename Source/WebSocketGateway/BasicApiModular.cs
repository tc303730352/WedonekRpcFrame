
using System;
using System.Text.RegularExpressions;

using ApiGateway.Interface;

using HttpWebSocket.Interface;
using HttpWebSocket.Model;

using RpcHelper;

using WebSocketGateway.Batch;
using WebSocketGateway.Config;
using WebSocketGateway.Helper;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway
{
        public class BasicApiModular : IApiModular
        {
                private IService _Service;
                private IUserIdentityCollect _Identity => ApiGateway.GatewayServer.UserIdentity;
                public BasicApiModular(string name)
                {
                        this.Config = new ModularConfig(this);
                        this.ServiceName = name;
                }

                public string ServiceName
                {
                        get;
                }

                public IModularConfig Config { get; }

                public string ApiRouteFormat => this.Config.ApiRouteFormat;
                /// <summary>
                /// 初始化模块
                /// </summary>
                public void InitModular()
                {
                        ApiHelper.LoadModular(this);
                        this.Init();
                }

                /// <summary>
                /// 初始化
                /// </summary>
                protected virtual void Init()
                {

                }
                /// <summary>
                /// 客户端链接授权
                /// </summary>
                /// <param name="request"></param>
                /// <returns></returns>
                public virtual bool Authorize(RequestBody request)
                {
                        if (this.Config.IsAccredit && this._Identity.IsEnableIdentity)
                        {
                                return Regex.IsMatch(request.Path, @"^/\w{32}/\w{32}$");
                        }
                        else if (this.Config.IsAccredit || this._Identity.IsEnableIdentity)
                        {
                                return Regex.IsMatch(request.Path, @"^/\w{32}$");
                        }
                        else
                        {
                                return true;
                        }
                }
                /// <summary>
                /// 客户端授权
                /// </summary>
                /// <param name="request"></param>
                /// <param name="session"></param>
                protected virtual AuthResult? UserAuthorize(RequestBody request, ISession session)
                {
                        if (!this.Config.IsAccredit && !this._Identity.IsEnableIdentity)
                        {
                                return null;
                        }
                        string accreditId = null;
                        string identityId = null;
                        string[] t = request.Path.Split('/');
                        if (t.Length == 3)
                        {
                                accreditId = t[1];
                                identityId = t[2];
                        }
                        else if (this.Config.IsAccredit)
                        {
                                accreditId = t[1];
                        }
                        else
                        {
                                identityId = t[1];
                        }
                        if (accreditId != null)
                        {
                                this.Config.CheckAccredit(accreditId);
                        }
                        if (identityId != null)
                        {
                                ApiGateway.GatewayServer.UserIdentity.CheckIdentity(identityId);
                        }
                        return new AuthResult
                        {
                                AccreditId = accreditId,
                                IdentityId = identityId
                        };
                }

                /// <summary>
                /// 会话离线
                /// </summary>
                /// <param name="session"></param>
                public virtual void SessionOffline(IClientSession session)
                {

                }

                /// <summary>
                /// 完成授权
                /// </summary>
                /// <param name="service"></param>
                public void AuthorizeComplate(IApiService service)
                {
                        AuthResult? result = this.UserAuthorize(service.Request.Head, new ClientSession(service.Session, this));
                        if (result.HasValue)
                        {
                                AuthResult res = result.Value;
                                service.Session.Accredit(res.AccreditId, res.IdentityId);
                        }
                }
                /// <summary>
                /// 启动
                /// </summary>
                public void Start()
                {
                        this._Service = HttpWebSocket.WebSocketService.AddServer(this.Config.SocketConfig);
                }
                /// <summary>
                /// 销毁
                /// </summary>
                public void Dispose()
                {
                        this._Service.Close();
                }
                /// <summary>
                /// 获取用户会话
                /// </summary>
                /// <param name="sessionId"></param>
                /// <returns></returns>
                public ISession GetSession(Guid sessionId)
                {
                        IClientSession session = this._Service.GetSession(sessionId);
                        if (session == null)
                        {
                                return null;
                        }
                        return new ClientSession(session, this);
                }
                public IBatchSend BatchSend(Func<ISessionBody, bool> find)
                {
                        return new BatchSend(find, this._Service, this.Config.ResponseTemplate);
                }
                public IBatchSend BatchSend(Guid[] sessionId)
                {
                        return new BatchSession(sessionId, this._Service, this.Config.ResponseTemplate);
                }
                public ISession[] GetSession(Guid[] sessionId)
                {
                        IClientSession[] session = this._Service.GetSession(sessionId);
                        if (session == null)
                        {
                                return null;
                        }
                        return session.ConvertAll(a => new ClientSession(a, this));
                }
                /// <summary>
                /// 取消授权
                /// </summary>
                /// <param name="accreditId"></param>
                /// <param name="error"></param>
                public void CancelAccredit(string accreditId, string error)
                {
                        this._Service.CancelAccredit(accreditId, error);
                }
                /// <summary>
                /// 查询同个授权的会话
                /// </summary>
                /// <param name="accreditId"></param>
                /// <returns></returns>
                public ISession[] FindSession(string accreditId)
                {
                        IClientSession[] list = this._Service.FindSession(accreditId);
                        if (list.IsNull())
                        {
                                return null;
                        }
                        return list.ConvertAll(a => new ClientSession(a, this));
                }
                /// <summary>
                /// 查询同个授权的会话
                /// </summary>
                /// <param name="accreditId"></param>
                /// <returns></returns>
                public ISession[] FindSession(Func<ISessionBody, bool> find)
                {
                        IClientSession[] list = this._Service.FindSession(find);
                        if (list.IsNull())
                        {
                                return null;
                        }
                        return list.ConvertAll(a => new ClientSession(a, this));
                }

                public ISession FindOnlineSession(string accreditId, string name)
                {
                        IClientSession session = this._Service.FindOnlineSession(accreditId,name);
                        if (session == null)
                        {
                                return null;
                        }
                        return new ClientSession(session, this);
                }
        }
}
