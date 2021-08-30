using System.IO;

namespace HttpApiGateway.Interface
{
        public interface IResponseStream
        {
                bool IsExists { get; }
                string FileName { get; }
                string Extension { get; }
                Stream Open();
        }
}