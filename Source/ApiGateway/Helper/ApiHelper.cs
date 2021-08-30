using System.Text;

using ApiGateway.Interface;

namespace ApiGateway.Helper
{
        internal class ApiHelper
        {
                internal static string GetApiPath(IApiConfig config, IModular modular, string format)
                {
                        if (format.IndexOf('{') != -1)
                        {
                                StringBuilder str = new StringBuilder(format);
                                str.Replace("{service}", modular.ServiceName);
                                str.Replace("{controller}", _FormatApiName(config.Name));
                                str.Replace("//", "/");
                                return str.ToString();
                        }
                        return format;
                }
                internal static string GetApiPath(IApiConfig config, string name, IModular modular)
                {
                        StringBuilder str = new StringBuilder(modular.ApiRouteFormat);
                        str.Replace("{service}", modular.ServiceName);
                        str.Replace("{controller}", _FormatApiName(config.Name));
                        str.Replace("{name}", name);
                        str.Replace("//", "/");
                        return str.ToString();
                }
                private static string _FormatApiName(string name)
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
