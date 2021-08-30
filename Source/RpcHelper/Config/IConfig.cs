using System;

using Newtonsoft.Json.Linq;
namespace RpcHelper.Config
{
        internal interface IConfig
        {
                string GetJson(string path);
                bool SetConfig(JToken jToken, IConfigItem configItem);
                bool SetConfig(string key, object value, IConfigItem parent);

                bool SetConfig(string key, Type type, object value, IConfigItem parent);
        }
}
