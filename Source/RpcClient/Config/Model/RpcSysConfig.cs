using RpcCacheClient.Interface;

using RpcHelper;
namespace RpcClient.Config.Model
{
        /// <summary>
        /// 系统配置
        /// </summary>
        public class RpcSysConfig
        {
                /// <summary>
                /// 事务超时时间
                /// </summary>
                public int TranOverTime
                {
                        get;
                        set;
                } = 300;
                /// <summary>
                /// 关闭延迟时间(秒)
                /// </summary>
                public int CloseDelayTime { get; set; } = 10;

                /// <summary>
                /// 消息过期时间(秒)
                /// </summary>
                public int ExpireTime { get; set; } = 10;

               
                /// <summary>
                /// 锁超时时间
                /// </summary>
                public int LockTimeOut { get; set; } = 3;

                /// <summary>
                /// 远程锁超时时间
                /// </summary>
                public int LockOverTime { get; set; } = 2;

                /// <summary>
                /// 消息最大重试数
                /// </summary>
                public int MaxRetryNum { get; set; } = 3;
                /// <summary>
                /// 是否验证数据
                /// </summary>
                public bool IsValidateData { get; set; } = false;
                /// <summary>
                /// 是否启用队列
                /// </summary>
                public bool IsEnableQueue { get; set; } = false;
                /// <summary>
                /// 是否上传日志
                /// </summary>
                public bool IsUpLog { get; set; } = true;

                /// <summary>
                /// 日志上传级别
                /// </summary>
                public LogGrade LogGradeLimit { get; set; } = LogGrade.ERROR;
                /// <summary>
                /// 缓存类型
                /// </summary>
                public CacheType CacheType { get; set; } = CacheType.Redis;
        }
}
