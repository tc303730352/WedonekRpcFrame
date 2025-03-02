using System;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.Tran
{
    public class TranScope : ITranScope
    {
        private readonly IDisposable _Source;
        public TranScope (IDisposable source)
        {
            this._Source = source;
        }
        public void Dispose ()
        {
            this._Source.Dispose();
        }
    }
}
