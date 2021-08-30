using System.Collections.Generic;
using System.Data;

using RpcModel;

using SqlExecHelper;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.DAL
{
        internal class SysConfigDAL : SqlBasicClass
        {
                public SysConfigDAL() : base("SysConfig")
                {

                }
                public bool QuerySysConfig(QuerySysParam query, IBasicPage paging, out SysConfigDatum[] configs, out long count)
                {
                        List<ISqlWhere> list = new List<ISqlWhere>(3);
                        if (query.Range.HasValue)
                        {
                                if (query.Range.Value == ConfigRange.全局)
                                {
                                        if (query.RpcMerId.HasValue)
                                        {
                                                list.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = query.RpcMerId });
                                        }
                                        else
                                        {
                                                list.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = 0 });
                                        }
                                        list.Add(new SqlWhere("ServerId", SqlDbType.BigInt) { Value = 0 });
                                        list.Add(new SqlWhere("SystemTypeId", SqlDbType.BigInt) { Value = 0 });
                                }
                                else if (query.Range.Value == ConfigRange.节点)
                                {
                                        list.Add(new SqlWhere("ServerId", SqlDbType.BigInt, QueryType.不等) { Value = 0 });
                                }
                                else
                                {
                                        if ((ConfigRange.集群 & query.Range.Value) == ConfigRange.集群)
                                        {
                                                list.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt, QueryType.不等) { Value = 0 });
                                        }
                                        if ((ConfigRange.节点类别 & query.Range.Value) == ConfigRange.节点类别)
                                        {
                                                if (query.RpcMerId.HasValue)
                                                {
                                                        list.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = query.RpcMerId.Value });
                                                }
                                                list.Add(new SqlWhere("SystemTypeId", SqlDbType.BigInt, QueryType.不等) { Value = 0 });
                                        }
                                        list.Add(new SqlWhere("ServerId", SqlDbType.BigInt) { Value = 0 });
                                }
                        }
                        else
                        {
                                if (query.ServerId.HasValue)
                                {
                                        list.Add(new SqlWhere("ServerId", SqlDbType.BigInt) { Value = query.ServerId.Value });
                                }
                                else if (query.RpcMerId.HasValue)
                                {
                                        list.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = query.RpcMerId.Value });
                                }
                                if (query.SystemTypeId.HasValue && query.SystemTypeId.Value != 0)
                                {
                                        list.Add(new SqlWhere("SystemTypeId", SqlDbType.BigInt) { Value = query.SystemTypeId.Value });
                                }
                        }
                        if (!string.IsNullOrEmpty(query.ConfigName))
                        {
                                list.Add(new SqlWhere("Name", SqlDbType.VarChar, 50) { Value = query.ConfigName });
                        }
                        return this.Query("Id desc", paging.Index, paging.Size, out configs, out count, list.ToArray());
                }

                public bool CheckIsExists(SysConfigAddParam config, out bool isExists)
                {
                        List<ISqlWhere> wheres = new List<ISqlWhere>(3);
                        if (config.ServerId != 0)
                        {
                                wheres.Add(new SqlWhere("ServerId", SqlDbType.BigInt) { Value = config.ServerId });
                        }
                        else if (config.RpcMerId != 0)
                        {
                                wheres.Add(new SqlWhere("RpcMerId", SqlDbType.BigInt) { Value = config.RpcMerId });
                        }
                        if (config.SystemTypeId != 0)
                        {
                                wheres.Add(new SqlWhere("SystemTypeId", SqlDbType.BigInt) { Value = config.SystemTypeId });
                        }
                        wheres.Add(new SqlWhere("Name", SqlDbType.VarChar, 50) { Value = config.Name });
                        return this.CheckIsExists(out isExists, wheres.ToArray());
                }

                public bool GetSysConfig(long Id, out SysConfigDatum datum)
                {
                        return this.GetRow("Id", Id, out datum);
                }
                public bool AddSysConfig(SysConfigAddParam add, out long id)
                {
                        return this.Insert(add, out id);
                }

                internal bool FindConfigId(long systemTypeId, string key, out long id)
                {
                        return this.ExecuteScalar("Id", out id, new ISqlWhere[] {
                               new SqlWhere("SystemTypeId", SqlDbType.BigInt) { Value =systemTypeId },
                               new SqlWhere("Name", SqlDbType.VarChar, 50) { Value =key}
                        });
                }

                public bool SetSysConfig(long id, SysConfigSetParam config)
                {
                        return this.Update(config, "Id", id);
                }

                public bool DropConfig(long id)
                {
                        return this.Drop("Id", id) > 0;
                }

                public bool BatchDropConfig(long sysTypeId)
                {
                        return this.Drop("SystemTypeId", sysTypeId) != -2;
                }
        }
}
