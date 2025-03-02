using System;
using System.Reflection;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class BlockUpApi : IHttpApi
    {
        protected readonly FuncParam _Param;
        private readonly Type _SourceType;

        public string ApiName => this._SourceType.FullName;

        public string ApiUri { get; }

        public MethodInfo Source => null;

        public BlockUpApi ( ApiModel model, FuncParam param )
        {
            this._SourceType = model.Type;
            this._Param = param;
            this.ApiUri = model.ApiUri;
        }

        public void ExecApi ( IService service )
        {
            IUpBlockFileTask api = service.Scope.Resolve<IUpBlockFileTask>(this.ApiName);
            IBlockUpTask task = BlockUpCollect.Add(service, this.ApiName);
            try
            {
                api.Init(service, task);
            }
            catch ( Exception e )
            {
                ErrorException error = ErrorException.FormatError(e);
                task.UpError(error.ErrorCode);
                service.ReplyError(error);
                return;
            }
            service.Reply(task.TaskId);
        }

        public void RegApi ( IApiRoute route )
        {
            ApiFuncBody param = new ApiFuncBody
            {
                PostParam = new ApiPostParam[] {
                    new ApiPostParam
                    {
                        Name = this._Param.Name,
                        PostType = this._Param.DataType,
                        ReceiveMethod = this._Param.ReceiveMethod,
                        ParamIndex = 0
                    }
                },
                ApiType = route.ApiType,
                UpConfig = route.GetUpSet(),
                Source = this._SourceType,
                IsAccredit = route.IsAccredit,
                Method = null,
                Prower = route.Prower,
                ApiUri = route.ApiUri
            };
            ApiGatewayService.RegApi(param);
        }
    }
}
