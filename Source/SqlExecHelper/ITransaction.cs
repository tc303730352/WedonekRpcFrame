namespace SqlExecHelper
{
        public interface ITransaction : System.IDisposable
        {
                IDAL Source { get; }
                void Commit();
        }
}