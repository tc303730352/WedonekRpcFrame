using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Tran
{
    [Attr.IocName("RpcTran")]
    internal class RpcTranExtendService : IRpcExtendService
    {
        public void Load(IRpcService service)
        {
            service.ReceiveEnd += this.Service_ReceiveEnd;
        }

        private void Service_ReceiveEnd(IMsg msg, TcpRemoteReply reply)
        {
            RpcTranService.ReceiveEnd();
        }
    }
}
