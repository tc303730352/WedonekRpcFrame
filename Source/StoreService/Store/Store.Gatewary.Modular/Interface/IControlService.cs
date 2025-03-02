using WeDonekRpc.Model;
using RpcStore.RemoteModel.Control.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IControlService
    {
        /// <summary>
        /// 添加服务中心
        /// </summary>
        /// <param name="datum">中控服务资料</param>
        /// <returns></returns>
        int AddControl(RpcControlDatum datum);

        /// <summary>
        /// 删除服务中心
        /// </summary>
        /// <param name="id">服务中心ID</param>
        void DeleteControl(int id);

        /// <summary>
        /// 获取服务中心信息
        /// </summary>
        /// <param name="id">服务中心ID</param>
        /// <returns></returns>
        RpcControl GetControl(int id);

        /// <summary>
        /// 查询服务中心
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        RpcControlData[] QueryControl(IBasicPage paging, out int count);

        /// <summary>
        /// 修改服务中心资料
        /// </summary>
        /// <param name="id">服务中心ID</param>
        /// <param name="datum">服务中心资料</param>
        void SetControl(int id, RpcControlDatum datum);

    }
}
