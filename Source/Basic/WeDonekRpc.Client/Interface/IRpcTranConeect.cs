using WeDonekRpc.Model.Tran.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 事务信号槽
    /// </summary>
    internal interface IRpcTranConeect
    {
        void BeginTran(CurTranState state);
    }
}
