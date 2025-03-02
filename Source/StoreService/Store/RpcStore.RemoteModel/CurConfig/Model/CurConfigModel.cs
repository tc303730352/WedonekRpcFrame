namespace RpcStore.RemoteModel.CurConfig.Model
{
    public class CurConfigModel
    {
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UpTime { get; set; }

        /// <summary>
        /// 配置项
        /// </summary>
        public ServerCurConfig[] Configs { get; set; }
    }
}
