using RpcModel;
namespace Wedonek.Demo.RemoteModel.File
{
        /// <summary>
        /// 上传文件测试
        /// </summary>
        [IRemoteConfig("demo.user")]
        public class UpDemoFile : RpcClient.RpcUpFile
        {
                public string Name
                {
                        get;
                        set;
                }
        }
}
