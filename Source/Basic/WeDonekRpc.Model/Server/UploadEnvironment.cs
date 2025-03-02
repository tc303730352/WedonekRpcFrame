namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 上传环境配置
    /// </summary>
    [IRemoteConfig("UploadEnvironment", "sys.sync", false)]
    public class UploadEnvironment
    {
        /// <summary>
        /// 环境配置
        /// </summary>
        public EnvironmentConfig Environment
        {
            get;
            set;
        }
    }
}
