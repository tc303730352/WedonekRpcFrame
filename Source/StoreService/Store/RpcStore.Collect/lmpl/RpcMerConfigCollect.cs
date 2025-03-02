using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMerConfig;
using RpcStore.RemoteModel.MerConfig.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
namespace RpcStore.Collect.lmpl
{
    internal class RpcMerConfigCollect : IRpcMerConfigCollect
    {
        private readonly IRpcMerConfigDAL _BasicDAL;
        public RpcMerConfigCollect (IRpcMerConfigDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public RpcMerConfigModel[] GetConfigs (long rpcMerId)
        {
            return this._BasicDAL.Gets(rpcMerId);
        }
        public RpcMerConfig GetConfig (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.Get(rpcMerId, systemTypeId);
        }
        public RpcMerConfigModel GetConfig (long id)
        {
            RpcMerConfigModel config = this._BasicDAL.Get(id);
            if (config == null)
            {
                throw new ErrorException("rpc.store.mer.config.not.find");
            }
            return config;
        }

        public long Add (MerConfigArg add)
        {
            RpcMerConfigModel obj = add.ConvertMap<MerConfigArg, RpcMerConfigModel>();
            this._BasicDAL.Add(obj);
            return obj.Id;
        }

        public bool Set (RpcMerConfig config, MerConfigSet param)
        {
            if (param.IsEquals(config))
            {
                return false;
            }
            this._BasicDAL.Set(config.Id, param);
            return true;
        }
        public void Delete (RpcMerConfigModel config)
        {
            this._BasicDAL.Delete(config.Id);
        }
    }
}
