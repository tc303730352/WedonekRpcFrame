using System;
using System.Collections.Generic;
using WeDonekRpc.Helper.Json;
namespace WeDonekRpc.Helper.Config
{
    /// <summary>
    /// 配置扩展
    /// </summary>
    internal interface IConfig
    {
        /// <summary>
        /// 获取指定路径的JSON字符串
        /// </summary>
        /// <param name="path">配置路径</param>
        /// <returns>JSON字符串</returns>
        string GetJson (string path);
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="configItem"></param>
        /// <returns></returns>
        bool SetConfig (string key, JsonBodyValue obj, IConfigItem configItem);
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool SetConfig (string key, Type type, object value, IConfigItem parent);
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Dictionary<string, ConfigItemModel> GetConfigItem (string path);

        /// <summary>
        /// 覆盖配置值
        /// </summary>
        /// <param name="children"></param>
        void CoverValue (Dictionary<string, ConfigItemModel> children);

        /// <summary>
        /// 清空所有下级
        /// </summary>
        /// <param name="itemName"></param>
        void ClearNull (string itemName);
    }
}
