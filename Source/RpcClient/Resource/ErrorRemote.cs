using RpcClient.Collect;

using RpcModel.ErrorManage;

namespace RpcClient.Resource
{
        internal class ErrorRemote
        {
                public static bool GetErrorId(string code, out long errorId, out string error)
                {
                        GetError obj = new GetError
                        {
                                ErrorCode = code
                        };
                        return RemoteCollect.Send(obj, out errorId, out error);
                }
                public static bool GetErrorMsg(string lang, string code, out string msg, out string error)
                {
                        GetErrorMsg obj = new GetErrorMsg
                        {
                                ErrorCode = code,
                                Lang = lang
                        };
                        return RemoteCollect.Send(obj, out msg, out error);
                }
                public static bool FindError(long errorId, out string code, out string error)
                {
                        GetErrorCode obj = new GetErrorCode
                        {
                                ErrorId = errorId
                        };
                        return RemoteCollect.Send(obj, out code, out error);
                }
        }
}
