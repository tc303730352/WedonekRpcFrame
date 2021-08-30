namespace RpcModel.ErrorManage
{
        [IRemoteConfig("InitError", "sys.sync", isReply: false)]
        public class InitError
        {
                public long ErrorId
                {
                        get;
                        set;
                }
                public string ErrorCode { get; set; }
        }
}
