
using System.Collections.Generic;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Config
{
    /// <summary>
    /// 配置项
    /// </summary>
    internal interface IConfigItem
    {
        /// <summary>
        /// 配置路径
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 项名称
        /// </summary>
        string ItemName { get; }
        /// <summary>
        /// 权限值
        /// </summary>
        int Prower { get; }
        /// <summary>
        /// 父级配置项
        /// </summary>
        IConfigItem Parent { get; }
        /// <summary>
        /// 配置项类型
        /// </summary>
        ItemType ItemType { get; }
        /// <summary>
        /// 设置配置项值
        /// </summary>
        /// <param name="obj">配置值(字符串)</param>
        /// <param name="itemType">项类型</param>
        /// <param name="prower">权限值</param>
        /// <returns>是否成功</returns>
        bool SetValue (string obj, ItemType itemType, int prower);

        /// <summary>
        /// 设置配置项值
        /// </summary>
        /// <param name="obj">配置值(字符串)</param>
        /// <param name="prower">权限值</param>
        /// <returns>是否成功</returns>
        bool SetValue (string obj, int prower);
        /// <summary>
        /// 覆盖值
        /// </summary>
        /// <param name="model"></param>
        void CoverValue (ConfigItemModel model);
        /// <summary>
        /// 设置配置项值
        /// </summary>
        /// <param name="token">配置值(对象)</param>
        /// <param name="prower">权限值</param>
        /// <returns>是否成功</returns>
        bool SetValue (JsonBodyValue token, int prower);
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ConfigItemModel> GetConfigItem ();
        /// <summary>
        /// 获取对象结构
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ConfigItemModel> GetObjectItem ();
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        ConfigItemModel[] GetArrayItem ();
        /// <summary>
        /// 刷新
        /// </summary>
        void Refresh ();
    }
}