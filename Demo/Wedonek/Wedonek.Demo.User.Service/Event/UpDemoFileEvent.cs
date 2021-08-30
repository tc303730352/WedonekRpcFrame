using System;
using System.IO;

using RpcClient.Model;
using RpcClient.Route;

using SocketTcpServer.FileUp.Model;

using RpcHelper;

using Wedonek.Demo.RemoteModel.File;
namespace Wedonek.Demo.User.Service.Event
{
        /// <summary>
        /// 远程文件上传
        /// </summary>
        public class UpDemoFileEvent : FileUpEvent<UpDemoFile, UpResult>
        {
                public UpDemoFileEvent() : base("UpDemoFile")
                {

                }
                protected override void CheckFile(UpFileDatum<UpDemoFile> file)
                {
                        if (file.FileSize >= (1024 * 1024 * 10))
                        {
                                throw new ErrorException("demo.file.size");
                        }
                        else if (!file.FileName.EndsWith(".png"))
                        {
                                throw new ErrorException("demo.file.format");
                        }
                }
                protected override UpResult UpComplate(UpFileDatum<UpDemoFile> file, UpFileResult stream)
                {
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"File\" + Guid.NewGuid().ToString("N") + ".png");
                        stream.Save(path);
                        return new UpResult
                        {
                                Name = Path.GetFileName(path)
                        };
                }
        }
}
