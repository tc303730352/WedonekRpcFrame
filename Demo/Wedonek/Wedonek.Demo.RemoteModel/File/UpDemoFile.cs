using WeDonekRpc.Client;
using WeDonekRpc.Model;
namespace Wedonek.Demo.RemoteModel.File
{
    /// <summary>
    /// 上传文件测试
    /// </summary>
    [IRemoteConfig("demo.user")]
    public class UpDemoFile : RpcUpFile
    {
        public string Name
        {
            get;
            set;
        }
    }
}
