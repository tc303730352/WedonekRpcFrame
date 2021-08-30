using SqlExecHelper.Column;

namespace SqlExecHelper.Join
{
        public class JoinColumn
        {
                public string Column
                {
                        get;
                        set;
                }
                public string Table
                {
                        get;
                        set;
                }
                public SqlFuncType? FuncType { get; set; }

                internal SqlColumn[] GetQueryColumn(JoinTable main, JoinTable[] tables)
                {
                        string col = null;
                        if (this.Table != null)
                        {
                                JoinTable table = tables.Find(b => b.TableName == this.Table);
                                col = table.Config.FormatName(this.Column);
                        }
                        else
                        {
                                col = main.Config.FormatName(this.Column);
                        }
                        if (this.FuncType.HasValue)
                        {
                                return new SqlColumn[]
                                {
                                         new SqlColumn(col,this.FuncType.Value,this.Column)
                                };
                        }
                        return new SqlColumn[]
                        {
                               new SqlColumn(col,this.Column)
                        };
                }
        }
}
