using WeDonekRpc.ApiGateway.Model;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IUpCheck
    {
        void CheckFileSize(long size);
        void CheckUpFile(string fileName, int num);

        ApiUpSet ToUpSet();
    }
}