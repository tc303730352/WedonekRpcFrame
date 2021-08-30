using SocketTcpServer.Manage;

namespace SocketTcpServer.SystemAllot
{
        internal class ReplyPageAllot : Interface.IAllot
        {
                public override object Action()
                {
                        PageManage.SubmitPage(this.PageId, this.Content, this.DataType);
                        return null;
                }
        }
}
