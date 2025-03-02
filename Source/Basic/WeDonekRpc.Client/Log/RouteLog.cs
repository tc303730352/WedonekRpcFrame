using System;
using System.Reflection;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Log
{
    internal class RouteLog
    {
        public static void AddRouteLog ( MethodInfo method, string name, Type source, string error )
        {
            new LogInfo("路由注册错误日志", "Rpc_RouteLog", LogGrade.Information)
                {
                    {"error",error },
                    {"Name",name },
                    {"Method",method.ToString() },
                    {"Source",source.FullName }
                }.Save();
        }
        public static void AddRouteLog ( MethodInfo method, string name, Type source )
        {
            new LogInfo("消息路由注册!", "Rpc_RouteLog", LogGrade.Information)
                {
                        { "Name",name},
                        { "Method",method.ToString()},
                        { "Source",source.FullName}
                }.Save();
        }
    }
}
