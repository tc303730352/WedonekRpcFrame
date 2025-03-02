using System;
using System.IO;
using System.Linq;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.FileUp;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    public class UpFileConfig : IUpFileConfig
    {
        private ApiUpSet _UpSet;
        public UpFileConfig ()
        {

        }
        public UpFileConfig (ApiUpSet upSet)
        {
            this._InitUpSet(upSet);
        }
        private void _InitUpSet (ApiUpSet upSet)
        {
            if (upSet.TempFileSaveDir.IsNull())
            {
                upSet.TempFileSaveDir = HttpService.HttpService.Config.File.TempDirPath;
            }
            else if (upSet.TempFileSaveDir.EndsWith('\\'))
            {
                upSet.TempFileSaveDir = upSet.TempFileSaveDir.Substring(0, upSet.TempFileSaveDir.Length - 1);
            }
            this._UpSet = upSet;
        }
        public ApiUpSet UpSet
        {
            get => this._UpSet;
            protected set
            {
                this._InitUpSet(value);
            }
        }
        public virtual string ApplyTempSavePath (IApiService service, UpFileParam param)
        {
            return string.Concat(this._UpSet.TempFileSaveDir, "\\", Guid.NewGuid().ToString("N"), Path.GetExtension(param.FileName));
        }

        public virtual void CheckFileSize (IApiService service, long fileSize)
        {
            if (this._UpSet.MaxSize < fileSize)
            {
                throw new ErrorException("file.up.size.error", "size:" + this._UpSet.MaxSize);
            }
        }
        //分期信贷
        public virtual void CheckUpFormat (IApiService service, string fileName, int num)
        {
            if (!this._CheckFormat(fileName))
            {
                throw new ErrorException("file.up.format.error", "format:" + this._UpSet.Extension.Join(','));
            }
            else if (this._UpSet.LimitFileNum != 0 && this._UpSet.LimitFileNum < num)
            {
                throw new ErrorException("file.up.num.limit", "limit:" + this._UpSet.LimitFileNum);
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
                return this._UpSet.Extension.IsExists(ext);
            }
        }

        public virtual void InitFileUp (IApiService service)
        {
        }

        public virtual void UpError (IApiService service, Exception e)
        {

        }
    }
}
