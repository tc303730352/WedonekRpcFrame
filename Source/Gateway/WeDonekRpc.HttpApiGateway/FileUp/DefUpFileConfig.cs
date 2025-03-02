using System;
using System.IO;
using System.Linq;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.FileUp;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class DefUpFileConfig : IUpFileConfig
    {
        public static IUpFileConfig CurConfig = new DefUpFileConfig();
        private DefUpFileConfig ()
        {
            this.UpSet = ApiGatewayService.Config.UpConfig.ToUpSet();
            ApiGatewayService.Config.RefreshEvent(this._Refresh);
        }

        private void _Refresh (IHttpConfig config, string name)
        {
            if (name.StartsWith("gateway:up"))
            {
                this.UpSet = ApiGatewayService.Config.UpConfig.ToUpSet();
            }
        }

        public ApiUpSet UpSet
        {
            get;
            private set;
        }
        public string ApplyTempSavePath (IApiService service, UpFileParam param)
        {
            return string.Concat(this.UpSet.TempFileSaveDir, "\\", Guid.NewGuid().ToString("N"), Path.GetExtension(param.FileName));
        }

        public void CheckFileSize (IApiService service, long fileSize)
        {
            if (this.UpSet.MaxSize < fileSize)
            {
                throw new ErrorException("file.up.size.error", "size:" + this.UpSet.MaxSize);
            }
        }
        //分期信贷
        public void CheckUpFormat (IApiService service, string fileName, int num)
        {
            if (!this._CheckFormat(fileName))
            {
                throw new ErrorException("file.up.format.error", "format:" + this.UpSet.Extension.Join(','));
            }
            else if (this.UpSet.LimitFileNum != 0 && this.UpSet.LimitFileNum < num)
            {
                throw new ErrorException("file.up.num.limit", "limit:" + this.UpSet.LimitFileNum);
            }
        }
        private bool _CheckFormat (string fileName)
        {
            if (fileName.IsNull())
            {
                return false;
            }
            else
            {
                int index = fileName.LastIndexOf(".");
                if (index == -1)
                {
                    return false;
                }
                string ext = fileName.Substring(index).ToLower();
                return this.UpSet.Extension.IsExists(ext);
            }
        }

        public void InitFileUp (IApiService service)
        {
        }

        public void UpError (IApiService service, Exception e)
        {

        }
    }
}
