using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.Client.Model
{
    internal class RemoteCursor : IRemoteCursor
    {
        private readonly long[] _ServerId;
        private int _Index = 0;
        public int Num { get; }

        public int Current => this._Index;

        public RemoteCursor (long[] serverId)
        {
            this.Num = serverId.Length;
            this._ServerId = serverId;
        }

        public bool ReadNode (bool isCloseTrace, out IRemote remote)
        {
            if (this._Index == this.Num)
            {
                remote = null;
                return false;
            }
            long id = this._ServerId[this._Index++];
            return RemoteServerCollect.GetRemoteServer(id, isCloseTrace, out remote);
        }
    }
}
