using System;
using System.IO;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.IpBlack.Model
{
    public enum FileState
    {
        无更改 = 0,
        已更改 = 1,
        已删除 = 2
    }
    internal class FileCache
    {
        public FileCache(FileInfo file)
        {
            this.Name = file.Name;
            this.LastUpdateTime = file.LastWriteTime;
        }
        public string Name
        {
            get;
        }

        public DateTime LastUpdateTime
        {
            get;
            private set;
        }
        public FileState CheckState(FileInfo[] files, out FileInfo file)
        {
            file = files.Find(c => c.Name == this.Name);
            if (file == null)
            {
                return FileState.已删除;
            }
            else if (file.LastWriteTime != this.LastUpdateTime)
            {
                this.LastUpdateTime = file.LastWriteTime;
                return FileState.已更改;
            }
            return FileState.无更改;
        }
    }
}
