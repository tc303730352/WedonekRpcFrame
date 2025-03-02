using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using static CSRedis.CSRedisClient;
namespace WeDonekRpc.CacheClient.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SubscriberController<T> : DataSyncClass, ISubscriberController
    {
        private SubscribeObject _Subscriber;
        private readonly Type _Type;
        public SubscriberController (string name, Action<T> notice)
        {
            this._Type = typeof(T);
            this._NoticeCallBack = notice;
            this.SubName = name;
        }

        public string SubName
        {
            get;
        }
        private readonly Action<T> _NoticeCallBack = null;

        protected override void SyncData ()
        {
            this._Subscriber = RedisHelper.Subscribe((this.SubName, this._SubMsg));
            if (this._Subscriber.IsUnsubscribed)
            {
                throw new ErrorException("cache.redis.subscribe.fail");
            }
        }

        private void _SubMsg (SubscribeMessageEventArgs args)
        {
            string msg = args.Body;
            if (msg.IsNull())
            {
                return;
            }
            T res = StringParseTools.Parse(msg, this._Type);
            this._NoticeCallBack(res);
        }

        public override void Dispose ()
        {
            base.Dispose();
            this._Subscriber?.Unsubscribe();
        }
    }
}
