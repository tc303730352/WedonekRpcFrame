using System.Data;

namespace SqlExecHelper.Insert
{
        public class TableColumn
        {
                public TableColumn(string name, SqlDbType type, int size)
                {
                        this.Name = name;
                        this.SqlDbType = type;
                        this.Size = size;
                }
                public TableColumn(string name, SqlDbType type)
                {
                        this.Name = name;
                        this.SqlDbType = type;
                }
                public string Name
                {
                        get;
                }
                public SqlDbType SqlDbType
                {
                        get;
                }

                public int Size
                {
                        get;
                }
                public bool IsIdentify
                {
                        get;
                        set;
                }
                public bool IsNull { get; set; }
                internal DataColumn GetColumn()
                {
                        return new DataColumn(this.Name, SqlToolsHelper.GetType(this.SqlDbType));
                }


        }
}
