using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Tran
{
    internal class DefTranTemplate : ITranTemplate
    {
        public string TranName => "DefTran";

        public RpcTranMode TranMode => RpcTranMode.NoReg;

        public IDisposable BeginTran (ICurTran tran)
        {
            return null;
        }

        public void Commit (ICurTran tran)
        {
        }

        public void Rollback (ICurTran tran)
        {
        }
    }
}
