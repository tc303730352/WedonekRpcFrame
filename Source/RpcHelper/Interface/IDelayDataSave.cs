namespace RpcHelper
{
        public interface IDelayDataSave<T> : System.IDisposable
        {
                void AddData(T[] data);
                void AddData(T data);

                void SaveData();
        }
}