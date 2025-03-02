using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Tran.TwoPc
{
    internal class RpcPcTranTemplate : ITranTemplate
    {
        private static readonly IIocService _Ioc;
        static RpcPcTranTemplate ()
        {
            _Ioc = RpcClient.Ioc;
        }
        public RpcPcTranTemplate (string tranName)
        {
            this.TranName = tranName;
        }
        public string TranName { get; }

        public RpcTranMode TranMode => RpcTranMode.TwoPC;

        public IDisposable BeginTran (ICurTran tran)
        {
            IRpcTwoPcTran dbTran = _Ioc.Resolve<IRpcTwoPcTran>(this.TranName);
            return dbTran.BeginTran(tran);
        }

        public void Commit (ICurTran cur)
        {
            IRpcTwoPcTran dbTran = _Ioc.Resolve<IRpcTwoPcTran>(this.TranName);
            dbTran.Commit(cur);
        }

        public void Rollback (ICurTran cur)
        {
            IRpcTwoPcTran dbTran = _Ioc.Resolve<IRpcTwoPcTran>(this.TranName);
            dbTran.Rollback(cur);
        }
    }
}
