using System;
using System.IO;
namespace WeDonekRpc.Helper.Config
{
    public class LocalConfig
    {
        public static readonly IConfigCollect Local = null;

        static LocalConfig ()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "LocalConfig.json");
            Local = new LocalConfigCollect(path);
        }

        public static IConfigCollect CreateConfig ()
        {
            return new LocalConfigCollect();
        }

        public static void SaveFile ()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "LocalConfig.json");
            SaveFile(path);
        }
        public static void SaveFile (string path)
        {
            Local.SaveFile(new FileInfo(path));
        }
    }
}
