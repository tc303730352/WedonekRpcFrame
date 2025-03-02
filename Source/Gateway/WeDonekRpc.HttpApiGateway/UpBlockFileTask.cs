using WeDonekRpc.ApiGateway.Json;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.FileUp;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway
{
    /// <summary>
    /// 分块文件上传控制器
    /// </summary>
    [IgnoreIoc]
    public class UpBlockFileTask : IUpBlockFileTask
    {

        public void Init ( IApiService service, IUpTask task )
        {
            string str = service.Request.PostString;
            if ( str.IsNull() )
            {
                this.InitTask(service, new BlockUp(task, null));
            }
            else
            {
                UpFileData data = GatewayJsonTools.Json<UpFileData>(str);
                this.InitTask(service, new BlockUp(task, data));
            }
        }

        public virtual void InitTask ( IApiService service, IBlockUp task )
        {

        }
        public virtual void Complete ( IUpFileResult result, IUpFile upFile )
        {

        }

        public virtual void UpFail ( UpBasicFile file, string error )
        {
        }

    }
    [IgnoreIoc]
    public class UpBlockFileTask<T> : IUpBlockFileTask
    {
        public void Init ( IApiService service, IUpTask task )
        {
            string str = service.Request.PostString;
            if ( str.IsNull() )
            {
                this.InitTask(service, new BlockUp<T>(task, null));
            }
            else
            {
                UpFileData<T> data = GatewayJsonTools.Json<UpFileData<T>>(str);
                this.InitTask(service, new BlockUp<T>(task, data));
            }
        }

        public virtual void InitTask ( IApiService service, IBlockUp<T> task )
        {

        }
        public virtual void Complete ( IUpFileResult result, IUpFile upFile )
        {

        }

        public virtual void UpFail ( UpBasicFile file, string error )
        {
        }

    }
}
