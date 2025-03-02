using System.Collections.Generic;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Model
{
    internal class RequestState : IState
    {
        private readonly Dictionary<string, dynamic> _states = [];

        public void Add<T> (string key, T value)
        {
            this._states.Add(key, value);
        }
        public bool ContainsKey (string key)
        {
            return this._states.ContainsKey(key);
        }
        public void AddOrSet<T> (string key, T value)
        {
            this._states.AddOrSet(key, value);
        }

        public T GetOrDefault<T> (string key)
        {
            return this._states.GetValueOrDefault(key, default(T));
        }

        public T GetOrDefault<T> (string key, T def)
        {
            return this._states.GetValueOrDefault(key, def);
        }

        public void Set<T> (string key, T value)
        {
            if (this._states.ContainsKey(key))
            {
                throw new ErrorException("http.request.state.key.reapeat");
            }
            this._states[key] = value;
        }

        public bool TryAdd<T> (string key, T value)
        {
            return this._states.TryAdd(key, value);
        }
        public T Get<T> (string key)
        {
            return this._states[key];
        }
        public bool TryGet<T> (string key, out T value)
        {
            if (this._states.TryGetValue(key, out dynamic val))
            {
                value = val;
                return true;
            }
            value = default;
            return false;
        }
    }
}
