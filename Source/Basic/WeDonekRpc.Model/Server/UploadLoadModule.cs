namespace WeDonekRpc.Model.Server
{
    [IRemoteConfig("sys.sync", IsReply = false)]
    public class UploadLoadModule
    {
        /// <summary>
        /// 模块列表
        /// </summary>
        public ProcModule[] Modules { get; set; }
    }
}
