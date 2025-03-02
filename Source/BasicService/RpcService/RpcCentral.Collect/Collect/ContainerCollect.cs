using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    /// <summary>
    /// 容器管理
    /// </summary>
    internal class ContainerCollect : IContainerCollect
    {
        private readonly IContainerDAL _Container;
        private readonly ICacheController _Cache;
        public ContainerCollect (ICacheController cache,
            IContainerDAL container)
        {
            this._Cache = cache;
            this._Container = container;
        }

        public ContrainerBasic RegContainer (ContrainerLogin param)
        {
            string key = string.Concat("Con_", param.ContrainerId, "_", param.ContGroupId);
            if (!this._Cache.TryGet(key, out ContrainerBasic contrainer))
            {
                contrainer = this._Container.Find(param.ContGroupId, param.ContrainerId);
                if (contrainer == null)
                {
                    contrainer = this._AddContrainer(param);
                }
                else if (( contrainer.InternalIp != param.InternalIp && param.InternalIp.IsNotNull() ) || ( contrainer.InternalPort != param.InternalPort && param.InternalPort != 0 ))
                {
                    contrainer.InternalIp = param.InternalIp.IsNull() ? contrainer.InternalIp : param.InternalIp;
                    contrainer.InternalPort = param.InternalPort == 0 ? contrainer.InternalPort : param.InternalPort;
                    this._Container.SetInternal(contrainer.Id, contrainer.InternalIp, contrainer.InternalPort);
                }
                _ = this._Cache.Set(key, contrainer);
            }
            return contrainer;
        }
        private ContrainerBasic _AddContrainer (ContrainerLogin param)
        {
            Contrainer add = new Contrainer
            {
                ContrainerId = param.ContrainerId,
                InternalIp = param.InternalIp,
                InternalPort = param.InternalPort,
                GroupId = param.ContGroupId
            };
            this._Container.Add(add);
            return add.ConvertMap<Contrainer, ContrainerBasic>();
        }
    }
}
