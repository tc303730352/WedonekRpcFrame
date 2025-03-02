namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IState
    {
        T Get<T>(string key);
        T GetOrDefault<T>(string key);
        T GetOrDefault<T>(string key, T def);
        bool TryAdd<T>(string key, T value);
        bool ContainsKey(string key);
        void Add<T>(string key, T value);

        void AddOrSet<T>(string key, T value);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value);
    }
    public interface IRequestState
    {
        /// <summary>
        /// 当前请求状态
        /// </summary>
        IState RequestState { get; }


    }
}
