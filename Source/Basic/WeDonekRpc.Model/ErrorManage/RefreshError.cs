namespace WeDonekRpc.Model.ErrorManage
{
    /// <summary>
    ///刷新本地错误信息缓存
    /// </summary>
    [IRemoteBroadcast("RefreshError", false, IsCrossGroup = true)]
    public class RefreshError
    {
        public long ErrorId
        {
            get;
            set;
        }
    }
}
