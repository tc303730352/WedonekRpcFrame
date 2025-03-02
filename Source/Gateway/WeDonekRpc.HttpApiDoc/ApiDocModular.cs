using System;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiDoc.Collect;
using WeDonekRpc.HttpApiDoc.Helper;
using WeDonekRpc.HttpApiDoc.Interface;
using WeDonekRpc.HttpApiDoc.Model;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Config;
namespace WeDonekRpc.HttpApiDoc
{
    public class ApiDocModular : BasicApiModular, IApiDocModular
    {

        public ApiDocModular () : base("ApiDoc", "Api接口文档")
        {

        }

        protected override void Load (IHttpGatewayOption option, IModularConfig config)
        {
            option.AddFileDir(new FileDirConfig
            {
                DirPath = "ApiHtml",
                Extension = new string[] { "js", "css", "html" },
                VirtualPath = "/ApiHtml"
            });
        }

        public static IApiTemplate Template
        {
            get;
        } = new ApiTemplate();

        public string GetApiShow (Uri uri)
        {
            return this.GetApiShow(uri.AbsolutePath, GatewayType.Http);
        }
        public string GetApiShow (string path, GatewayType type)
        {
            return ApiCollect.GetApiName(path, type);
        }

        public void RegApi (ApiFuncBody api)
        {
            ApiCollect.LoadApi(api);
        }

        public void RegModular (string name, Type source, Uri root)
        {
            string show = XmlShowHelper.FindParamShow(source);
            if (show.IsNull())
            {
                show = string.Concat(name, "-", source.Name);
            }
            ApiGroupCollect.AddGroup(new ApiGroup(show, root, source.FullName));
        }
    }
}
