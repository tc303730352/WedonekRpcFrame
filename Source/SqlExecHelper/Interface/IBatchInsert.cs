namespace SqlExecHelper
{
        public interface IBatchInsert : IInsertTable
        {
                bool Save<T>(out T[] datas);

                bool Save<T>(string column, out T[] datas);
                bool Save(out int[] ids);

                bool Save(out long[] ids);
        }
}