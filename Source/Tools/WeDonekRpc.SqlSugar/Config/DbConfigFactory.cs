using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.SqlSugar.Config
{
    internal class DbConfigFactory : IDBConfig
    {
        private static readonly string[] _AllowSetPro = new string[]
        {
            "IsAutoCloseConnection",
            "InitKeyType",
             "LanguageType",
             "SlaveConnectionConfigs",
             "IndexSuffix"
        };
        private List<ConnectionConfig> _Config;
        private DefDbConfig[] _DefConfig;
        private readonly ISugarConfig _SugarConfig;
        public DbConfigFactory (IConfigCollect config, ISugarConfig sugarConfig)
        {
            this._SugarConfig = sugarConfig;
            IConfigSection section = config.GetSection("db:SqlSugar");
            section.AddRefreshEvent(this._Refresh);
        }
        private void _Init ()
        {
            if (this._DefConfig.IsNull())
            {
                throw new ErrorException("sqlSugar.con.no.config");
            }
            this._Config = this._DefConfig.Select(c =>
            {
                ConnectionConfig config = new ConnectionConfig
                {
                    IndexSuffix = c.IndexSuffix,
                    ConnectionString = c.ConnectionString,
                    ConfigId = c.ConfigId ?? "default",
                    IsAutoCloseConnection = c.IsAutoCloseConnection,
                    InitKeyType = c.InitKeyType,
                    DbLinkName = c.DbLinkName,
                    DbType = c.DbType,
                    LanguageType = c.LanguageType
                };
                ConMoreSet moreSet = c.MoreSettings;
                if (moreSet == null)
                {
                    moreSet = new ConMoreSet();
                }
                config.MoreSettings = new ConnMoreSettings
                {
                    DbMinDate = moreSet.DbMinDate,
                    DefaultCacheDurationInSeconds = moreSet.DefaultCacheDurationInSeconds,
                    PgSqlIsAutoToLower = moreSet.PgSqlIsAutoToLower,
                    TableEnumIsString = moreSet.TableEnumIsString,
                    DisableMillisecond = moreSet.DisableMillisecond,
                    DisableNvarchar = moreSet.DisableNvarchar,
                    IsAutoRemoveDataCache = moreSet.IsAutoRemoveDataCache,
                    IsWithNoLockQuery = moreSet.IsWithNoLockQuery
                };
                if (!c.SlaveConnectionConfigs.IsNull())
                {
                    config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>(c.SlaveConnectionConfigs.Length);
                    c.SlaveConnectionConfigs.ForEach(d =>
                    {
                        config.SlaveConnectionConfigs.Add(new SlaveConnectionConfig
                        {
                            ConnectionString = d.ConnectionString,
                            HitRate = d.HitRate
                        });
                    });
                }
                config.AopEvents = new AopService(this._SugarConfig);
                return config;
            }).ToList();
            DbType db = this._Config.GroupBy(a => a.DbType).Select(a => new
            {
                a.Key,
                count = a.Count()
            }).OrderByDescending(a => a.count).Select(a => a.Key).FirstOrDefault();
            if (db == DbType.SqlServer)
            {
                Tools.SetDefaultGuidType(SequentialGuidType.SequentialAtEnd);
            }
            else if (db == DbType.MySql || db == DbType.PostgreSQL)
            {
                Tools.SetDefaultGuidType(SequentialGuidType.SequentialAsString);
            }
            else if (db == DbType.Oracle)
            {
                Tools.SetDefaultGuidType(SequentialGuidType.SequentialAsBinary);
            }
        }

        private void _Refresh (IConfigSection config, string name)
        {
            if (name == string.Empty || _AllowSetPro.Contains(name))
            {
                this._DefConfig = config.GetValue<DefDbConfig[]>();
                this._Init();
            }
        }
        public List<ConnectionConfig> Configs
        {
            get => this._Config;
        }
    }
}
