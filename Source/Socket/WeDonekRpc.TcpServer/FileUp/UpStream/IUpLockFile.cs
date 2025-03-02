using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.TcpServer.FileUp.UpStream
{
        public interface IUpLockFile : System.IDisposable
        {
                bool InitLock(UpFile file, int blockSize, out string error);
                bool LoadLock(out string error);
                FileUpState GetUpState();
                bool BeginWrite(ushort blockId, out int position);
                bool EndWrite(ushort blockId, int count);

                bool CheckFileSign(ISaveStream stream);
                void UnLock();
        }
}