using System.Collections.Generic;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Modular.Accredit
{
    [IocName("AccreditService")]
    internal class AccreditExtendService : IRpcExtendService
    {
        private readonly IAccreditController _Service;

        public AccreditExtendService (IAccreditController service)
        {
            this._Service = service;
        }
        public void Load (IRpcService service)
        {
            service.LoadExtend += this._Service_LoadExtend;
            service.ReceiveEnd += this._Service_ReceiveEnd;
            service.ReceiveMsg += this._Service_ReceiveMsg;
        }
        private void _Service_ReceiveMsg (IMsg msg)
        {
            if (msg.Extend != null && msg.Extend.TryGetValue("AccreditId", out string val))
            {
                this._Service.SetAccreditId(val);
            }
            else
            {
                this._Service.SetAccreditId(null);
            }
        }

        private void _Service_LoadExtend (string dictate, ref Dictionary<string, string> extend)
        {
            if (this._Service.AccreditId.IsNotNull())
            {
                extend.Add("AccreditId", this._Service.AccreditId);
            }
        }
        private void _Service_ReceiveEnd (IMsg msg, TcpRemoteReply reply)
        {
            this._Service.ClearAccredit();
        }
    }
}
