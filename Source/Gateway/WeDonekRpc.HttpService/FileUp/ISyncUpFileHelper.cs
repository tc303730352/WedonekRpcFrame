using System.IO;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.FileUp
{
    internal interface ISyncUpFileHelper
    {
        void LoadFile ( Stream stream, IUpFileRequest request );
    }
}