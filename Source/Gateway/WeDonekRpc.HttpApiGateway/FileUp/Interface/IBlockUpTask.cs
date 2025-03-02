using System.IO;
using WeDonekRpc.HttpApiGateway.FileUp.Model;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.FileUp.Interface
{
    internal interface IBlockUpTask : IUpTask
    {
        string TaskId { get; }

        string TaskKey { get; }

        UpBasicFile File { get; }

        UpTaskState UpState { get; }

        string ServerName { get; }

        int TimeOut { get; }

        BlockUpSate GetUpState ();

        bool WriteUpFile ( IUpFile file, int index );

        bool CheckIsUp ( int index );

        Stream GetStream ( int index );
    }
}