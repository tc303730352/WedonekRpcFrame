using System;
using System.Net;

using RabbitMQ.Client;

using RpcClient.Helper;
using RpcClient.Queue.Model;

using RpcHelper;

namespace RpcClient.Rabbitmq
{
        internal class RabbitmqQueue : IDisposable
        {
                private ConnectionFactory _Factory = null;
                private IConnection _Connection = null;
                private AmqpTcpEndpoint[] _EndPoint = null;
                private readonly QueueConfig _Config = null;
                private bool _IsMany = false;

                public QueueConfig Config => this._Config;

                public RabbitmqQueue(QueueConfig config)
                {
                        this._Config = config;
                        this._Init();
                }
                public virtual void Dispose()
                {
                        if (this._Connection != null)
                        {
                                this._Connection.Close();
                                this._Connection.Dispose();
                        }
                }

                protected virtual void _Init()
                {
                        this._Factory = new ConnectionFactory
                        {
                                UserName = this._Config.UserName,
                                Password = this._Config.Pwd,
                                VirtualHost = this._Config.VirtualHost,
                                ClientProvidedName = this._Config.ClientProvidedName,
                                RequestedHeartbeat = TimeSpan.FromSeconds(60)
                        };
                        if (this._Config.Servers.Length == 1)
                        {
                                QueueCon con = this._Config.Servers[0];
                                if (con.ServerIp != null)
                                {
                                        IPEndPoint address = QueueHelper.GetServer(con.ServerIp, 5672);
                                        this._Factory.HostName = address.Address.ToString();
                                        this._Factory.Port = address.Port;
                                }
                                else
                                {
                                        this._Factory.Uri = con.ServerUri;
                                }
                        }
                        else
                        {
                                this._IsMany = true;
                                this._EndPoint = this._Config.Servers.ConvertAll(a =>
                                {
                                        if (a.ServerIp != null)
                                        {
                                                IPEndPoint address = QueueHelper.GetServer(a.ServerIp, 5672);
                                                return new AmqpTcpEndpoint(address.Address.ToString(), address.Port);
                                        }
                                        return new AmqpTcpEndpoint(a.ServerUri);
                                });
                        }
                        this._Connection = this._GetConnection();
                }

                public IModel CreateChannel()
                {
                        return this._Connection.CreateModel();
                }

                private IConnection _GetConnection()
                {
                        if (this._IsMany)
                        {
                                return this._Factory.CreateConnection(this._EndPoint);
                        }
                        return this._Factory.CreateConnection();
                }


        }
}
