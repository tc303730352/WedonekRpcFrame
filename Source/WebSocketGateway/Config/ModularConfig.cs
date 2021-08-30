using System.Security.Cryptography.X509Certificates;
using System.Text;

using HttpWebSocket.Interface;

using RpcClient.Interface;

using RpcHelper;

using RpcModular;
using RpcModular.Model;

using WebSocketGateway.Interface;

namespace WebSocketGateway.Config
{
        internal class ModularConfig : IModularConfig
        {
                private IAccreditService _Accredit;

                private readonly string _ConfigKey = null;
                public ModularConfig(IApiModular modular)
                {
                        this._ConfigKey = string.Concat("gateway:", modular.ServiceName);
                        this._Accredit = RpcClient.RpcClient.Unity.Resolve<IAccreditService>();
                        this.SocketConfig = new HttpWebSocket.Config.WebSocketConfig(new WebSocketEvent(modular))
                        {
                                IsSingle = RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":IsSingle", false),
                                HeartbeatTime= RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":HeartbeatTime", 10),
                                ServerPort= RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":ServerPort", 1254),
                                BufferSize= RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":BufferSize", 5242880),
                                TcpBacklog= RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":TcpBacklog", 5000)
                        };
                        CertificateSet cert = RpcClient.RpcClient.Config.GetConfigVal<CertificateSet>(this._ConfigKey + ":Certificate");
                        if (cert != null && !cert.FileName.IsNull())
                        {
                                this.SocketConfig.Certificate = new X509Certificate2(cert.FileName, cert.Pwd);
                        }
                        this.ApiRouteFormat = RpcClient.RpcClient.Config.GetConfigVal(this._ConfigKey + ":ApiRouteFormat", WebSocketConfig.ApiRouteFormat);
                        RpcClient.RpcClient.Config.AddRefreshEvent(this._Refresh);
                }

                private void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith(this._ConfigKey) || name == string.Empty)
                        {
                                this.IsAccredit = config.GetConfigVal(this._ConfigKey + ":IsAccredit", true);
                                string encoding = config.GetConfigVal(this._ConfigKey + ":RequestEncoding", WebSocketConfig.RequestEncoding);
                                if (encoding != this.RequestEncoding.BodyName)
                                {
                                        this.RequestEncoding = Encoding.GetEncoding(encoding);
                                }
                        }
                }
                public IWebSocketConfig SocketConfig
                {
                        get;
                } = new HttpWebSocket.Config.WebSocketConfig();
                /// <summary>
                ///  Api 接口路径生成格式
                /// </summary>
                public string ApiRouteFormat
                {
                        get;
                        set;
                }
                public IResponseTemplate ResponseTemplate
                {
                        get;
                        set;
                } = new ResponseTemplate();

                public Encoding RequestEncoding
                {
                        get;
                        set;
                } = Encoding.UTF8;

                public bool IsAccredit { get; private set; }

                public void RegUserState<T>() where T : UserState
                {
                        this._Accredit = this._Accredit.Create<T>();
                }

                public IUserState GetAccredit(string accreditId)
                {
                        return this._Accredit.GetAccredit(accreditId);
                }

                public void CheckAccredit(string accreditId)
                {
                        this._Accredit.CheckAccredit(accreditId);
                }
        }
}
