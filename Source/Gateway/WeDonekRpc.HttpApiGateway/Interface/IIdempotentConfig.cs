using System;
using WeDonekRpc.HttpApiGateway.Idempotent;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    /// <summary>
    /// 重复请求配置
    /// </summary>
    public interface IIdempotentConfig
    {
        /// <summary>
        /// 刷新事件
        /// </summary>
        event Action<IIdempotentConfig> RefreshEvent;
        /// <summary>
        /// 处理方式
        /// </summary>
        IdempotentType IdempotentType { get;}
        /// <summary>
        /// 是否启用路由
        /// </summary>
        bool IsEnableRoute { get; }
        /// <summary>
        /// 申请token的路由配置
        /// </summary>
        RouteSet TokenRoute { get; }
        /// <summary>
        /// 参数名字
        /// </summary>
         string ArgName { get; }
        /// <summary>
        /// 请求参数存放方式
        /// </summary>
        RequestMehod RequestMehod { get; }
        /// <summary>
        /// Token过期时间(秒)
        /// </summary>
        int Expire { get; }
        /// <summary>
        /// Token存储方式
        /// </summary>
        StatusSaveType SaveType { get; }

        /// <summary>
        /// 检查路由地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool CheckIsLimit(string path);
    }
}