using System;
using System.Collections.Generic;
using System.IO;

namespace WeDonekRpc.Helper.Config
{
    /// <summary>
    /// 配置集合
    /// </summary>
    public interface IConfigCollect
    {
        /// <summary>
        /// 获取下级所有键
        /// </summary>
        string[] GetKeys (string prefix);
        /// <summary>
        /// 刷新事件
        /// </summary>
        event Action<IConfigCollect, string> RefreshEvent;

        /// <summary>
        /// 获取配置集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConfigSection GetSection (string name);

        /// <summary>
        /// 清理配置项
        /// </summary>
        void Clear ();

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <returns>配置值</returns>
        string this[string name] { get; }

        /// <summary>
        /// 获取指定类型的配置项
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="type">配置值类型</param>
        /// <returns>配置指定类型的值</returns>
        dynamic this[string name, Type type] { get; }

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
        /// 设置配置值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="value">配置值</param>
        /// <param name="prower">权限值</param>
        void SetConfig (string name, string value, int prower = 0);

        /// <summary>
        /// 设置配置值
        /// </summary>
        /// <param name="name">配置名</param>
        /// <param name="json">配置值的JSON字符串</param>
        /// <param name="prower">权限值</param>
        void SetJson (string name, string json, int prower = 0);

        /// <summary>
        /// 设置配置值
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="name">配置名</param>
        /// <param name="value">配置的值</param>
        /// <param name="prower">权限</param>
        void SetConfig<T> (string name, T value, int prower = 0) where T : class;

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="prower"></param>
        void SetValue<T> (string name, T value, int prower = 0) where T : struct;

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file"></param>
        void SaveFile (FileInfo file);

        /// <summary>
        /// 获取所有配置项
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ConfigItemModel> GetConfigItem ();

        /// <summary>
        /// 覆盖配置项
        /// </summary>
        /// <param name="config"></param>
        void CoverValue (Dictionary<string, ConfigItemModel> config);

    }
}