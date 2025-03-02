using SqlSugar;

namespace WeDonekRpc.SqlSugar
{
    public interface IDBConfig
    {
        List<ConnectionConfig> Configs { get; }
    }
}