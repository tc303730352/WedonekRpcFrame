using System;

using RpcModel;
using RpcHelper;
using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        internal class SysConfigCollect : BasicCollect<SysConfigDAL>, ISysConfigCollect
        {
                public SysConfigDatum[] Query(QuerySysParam query, IBasicPage paging, out long count)
                {
                        if (this.BasicDAL.QuerySysConfig(query, paging, out SysConfigDatum[] configs, out count))
                        {
                                return configs;
                        }
                        throw new ErrorException("rpc.sys.config.drop.error");
                }

                private bool _CheckIsExists(SysConfigAddParam config)
                {
                        if (this.BasicDAL.CheckIsExists(config, out bool isExists))
                        {
                                return isExists;
                        }
                        throw new ErrorException("rpc.sys.config.check.error");
                }

                public SysConfigDatum GetSysConfig(long Id)
                {
                        if (this.BasicDAL.GetSysConfig(Id, out SysConfigDatum datum))
                        {
                                return datum;
                        }
                        throw new ErrorException("rpc.sys.config.get.error");
                }
                public long AddSysConfig(SysConfigAddParam add)
                {
                        if (this._CheckIsExists(add))
                        {
                                throw new ErrorException("rpc.sys.config.repeat");
                        }
                        else if (this.BasicDAL.AddSysConfig(add, out long id))
                        {
                                return id;
                        }
                        throw new ErrorException("rpc.sys.config.add.error");
                }
                public void SetSysConfig(long id, SysConfigSetParam config)
                {
                        config.ToUpdateTime = DateTime.Now;
                        if (!this.BasicDAL.SetSysConfig(id, config))
                        {
                                throw new ErrorException("rpc.sys.config.set.error");
                        }
                }

                public void DropConfig(long id)
                {
                        if (!this.BasicDAL.DropConfig(id))
                        {
                                throw new ErrorException("rpc.sys.config.drop.error");
                        }
                }

                public long FindConfigId(long systemTypeId, string key)
                {
                        if (!this.BasicDAL.FindConfigId(systemTypeId, key, out long id))
                        {
                                throw new ErrorException("rpc.sys.config.find.error");
                        }
                        else if (id == 0)
                        {
                                throw new ErrorException("rpc.sys.config.find.not.find");
                        }
                        return id;
                }
        }
}
