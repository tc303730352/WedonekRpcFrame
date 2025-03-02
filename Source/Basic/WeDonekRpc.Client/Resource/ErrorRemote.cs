using WeDonekRpc.Client.Collect;

using WeDonekRpc.Helper;

using WeDonekRpc.Model.ErrorManage;

namespace WeDonekRpc.Client.Resource
{
    internal class ErrorRemote
    {
        public static long GetErrorId (string code)
        {
            GetErrorId obj = new GetErrorId
            {
                ErrorCode = code
            };
            if (!RemoteCollect.Send(obj, out long errorId, out string error))
            {
                throw new ErrorException(error)
                {
                    Args = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "code", code }
                    }
                };
            }
            return errorId;
        }
        public static string GetErrorMsg (string lang, string code)
        {
            GetErrorMsg obj = new GetErrorMsg
            {
                ErrorCode = code,
                Lang = lang
            };
            if (RemoteCollect.Send(obj, out string msg, out string error))
            {
                return msg;
            }
            throw new ErrorException(error);
        }
        public static string FindError (long errorId)
        {
            GetErrorCode obj = new GetErrorCode
            {
                ErrorId = errorId
            };
            if (RemoteCollect.Send(obj, out string code, out string error))
            {
                return code;
            }
            throw new ErrorException(error);
        }
    }
}
