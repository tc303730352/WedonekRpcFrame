using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket.Config
{
    public class WebSocketConfig : IWebSocketConfig
    {

        public WebSocketConfig (IConfigSection section) : this(new SocketEvent(), section)
        {

        }
        public WebSocketConfig (ISocketEvent socketEvent, IConfigSection config)
        {
            this.SocketEvent = socketEvent;
            config.AddRefreshEvent(this._Refresh);
        }
        private void _Refresh (IConfigSection config, string name)
        {
            IConfigSection section = config.GetSection("Socket");
            this.IsSingle = section.GetValue("IsSingle", false);
            this.HeartbeatTime = section.GetValue("HeartbeatTime", 10);
            this.ServerPort = section.GetValue("Port", 1254);
            this.BufferSize = section.GetValue("BufferSize", 2097152);
            this.TcpBacklog = section.GetValue("TcpBacklog", 5000);
            CertificateSet cert = section.GetValue<CertificateSet>("Certificate");
            if (cert != null && cert.FileName.IsNotNull() && this.Certificate == null)
            {
                if (!Path.IsPathRooted(cert.FileName))
                {
                    cert.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cert.FileName);
                }
                if (File.Exists(cert.FileName))
                {
                    this.Certificate = new X509Certificate2(cert.FileName, cert.Pwd);
                }
            }
        }
        /// <summary>
        /// 心跳时间(秒)
        /// </summary>
        public int HeartbeatTime { get; private set; } = 10;

        /// <summary>
        /// 响应的GUID
        /// </summary>
        public string ResponseGuid { get; private set; } = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";


        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int BufferSize { get; private set; } = 5242880;

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPort { get; private set; } = 1254;

        /// <summary>
        /// HTTPS证书
        /// </summary>
        public X509Certificate2 Certificate { get; private set; }


        public ISocketEvent SocketEvent { get; }

        /// <summary>
        /// TCP排队数量
        /// </summary>
        public int TcpBacklog { get; private set; } = 5000;
        /// <summary>
        /// 是否单点登陆
        /// </summary>
        public bool IsSingle { get; private set; } = false;

        public Uri ToUri ()
        {
            if (this.Certificate == null)
            {
                return new Uri(string.Format("ws://127.0.0.1:{0}/", this.ServerPort));
            }
            return new Uri(string.Format("wss://127.0.0.1:{0}/", this.ServerPort));
        }
    }
}
