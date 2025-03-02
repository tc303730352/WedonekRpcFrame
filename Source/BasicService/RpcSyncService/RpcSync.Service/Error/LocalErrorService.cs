using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper.Error;

namespace RpcSync.Service.Error
{
    /// <summary>
    /// 错误系统-覆盖掉框架内的错误服务    
    /// </summary>
    [IocName("LocalErrorService")]
    internal class LocalErrorService : IRpcExtendService
    {
        private readonly IIocService _Ioc;

        public LocalErrorService (IIocService unity)
        {
            this._Ioc = unity;
        }

        private void _Init ()
        {
            LocalErrorManage.SetAction(() =>
            {
                using (IocScope scope = this._Ioc.CreateScore())
                {
                    return scope.Resolve<IErrorEvent>("RpcSync");
                }
            });
        }

        public void Load (IRpcService service)
        {
            service.StartUpComplating += this._Init;
        }
    }
}
