using System.Collections.Concurrent;
using WeDonekRpc.Client.Resource;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Controller
{

    public class ErrorController : DataSyncClass
    {
        internal ErrorController (string code)
        {
            this.ErrorCode = code;
        }
        public string ErrorCode
        {
            get;
        }
        public long ErrorId { get; private set; }

        private readonly ConcurrentDictionary<string, string> _Lang = new ConcurrentDictionary<string, string>();

        protected override void SyncData ()
        {
            this.ErrorId = ErrorRemote.GetErrorId(this.ErrorCode);
        }

        public string GetErrorText (string lang)
        {
            if (this._Lang.TryGetValue(lang, out string text))
            {
                return text;
            }
            text = ErrorRemote.GetErrorMsg(lang, this.ErrorCode);
            if (text == null)
            {
                text = string.Empty;
            }
            _ = this._Lang.TryAdd(lang, text);
            return text;
        }
    }
}
