using System.Data;

namespace SqlExecHelper
{
        public class SqlTableColumn
        {
                public SqlTableColumn(string name, SqlDbType dbType, int size)
                {
                        this.Name = name;
                        this.SqlDbType = dbType;
                        this.Size = size;
                }
                public SqlTableColumn(string name, SqlDbType dbType)
                {
                        this.Name = name;
                        this.SqlDbType = dbType;
                }
                /// <summary>
                /// 列名
                /// </summary>
                public string Name
                {
                        get;
                }

                /// <summary>
                /// 数据类型
                /// </summary>
                public SqlDbType SqlDbType
                {
                        get;
                }
                /// <summary>
                /// 数据大小
                /// </summary>
                public int Size
                {
                        get;
                }
                /// <summary>
                /// 设置方式
                /// </summary>
                public SqlSetType SetType
                {
                        get;
                        set;
                }

                /// <summary>
                /// 列类型
                /// </summary>
                public SqlColType ColType
                {
                        get;
                        set;
                } = SqlColType.通用;
                /// <summary>
                /// Decimal小数精度
                /// </summary>
                public short DecimalPoint { get; set; }

                /// <summary>
                /// 查询类型
                /// </summary>
                public QueryType QueryType { get; set; } = QueryType.等号;
                /// <summary>
                /// 是否允许空
                /// </summary>
                public bool IsNull { get; set; } = false;
                internal object FormatValue(object val)
                {
                        if (val == null && this.IsNull)
                        {
                                return null;
                        }
                        return SqlToolsHelper.FormatValue(this.SqlDbType, val, this.Size);
                }
        }
}
