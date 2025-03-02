
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class UserPage : IUserPage
    {
        public UserPage ()
        {

        }
        public UserPage (string direct)
        {
            this.Direct = direct;
        }
        public string Direct { get; set; }

        public string PageId { get; set; }
    }
}
