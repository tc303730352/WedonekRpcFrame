using System;
using System.Collections.Generic;

using RabbitMQ.Client;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Rabbitmq.Model
{
        public class MsgReplyAddress
        {
                public string ExchangeName { get; }

                public string ExchangeType { get; }
                public string RoutingKey { get; }

                internal PublicationAddress ToPublicationAddress ()
                {
                        return new PublicationAddress (this.ExchangeType, this.ExchangeName, this.RoutingKey);
                }
        }

        public class MsgProperties
        {
                //static MsgProperties()
                //{
                //        IMapperCollect mapper = RpcClient.Unity.Resolve<IMapperCollect>();
                //        mapper.Config.ConvertUsing<MsgReplyAddress, PublicationAddress> (a => {

                //                return a?.ToPublicationAddress ();
                //        });
                //}
                public string ContentType
                {
                        get;
                        set;
                }
                public bool IsLasting
                {
                        get;
                        set;
                }
                public int Expiration
                {
                        get;
                        set;
                }
                public string AppId { get; set; }
                public string ClusterId { get; set; }
                public string ContentEncoding { get; set; }
                public string CorrelationId { get; set; }
                public IDictionary<string, object> Headers { get; set; }
                public string MessageId { get; set; }
                public bool Persistent { get; set; }
                public byte Priority { get; set; }
                public string ReplyTo { get; set; }
                public MsgReplyAddress ReplyToAddress { get; set; }
                public string Type { get; set; }
                public string UserId { get; set; }

                internal void InitProperties (IBasicProperties props)
                {
                        props.ContentType = this.ContentType;
                        props.UserId = this.UserId;
                        props.MessageId = this.MessageId;
                        props.AppId = this.AppId;
                        props.ClusterId = this.ClusterId;
                        props.CorrelationId = this.CorrelationId;
                        props.Headers = this.Headers;
                        if (this.ReplyToAddress != null)
                        {
                                props.ReplyToAddress = this.ReplyToAddress.ToPublicationAddress ();
                        }
                        props.Type = this.Type;
                        if (this.Expiration != 0)
                        {
                                props.Expiration = this.Expiration.ToString ();
                        }
                        props.ReplyTo = this.ReplyTo;
                        props.Priority = this.Priority;
                        props.Persistent = this.Persistent;
                        if (this.ContentEncoding == null)
                        {
                                props.ContentEncoding = "utf-8";
                        }
                        if (this.IsLasting)
                        {
                                props.DeliveryMode = 2;
                        }
                        else
                        {
                                props.DeliveryMode = 1;
                        }
                        props.Timestamp = new AmqpTimestamp (DateTime.Now.ToLong ());
                }
        }
}
