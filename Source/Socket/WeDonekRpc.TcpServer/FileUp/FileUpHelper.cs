using System;
using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.TcpServer.FileUp
{
    internal class FileUpHelper
    {
        public static string GetFileKey (FileLockState state, Stream stream)
        {
            if (state.IsMd5)
            {
                byte[] myByte = new byte[state.FileSize];
                stream.Position = 0;
                _ = stream.Read(myByte, 0, myByte.Length);
                return Tools.GetMD5(myByte);
            }
            else
            {
                byte[] myByte = new byte[158];
                long[] index = new long[]
                {
                                        0,
                                        (int)(state.FileSize / 4 * 1.5),
                                        (int)(state.FileSize / 2),
                                        (int)(state.FileSize / 4 * 2.5),
                                        state.FileSize - 30
                };
                BitConverter.GetBytes(state.FileSize).CopyTo(myByte, 0);
                foreach (long i in index)
                {
                    stream.Position = i;
                    _ = stream.Read(myByte, 128, 30);
                }
                return Tools.GetMD5(myByte);
            }
        }
    }
}
