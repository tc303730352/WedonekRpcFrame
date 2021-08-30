using System;
using System.IO;

namespace SocketTcpClient.UpFile
{
        internal class UpFileHelper
        {
                public static string GetFileCheckKey(long fileSize, FileStream stream, out bool isMd5)
                {
                        if (fileSize <= Config.SocketConfig.UpFileMD5LimitSize)
                        {
                                isMd5 = true;
                                byte[] myByte = new byte[fileSize];
                                stream.Read(myByte, 0, myByte.Length);
                                return RpcHelper.Tools.GetMD5(myByte);
                        }
                        else
                        {
                                byte[] myByte = new byte[158];
                                long[] index = new long[]
                                {
                                        0,
                                        (int)(fileSize / 4 * 1.5),
                                        (int)(fileSize / 2),
                                        (int)(fileSize / 4 * 2.5),
                                        fileSize - 30
                                };
                                BitConverter.GetBytes(fileSize).CopyTo(myByte, 0);
                                foreach (long i in index)
                                {
                                        stream.Position = i;
                                        stream.Read(myByte, 128, 30);
                                }
                                isMd5 = false;
                                return RpcHelper.Tools.GetMD5(myByte);
                        }
                }
        }
}
