namespace SqlExecHelper.Interface
{
        public interface IOrderBy
        {
                string Table
                {
                        get;
                }
                string Column
                {
                        get;
                }
                bool IsDesc
                {
                        get;
                }

                string GetOrderBy(ISqlRunConfig config);
        }
}
