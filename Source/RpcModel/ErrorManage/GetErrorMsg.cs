namespace RpcModel.ErrorManage
{
        [IRemoteConfig("GetErrorMsg", "sys.sync")]
        public class GetErrorMsg
        {
                public string ErrorCode
                {
                        get;
                        set;
                }
                public string Lang { get; set; }
        }
}
