using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

namespace WeDonekRpc.Helper.Http
{
    /// <summary>
    /// 方案修改
    /// </summary>
    /// <param name="old">当前请求配置</param>
    /// <param name="newHandler">新的请求配置</param>
    /// <returns>是否修改</returns>
    public delegate bool ClientHandlerUpdate ( HttpClientHandler old, HttpClientHandler newHandler );
    public static class HttpClientFactory
    {
        private static ConcurrentDictionary<string, HttpClientHandler> _Handlers = new ConcurrentDictionary<string, HttpClientHandler>();
        private static HttpClientConfig _DefConfig;
        private static HttpClientHandler _DefaultHandler;
        static HttpClientFactory ()
        {
            _DefConfig = new HttpClientConfig();
            _DefaultHandler = _DefConfig.Create();
        }
        /// <summary>
        /// 设置默认Http配置
        /// </summary>
        /// <param name="config"></param>
        public static void SetDefConfig ( Action<HttpClientConfig> config )
        {
            config(_DefConfig);
            HttpClientHandler old = _DefaultHandler;
            _DefaultHandler = _DefConfig.Create();
            old.Dispose();
        }
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        public static HttpClient Create ()
        {
            return new HttpClient(_DefaultHandler, false);
        }
        /// <summary>
        /// 创建对应方案名的客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ErrorException"></exception>
        public static HttpClient Create ( string name )
        {
            if ( _Handlers.TryGetValue(name, out HttpClientHandler handler) )
            {
                return new HttpClient(handler, false);
            }
            throw new ErrorException("http.client.name.not.find");
        }
        /// <summary>
        /// 修改Http客户端方案
        /// </summary>
        /// <param name="name"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public static bool SetClient ( string name, ClientHandlerUpdate update )
        {
            HttpClientHandler old = _Handlers.GetValueOrDefault(name);
            HttpClientHandler handler = _DefConfig.Create();
            if ( !update(old, handler) )
            {
                return false;
            }
            else if ( old == null )
            {
                return _Handlers.TryAdd(name, handler);
            }
            else if ( _Handlers.TryUpdate(name, handler, old) )
            {
                old.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 修改请求配置方案
        /// </summary>
        /// <param name="name">方案名</param>
        /// <param name="config">方案信息</param>
        /// <returns></returns>
        public static bool SetClient ( string name, HttpClientConfig config )
        {
            HttpClientHandler handler = config.Create();
            if ( !_Handlers.TryGetValue(name, out HttpClientHandler old) )
            {
                return _Handlers.TryAdd(name, handler);
            }
            else if ( _Handlers.TryUpdate(name, handler, old) )
            {
                old.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 使用默认配置设置对应请求方案
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetClient ( string name )
        {
            return SetClient(name, _DefConfig);
        }
        /// <summary>
        /// 修改请求配置方案
        /// </summary>
        /// <param name="name">方案名</param>
        /// <param name="option">方案信息</param>
        /// <returns></returns>
        public static bool SetClient ( string name, Action<HttpClientHandler> option )
        {
            HttpClientHandler handler = _DefConfig.Create();
            option(handler);
            if ( !_Handlers.TryGetValue(name, out HttpClientHandler old) )
            {
                return _Handlers.TryAdd(name, handler);
            }
            else if ( _Handlers.TryUpdate(name, handler, old) )
            {
                old.Dispose();
                return true;
            }
            return false;
        }
        public static bool AddClient ( string name )
        {
            if ( _Handlers.ContainsKey(name) )
            {
                return false;
            }
            HttpClientHandler handler = _DefConfig.Create();
            if ( !_Handlers.TryAdd(name, handler) )
            {
                throw new ErrorException("http.client.add.fail");
            }
            return true;
        }
        public static bool AddClient ( string name, HttpClientConfig config )
        {
            if ( _Handlers.ContainsKey(name) )
            {
                return false;
            }
            HttpClientHandler handler = config.Create();
            if ( !_Handlers.TryAdd(name, handler) )
            {
                throw new ErrorException("http.client.add.fail");
            }
            return true;
        }
        public static bool AddClient ( string name, Action<HttpClientHandler> action )
        {
            if ( _Handlers.ContainsKey(name) )
            {
                return false;
            }
            HttpClientHandler handler = _DefConfig.Create();
            action(handler);
            if ( !_Handlers.TryAdd(name, handler) )
            {
                throw new ErrorException("http.client.add.fail");
            }
            return true;
        }
        public static void Remove ( string name )
        {
            if ( _Handlers.TryRemove(name, out HttpClientHandler handler) )
            {
                handler.Dispose();
            }
        }
    }
}
