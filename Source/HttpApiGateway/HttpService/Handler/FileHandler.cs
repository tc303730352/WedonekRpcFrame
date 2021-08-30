using System;
using System.IO;
using System.Net;

using HttpService.Helper;
namespace HttpService.Handler
{

        public class FileHandler : BasicHandler
        {
                public FileHandler(string uri, short sortNum = 100, bool isRegex = false) : base(uri, sortNum, isRegex)
                {

                }
                protected virtual string GetFilePath()
                {
                        return FileHelper.GetFilePath(this.Request.Url);
                }

                public override void Execute()
                {
                        string path = this.GetFilePath();
                        if (string.IsNullOrEmpty(path) || !File.Exists(path))
                        {
                                this.Response.SetHttpStatus(HttpStatusCode.NotFound);
                                return;
                        }
                        FileInfo file = new FileInfo(path);
                        if (this.LoadFile(file))
                        {
                                this.Response.WriteFile(file);
                        }
                }
                protected virtual bool LoadFile(FileInfo file)
                {
                        return true;
                }

                protected override bool CheckCache(string etag, string toUpdateTime)
                {
                        if (string.IsNullOrEmpty(toUpdateTime))
                        {
                                return false;
                        }
                        string filePath = this.GetFilePath();
                        if (!string.IsNullOrEmpty(filePath))
                        {
                                DateTime time = File.GetLastWriteTime(filePath);
                                DateTime myTime = DateTime.Parse(toUpdateTime);
                                return myTime < time;
                        }
                        return false;
                }
        }
}
