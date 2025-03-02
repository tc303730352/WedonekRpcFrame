using System;
using System.IO;
using Wedonek.Gateway.Modular.Interface;
using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpService;
using WeDonekRpc.HttpService.Interface;

namespace Wedonek.Gateway.Modular.Controller
{
    /// <summary>
    /// 文件上传演示
    /// </summary>
    internal class FileUpController : ApiController
    {
        private readonly IFileService _Service = null;

        public FileUpController ( IFileService service )
        {
            this._Service = service;
        }
        /// <summary>
        /// 普通文件上传 最大10MB
        /// </summary>
        /// <returns></returns>
        [ApiUpConfig(10 * 1024 * 1024, new string[] { ".jpg", ".png" }, LimitFileNum = 1)]
        public UpResult UpHead ()
        {
            IUpFile file = this.Request.Files[0];
            string path = file.SaveFile(string.Concat(@"file\", Guid.NewGuid().ToString("N"), Path.GetExtension(file.FileName)));
            return new UpResult
            {
                FileUri = this.Request.GetUri(path),
                FileSize = file.FileSize
            };
        }
        /// <summary>
        /// RPC服务间上传文件演示
        /// </summary>
        [ApiPrower(false)]
        public void UpDemo ()
        {
            this._Service.SendFile();
        }
    }
}
