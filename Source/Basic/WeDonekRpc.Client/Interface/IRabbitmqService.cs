using System;

using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IRabbitmqService
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="properties"></param>
        /// <param name="routeKey"></param>
        void Public(string msg, string exchange, MsgProperties properties, params string[] routeKey);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        void Public(string msg, string exchange, params string[] routeKey);
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>

        void Public<T>(T msg, string exchange, params string[] routeKey) where T : class;
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="properties"></param>
        /// <param name="routeKey"></param>
        void Public<T>(T msg, string exchange, MsgProperties properties, params string[] routeKey) where T : class;
        /// <summary>
        /// 发布事务消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="properties"></param>
        /// <param name="routeKey"></param>
        void PublicTran(string msg, string exchange, MsgProperties properties, params string[] routeKey);
        /// <summary>
        /// 发布事务消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        void PublicTran(string msg, string exchange, params string[] routeKey);
        /// <summary>
        /// 发布事务消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="routeKey"></param>
        void PublicTran<T>(T msg, string exchange, params string[] routeKey) where T : class;
        /// <summary>
        /// 发布事务消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="exchange"></param>
        /// <param name="properties"></param>
        /// <param name="routeKey"></param>
        void PublicTran<T>(T msg, string exchange, MsgProperties properties, params string[] routeKey) where T : class;

        /// <summary>
        /// 获取订阅
        /// </summary>
        /// <param name="exchange"></param>
        /// <returns></returns>
        ISubscribe Subscribe(string exchange, Action<ISubscribe, ISubEventArgs> action, bool isAutoAck = true, string exchangeType = "direct");
    }
}