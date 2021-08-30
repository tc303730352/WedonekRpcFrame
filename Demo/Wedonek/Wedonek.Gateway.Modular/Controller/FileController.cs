using ApiGateway.Attr;
using HttpService;
using System;
using System.IO;
using Wedonek.Gateway.Modular.Model;
using Wedonek.Gateway.Modular.Interface;
namespace Wedonek.Gateway.Modular.Controller
{
        /// <summary>
        /// 文件接口
        /// </summary>
        internal class FileController : HttpApiGateway.ApiController
        {
                private IFileService _Service = null;

                public FileController(IFileService service)
                {
                        _Service = service;
                }
                /// <summary>
                /// 上传头像
                /// </summary>
                /// <returns></returns>
                [ApiUpConfig(10 * 1024 * 1024, new string[] { ".jpg", ".png" }, LimitFileNum = 1)]
                public UpResult UpHead()
                {
                        IUpFile file = this.Request.Files[0];
                        string path = file.SaveFile(string.Concat(@"file\", Guid.NewGuid().ToString("N"), Path.GetExtension(file.FileName)));
                        return new UpResult
                        {
                                FileUri = this.Request.GetLocalUri(path),
                                FileSize = file.FileSize
                        };
                }
                /// <summary>
                /// 上传文件演示
                /// </summary>
                [ApiPrower(false)]
                public void UpDemo()
                {
                        _Service.SendFile();
                }
        }
}
