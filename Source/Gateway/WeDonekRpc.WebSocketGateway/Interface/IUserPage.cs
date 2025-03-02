namespace WeDonekRpc.WebSocketGateway.Interface
{
        public interface IUserPage
        {
                /// <summary>
                /// 指令
                /// </summary>
                string Direct { get; }
                /// <summary>
                /// 包Id
                /// </summary>
                string PageId { get; }
        }
}