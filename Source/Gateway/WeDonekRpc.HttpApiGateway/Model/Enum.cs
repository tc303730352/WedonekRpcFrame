namespace WeDonekRpc.HttpApiGateway.Model
{
    public enum ResponseType
    {
        JSON = 0,
        XML = 1,
        Stream = 2,
        JumpUri = 3,
        HttpStatus = 4,
        String = 5,
        Html = 6
    }

    internal enum FuncParamType
    {
        参数 = 0,
        登陆状态 = 1,
        身份标识 = 2,
        Interface = 3,
        XML = 4,
        当前请求 = 5
    }
}
