namespace WeDonekRpc.HttpService.Model
{
    public enum HttpMethod
    {
        GET = 0,
        POST = 1,
        OPTIONS = 2
    }

    public enum RequestPathType
    {
        Full = 0,
        Regex = 1,
        Relative = 2,
        RulePath = 3
    }
}
