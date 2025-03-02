using System.IO;
using System.Text;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.HttpApiDoc.Postman;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Response;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
namespace WeDonekRpc.HttpApiDoc.Api
{
    /// <summary>
    /// API文档接口
    /// </summary>
    [ApiRouteName("helper")]
    [ApiPrower(false)]
    internal class ApiHelperController : ApiController
    {
        /// <summary>
        /// 获取Api信息
        /// </summary>
        /// <param name="id">API接口Id</param>
        /// <returns>Api信息</returns>
        public ApiInfo GetApi ([NullValidate("gateway.api.id.null")] string id)
        {
            return ApiCollect.GetApiInfo(id);
        }
        /// <summary>
        /// 获取类信息
        /// </summary>
        /// <param name="id">类Id</param>
        /// <returns>类信息</returns>
        public ClassData GetClass ([NullValidate("gateway.class.id.null")] string id)
        {
            return ClassCollect.GetClassData(id);
        }
        /// <summary>
        /// 获取Api列表
        /// </summary>
        /// <returns>Api接口列表</returns>
        public ApiModel[] GetApiList ()
        {
            return ApiCollect.ApiList;
        }
        /// <summary>
        /// 获取Api分组列表
        /// </summary>
        /// <returns>活动组列表</returns>
        public Model.ApiGroupList[] GetApiGroup ()
        {
            return ApiGroupCollect.ApiGroup;
        }

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        /// <param name="id">枚举Id</param>
        /// <returns>说明信息</returns>
        public EnumShow GetEnum ([NullValidate("gateway.enum.id.null")] string id)
        {
            return EnumCollect.GetEnumFormat(id);
        }
        /// <summary>
        /// 下载Postman配置
        /// </summary>
        /// <param name="id">活动组别Id</param>
        /// <returns>PostMan配置文件</returns>
        public StreamResponse DownPostman (int id)
        {
            string json;
            if (id == 0)
            {
                json = PostmanHelper.CreatePostman(ApiGroupCollect.ApiGroup).ToJson();
            }
            else
            {
                Model.ApiGroupList group = ApiGroupCollect.GetGroup(id);
                json = PostmanHelper.CreatePostman(group).ToJson();
            }
            return new StreamResponse(new MemoryStream(Encoding.UTF8.GetBytes(json)), "config.json") { IsBinary = true };
        }

    }
}
