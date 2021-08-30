using System;
using System.Linq;

using RpcModel.Server;

using RpcService.DAL;
using RpcService.Model.DAL_Model;

using RpcHelper;
namespace RpcService.Save
{
        internal class SaveServiceState
        {
                private static readonly IDelayDataSave<ServiceState> _SaveState = new DelayDataSave<ServiceState>(_Save, _Filter, 10, 5);

                private static void _Save(ref ServiceState[] datas)
                {
                        if (!new ServerRunStateDAL().SetServiceState(datas))
                        {
                                throw new ErrorException("rpc.service.state.save.erro");
                        }
                }

                private static void _Filter(ref ServiceState[] datas)
                {
                        if (datas.Length > 1)
                        {
                                Array.Reverse(datas);
                                datas = datas.Distinct().ToArray();
                        }
                }

                public static void Add(RunState run, long serverId)
                {
                        _SaveState.AddData(new ServiceState
                        {
                                ServerId = serverId,
                                ConNum = run.ConNum,
                                CpuRunTime = run.CpuRunTime,
                                Memory = run.Memory
                        });
                }
        }
}
