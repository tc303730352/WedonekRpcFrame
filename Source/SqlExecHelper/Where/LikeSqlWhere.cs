using System.Data;

namespace SqlExecHelper
{
        /// <summary>
        /// 执行Like查询参数
        /// </summary>
        public class LikeSqlWhere : BasicSqlWhere
        {
                public LikeSqlWhere(string name, int size, bool isNot = false) : this(name, size, LikeQueryType.全, isNot)
                {
                }
                public LikeSqlWhere(string name, string value, bool isNot = false) : this(name, value.Length, LikeQueryType.全, isNot)
                {
                        this.Value = value;
                }
                public LikeSqlWhere(string name, string value, LikeQueryType type, bool isNot = false) : this(name, value.Length, type, isNot)
                {
                        this.Value = value;
                }
                public LikeSqlWhere(string name, int size, LikeQueryType type) : this(name, size, type, false)
                {
                }
                public LikeSqlWhere(string name, int size, LikeQueryType type, bool isNot) : base(name, SqlDbType.NVarChar, isNot ? "not like" : "like")
                {
                        this._LikeType = type;
                        this.Size = type == LikeQueryType.自定义 ? size : type == LikeQueryType.全 ? size + 2 : size + 1;
                }
                private readonly LikeQueryType _LikeType = LikeQueryType.全;

                protected sealed override object _FormatValue(object val)
                {
                        if (this._LikeType == LikeQueryType.自定义)
                        {
                                return val;
                        }
                        if (this._LikeType == LikeQueryType.全)
                        {
                                return val.ToString().Connect("%", "%");
                        }
                        else if (this._LikeType == LikeQueryType.左)
                        {
                                return val.ToString().Left("%");
                        }
                        else
                        {
                                return val.ToString().Right("%");
                        }
                }
        }
}
