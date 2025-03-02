using Newtonsoft.Json;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.SqlSugar.Config;
using WeDonekRpc.SqlSugar.Queue;
using WeDonekRpc.SqlSugar.Repository;

namespace WeDonekRpc.SqlSugar
{
    public class SqlSugarService
    {
        private static readonly JsonSerializerSettings _DefSettings = new JsonSerializerSettings
        {
            MaxDepth = 64,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            NullValueHandling = NullValueHandling.Ignore
        };
        public static void Init (IUnityContainer unity)
        {
            JsonConvert.DefaultSettings = () =>
            {
                return _DefSettings;
            };
            unity.RegisterSingle<IConfigCollect>(LocalConfig.Local);
            unity.RegisterSingle<ISugarConfig, Config.SugarConfig>();
            unity.Register<ITransactionService, Tran.LocalTransactionService>();
            unity.RegisterSingle<IDBConfig, DbConfigFactory>();
            unity.Register(typeof(IRepository<>), typeof(DefRepository<>));
            unity.Register(typeof(ISqlQueueService), typeof(DefSqlQueueService));
            unity.Register<ISqlClientFactory, SqlClientFactory>();
        }
    }
}
