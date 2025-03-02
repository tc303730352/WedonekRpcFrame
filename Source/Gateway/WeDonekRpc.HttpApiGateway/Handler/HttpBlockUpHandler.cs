using System.IO;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Collect;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.FileUp;
using WeDonekRpc.HttpService.Handler;

namespace WeDonekRpc.HttpApiGateway.Handler
{
    internal class HttpBlockUpHandler : BasicHandler, IApiHandler
    {
        private IBlockUpTask _Task;
        private int _Index = -1;
        private IService _ApiService = null;
        public HttpBlockUpHandler ( string uri, bool isRegex ) : base(uri, 99, isRegex)
        {
        }

        protected sealed override bool InitHandler ()
        {
            string fileId = this.Request.QueryString.Get("fileId");
            string index = this.Request.QueryString.Get("index");
            if ( fileId.IsNull() || index.IsNull() )
            {
                this.Response.SetHttpStatus(System.Net.HttpStatusCode.Forbidden);
                return false;
            }
            else if ( !fileId.Validate(WeDonekRpc.Helper.Validate.ValidateFormat.Guid) || !int.TryParse(index, out this._Index) )
            {
                this.Response.SetHttpStatus(System.Net.HttpStatusCode.Forbidden);
                return false;
            }
            else if ( !BlockUpCollect.GetTask(fileId, out this._Task) )
            {
                this.Response.SetHttpStatus(System.Net.HttpStatusCode.Forbidden);
                return false;
            }
            using ( IocScope scope = RpcClient.Ioc.CreateScore() )
            {
                this._ApiService = new ApiService(this, this._Task.ServerName, scope);
                if ( this._Task.CheckIsUp(this._Index) )
                {
                    this._ApiService.ReplyError("gateway.file.already.up");
                    return false;
                }
                return true;
            }
        }
        public override Stream GetSaveStream ( UpFileParam param )
        {
            return this._Task.GetStream(this._Index);
        }
        protected override bool CheckCache ( string etag, string toUpdateTime )
        {
            return false;
        }
        public override void Execute ()
        {
            if ( this._Task.WriteUpFile(this.Request.Files[0], this._Index) )
            {
                this._ApiService.Reply();
            }
            else
            {
                this._ApiService.ReplyError("gateway.file.already.up");
            }
        }
        public override void VerificationFile ( UpFileParam param, long fileSize )
        {
        }

        public override void CheckUpFile ( UpFileParam param )
        {
            if ( this.Request.Files.Count > 1 )
            {
                throw new ErrorException("http.file.no.allow.multiple");
            }
        }
    }
}
