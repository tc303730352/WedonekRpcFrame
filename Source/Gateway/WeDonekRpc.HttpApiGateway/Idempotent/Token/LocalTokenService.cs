using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Token
{
    [IocName("Local")]
    internal class LocalTokenService : ITokenIdempotent
    {
        private readonly IIdentityService _Service;
        private readonly IIdempotentConfig _Config;
        private readonly Timer _Timer;
        private readonly ConcurrentDictionary<string, int> _TokenCache = new ConcurrentDictionary<string, int>();
        public LocalTokenService (IIdentityService service, IIdempotentConfig config)
        {
            this._Config = config;
            this._Service = service;
            this._Timer = new Timer(this._ClearToken, null, 1000, 1000);
        }
        public StatusSaveType SaveType => StatusSaveType.Local;
        private void _ClearToken (object state)
        {
            if (this._TokenCache.IsEmpty)
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime - this._Config.Expire;
            string[] keys = this._TokenCache.Where(a => a.Value <= time).Select(a => a.Key).ToArray();
            if (keys.Length > 0)
            {
                keys.ForEach(a => this._TokenCache.TryRemove(a, out _));
            }
        }

        public string ApplyToken ()
        {
            string token = this._Service.CreateId().ToString();
            return this._TokenCache.TryAdd(token, HeartbeatTimeHelper.HeartbeatTime) ? token : this.ApplyToken();
        }
        public bool SubmitToken (string tokenId)
        {
            return this._TokenCache.TryRemove(tokenId, out _);
        }

        public void Dispose ()
        {
            this._Timer.Dispose();
        }
    }
}
