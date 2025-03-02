namespace WeDonekRpc.SqlSugar.Config
{
    public class ConMoreSet
    {
        public bool PgSqlIsAutoToLower { get; set; }= true;

        public bool IsAutoRemoveDataCache { get; set; } = false;

        public bool IsWithNoLockQuery { get; set; } = true;

        public bool DisableNvarchar { get; set; } = false;

        public bool DisableMillisecond { get; set; } = false;

        public int DefaultCacheDurationInSeconds { get; set; }

        public bool? TableEnumIsString { get; set; } = false;

        public DateTime? DbMinDate { get; set; } = Convert.ToDateTime("1900-01-01");

    }
}
