using System;

namespace RpcHelper.Config
{
        public interface IConfigCollect
        {
                event Action<IConfigCollect,string> RefreshEvent;
                void Clear();
                string this[string name] { get; }
                dynamic this[string name, Type type] { get; }

                string GetValue(string name);
                T GetValue<T>(string name);
                T GetValue<T>(string name, T def);
                void SetConfig(string name, string value, short prower = 0);
                void SetJson(string name, string json, short prower = 0);
                void SetConfig<T>(string name, T value, short prower = 0) where T : class;
        }
}