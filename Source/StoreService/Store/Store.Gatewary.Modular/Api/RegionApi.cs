using RpcStore.RemoteModel.ServerRegion.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 机房管理
    /// </summary>
    internal class RegionApi : ApiController
    {
        private readonly IRegionService _Service;

        public RegionApi (IRegionService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加机房
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [ApiPrower("rpc.store.admin")]
        public int Add (RegionDatum add)
        {
            return this._Service.Add(add);
        }
        /// <summary>
        /// 检查机房名
        /// </summary>
        /// <param name="name"></param>
        public void CheckName ([NullValidate("rpc.store.region.name.null")]
        [LenValidate("rpc.store.region.name.len", 2, 50)]string name)
        {
            this._Service.CheckName(name);
        }
        /// <summary>
        /// 删除机房
        /// </summary>
        /// <param name="id"></param>
        [ApiPrower("rpc.store.admin")]
        public void Delete ([NumValidate("rpc.store.region.id.error", 1, int.MaxValue)] int id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 获取机房信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegionDto Get ([NumValidate("rpc.store.region.id.error", 1, int.MaxValue)] int id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 获取机房列表
        /// </summary>
        /// <returns></returns>
        public RegionBasic[] GetList ()
        {
            return this._Service.GetRegionList();
        }
        /// <summary>
        /// 查询机房信息
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public PagingResult<RegionData> Query (BasicPage paging)
        {
            return this._Service.Query(paging);
        }
        /// <summary>
        /// 修改机房信息
        /// </summary>
        /// <param name="set"></param>
        [ApiPrower("rpc.store.admin")]
        public void Update (IntParam<RegionDatum> set)
        {
            this._Service.Update(set.Id, set.Param);
        }
    }
}
