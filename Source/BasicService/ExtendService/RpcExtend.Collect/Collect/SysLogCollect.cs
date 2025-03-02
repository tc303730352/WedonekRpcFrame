using RpcExtend.DAL;
using RpcExtend.Model.DB;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Interface;

namespace RpcExtend.Collect.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class SysLogCollect : ISysLogCollect
    {
        private readonly IDelayDataSave<SystemErrorLog> _LogSave;
        private readonly IIocService _Unity;
        public SysLogCollect (IIocService unity)
        {
            this._Unity = unity;
            this._LogSave = new DelayDataSave<SystemErrorLog>(this._SaveLog, 2, 10);
        }


        private void _SaveLog (ref SystemErrorLog[] datas)
        {
            if (IdentityHelper.IsInit == false)
            {
                this._LogSave.AddData(datas);
                return;
            }
            using (IocScope score = this._Unity.CreateTempScore())
            {
                datas.ForEach(c => c.Id = IdentityHelper.CreateId());
                score.Resolve<ISystemLogDAL>().Adds(datas);
            }
        }

        public void AddLog (SystemErrorLog[] logs)
        {
            this._LogSave.AddData(logs);
        }
    }
}
