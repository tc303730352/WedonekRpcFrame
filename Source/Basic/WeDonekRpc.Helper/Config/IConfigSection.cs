using System;

namespace WeDonekRpc.Helper.Config
{
    public interface IConfigSection
    {
        /// <summary>
        /// 下级所有键
        /// </summary>
        string[] Keys { get; }
        /// <summary>
        /// 项名称
        /// </summary>
        string ItemName { get; }
        /// <summary>
        /// 添加刷新事件
        /// </summary>
        /// <param name="action"></param>
        void AddRefreshEvent (Action<IConfigSection, string> action);

        void RemoveRefreshEvent (Action<IConfigSection, string> action);
        /// <summary>
        /// 获取当前项值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetValue<T> ();
        /// <summary>
        /// 获取当前项的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="def"></param>
        /// <returns></returns>
        T GetValue<T> (T def);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置值</returns>
        string GetValue (string name);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="name">配置名</param>
        /// <returns>配置值</returns>
        T GetValue<T> (string name);
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="name">配置名</param>
        /// <param name="def">默认值</param>
        /// <returns>配置值</returns>
        T GetValue<T> (string name, T def);
        /// <summary>
        /// 获取下级配置节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConfigSection GetSection (string name);
    }
}
