namespace WeDonekRpc.HttpApiDoc.Model
{
    public enum ApiReturnType : short
    {
        无 = 0,
        Json = 1,
        XML = 2,
        数据流 = 3,
        字符串 = 4,
        HttpStatus = 5,
        地址跳转 = 6,
        未知 = 7
    }
    public enum ApiDataType : short
    {
        数字 = 0,
        字符串 = 1,
        类 = 2,
        泛型字典 = 3,
        数组 = 4,
        泛型 = 5,
        布尔 = 6
    }
    public enum ApiRequestType : short
    {
        对象 = 0,
        数组 = 1,
        字典 = 2
    }
    public enum ElementType : short
    {
        基本 = 0,
        对象 = 1,
        枚举 = 2
    }
}
