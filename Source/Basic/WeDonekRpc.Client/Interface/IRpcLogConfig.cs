using WeDonekRpc.Client.Config;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Interface
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    public interface IRpcLogConfig
    {
        bool CheckIsRecord(string key, LogGrade grade, out LocalLogSet logSet);
        bool CheckIsRecord(string key,out LocalLogSet logSet);

        bool CheckIsRecord(string key,ErrorException e, out LocalLogSet logSet);
    }
}