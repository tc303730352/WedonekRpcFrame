using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Model.ErrorManage;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 错误事件
    /// </summary>
    internal class RpcErrorEvent : IRpcApiService
    {
        private readonly Interface.IErrorService _Service;
        public RpcErrorEvent ( Interface.IErrorService service)
        {
            this._Service = service;
        }

        /// <summary>
        /// 初始化错误信息
        /// </summary>
        /// <param name="obj"></param>
        public void InitError (InitError obj)
        {
            this._Service.InitError(obj.ErrorId);
        }
        public void RefreshError (RefreshError obj)
        {
            LocalErrorManage.Drop(obj.ErrorId);
        }
        /// <summary>
        /// 获取错误描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>错误描述</returns>
        public string GetErrorMsg (GetErrorMsg obj)
        {
            return LocalErrorManage.GetText(obj.ErrorCode, obj.Lang, obj.ErrorCode);
        }
        /// <summary>
        /// 通过错误Code获取错误ID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long GetErrorId (GetErrorId obj)
        {
            return LocalErrorManage.GetErrorId(obj.ErrorCode);
        }
        /// <summary>
        /// 查询错误
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string FindError (GetErrorCode obj)
        {
            return LocalErrorManage.GetCode(obj.ErrorId);
        }
    }
}
