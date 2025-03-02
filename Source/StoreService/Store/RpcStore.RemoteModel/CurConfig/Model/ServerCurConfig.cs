using WeDonekRpc.Helper.Config;

namespace RpcStore.RemoteModel.CurConfig.Model
{
    public class ServerCurConfig
    {
        public string Name { get; set; }

        public ItemType ItemType { get; set; }

        public int? Prower { get; set; }

        public string Value { get; set; }

        public string Show { get; set; }

        public ServerCurConfig[] Children { get; set; }
    }
}
