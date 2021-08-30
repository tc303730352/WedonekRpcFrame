namespace WebSocketGateway.Config
{
        internal class DefSocketConfig
        {
                public string RequestEncoding
                {
                        get;
                        set;
                } = "utf-8";

                public string ApiRouteFormat
                {
                        get;
                        set;
                } = "/api/{controller}/{name}";
        }
}
