using System.Collections.Specialized;
using System.Text;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Modular;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway
{
    internal class ApiSocketService : Interface.IWebSocketService
    {
        private IAccredit _Accredit;
        private readonly IResponseTemplate _Template = null;
        private readonly ISession _Session = null;
        private readonly IApiService _Service = null;
        private readonly IUserPage _Page = null;
        private readonly Encoding _RequestEncoding = null;
        private string _PostString = null;
        private readonly byte[] _Content = null;
        private IUserState _UserState = null;
        private ICurrentModular _Modular;
        private NameValueCollection _Form = null;

        public ApiSocketService (IUserPage page, byte[] content, IApiModular modular, IApiService service)
        {
            this._Content = content;
            this._Page = page;
            this._Service = service;
            this._RequestEncoding = modular.Config.RequestEncoding;
            this._Session = new ClientSession(service.Session, modular);
            this._Template = modular.Config.ResponseTemplate;
            this.ServiceName = modular.ServiceName;
        }
        /// <summary>
        /// 授权Id
        /// </summary>
        public string AccreditId => this._Session.AccreditId;
        /// <summary>
        /// 身份标识
        /// </summary>
        public string IdentityId => this._Session.IdentityId;

        public NameValueCollection Form
        {
            get
            {
                if (this._Form == null)
                {
                    this._Form = [];
                    string[] str = this.PostString.Split('&');
                    if (!str.IsNull())
                    {
                        str.ForEach(a =>
                        {
                            if (a != string.Empty)
                            {
                                int index = a.IndexOf('=');
                                int len = a.Length - 1;
                                if (index != -1 && index != len)
                                {
                                    this._Form.Add(a.Substring(0, index), a.Substring(index + 1, len - index));
                                }
                            }
                        });
                    }
                }
                return this._Form;
            }
        }
        public ISession Session => this._Session;

        public IUserState UserState => this._UserState;

        /// <summary>
        /// 请求头
        /// </summary>
        public RequestBody Head => this._Service.Request.Head;


        public string ServiceName { get; }


        public string ResponseText { get; private set; }

        public string PostString
        {
            get
            {
                if (this._PostString == null)
                {
                    this._PostString = this._RequestEncoding.GetString(this._Content);
                }
                return this._PostString;
            }
        }

        public IUserIdentity Identity => RpcClient.Ioc.Resolve<IUserIdentity>();

        public bool IsError { get; private set; }

        public string ErrorCode { get; private set; }

        public ICurrentModular Modular
        {
            get
            {
                if (this._Modular == null)
                {
                    this._Modular = RpcClient.Ioc.Resolve<ICurrentModular>(this.ServiceName);
                }
                return this._Modular;
            }
        }

        public void ReplyError (string error)
        {
            this.IsError = true;
            this.ErrorCode = error;
            string text = this._Template.GetErrorResponse(this._Page, error);
            this._Reply(text);
        }
        public void ReplyError (ErrorException error)
        {
            this.IsError = true;
            this.ErrorCode = error.ErrorCode;
            string text = this._Template.GetErrorResponse(this._Page, error);
            this._Reply(text);
        }

        private void _Reply (string text)
        {
            this.ResponseText = text;
            this._Service.Response.Write(text);
        }
        public void Reply ()
        {
            string text = this._Template.GetResponse(this._Page);
            this._Reply(text);
        }

        public void Reply (object result)
        {
            if (result == null)
            {
                this.Reply();
                return;
            }
            string text = this._Template.GetResponse(this._Page, result);
            this._Reply(text);
        }

        public void InitService (IApiModular modular)
        {
            this._Accredit = RpcClient.Ioc.Resolve<IAccredit>();
            if (this._Session.IdentityId != null)
            {
                ApiGateway.GatewayServer.UserIdentity.SetIdentityId(this.IdentityId);
            }
            if (this._Session.IsAccredit)
            {
                this._UserState = this._Accredit.SetCurrentAccredit(this.AccreditId);
            }
        }

        public void Dispose ()
        {
            this._Accredit.ClearAccredit();
            ApiGateway.GatewayServer.UserIdentity.Clear();
        }
    }
}