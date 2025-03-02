using System;
using RabbitMQ.Client;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Rabbitmq
{
    internal class RabbitmqQueue : IDisposable
    {
        private ConnectionFactory _Factory = null;
        private IConnection _Connection = null;
        private AmqpTcpEndpoint[] _EndPoint = null;
        private readonly string _Expiration;
        private bool _IsMany = false;

        public RabbitmqConfig Config { get; } = null;

        public RabbitmqQueue ( RabbitmqConfig config )
        {
            this.Config = config;
            this._Expiration = config.Expiration.ToString();
            this._Init();
        }
        public virtual void Dispose ()
        {
            if ( this._Connection != null )
            {
                this._Connection.Close();
                this._Connection.Dispose();
            }
        }

        protected virtual void _Init ()
        {
            this._Factory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                UserName = this.Config.UserName,
                Password = this.Config.Pwd,
                VirtualHost = this.Config.VirtualHost,
                ClientProvidedName = this.Config.ClientProvidedName,
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };
            ConServer[] ips = QueueHelper.GetServer(this.Config.Servers);
            if ( ips.Length == 1 )
            {
                ConServer ser = ips[0];
                this._Factory.HostName = ser.ip;
                this._Factory.Port = ser.port;
            }
            else
            {
                this._IsMany = true;
                this._EndPoint = ips.ConvertAll(a =>
                 {
                     return new AmqpTcpEndpoint(a.ip, a.port);
                 });
            }
            this._Connection = this._GetConnection();
        }
        public IModel GetChannel ()
        {
            if ( this._Connection.IsOpen )
            {
                return this._Connection.CreateModel();
            }
            this._Connection = this._GetConnection();
            if ( !this._Connection.IsOpen )
            {
                throw new ErrorException("rpc.rabbitmq.connection.already.close");
            }
            return this._Connection.CreateModel();
        }
        public IModel CreateChannel ()
        {
            IConnection con = this._GetConnection();
            if ( !con.IsOpen )
            {
                throw new ErrorException("rpc.rabbitmq.connection.already.close");
            }
            return con.CreateModel();
        }
        public IBasicProperties GetProperties ( IModel channel, MsgProperties pro )
        {
            IBasicProperties props = channel.CreateBasicProperties();
            pro.InitProperties(props);
            return props;
        }
        public IBasicProperties GetProperties ( IModel channel )
        {
            IBasicProperties props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf-8";
            props.DeliveryMode = this.Config.IsLasting ? (byte)2 : (byte)1;
            props.Timestamp = new AmqpTimestamp(DateTime.Now.ToLong());
            props.Expiration = this._Expiration;
            return props;
        }
        private IConnection _GetConnection ()
        {
            if ( this._IsMany )
            {
                return this._Factory.CreateConnection(this._EndPoint);
            }
            return this._Factory.CreateConnection();
        }

    }
}
