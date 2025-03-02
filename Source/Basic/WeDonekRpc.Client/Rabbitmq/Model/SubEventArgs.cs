
using System.Collections.Generic;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Rabbitmq.Model
{
    internal class SubEventArgs : ISubEventArgs
    {
        private readonly BasicDeliverEventArgs _Args = null;
        private static IRabbitmqService _Service;
        private byte[] _Content = null;
        internal SubEventArgs(BasicDeliverEventArgs args)
        {
            this._Args = args;
        }
        static SubEventArgs()
        {
            _Service = RpcClient.Ioc.Resolve<IRabbitmqService>();
        }
        /// <summary>
        /// 头部
        /// </summary>
        public IDictionary<string, object> Headers => this._Args.BasicProperties.Headers;
        /// <summary>
        /// 发送时间
        /// </summary>
        public long SendTime => this._Args.BasicProperties.Timestamp.UnixTime;
        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId => this._Args.BasicProperties.AppId;
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId => this._Args.BasicProperties.MessageId;
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId => this._Args.BasicProperties.UserId;
        /// <summary>
        /// 集群Id
        /// </summary>
        public string ClusterId => this._Args.BasicProperties.ClusterId;
        /// <summary>
        /// 类型
        /// </summary>
        public string Type => this._Args.BasicProperties.Type;
        /// <summary>
        /// 关联Id
        /// </summary>
        public string CorrelationId => this._Args.BasicProperties.CorrelationId;


        public byte[] Body
        {
            get
            {
                if (this._Content == null)
                {
                    this._Content = this._Args.Body.ToArray();
                }
                return this._Content;
            }
        }
        public T GetValue<T>() where T : class
        {
            string json = this.GetValue();
            return json.Json<T>();
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            if (this._Args.BasicProperties.ContentEncoding.IsNull())
            {
                return Encoding.UTF8.GetString(this.Body);
            }
            return Encoding.GetEncoding(this._Args.BasicProperties.ContentEncoding).GetString(this.Body);
        }
        /// <summary>
        /// 会话Id
        /// </summary>
        public string ConsumerTag => this._Args.ConsumerTag;
        /// <summary>
        /// 处理通道的名字
        /// </summary>
        public ulong DeliveryTag => this._Args.DeliveryTag;
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Exchange => this._Args.Exchange;

        /// <summary>
        /// 是否重复发送
        /// </summary>
        public bool Redelivered => this._Args.Redelivered;
        /// <summary>
        /// 路由Key
        /// </summary>
        public string RoutingKey => this._Args.RoutingKey;

        public void ReplyMsg<T>(T msg) where T : class
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            MsgProperties properties = new MsgProperties
            {
                ReplyTo = this._Args.BasicProperties.ReplyTo,
                CorrelationId = this._Args.BasicProperties.CorrelationId,
                Headers = this._Args.BasicProperties.Headers
            };
            _Service.Public(msg, address.ExchangeName, properties, address.RoutingKey);
        }
        public void ReplyMsg<T>(T msg, MsgProperties properties) where T : class
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            this._Init(properties);
            _Service.Public(msg, address.ExchangeName, properties, address.RoutingKey);
        }

        public void ReplyMsg(string msg)
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            MsgProperties properties = new MsgProperties
            {
                ReplyTo = this._Args.BasicProperties.ReplyTo,
                CorrelationId = this._Args.BasicProperties.CorrelationId,
                Headers = this._Args.BasicProperties.Headers
            };
            _Service.Public(msg, address.ExchangeName, properties, address.RoutingKey);
        }
        public void ReplyMsg(string msg, MsgProperties properties)
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            this._Init(properties);
            _Service.Public(msg, address.ExchangeName, properties, address.RoutingKey);
        }

        public void ReplyTranMsg<T>(T msg) where T : class
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            MsgProperties properties = new MsgProperties
            {
                ReplyTo = this._Args.BasicProperties.ReplyTo,
                CorrelationId = this._Args.BasicProperties.CorrelationId,
                Headers = this._Args.BasicProperties.Headers
            };
            _Service.PublicTran(msg, address.ExchangeName, properties, address.RoutingKey);
        }
        public void ReplyTranMsg<T>(T msg, MsgProperties properties) where T : class
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            this._Init(properties);
            _Service.PublicTran(msg, address.ExchangeName, properties, address.RoutingKey);
        }

        public void ReplyTranMsg(string msg)
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            MsgProperties properties = new MsgProperties
            {
                ReplyTo = this._Args.BasicProperties.ReplyTo,
                CorrelationId = this._Args.BasicProperties.CorrelationId,
                Headers = this._Args.BasicProperties.Headers
            };
            _Service.PublicTran(msg, address.ExchangeName, properties, address.RoutingKey);
        }
        public void ReplyTranMsg(string msg, MsgProperties properties)
        {
            if (this._Args.BasicProperties.ReplyToAddress == null)
            {
                return;
            }
            PublicationAddress address = this._Args.BasicProperties.ReplyToAddress;
            this._Init(properties);
            _Service.PublicTran(msg, address.ExchangeName, properties, address.RoutingKey);
        }

        private void _Init(MsgProperties properties)
        {
            properties.ReplyTo = properties.ReplyTo ?? this._Args.BasicProperties.ReplyTo;
            properties.CorrelationId = properties.CorrelationId ?? this._Args.BasicProperties.CorrelationId;
            properties.Headers = properties.Headers ?? this._Args.BasicProperties.Headers;
        }
    }
}
