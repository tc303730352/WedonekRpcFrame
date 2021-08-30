namespace SqlExecHelper
{
        public interface ISqlTableWhere : ISqlWhere
        {
                string Table { get; set; }
        }
}
