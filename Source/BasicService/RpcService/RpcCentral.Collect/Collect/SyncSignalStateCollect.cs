using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect.Collect
{
    internal class SyncSignalStateCollect : ISyncSignalStateCollect
    {
        private static readonly IDelayDataSave<ServerSignalState> _SaveState = new DelayDataSave<ServerSignalState>(_Save, _Filter, 10, 5);

        private static void _Save (ref ServerSignalState[] datas)
        {
            using (IocScope score = UnityHelper.CreateTempScore())
            {
                IServerSignalStateDAL basicDAL = score.Resolve<IServerSignalStateDAL>();
                if (!basicDAL.SyncState(datas))
                {
                    throw new ErrorException("rpc.state.save.error");
                }
            }
        }

        private static void _Filter (ref ServerSignalState[] datas)
        {
            if (datas.Length > 1)
            {
                Array.Reverse(datas);
                datas = datas.Distinct().ToArray();
            }
        }
        public void Sync (long serverId, RemoteState[] remotes)
        {
            remotes.ForEach(a =>
            {
                ServerSignalState state = a.ConvertMap<RemoteState, ServerSignalState>();
                state.SyncTime = DateTime.Now;
                state.ServerId = serverId;
                _SaveState.AddData(state);
            });
        }
    }
}
