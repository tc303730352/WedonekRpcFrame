namespace RpcStore.RemoteModel
{
    public enum StageType
    {
        Send = 0,
        Answer = 1
    }
    public enum SchemeStatus
    {
        起草 = 0,
        启用 = 1,
        停用 = 2,
    }
    /// <summary>
    /// 配置范围
    /// </summary>
    public enum ConfigRange
    {
        全局 = 0,
        集群 = 2,
        节点类别 = 4,
        节点 = 1
    }
    public enum SysEventLevel
    {
        消息 = 0,
        警告 = 1,
        严重 = 2
    }
    public enum SysEventType
    {
        性能 = 0,
        错误 = 1,
        资源 = 2
    }
    public enum IpType
    {
        Ip4 = 0,
        Ip6 = 1,
    }
    public enum IdentityProwerType
    {
        API接口 = 2,
        RPC接口 = 4,
        RPC节点 = 8
    }
    public enum ResourceType
    {
        API接口 = 2,
        RPC接口 = 4
    }
    public enum ResourceState
    {
        正常 = 1,
        失效 = 2,
    }
    public enum SysConfigValueType
    {
        字符串 = 0,
        JSON = 1,
        数字 = 2,
        Bool = 3,
        Null = 4
    }
}
