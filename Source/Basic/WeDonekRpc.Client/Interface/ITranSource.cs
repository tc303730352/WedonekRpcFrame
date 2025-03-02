namespace WeDonekRpc.Client.Interface
{
    public interface ITranSource
    {
        /// <summary>
        /// 请求体
        /// </summary>
        string Body { get; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        string Extend { get; set; }
        /// <summary>
        /// 获取请求体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetBody<T>() where T : class;
        /// <summary>
        /// 获取扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetExtend<T>() where T : class;
    }
}