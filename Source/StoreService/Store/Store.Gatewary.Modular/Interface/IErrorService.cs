using WeDonekRpc.Model;
using RpcStore.RemoteModel.Error.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IErrorService
    {
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="code">错误码</param>
        /// <returns></returns>
        ErrorDatum GetError (string code);

        /// <summary>
        /// 查询已有错误信息
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        ErrorData[] QueryError (ErrorQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="datum">错误信息</param>
        void SetErrorMsg (ErrorSet datum);

        /// <summary>
        /// 同步错误信息（有修改，无添加）
        /// </summary>
        /// <param name="datum"></param>
        /// <returns></returns>
        long SyncError (ErrorDatum datum);

    }
}
