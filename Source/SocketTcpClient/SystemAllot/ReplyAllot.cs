
namespace SocketTcpClient.SystemAllot
{
        internal class ReplyAllot : Interface.IAllot
        {
                internal override object Action(ref string type)
                {
                        Manage.PageManage.SubmitPage(this.PageId, this.Content, this.DataType);
                        return null;
                }
        }
}
