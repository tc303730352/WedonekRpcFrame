using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Tran
{
    internal class TranSource : ITranSource
    {
        public TranSource ()
        {
        }
        public TranSource (string body)
        {
            this.Body = body;
        }
        public TranSource (string body, string extend)
        {
            this.Body = body;
            this.Extend = extend;
        }
        public string Body
        {
            get;
        }
        public string Extend
        {
            get;
            set;
        }

        public T GetBody<T> () where T : class
        {
            if (this.Body.IsNull())
            {
                return default;
            }
            return this.Body.Json<T>();
        }

        public T GetExtend<T> () where T : class
        {
            if (this.Extend.IsNull())
            {
                return default;
            }
            return this.Extend.Json<T>();
        }
    }
}
