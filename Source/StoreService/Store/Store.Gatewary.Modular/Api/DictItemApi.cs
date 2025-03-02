using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.DictItem.Model;
using Store.Gatewary.Modular.Interface;
namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 字典项
    /// </summary>
    internal class DictItemApi : ApiController
    {
        private readonly IDictItemService _Service;

        public DictItemApi (IDictItemService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取字典项树形列表
        /// </summary>
        /// <param name="code">字典code</param>
        /// <returns></returns>
        public DictTreeItem[] GetTrees ([NullValidate("rpc.store.dict.code.null")][FormatValidate("rpc.store.dict.code.error", ValidateFormat.数字字母)] string code)
        {
            return this._Service.GetTrees(code);
        }
        /// <summary>
        /// 获取字典项列表
        /// </summary>
        /// <param name="code">字典code</param>
        /// <returns></returns>
        public DictItemDto[] GetDictItem ([NullValidate("rpc.store.dict.code.null")][FormatValidate("rpc.store.dict.code.error", ValidateFormat.数字字母)] string code)
        {
            return this._Service.GetDictItem(code);
        }
    }
}
