using RpcModel.ErrorManage;

using RpcSyncService.Collect;

using RpcHelper;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 错误事件
        /// </summary>
        internal class RpcErrorEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 初始化错误信息
                /// </summary>
                /// <param name="obj"></param>
                public static void InitError(InitError obj)
                {
                        ErrorLangCollect.Clear(obj.ErrorId);
                        ErrorCollect.InitError(obj.ErrorId, obj.ErrorCode);
                }
                /// <summary>
                /// 获取错误描述
                /// </summary>
                /// <param name="obj"></param>
                /// <returns>错误描述</returns>
                public static string GetErrorMsg(GetErrorMsg obj)
                {
                        if (ErrorManage.GetErrorMsg(obj.ErrorCode, obj.Lang, out ErrorMsg emsg))
                        {
                                return emsg.Msg;
                        }
                        throw new ErrorException("rpc.error.get.error");
                }
                /// <summary>
                /// 通过错误Code获取错误ID
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static long GetError(GetError obj)
                {
                        if (ErrorManage.GetErrorMsg(obj.ErrorCode, out ErrorMsg emsg))
                        {
                                return emsg.ErrorId;
                        }
                        throw ErrorCollect.DefError;
                }
                /// <summary>
                /// 查询错误
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public static string FindError(GetErrorCode obj)
                {
                        string code = ErrorManage.QueryCode(obj.ErrorId);
                        if (string.IsNullOrEmpty(code))
                        {
                                throw ErrorCollect.DefError;
                        }
                        return code;
                }
        }
}
