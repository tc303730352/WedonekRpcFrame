using System;

namespace WeDonekRpc.HttpApiDoc.Model
{
    /// <summary>
    /// Api属性说明
    /// </summary>
    public class ApiAttrShow
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string AttrName
        {
            get;
            set;
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type AttrType
        {
            get;
            set;
        }
        /// <summary>
        /// 说明
        /// </summary>
        public string AttrShow
        {
            get;
            set;
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefValue
        {
            get;
            set;
        }
    }
}
