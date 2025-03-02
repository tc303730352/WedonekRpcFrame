namespace RpcStore.RemoteModel.SysConfig.Model
{
    public class SysConfigSet
    {
        /// <summary>
        /// 值类型
        /// </summary>
        public SysConfigValueType ValueType
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 配置权限
        /// </summary>
        public int Prower
        {
            get;
            set;
        }
        /// <summary>
        /// 配置摸版
        /// </summary>
        public string TemplateKey
        {
            get;
            set;
        }
    }
}
