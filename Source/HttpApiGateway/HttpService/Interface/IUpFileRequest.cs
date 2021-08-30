using System.IO;
namespace HttpService.Interface
{
        internal interface IUpFileRequest
        {
                void SetForm(string form);
                bool IsGenerateMd5 { get; }
                bool CheckUpFile(UpFileParam param);
                Stream GetSaveStream(UpFileParam param);

                void SaveFile(UpFileParam upParam, Stream stream);
                void UpFail();
                bool VerificationFile(UpFileParam upParam, long length);
        }
}
