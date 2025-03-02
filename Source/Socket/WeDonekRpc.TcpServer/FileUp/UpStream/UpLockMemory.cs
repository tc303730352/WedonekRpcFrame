using System.IO;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        internal class UpLockMemory : UpLockStream
        {
                public UpLockMemory()
                {
                }

                protected override bool _LoadStream(out Stream stream, out string error)
                {
                        stream = new MemoryStream();
                        error = null;
                        return true;
                }
        }
}
