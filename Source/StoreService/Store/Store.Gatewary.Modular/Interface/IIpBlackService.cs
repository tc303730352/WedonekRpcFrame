using WeDonekRpc.Model;
using RpcStore.RemoteModel.IpBlack.Model;
using Store.Gatewary.Modular.Model.IpBlack;

namespace Store.Gatewary.Modular.Interface
{
    public interface IIpBlackService
    {
        /// <summary>
        /// 添加Ip黑名单
        /// </summary>
        /// <param name="datum">Ip黑名单</param>
        /// <returns></returns>
        long AddIpBack (IpBlackAddData datum);

        /// <summary>
        /// 删除Ip黑名单
        /// </summary>
        /// <param name="id">黑名单ID</param>
        void DropIpBack (long id);

        /// <summary>
        /// 获取Ip黑名单
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns></returns>
        IpBlackDatum GetIpBack (long id);

        /// <summary>
        /// 查询Ip黑名单
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        IpBlack[] QueryIpBlack (IpBlackQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改Ip黑名单
        /// </summary>
        /// <param name="id">Ip黑名单ID</param>
        /// <param name="datum">黑名单资料</param>
        void SetIpBlack (long id, IpBlackSet datum);

        /// <summary>
        /// 修改Ip黑名单备注
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="remark">备注</param>
        void SetIpBlackRemark (long id, string remark);

    }
}
