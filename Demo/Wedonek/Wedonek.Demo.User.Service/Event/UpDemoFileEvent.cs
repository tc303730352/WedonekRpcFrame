using System;
using System.IO;
using Wedonek.Demo.RemoteModel.File;
using WeDonekRpc.Client.FileUp;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Route;
using WeDonekRpc.Helper;
namespace Wedonek.Demo.User.Service.Event
{
    /// <summary>
    /// 远程文件上传
    /// </summary>
    public class UpDemoFileEvent : FileUpEvent<UpDemoFile, UpResult>
    {
        public UpDemoFileEvent ()
        {

        }
        /// <summary>
        /// 上传前检查
        /// </summary>
        /// <param name="file">上传的文件信息和参数</param>
        /// <exception cref="ErrorException"></exception>
        protected override void CheckFile ( UpFileDatum<UpDemoFile> file )
        {
            if ( file.FileSize >= ( 1024 * 1024 * 10 ) )
            {
                throw new ErrorException("demo.file.size");
            }
            else if ( !file.FileName.EndsWith(".png") || !file.FileName.EndsWith(".zip") )
            {
                throw new ErrorException("demo.file.format");
            }
        }
        /// <summary>
        /// 上传完成
        /// </summary>
        /// <param name="file">上传的文件信息和参数</param>
        /// <param name="result">上传结果</param>
        /// <returns>返回发送方结果</returns>
        protected override UpResult UpComplate ( UpFileDatum<UpDemoFile> file, IUpResult result )
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UpFile\" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName));
            result.Save(path);
            return new UpResult
            {
                Name = Path.GetFileName(path)
            };
        }
    }
}
