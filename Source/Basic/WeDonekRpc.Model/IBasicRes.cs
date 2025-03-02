namespace WeDonekRpc.Model
{
    /// <summary>
    /// 结果
    /// </summary>
    public interface IBasicRes
    {
        /// <summary>
        /// 错误信息码
        /// </summary>
        string ErrorMsg { get; }
        /// <summary>
        /// 是否错误
        /// </summary>
        bool IsError { get; }
    }
}