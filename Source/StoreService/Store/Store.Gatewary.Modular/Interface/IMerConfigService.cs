using RpcStore.RemoteModel.MerConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IMerConfigService
    {
        /// <summary>
        /// 添加集群系统类别配置
        /// </summary>
        /// <param name="config">集群系统类别配置</param>
        /// <returns></returns>
        long SetMerConfig (MerConfigArg config);

        /// <summary>
        /// 删除集群配置
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteMerConfig (long id);

        /// <summary>
        /// 获取单个集群配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        RpcMerConfig GetMerConfig (long rpcMerId, long systemTypeId);

        /// <summary>
        /// 获取集群下的所有配置
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <returns>服务集群配置</returns>
        RpcMerConfigDatum[] GetMerConfigByMerId (long rpcMerId);


    }
}
