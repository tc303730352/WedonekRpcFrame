
using System;
using System.Text.RegularExpressions;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.WebSocketGateway.Batch;
using WeDonekRpc.WebSocketGateway.Config;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway
{
    public class BasicApiModular : IApiModular
    {
        private IService _Service;
        private WebSocketGatewayOption _Option;
        private bool _IsInit = false;
        public BasicApiModular (string name, string show = null)
        {
            this.Config = new ModularConfig(this);
            this.ServiceName = name;
            this.Show = show;
        }

        public bool IsInit { get { return this._IsInit; } }
        public string ServiceName
        {
            get;
        }

        public IModularConfig Config { get; }

        public string ApiRouteFormat => this.Config.ApiRouteFormat;

        public string Show { get; }

        /// <summary>
        /// 初始化模块
        /// </summary>
        public void InitModular ()
        {
            if (!this._IsInit)
            {
                this._IsInit = true;
                this._Option.Init();
                this._Option = null;
                this.Init();
            }
        }

        protected virtual void Init ()
        {
        }

        public void Load (IGatewayOption option)
        {
            this._Option = new WebSocketGatewayOption(option, this);
            this.Load(this._Option, this.Config);
            this._Option.Load();

        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        protected virtual void Load (IWebSocketGatewayOption option, IModularConfig config)
        {

        }
        /// <summary>
        /// 客户端链接授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual bool Authorize (RequestBody request)
        {
            if (this.Config.IsAccredit && this.Config.IsIdentity)
            {
                return Regex.IsMatch(request.Path, @"^/\w{32}/\w{32}$");
            }
            else if (this.Config.IsAccredit || this.Config.IsIdentity)
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
        protected virtual AuthResult? UserAuthorize (RequestBody request, ISession session)
        {
            if (!this.Config.IsAccredit && !this.Config.IsIdentity)
            {
                return new AuthResult();
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
        public virtual void SessionOffline (IClientSession session)
        {

        }

        /// <summary>
        /// 完成授权
        /// </summary>
        /// <param name="service"></param>
        public void AuthorizeComplate (IApiService service)
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
        public void Start ()
        {
            this._Service = HttpWebSocket.WebSocketService.AddServer(this.Config.SocketConfig);
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose ()
        {
            this._Service.Close();
        }
        /// <summary>
        /// 获取用户会话
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ISession GetSession (Guid sessionId)
        {
            IClientSession session = this._Service.GetSession(sessionId);
            if (session == null)
            {
                return null;
            }
            return new ClientSession(session, this);
        }
        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="find">查找可发送的会话</param>
        /// <returns></returns>
        public IBatchSend BatchSend (Func<ISessionBody, bool> find)
        {
            return new BatchSend(find, this._Service, this.Config.ResponseTemplate);
        }
        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public IBatchSend BatchSend (Guid[] sessionId)
        {
            return new BatchSession(sessionId, this._Service, this.Config.ResponseTemplate);
        }
        /// <summary>
        /// 获取回话
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ISession[] GetSession (Guid[] sessionId)
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
        public void CancelAccredit (string accreditId, string error)
        {
            this._Service.CancelAccredit(accreditId, error);
        }
        /// <summary>
        /// 查询同个授权的会话
        /// </summary>
        /// <param name="accreditId"></param>
        /// <returns></returns>
        public ISession[] FindSession (string accreditId)
        {
            IClientSession[] list = this._Service.FindSession(accreditId);
            if (list.IsNull())
            {
                return null;
            }
            return list.ConvertAll(a => new ClientSession(a, this));
        }
        /// <summary>
        /// 查询授权的会话
        /// </summary>
        /// <returns></returns>
        public ISession[] FindSession (Func<ISessionBody, bool> find)
        {
            IClientSession[] list = this._Service.FindSession(find);
            if (list.IsNull())
            {
                return null;
            }
            return list.ConvertAll(a => new ClientSession(a, this));
        }
        /// <summary>
        /// 模块中API方法初始化事件
        /// </summary>
        /// <param name="config">API方法配置</param>
        public virtual void InitRouteConfig (IApiModel config)
        {

        }
        /// <summary>
        /// 查找在线的会话
        /// </summary>
        /// <param name="accreditId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ISession FindOnlineSession (string accreditId, string name)
        {
            IClientSession session = this._Service.FindOnlineSession(accreditId, name);
            if (session == null)
            {
                return null;
            }
            return new ClientSession(session, this);
        }

    }
}
