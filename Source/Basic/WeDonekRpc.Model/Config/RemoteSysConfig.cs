namespace WeDonekRpc.Model.Config
{
    /// <summary>
    /// 远程系统配置
    /// </summary>
    public class RemoteSysConfig
    {
        /// <summary>
        /// 配置MD5
        /// </summary>
        public string ConfigMd5
        {
            get;
            set;
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        public SysConfigData[] ConfigData
        {
            get;
            set;
        }

    }
}
