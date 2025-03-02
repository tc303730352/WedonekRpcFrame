using System.Collections.Generic;

namespace WeDonekRpc.ModularModel.Identity.Model
{
    public class IdentityDatum
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用扩展参数
        /// </summary>
        public Dictionary<string, string> AppExtend { get; set; }
    }
}
