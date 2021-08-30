namespace RpcHelper
{
        public interface IErrorEvent
        {
                /// <summary>
                /// 加载所有错误
                /// </summary>
                /// <returns></returns>
                ErrorMsg[] LoadError(string lang);
                /// <summary>
                /// 查找错误信息
                /// </summary>
                /// <param name="code"></param>
                /// <param name="lang"></param>
                /// <param name="msg"></param>
                /// <returns></returns>
                bool FindError(string code, string lang, out ErrorMsg msg);
                /// <summary>
                /// 查找错误Code
                /// </summary>
                /// <param name="code"></param>
                /// <returns></returns>
                string FindErrorCode(long code);
                /// <summary>
                /// 删除错误
                /// </summary>
                /// <param name="errorId">错误Id</param>
                /// <returns></returns>
                bool DropError(long errorId);
        }
}
