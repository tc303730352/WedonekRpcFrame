using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar.Config;

namespace WeDonekRpc.SqlSugar
{
    public interface ISugarConfig
    {
        bool StringIsAutoEmpty { get; }
        bool IsLocalLog { get; }
        bool CheckIsRecord (string key, out LogConfig logSet);
        bool CheckIsRecord (string key, ErrorException e, out LogConfig logSet);
    }
}