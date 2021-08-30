using ApiGateway.Model;

namespace ApiGateway.Interface
{
        public interface IUpCheck
        {
                void CheckFileSize(long size);
                void CheckUpFile(string fileName, int num);

                ApiUpSet ToUpSet();
        }
}