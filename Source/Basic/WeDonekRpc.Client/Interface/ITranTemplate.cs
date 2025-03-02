using System;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    [Attr.IgnoreIoc]
    internal interface ITranTemplate
    {
        string TranName { get; }
        RpcTranMode TranMode { get; }
        IDisposable BeginTran (ICurTran tran);
        void Commit (ICurTran tran);
        void Rollback (ICurTran tran);
    }
}