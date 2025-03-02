using System.Collections.Generic;

namespace WeDonekRpc.Helper.Error
{
    /// <summary>
    /// 错误配置
    /// </summary>
    public class ErrorConfig
    {
        /// <summary>
        /// 语言项
        /// </summary>
        public string[] Lang
        {
            get;
            set;
        } = new string[] { "zh" };
        /// <summary>
        /// 是否自动保存
        /// </summary>

        public bool IsAutoSave
        {
            get;
            set;
        } = true;
        /// <summary>
        /// 保存间隔
        /// </summary>
        public int IntervalTime
        {
            get;
            set;
        } = 60;
        /// <summary>
        /// 默认错误文本设置
        /// </summary>
        public Dictionary<string, string> DefErrorText
        {
            get;
            set;
        } = new Dictionary<string, string>();
    }
}
