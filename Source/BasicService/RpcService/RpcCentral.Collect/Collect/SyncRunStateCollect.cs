using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect.Collect
{
    internal class SyncRunStateCollect : ISyncRunStateCollect
    {
        private static readonly IDelayDataSave<ServiceRunState> _SaveState = new DelayDataSave<ServiceRunState>(_Save, _Filter, 10, 5);
        private static void _Save (ref ServiceRunState[] datas)
        {
            using (IocScope score = UnityHelper.CreateTempScore())
            {
                IServerRunStateDAL basicDAL = score.Resolve<IServerRunStateDAL>();
                if (!basicDAL.SetServiceState(datas))
                {
                    throw new ErrorException("rpc.service.state.save.error");
                }
            }
        }

        private static void _Filter (ref ServiceRunState[] datas)
        {
            if (datas.Length > 1)
            {
                Array.Reverse(datas);
                datas = datas.Distinct().ToArray();
            }
        }
        public void Sync (RunState run, long serverId)
        {
            ServiceRunState state = run.ConvertMap<RunState, ServiceRunState>();
            state.SyncTime = DateTime.Now;
            state.ServerId = serverId;
            _SaveState.AddData(state);
        }
    }
}
