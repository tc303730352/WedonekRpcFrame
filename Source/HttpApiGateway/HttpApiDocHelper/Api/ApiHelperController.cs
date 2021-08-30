using System.IO;
using System.Text;

using ApiGateway.Attr;

using HttpApiDocHelper.Collect;
using HttpApiDocHelper.Model;
using HttpApiDocHelper.Postman;

using HttpApiGateway;
using HttpApiGateway.Response;

using RpcHelper;

namespace HttpApiDocHelper.Api
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
                /// <param name="api">api信息</param>
                /// <param name="error">错误信息</param>
                /// <returns>是否成功</returns>
                public bool GetApi(string id, out ApiInfo api, out string error)
                {
                        if (string.IsNullOrEmpty(id))
                        {
                                api = null;
                                error = "public.param.error";
                                return false;
                        }
                        api = ApiCollect.GetApiInfo(id);
                        error = null;
                        return true;
                }
                /// <summary>
                /// 获取类信息
                /// </summary>
                /// <param name="id">类Id</param>
                /// <param name="data">类数据结构</param>
                /// <param name="error">错误信息</param>
                /// <returns>是否成功</returns>
                public bool GetClass(string id, out ClassData data, out string error)
                {
                        if (string.IsNullOrEmpty(id))
                        {
                                data = null;
                                error = "public.param.error";
                                return false;
                        }
                        data = ClassCollect.GetClassData(id);
                        error = null;
                        return true;
                }
                /// <summary>
                /// 获取Api列表
                /// </summary>
                /// <returns>Api接口列表</returns>
                public ApiModel[] GetApiList()
                {
                        return ApiCollect.ApiList;
                }
                /// <summary>
                /// 获取Api分组列表
                /// </summary>
                /// <returns>活动组列表</returns>
                public Model.ApiGroupList[] GetApiGroup()
                {
                        return ApiGroupCollect.ApiGroup;
                }

                /// <summary>
                /// 获取枚举说明
                /// </summary>
                /// <param name="id">枚举Id</param>
                /// <param name="api">说明信息</param>
                /// <param name="error">错误信息</param>
                /// <returns></returns>
                public bool GetEnum(string id, out EnumShow api, out string error)
                {
                        if (string.IsNullOrEmpty(id))
                        {
                                api = null;
                                error = "public.param.error";
                                return false;
                        }
                        error = null;
                        api = EnumCollect.GetEnumFormat(id);
                        return true;
                }
                /// <summary>
                /// 下载Postman配置
                /// </summary>
                /// <param name="id">活动组别Id</param>
                /// <returns>PostMan配置文件</returns>
                public StreamResponse DownPostman(int id)
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
