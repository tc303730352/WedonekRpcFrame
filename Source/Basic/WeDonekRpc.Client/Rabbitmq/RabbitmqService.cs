using System;
using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RabbitmqService : IRabbitmqService
    {
        private static readonly ConcurrentDictionary<string, Subscribe> _SubscribeDic = new ConcurrentDictionary<string, Subscribe>();
        private static readonly RabbitmqQueue _Rabbitmq = null;

        private static readonly MsgProperties _DefJson = null;
        private static readonly MsgProperties _DefText = null;

        static RabbitmqService ()
        {
            RabbitmqConfig config = Config.WebConfig.GetRabbitmqConfig();
            _Rabbitmq = new RabbitmqQueue(config);
            _DefJson = new MsgProperties
            {
                ContentEncoding = "utf-8",
                Expiration = config.Expiration,
                ContentType = "application/json",
                IsLasting = config.IsLasting
            };
            _DefText = new MsgProperties
            {
                ContentEncoding = "utf-8",
                Expiration = config.Expiration,
                ContentType = "text/plain",
                IsLasting = config.IsLasting
            };
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        public void Public<T> (T msg, string exchange, params string[] routeKey) where T : class
        {
            this._Public(msg.ToJson(), exchange, _DefJson, routeKey);
        }
        public void Public<T> (T msg, string exchange, MsgProperties properties, params string[] routeKey) where T : class
        {
            properties.ContentType = "application/json";
            this._Public(msg.ToJson(), exchange, _DefJson, routeKey);
        }
        public void Public (string msg, string exchange, params string[] routeKey)
        {
            this._Public(msg, exchange, _DefText, routeKey);
        }
        public void Public (string msg, string exchange, MsgProperties properties, params string[] routeKey)
        {
            properties.ContentType = "text/plain";
            this._Public(msg, exchange, properties, routeKey);
        }
        private void _Public (string msg, string exchange, MsgProperties properties, params string[] routeKey)
        {
            using (IModel channel = _Rabbitmq.CreateChannel())
            {
                IBasicProperties props = _Rabbitmq.GetProperties(channel, properties);
                byte[] datas = Encoding.UTF8.GetBytes(msg);
                routeKey.ForEach(a =>
                 {
                     channel.BasicPublish(exchange, a, false, props, datas);
                 });
            }
        }

        private bool _PublicTran (string msg, string exchange, MsgProperties properties, params string[] routeKey)
        {
            using (IModel channel = _Rabbitmq.CreateChannel())
            {
                IBasicProperties props = _Rabbitmq.GetProperties(channel, properties);
                byte[] datas = Encoding.UTF8.GetBytes(msg);
                channel.ConfirmSelect();
                routeKey.ForEach(a =>
                 {
                     channel.BasicPublish(exchange, a, false, props, datas);
                 });
                try
                {
                    channel.WaitForConfirmsOrDie();
                }
                catch (Exception e)
                {
                    ErrorException error = ErrorException.FormatError(e);
                    error.SaveLog("Rpc_Rabbitmq");
                    return false;
                }
                return true;
            }
        }

        public void PublicTran<T> (T msg, string exchange, params string[] routeKey) where T : class
        {
            _ = this._PublicTran(msg.ToJson(), exchange, _DefJson, routeKey);
        }
        public void PublicTran<T> (T msg, string exchange, MsgProperties properties, params string[] routeKey) where T : class
        {
            properties.ContentType = "application/json";
            _ = this._PublicTran(msg.ToJson(), exchange, _DefJson, routeKey);
        }
        public void PublicTran (string msg, string exchange, params string[] routeKey)
        {
            _ = this._PublicTran(msg, exchange, _DefText, routeKey);
        }
        public void PublicTran (string msg, string exchange, MsgProperties properties, params string[] routeKey)
        {
            properties.ContentType = "text/plain";
            _ = this._PublicTran(msg, exchange, properties, routeKey);
        }
        public ISubscribe Subscribe (string exchange, Action<ISubscribe, ISubEventArgs> action, bool isAutoAck = true, string exchangeType = "direct")
        {
            if (_SubscribeDic.TryGetValue(exchange, out Subscribe sub))
            {
                return sub;
            }
            sub = new Subscribe(_Rabbitmq, exchange, isAutoAck, exchangeType, action);
            if (_SubscribeDic.TryAdd(exchange, sub))
            {
                sub.Init();
                return sub;
            }
            return _SubscribeDic[exchange];
        }
    }
}
