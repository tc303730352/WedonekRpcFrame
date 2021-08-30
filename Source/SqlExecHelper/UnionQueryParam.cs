namespace SqlExecHelper
{
        public class UnionQueryParam
        {
                public string Table
                {
                        get;
                        set;
                }

                public ISqlWhere[] Where
                {
                        get;
                        set;
                }
        }
}
