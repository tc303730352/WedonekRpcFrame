
using WeDonekRpc.TcpClient.Log;

namespace WeDonekRpc.TcpClient.SystemAllot
{
    internal class ReplyAllot : Interface.IAllot
    {
        internal override object Action (ref string type)
        {
            if (!Manage.PageManage.SubmitPage(this.PageId, this.Content, this.DataType))
            {
                IoLogSystem.AddTimeOutPage(type, this.Content, this.DataType);
            }
            return null;
        }
    }
}
