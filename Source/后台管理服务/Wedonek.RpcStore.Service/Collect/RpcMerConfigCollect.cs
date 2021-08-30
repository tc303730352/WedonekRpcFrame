using System;

using RpcClient;

using RpcManageClient;

using RpcHelper;

using Wedonek.RpcStore.Service.DAL;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;
namespace Wedonek.RpcStore.Service.Collect
{
        internal class RpcMerConfigCollect : BasicCollect<RpcMerConfigDAL>, IRpcMerConfigCollect
        {
                private IRpcServerCollect _Server => this.GetCollect<IRpcServerCollect>();


                public RpcMerConfig[] GetConfigs(long rpcMerId)
                {
                        if (this.BasicDAL.GetConfigs(rpcMerId, out RpcMerConfig[] configs))
                        {
                                return configs;
                        }
                        throw new ErrorException("rpc.mer.config.get.error");
                }
                public RpcMerConfig GetConfig(Guid id)
                {
                        if (this.BasicDAL.GetConfig(id, out RpcMerConfig config))
                        {
                                return config;
                        }
                        throw new ErrorException("rpc.mer.config.get.error");
                }

                public Guid Add(AddMerConfig add)
                {
                        if (!this.BasicDAL.CheckIsExists(add.RpcMerId, add.SystemTypeId, out bool isExists))
                        {
                                throw new ErrorException("rpc.mer.config.check.error");
                        }
                        else if (isExists)
                        {
                                throw new ErrorException("rpc.mer.config.exists");
                        }
                        RpcMerConfig obj = add.ConvertMap<AddMerConfig, RpcMerConfig>();
                        obj.Id = Tools.NewGuid();
                        if (this.BasicDAL.Add(obj))
                        {
                                this._Server.RefreshMerConfig(add.RpcMerId, add.SystemTypeId);
                                return obj.Id;
                        }
                        throw new ErrorException("rpc.mer.config.add.error");
                }

                public void Set(Guid id, SetMerConfig param)
                {
                        RpcMerConfig config = this.GetConfig(id);
                        if (config == null)
                        {
                                throw new ErrorException("rpc.mer.config.not.find");
                        }
                        else if (!this.BasicDAL.Set(id, param))
                        {
                                throw new ErrorException("rpc.mer.config.set.error");
                        }
                        else
                        {
                                this._Server.RefreshMerConfig(config.RpcMerId, config.SystemTypeId);
                        }
                }
                public void Drop(Guid id)
                {
                        RpcMerConfig config = this.GetConfig(id);
                        if (config == null)
                        {
                                throw new ErrorException("rpc.mer.config.not.find");
                        }
                        else if (!this.BasicDAL.Drop(id))
                        {
                                throw new ErrorException("rpc.mer.config.set.error");
                        }
                        else
                        {
                                this._Server.RefreshMerConfig(config.RpcMerId, config.SystemTypeId);
                        }
                }
        }
}
