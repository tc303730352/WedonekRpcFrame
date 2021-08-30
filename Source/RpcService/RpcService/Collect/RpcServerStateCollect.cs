using RpcModel.Server;

using RpcService.DAL;
using RpcService.Model.DAL_Model;

namespace RpcService.Collect
{
        internal class RpcServerStateCollect
        {
                private static bool _AddRunState(long id, ProcessDatum datum, out string error)
                {
                        if (!new ServerRunStateDAL().AddRunState(new ServerRunState
                        {
                                ConNum = 0,
                                Pid = datum.Pid,
                                CpuRunTime = 0,
                                PName = datum.PName,
                                ServerId = id,
                                StartTime = datum.StartTime,
                                WorkMemory = datum.WorkMemory
                        }))
                        {
                                error = "rpc.sever.runstate.add.error";
                                return false;
                        }
                        error = null;
                        return true;
                }
                private static bool _SetRunState(long id, ProcessDatum datum, out string error)
                {
                        if (!new ServerRunStateDAL().UpdateRunState(id, datum))
                        {
                                error = "rpc.sever.runstate.set.error";
                                return false;
                        }
                        error = null;
                        return true;
                }
                public static bool SyncRunState(long id, ProcessDatum datum, out string error)
                {
                        if (!new ServerRunStateDAL().CheckIsReg(id, out bool isReg))
                        {
                                error = "rpc.sever.runstate.check.error";
                                return false;
                        }
                        else if (!isReg)
                        {
                                return _AddRunState(id, datum, out error);
                        }
                        return _SetRunState(id, datum, out error);
                }
        }
}
