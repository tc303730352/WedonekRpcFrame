using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ISysConfigService
    {
        void SetBasicConfig (BasicConfigSet set);
        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="config">配置资料</param>
        /// <returns></returns>
        void AddSysConfig (SysConfigAdd config);

        BasicSysConfig GetBasicConfig (BasicGetParam param);

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteSysConfig (long id);

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="id">配置ID</param>
        /// <returns>配置资料</returns>
        SysConfigDatum GetSysConfig (long id);

        /// <summary>
        /// 查询系统配置
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ConfigQueryData[] QuerySysConfig (QuerySysParam query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="configSet">配置值</param>
        void SetSysConfig (long id, SysConfigSet configSet);

        /// <summary>
        /// 设置配置的启用状态
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="isEnable">是否启用</param>
        void SetSysConfigIsEnable (long id, bool isEnable);

    }
}
