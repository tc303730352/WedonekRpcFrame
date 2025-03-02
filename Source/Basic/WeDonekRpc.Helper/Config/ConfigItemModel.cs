using System.Collections.Generic;

namespace WeDonekRpc.Helper.Config
{
    public class ConfigItemModel
    {
        /// <summary>
        /// 配置项类型
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int? Prower { get; set; }
        /// <summary>
        /// 配置项值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 数组项
        /// </summary>
        public ConfigItemModel[] ArrayItem { get; set; }
        /// <summary>
        /// 下级项
        /// </summary>
        public Dictionary<string, ConfigItemModel> Children { get; set; }
    }
}
