using Wedonek.Gateway.Modular.Model;
using WeDonekRpc.HttpApiGateway.FileUp.Interface;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace Wedonek.Gateway.Modular.Interface
{
    public interface IBlockUpService
    {
        void Complete ( IUpFileResult result, IUpFile file );
        void InitTask ( IBlockUp<UpFileArg> task, IUserState state );
        void UpFail ( UpBasicFile file, string error );
    }
}