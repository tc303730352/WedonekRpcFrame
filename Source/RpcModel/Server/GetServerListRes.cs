namespace RpcModel
{
        public class GetServerListRes
        {
                public BalancedType BalancedType
                {
                        get;
                        set;
                }
                public ServerConfig[] Servers
                {
                        get;
                        set;
                }
                public ServerConfig[] BackUp
                {
                        get;
                        set;
                }
        }
}
