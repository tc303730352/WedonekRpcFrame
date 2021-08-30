using System;

using RpcCacheClient.Interface;

using StackExchange.Redis;

using RpcHelper;
namespace RpcCacheClient.Redis
{
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class SubscriberController<T> : DataSyncClass, ISubscriberController
        {
                public SubscriberController(string name, Action<T> notice)
                {
                        this._NoticeCallBack = notice;
                        this.SubName = name;
                }

                public string SubName
                {
                        get;
                }
                private readonly Action<T> _NoticeCallBack = null;
                private ISubscriber _Subscriber = null;
                protected override bool SyncData()
                {
                        this._Subscriber = RedisHelper.GetSubscriber(this.SubName);
                        this._Subscriber.Subscribe(this.SubName, (a, msg) =>
                        {
                                if (msg.IsNull || msg.IsNullOrEmpty)
                                {
                                        return;
                                }
                                else if (RedisTools.GetT<T>(msg, out T data))
                                {
                                        this._NoticeCallBack.Invoke(data);
                                }
                        });
                        if (!this._Subscriber.IsConnected())
                        {
                                this.Error = "redis.con.error";
                                return false;
                        }
                        return true;
                }

                public override void Dispose()
                {
                        base.Dispose();
                        if (this._Subscriber != null && this._Subscriber.IsConnected())
                        {
                                this._Subscriber.Unsubscribe(this.SubName);
                        }
                }
        }
}
