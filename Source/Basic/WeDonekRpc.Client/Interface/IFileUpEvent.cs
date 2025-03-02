using WeDonekRpc.Client.FileUp;

namespace WeDonekRpc.Client.Interface
{
    public interface IFileUpEvent<Result>
    {
        void CheckFile(IUpFileInfo file);
        string GetFileSaveDir(IUpFileInfo file);

        Result UpComplate(IUpFileInfo file, IUpResult result);
    }
}
