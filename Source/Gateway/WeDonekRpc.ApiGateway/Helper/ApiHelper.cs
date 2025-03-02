using System.Reflection;
using System.Text;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ApiGateway.Helper
{
    public class ApiHelper
    {

        private static string _GetApiRouteFormat (ApiRouteName rname, IApiConfig config, IModular modular)
        {
            if (rname != null && rname.IsPath)
            {
                return rname.Value;
            }
            return _GetApiRouteFormat(config, modular);
        }
        private static string _GetApiRouteFormat (IApiConfig config, IModular modular)
        {
            if (config.Route != null && config.Route.IsPath)
            {
                return config.Route.Value;
            }
            return modular.ApiRouteFormat;
        }
        private static string _GetControllerName (IApiConfig config)
        {
            if (config.Route != null && config.Route.IsPath == false)
            {
                return config.Route.Value;
            }
            return _FormatApiName(config.Type.Name);
        }
        private static string _GetMethodName (ApiRouteName rname, MethodInfo method)
        {
            if (rname != null && rname.IsPath == false)
            {
                return rname.Value;
            }
            return method.Name;
        }
        public static string FormatUri (string uri)
        {
            if (uri.IndexOf('{') != -1)
            {
                MsgSource source = RpcClient.CurrentSource;
                StringBuilder str = new StringBuilder(uri);
                _ = str.Replace("{serverId}", source.ServerId.ToString());
                _ = str.Replace("{rpcMerId}", source.RpcMerId.ToString());
                _ = str.Replace("{sysGroup}", source.SysGroup);
                _ = str.Replace("{systemType}", source.SystemType);
                return str.ToString();
            }
            return uri;
        }
        public static string GetApiPath (IApiConfig config, IModular modular, MethodInfo method, out bool isRegex)
        {
            ApiRouteName rname = (ApiRouteName)method.GetCustomAttribute(ApiPublicDict.ApiRouteName);
            if (config.Route != null && config.Route.IsRegex)
            {
                isRegex = true;
            }
            else if (rname != null && rname.IsRegex)
            {
                isRegex = true;
            }
            else
            {
                isRegex = false;
            }
            string format = _GetApiRouteFormat(rname, config, modular);
            if (format.IndexOf('{') != -1)
            {
                StringBuilder str = new StringBuilder(format);
                _ = str.Replace("{modular}", modular.ServiceName);
                _ = str.Replace("{controller}", _GetControllerName(config));
                _ = str.Replace("{name}", _GetMethodName(rname, method));
                _ = str.Replace("//", "/");
                return str.ToString();
            }
            return format;
        }
        public static string GetApiPath (IApiConfig config, IModular modular, out bool isRegex)
        {
            isRegex = config.Route != null && config.Route.IsRegex;
            string format = _GetApiRouteFormat(config, modular);
            if (format.IndexOf('{') != -1)
            {
                StringBuilder str = new StringBuilder(format);
                _ = str.Replace("{modular}", modular.ServiceName);
                _ = str.Replace("//", "/");
                return str.ToString();
            }
            return format;
        }

        private static string _FormatApiName (string name)
        {
            name = name.ToLower();
            if (name.EndsWith("api") && name != "api")
            {
                name = name.Remove(name.Length - 3);
            }
            else if (name.EndsWith("controller"))
            {
                name = name.Remove(name.Length - 10);
            }
            else if (name.EndsWith("service"))
            {
                name = name.Remove(name.Length - 7);
            }
            return name;
        }
    }
}
