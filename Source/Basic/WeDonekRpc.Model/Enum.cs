namespace WeDonekRpc.Model
{
    public enum RpcConfigType
    {
        独立配置 = 0,
        基础配置 = 1,
        自定义配置 = 2
    }
    /// <summary>
    /// 事务模式
    /// </summary>
    public enum RpcTranMode
    {
        NoReg = 0,
        Saga = 1,
        Tcc = 2,
        TwoPC = 3
    }
    /// <summary>
    /// 屏蔽类型
    /// </summary>
    public enum ShieldType
    {
        接口 = 0,
        指令 = 1
    }
    /// <summary>
    /// 服务类型
    /// </summary>
    public enum RpcServerType
    {
        未知 = 0,
        后台服务 = 1,
        服务网关 = 2
    }
    /// <summary>
    /// TCC事务提交状态
    /// </summary>
    public enum TranCommitStatus
    {
        待提交 = 0,
        已提交 = 1,
        提交失败 = 2,
        提交错误 = 3
    }

    /// <summary>
    /// 事务状态
    /// </summary>
    public enum TransactionStatus
    {
        执行中 = 0,
        已提交 = 1,
        已回滚 = 2,
        待回滚 = 3,
        回滚失败 = 4,
        回滚错误 = 5,
        待提交 = 6
    }
    /// <summary>
    /// 容器类型
    /// </summary>
    public enum ContainerType
    {
        无 = 0,
        docker = 1,
        k8s = 2,
        Other = 3
    }
    /// <summary>
    /// 服务限流类型
    /// </summary>
    public enum ServerLimitType
    {
        不启用 = 0,
        固定时间窗 = 1,
        流动时间窗 = 2,
        令牌桶 = 3,
        漏桶 = 4
    }
    /// <summary>
    /// 服务状态
    /// </summary>
    public enum RpcServiceState
    {
        正常 = 0,
        下线 = 2,
        停用 = 3,
        待启用 = 1
    }
    /// <summary>
    /// 可用状态
    /// </summary>
    public enum UsableState
    {
        正常 = 0,
        熔断 = 1,
        降级 = 2,
        限流 = 3,
        停用 = 4
    }
    /// <summary>
    /// 广播类型
    /// </summary>
    public enum BroadcastType
    {
        默认 = 0,
        消息队列 = 1,
        订阅 = 2
    }
    /// <summary>
    /// 远程锁类型
    /// </summary>
    public enum RemoteLockType
    {
        /// <summary>
        ///  只允许一个请求执行设定时间内所有请求返回相同结果
        /// </summary>
        同步锁 = 0,
        /// <summary>
        /// 只允许一个请求执行其它不是同服务的请求返回固定错误
        /// </summary>
        排斥锁 = 1,
        /// <summary>
        ///   只允许一个请求执行执行期间内所有请求返回这个执行请求的结果
        /// </summary>
        普通锁 = 2
    }

    /// <summary>
    /// 平衡方式
    /// </summary>
    public enum BalancedType
    {
        avg = 4,//平均
        single = 0,//单例
        random = 1,//随机
        weight = 2,//权重
        avgtime = 3,//平均响应时间
        randomWeight = 5//权重随机
    }
    /// <summary>
    /// 转发方式
    /// </summary>
    public enum TransmitType
    {
        //关闭
        close = 0,
        /// <summary>
        /// 计算字符串开始和结尾的字符ASCll码的与: ascii('a') | ascii('c') 获得范围48-127 之间的数字
        /// </summary>
        ZoneIndex = 1,
        /// <summary>
        /// HashCode
        /// </summary>
        HashCode = 2,
        /// <summary>
        /// 数字范围
        /// </summary>
        Number = 3,
        /// <summary>
        /// 固定值
        /// </summary>
        FixedType = 4
    }
    /// <summary>
    /// 远程锁状态
    /// </summary>
    public enum RemoteLockStatus
    {
        已锁 = 0,
        待同步 = 1,
        已释放 = 2
    }

}
