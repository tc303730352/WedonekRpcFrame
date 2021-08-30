using RpcModel;

namespace Wedonek.Demo.RemoteModel.User
{
        [IRemoteConfig("demo.user")]
        public class AddUser : RpcClient.RpcRemote<long>
        {
                /// <summary>
                /// 用户名
                /// </summary>
                public string UserName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 用户手机
                /// </summary>
                public string UserPhone
                {
                        get;
                        set;
                }
        }
}
