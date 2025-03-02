namespace WeDonekRpc.HttpApiGateway.Idempotent
{
    public enum IdempotentType
    {
        关闭 = 0,
        Token = 1,
        Key = 2
    }
    public enum RequestMehod
    {
        Head = 0,
        Get = 1
    }
    public enum StatusSaveType
    {
        Local = 0,
        Redis = 1,
        Memcached = 2,
        Cache = 3
    }
}
