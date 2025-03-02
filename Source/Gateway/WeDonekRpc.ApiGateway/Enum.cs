namespace WeDonekRpc.ApiGateway
{
    public enum ApiType : short
    {
        API接口 = 0,
        文件上传 = 1,
        数据流 = 2
    }
    public enum GatewayType : short
    {
        Http = 0,
        WebSocket = 1
    }
    public enum UpFileFormat : short
    {
        图片 = 2,
        Excel = 4,
        Word = 8,
        Pdf = 16,
        PPT = 32,
        自定义 = 64
    }
}
