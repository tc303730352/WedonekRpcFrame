using WeDonekRpc.TcpServer.FileUp.Collect;
using WeDonekRpc.TcpServer.FileUp.Model;
using WeDonekRpc.TcpServer.Interface;
namespace WeDonekRpc.TcpServer.FileUp.Allot
{
    internal class FileAllot : IAllot
    {
        public override object Action()
        {
            if (this.Type == "FileAccredit")
            {
                FilePage page = this.GetData<FilePage>();
                return _FileAccredit(page);
            }
            else if (this.Type == "WriteStream")
            {
                FileAccreditCollect.WriteStream(this.Content);
            }
            else if (this.Type == "SyncUpState")
            {
                return _SyncUpResult(this.GetValue<int>());
            }
            return null;
        }
        private static FileBasicResult _SyncUpResult(int id)
        {
            if (!FileAccreditCollect.SyncUpState(id, out FileUpResult result, out string error))
            {
                return new FileBasicResult
                {
                    IsError = true,
                    ErrorCode = error
                };
            }
            return new SyncUpResult
            {
                Result = result
            };
        }
        private static FileBasicResult _FileAccredit(FilePage page)
        {
            if (!FileUpRouteCollect.GetStreamAllot(page.DirectName, out IStreamAllot allot))
            {
                return new FileBasicResult
                {
                    IsError = true,
                    ErrorCode = "socket.file.allot.no.reg"
                };
            }
            UpFile file = new UpFile(page.File);
            if (!FileUpRouteCollect.FileAccredit(allot, file, out FileUpState result, out int pageId, out string error))
            {
                return new FileBasicResult
                {
                    IsError = true,
                    ErrorCode = error
                };
            }
            else
            {
                return new FileAccreditResult
                {
                    PageId = pageId,
                    UpState = result
                };
            }
        }
    }
}
