namespace SqlExecHelper
{
        public interface ISqlJoinWhere : ISqlLinkWhere
        {
                string Table
                {
                        get;
                        set;
                }
        }
}
