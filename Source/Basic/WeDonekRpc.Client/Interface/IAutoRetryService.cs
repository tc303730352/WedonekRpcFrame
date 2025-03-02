using WeDonekRpc.ExtendModel.RetryTask.Model;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 自动重试服务
    /// </summary>
    public interface IAutoRetryService
    {
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="identityId"></param>
        void Cancel ( long serverId, string identityId );
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <typeparam name="T">指令对象</typeparam>
        /// <param name="model">指令体</param>
        /// <param name="identityId">唯一标识</param>
        /// <param name="show">任务说明</param>
        void AddTask<T> ( T model, string identityId, string show ) where T : class;
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <typeparam name="T">指令对象</typeparam>
        /// <param name="model">指令体</param>
        /// <param name="identityId">唯一标识</param>
        /// <param name="retry">重试配置</param>
        /// <param name="show">任务说明</param>
        void AddTask<T> ( T model, string identityId, RetryConfig retry, string show ) where T : class;
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identityId"></param>
        void Cancel<T> ( string identityId ) where T : class;

        /// <summary>
        /// 获取任务结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identityId"></param>
        /// <returns></returns>
        RetryResult GetRetryResult<T> ( string identityId ) where T : class;
        /// <summary>
        /// 重新执行任务
        /// </summary>
        /// <param name="identityId"></param>
        void Restart ( long serverId, string identityId );
    }
}