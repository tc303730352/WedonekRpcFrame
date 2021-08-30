namespace SqlExecHelper
{
        public interface ISqlFactory
        {
                IDAL GetDAL();
                IDAL GetDAL(string conName);
        }
}