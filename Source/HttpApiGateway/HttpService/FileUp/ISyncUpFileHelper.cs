using System.IO;

using HttpService.Interface;

namespace HttpService
{
        internal interface ISyncUpFileHelper
        {
                bool LoadFile(Stream stream, IUpFileRequest request);
        }
}