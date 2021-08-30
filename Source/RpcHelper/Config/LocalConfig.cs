using System;
using System.IO;
using System.Text;
namespace RpcHelper.Config
{
        public class LocalConfig
        {
                public static readonly IConfigCollect Local = null;

                static LocalConfig()
                {
                        string path = Path.Combine(AppContext.BaseDirectory, "LocalConfig.json");
                        Local = new LocalConfigCollect(path);
                }

                public static IConfigCollect Create()
                {
                        return new ConfigCollect();
                }

                public static void SaveFile()
                {
                        string json = Local.GetValue("root");
                        string path = Path.Combine(AppContext.BaseDirectory, "LocalConfig.json");
                        Tools.WriteText(path, json, Encoding.UTF8);
                }
        }
}
