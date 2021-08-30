using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SqlExecHelper.Query
{
        internal class SqlUnionQuery : ISqlBasic
        {
                private readonly ISqlBasic[] _QueryList = null;
                public SqlUnionQuery(ISqlBasic[] query)
                {
                        this._QueryList = query;
                }
                private readonly bool _IsAll = true;
                public StringBuilder GenerateSql(out IDataParameter[] param)
                {
                        List<IDataParameter> list = new List<IDataParameter>();
                        StringBuilder sql = this._QueryList[0].GenerateSql(out IDataParameter[] p);
                        list.AddRange(p);
                        string cen = " union all (";
                        if (!this._IsAll)
                        {
                                cen = " union (";
                        }
                        for (int i = 1; i < this._QueryList.Length; i++)
                        {
                                sql.Append(cen);
                                sql.Append(this._QueryList[i].GenerateSql(out p));
                                list.AddRange(p);
                                sql.Append(")");
                        }
                        param = list.ToArray();
                        return sql;
                }
        }
}
