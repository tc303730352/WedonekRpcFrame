using System.Data;

namespace SqlExecHelper.Param
{
        public class WhereSqlParameter : BasicParameter
        {
                public WhereSqlParameter(string name, SqlDbType type) : base(type)
                {
                        this.ColumnName = name;
                }

                public WhereSqlParameter(string name, SqlDbType type, int size) : base(type, size)
                {
                        this.ColumnName = name;
                }
                public WhereSqlParameter(string name, SqlDbType type, object value, int size) : base(type, value, size)
                {
                        this.ColumnName = name;
                }
                public WhereSqlParameter(string name, SqlDbType type, object value) : base(type, value)
                {
                        this.ColumnName = name;
                }
                protected string ColumnName
                {
                        get;
                        private set;
                }
                public override void InitParam(ISqlRunConfig config)
                {
                        this.ColumnName = config.FormatName(this.ColumnName);
                        base.InitParam(config);
                }
        }
}
