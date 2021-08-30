using System;
using System.Data;

using RpcModel.Server;

using RpcService.Model.DAL_Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcService.DAL
{
        internal class ServerRunStateDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerRunStateDAL() : base("ServerRunState")
                {

                }
                public bool SetServiceState(ServiceState[] states)
                {
                        using (IBatchUpdate table = this.BatchUpdate(states.Length, 5))
                        {
                                table.Column = new SqlTableColumn[]
                                {
                                        new SqlTableColumn("ServerId", SqlDbType.BigInt){ColType= SqlColType.标识},
                                        new SqlTableColumn("ConNum", SqlDbType.Int){ColType= SqlColType.修改},
                                        new SqlTableColumn("WorkMemory", SqlDbType.Int){ColType= SqlColType.修改},
                                        new SqlTableColumn("CpuRunTime", SqlDbType.Int){ColType= SqlColType.修改},
                                        new SqlTableColumn("SyncTime", SqlDbType.SmallDateTime){ ColType= SqlColType.修改}
                                };
                                DateTime now = DateTime.Now;
                                states.ForEach(a => table.AddRow(a.ServerId, a.ConNum, a.Memory, a.CpuRunTime, now));
                                return table.Update();
                        }
                }
                public bool CheckIsReg(long id, out bool isReg)
                {
                        return this.CheckIsExists(out isReg, new SqlWhere("ServerId", SqlDbType.BigInt) { Value = id });
                }
                public bool AddRunState(ServerRunState add)
                {
                        return this.Insert(add);
                }
                public bool UpdateRunState(long serverId, ProcessDatum datum)
                {
                        return this.Update(datum, "ServerId", serverId);
                }
        }
}
