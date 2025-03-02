using System.Reflection;
using System.Text;
using WeDonekRpc.Helper;

namespace GatewayBuildCli.Model
{
    internal class AssemblyBody
    {
        private readonly Assembly _Assembly;
        public AssemblyBody ( Assembly assembly )
        {
            this._Assembly = assembly;
        }
        private string _Namespace;
        private ApiServiceBody[] _Services;
        private string _RootName;
        public void Init ()
        {
            Type[] types = this._Assembly.GetTypes();
            types = types.FindAll(a => a.IsClass && ( a.GetCustomAttribute(ConstDic._IConfigType) != null || a.GetCustomAttribute(ConstDic._IBroadcastType) != null ) && a.BaseType != ConstDic._objectType);
            if ( types.Length == 0 )
            {
                return;
            }
            string name = this._Assembly.GetName().Name;
            this._RootName = Helper.GetRootName(name);
            this._Namespace = Helper.GetNamespace(name);
            string[] names = types.Distinct(a => a.Namespace);
            this._Services = names.ConvertAll(a =>
            {
                Type[] t = types.FindAll(c => c.Namespace == a);
                string sName = Helper.FormatNamespace(a, name, this._RootName);
                ApiServiceBody body = new ApiServiceBody(sName, this._Namespace, this._RootName);
                body.Init(t);
                return body;
            });

        }

        public void Save ( DirectoryInfo dir, FileInfo source )
        {
            if ( !dir.Exists )
            {
                dir.Create();
            }
            string path = Path.Combine(dir.FullName, "Reference");
            DirectoryInfo reDir = new DirectoryInfo(path);
            if ( !reDir.Exists )
            {
                reDir.Create();
            }
            string toPath = Path.Combine(path, source.Name);
            if ( !File.Exists(toPath) )
            {
                _ = source.CopyTo(toPath);
            }
            string dllDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reference");
            if ( Directory.Exists(dllDir) )
            {
                Helper.CopDir(new DirectoryInfo(dllDir), reDir);
            }
            string caspDir = Path.Combine(dir.FullName, this._Namespace);
            dir = new DirectoryInfo(caspDir);
            if ( dir.Exists )
            {
                dir.Delete(true);
            }
            dir.Create();
            string csproj = Path.Combine(dir.FullName, string.Concat(this._Namespace, ".csproj"));
            string casprojVal = string.Format(ConstDic.CsprojTemplate, source.Name, this._Namespace);
            Tools.WriteText(csproj, casprojVal, Encoding.UTF8);
            DirectoryInfo serviceDir = dir.CreateSubdirectory("Services");
            DirectoryInfo interDir = dir.CreateSubdirectory("Interface");
            this._Services.ForEach(a =>
            {
                a.Save(serviceDir, interDir);
            });
            DirectoryInfo apiDir = dir.CreateSubdirectory("Api");
            DirectoryInfo mode = dir.CreateSubdirectory("Model");
            this._Services.ForEach(a =>
            {
                ApiGatewayBody api = new ApiGatewayBody(a);
                api.Save(apiDir, mode);
            });
            GatewayConfigModel.Save(this._Namespace, dir, this._RootName);
        }

    }
}
