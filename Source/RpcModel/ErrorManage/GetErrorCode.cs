namespace RpcModel.ErrorManage
{
        [IRemoteConfig("FindError", "sys.sync")]
        public class GetErrorCode
        {
                public long ErrorId
                {
                        get;
                        set;
                }
        }
}
