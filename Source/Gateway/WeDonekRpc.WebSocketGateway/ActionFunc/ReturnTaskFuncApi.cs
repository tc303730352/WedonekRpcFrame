using System.Threading.Tasks;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Reflection;
using WeDonekRpc.WebSocketGateway.Helper;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.ActionFunc
{
    internal class ReturnTaskFuncApi : BasicFunc
    {
        private readonly ExecTaskAction _Action;
        private readonly IFastGetProperty _ResultPro;
        public ReturnTaskFuncApi (ApiModel param) : base(param)
        {
            this._ResultPro = ReflectionHepler.GetFastGetPro(param.Method.ReturnType, "Result");
            this._Action = ApiRouteHelper.GetExecTaskAction(param.Method);
        }

        protected override void Invoke (IWebSocketService service, IApiGateway api, object[] param)
        {
            Task task = this._Action(api, param);
            if (task.Status == TaskStatus.Running)
            {
                task.Wait();
            }
            if (task.IsCompletedSuccessfully)
            {
                object res = this._ResultPro.GetValue(task);
                service.Reply(res);
            }
            else
            {
                service.ReplyError(ErrorException.FormatError(task.Exception));
            }
        }

        protected override void InitApiBody (IApiRoute route, ApiFuncBody body)
        {
            body.Results = new ResultBody[]
            {
                new ResultBody
                {
                    ResultType = this._Method.ReturnType.GetGenericArguments()[0],
                    ParamName = "returns",
                    AttrName = "Returns"
                }
            };
        }
    }
}
