using System;
using System.IO;
using Wedonek.Demo.RemoteModel.File;
using Wedonek.Gateway.Modular.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;

namespace Wedonek.Gateway.Modular.Service
{
    internal class FileService : IFileService
    {
        public void SendFile ()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"File\22.png");
            IUpTask task = new UpDemoFile
            {
                Name = "Demo"
            }.Send(path, new UpFileAsync(this._UpFile), new UpProgressAction(this._Progress));
        }

        private void _UpFile (IFileUpResult result)
        {
            if (result.IsSuccess)
            {
                UpResult res = result.GetObject<UpResult>();
                Console.WriteLine("上传完成!");
                Console.WriteLine(string.Format("ConsumeTime:{0}\r\nres:{1}", result.ConsumeTime, res.ToJson()));
                return;
            }
            Console.WriteLine("上传失败!");
            Console.WriteLine(string.Format("ConsumeTime:{0}\r\nerror:{1}", result.ConsumeTime, result.Error));
        }

        private void _Progress (FileInfo file, int progress, long alreadyUpNum)
        {
            Console.WriteLine("上传进度!");
            Console.WriteLine(string.Format("progress:{0}\r\nalreadyUpNum:{1}", progress, alreadyUpNum));
        }
    }
}
