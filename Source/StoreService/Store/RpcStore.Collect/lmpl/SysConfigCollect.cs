using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.SysConfig;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.Collect.lmpl
{
    internal class SysConfigCollect : ISysConfigCollect
    {
        private readonly ISysConfigDAL _BasicDAL;

        public SysConfigCollect (ISysConfigDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public SysConfigBasic[] Query (QuerySysParam query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public BasicSysConfig FindBasicConfig (long rpcMerId, string name)
        {
            return this._BasicDAL.FindBasicConfig(rpcMerId, name);
        }
        private void _CheckIsExists (SysConfigModel config)
        {
            if (this._BasicDAL.CheckIsExists(config))
            {
                throw new ErrorException("rpc.store.sys.config.repeat");
            }
        }

        public SysConfigModel Get (long id)
        {
            SysConfigModel config = this._BasicDAL.Get(id);
            if (config == null)
            {
                throw new ErrorException("rpc.store.config.not.find");
            }
            return config;
        }
        public bool SetIsEnable (SysConfigModel config, bool isEnable)
        {
            if (config.IsEnable == isEnable)
            {
                return false;
            }
            this._BasicDAL.SetIsEnable(config.Id, isEnable);
            return true;
        }
        public void SetBasicConfig (BasicConfigSet config)
        {
            BasicSysConfig model = this.FindBasicConfig(config.RpcMerId, config.Name);
            if (model == null)
            {
                SysConfigModel add = config.ConvertMap<BasicConfigSet, SysConfigModel>();
                add.ConfigType = RpcConfigType.基础配置;
                this._BasicDAL.Add(add);
            }
            else if (config.IsEquals(model))
            {
                return;
            }
            else
            {
                this._BasicDAL.Set(model.Id, new SysConfigSetParam
                {
                    IsEnable = config.IsEnable,
                    Prower = config.Prower,
                    ToUpdateTime = DateTime.Now,
                    Show = config.Show,
                    Value = config.Value,
                    ValueType = config.ValueType,
                    TemplateKey = config.TemplateKey
                });
            }
        }
        public void Add (SysConfigAdd config)
        {
            SysConfigModel add = config.ConvertMap<SysConfigAdd, SysConfigModel>();
            this._CheckIsExists(add);
            this._BasicDAL.Add(add);
        }
        public bool Set (SysConfigModel config, SysConfigSet set)
        {
            if (set.IsEquals(config))
            {
                return false;
            }
            SysConfigSetParam param = set.ConvertMap<SysConfigSet, SysConfigSetParam>();
            param.ToUpdateTime = DateTime.Now;
            this._BasicDAL.Set(config.Id, param);
            return true;
        }

        public void Delete (SysConfigModel config)
        {
            this._BasicDAL.Delete(config.Id);
        }

        public SysConfigModel Find (string systemType, string key)
        {
            SysConfigModel config = this._BasicDAL.FindConfig(systemType, key);
            if (config == null)
            {
                throw new ErrorException("rpc.store.sys.config.find.not.find");
            }
            return config;
        }
    }
}
