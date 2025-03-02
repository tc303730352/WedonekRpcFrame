namespace WeDonekRpc.HttpApiGateway.Model
{
    public class UrlRewrite
    {
        public string FormAddr { get; set; }

        public bool IsRegex { get; set; }

        public string ToUri { get; set; }

    }
}
