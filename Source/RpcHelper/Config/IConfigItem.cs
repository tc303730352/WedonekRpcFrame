using Newtonsoft.Json.Linq;
namespace RpcHelper.Config
{
        internal interface IConfigItem
        {
                public string Path { get; }

                string ItemName { get; }

                short Prower { get; }
                IConfigItem Parent { get; }
                ItemType ItemType { get; }
                bool SetValue(string obj, ItemType itemType, short prower);
                bool SetValue(object obj, short prower);

                bool SetValue(JToken token, short prower);
                void Refresh();
        }
}