using System.Collections.Generic;
using WeDonekRpc.Client.Rabbitmq.Model;

namespace WeDonekRpc.Client.Rabbitmq.Interface
{
    public interface ISubEventArgs
    {
        string AppId { get; }
        byte[] Body { get; }
        string ClusterId { get; }
        string ConsumerTag { get; }
        string CorrelationId { get; }
        ulong DeliveryTag { get; }
        string Exchange { get; }
        IDictionary<string, object> Headers { get; }
        string MessageId { get; }
        bool Redelivered { get; }
        string RoutingKey { get; }
        long SendTime { get; }
        string Type { get; }
        string UserId { get; }

        string GetValue ();
        T GetValue<T> () where T : class;
        void ReplyMsg ( string msg );
        void ReplyMsg ( string msg, MsgProperties properties );
        void ReplyMsg<T> ( T msg ) where T : class;
        void ReplyMsg<T> ( T msg, MsgProperties properties ) where T : class;
        void ReplyTranMsg ( string msg );
        void ReplyTranMsg ( string msg, MsgProperties properties );
        void ReplyTranMsg<T> ( T msg ) where T : class;
        void ReplyTranMsg<T> ( T msg, MsgProperties properties ) where T : class;
    }
}