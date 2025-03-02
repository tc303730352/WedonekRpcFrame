using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.Client.FileUp
{
    public interface ISocketFileUpEvent<Result> : IStreamAllot
    {
        IFileUpEvent<Result> UpEvent { get; set; }
        void Install (IocBuffer buffer);
    }
}