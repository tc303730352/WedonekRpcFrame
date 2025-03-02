using WeDonekRpc.Helper.Config;
using System;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public interface ISysConfig
    {
        IConfigServer Config { get; }
        /// <summary>
        /// 获取配置集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConfigSection GetSection(string name);
        /// <summary>
        /// 添加配置刷新事件
        /// </summary>
        /// <param name="action"></param>
        void AddRefreshEvent(Action<IConfigServer, string> action);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConfigVal(string name);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetConfigVal<T>(string name);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        T GetConfigVal<T>(string name, T defValue);
        /// <summary>
        /// 覆盖配置刷新事件
        /// </summary>
        /// <param name="action"></param>
        void SetRefreshEvent(Action<IConfigServer, string> action);
        /// <summary>
        /// 设置本地配置服务程序
        /// </summary>
        /// <param name="config"></param>
        void SetLocalServer(IConfigServer config);
    }
}