using System;
using System.Data;

using RpcService.Model.DAL_Model;

using SqlExecHelper;

using RpcHelper;

namespace RpcService.DAL
{
        internal class ServerSignalStateDAL : SqlExecHelper.SqlBasicClass
        {
                public ServerSignalStateDAL() : base("ServerSignalState")
                {

                }

                public bool SyncState(ServerSignalState[] states)
                {
                        using (IBatchMerge table = this.BatchMerge(states.Length, 8))
                        {
                                table.Column = new SqlTableColumn[]
                                {
                                        new SqlTableColumn("ServerId", SqlDbType.BigInt){ColType= SqlColType.查询| SqlColType.添加},
                                        new SqlTableColumn("RemoteId", SqlDbType.BigInt){ColType= SqlColType.查询| SqlColType.添加},
                                        new SqlTableColumn("ConNum", SqlDbType.Int){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("AvgTime", SqlDbType.Int){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("SendNum", SqlDbType.Int){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("ErrorNum", SqlDbType.Int){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("UsableState", SqlDbType.SmallInt){ColType= SqlColType.修改| SqlColType.添加},
                                        new SqlTableColumn("SyncTime", SqlDbType.SmallDateTime){ ColType= SqlColType.修改| SqlColType.添加}
                                };
                                DateTime now = DateTime.Now;
                                states.ForEach(a => table.AddRow(a.ServerId, a.RemoteId, a.ConNum, a.AvgTime, a.SendNum, a.ErrorNum, a.UsableState, now));
                                return table.InsertOrUpdate();
                        }
                }
        }
}
