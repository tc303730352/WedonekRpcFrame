using System.Threading.Tasks;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Reflection;
using WeDonekRpc.HttpApiGateway.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.ActionFun
{
    internal class TaskResponseApi : BasicFunc
    {
        private readonly ExecTaskAction _Action;
        private readonly IFastGetProperty _ResultPro;
        public TaskResponseApi (ApiModel param) : base(param)
        {
            this._ResultPro = ReflectionHepler.GetFastGetPro(param.Method.ReturnType, "Result");
            this._Action = ApiRouteHelper.GetExecTaskAction(param.Method);
        }

        protected override void Invoke (IService service, IApiGateway api, object[] param)
        {
            Task task = this._Action(api, param);
            if (task.Status == TaskStatus.Running)
            {
                task.Wait();
            }
            if (task.IsCompletedSuccessfully)
            {
                object res = this._ResultPro.GetValue(task);
                if (res == null)
                {
                    service.ReplyError("http.404");
                    return;
                }
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
                        ResultType=this._Method.ReturnType.GetGenericArguments()[0],
                        AttrName="Returns",
                        ParamName="returns"
                }
            };
        }
    }
}
