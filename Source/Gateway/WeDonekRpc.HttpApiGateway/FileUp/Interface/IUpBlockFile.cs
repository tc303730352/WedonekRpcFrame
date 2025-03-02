using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    [IgnoreIoc]
    internal interface IUpBlockFile : IApiGateway
    {
        void Init ( IApiService service, IUpTask task );

        void UpFail ( string error );
    }
}
