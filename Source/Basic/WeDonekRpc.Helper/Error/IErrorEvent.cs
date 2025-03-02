namespace WeDonekRpc.Helper.Error
{
    public interface IErrorEvent
    {
        /// <summary>
        /// 查找错误信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="lang"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        ErrorMsg GetError ( string code, string lang );
        /// <summary>
        /// 查找错误Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string FindErrorCode ( long errorId );

        long FindErrorId ( string code );
        /// <summary>
        /// 删除错误
        /// </summary>
        /// <param name="errorId">错误Id</param>
        /// <returns></returns>
        void DropError ( ErrorMsg error );

        void DropError ( long errorId, string code );
    }
}
