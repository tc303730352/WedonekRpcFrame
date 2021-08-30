namespace RpcModel.ErrorManage
{
        [IRemoteConfig("GetError", "sys.sync")]
        public class GetError
        {
                public string ErrorCode
                {
                        get;
                        set;
                }
        }
}
