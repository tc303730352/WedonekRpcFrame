namespace RpcClient.Interface
{
        public interface IConfigServer : System.IDisposable
        {
                bool LoadConfig(out string error);
                string GetConfigVal(string name);
                T GetConfigVal<T>(string name);
                T GetConfigVal<T>(string name, T defValue);
        }
}
