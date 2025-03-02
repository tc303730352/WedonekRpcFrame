using System.Threading;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.Modular.Model
{
        internal class DictateVisit
        {
                private int _Success;
                private int _Failure;
                public DictateVisit (string dictate)
                {
                        this.Dictate = dictate;
                }
                public string Dictate
                {
                        get;
                }

                public RpcDictateVisit Sync (int interval)
                {
                        int succ = Interlocked.Exchange (ref this._Success, 0);
                        int fail = Interlocked.Exchange (ref this._Failure, 0);
                        if (succ == 0 && fail == 0)
                        {
                                return null;
                        }
                        return new RpcDictateVisit
                        {
                                Dictate = this.Dictate,
                                Success = succ,
                                Failure = fail,
                                Concurrent = ( succ + fail ) / interval
                        };
                }
                public void Failure ()
                {
                        Interlocked.Add (ref this._Failure, 1);
                }
                public void Success ()
                {
                        Interlocked.Add (ref this._Success, 1);
                }
        }
}
