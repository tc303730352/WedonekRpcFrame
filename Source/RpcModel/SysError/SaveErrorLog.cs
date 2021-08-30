namespace RpcModel.SysError
{
        [IRemoteConfig("SaveErrorLog", "sys.sync", isReply: false)]
        public class SaveErrorLog
        {
                public SysErrorLog[] Errors
                {
                        get;
                        set;
                }
        }
}
