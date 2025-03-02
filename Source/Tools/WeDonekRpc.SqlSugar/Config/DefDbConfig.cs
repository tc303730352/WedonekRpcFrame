using SqlSugar;

namespace WeDonekRpc.SqlSugar.Config
{
    internal class DefDbConfig
    {
        /// <summary>
        /// 多租户ID
        /// </summary>
        public string ConfigId { get; set; }
        public string QueryLock { get; set; } = SqlWith.NoLock;

        public InitKeyType InitKeyType { get; set; } = InitKeyType.SystemTable;


        public DbType DbType { get; set; } = DbType.SqlServer;

        public string ConnectionString { get; set; }

        public bool IsAutoCloseConnection { get; set; } = true;

        public LanguageType LanguageType { get; set; } = LanguageType.Chinese;

        public DBConConfig[] SlaveConnectionConfigs { get; set; }

        public ConMoreSet MoreSettings { get; set; }

        public string IndexSuffix { get; set; }

        public string DbLinkName { get; set; }
    }
}
