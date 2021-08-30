using System.IO;

using HttpService.Handler;

namespace HttpService.FileExtend
{
        internal class FontFileHandler : FileHandler
        {
                public FontFileHandler() : base(@"^(/.+)*/.+[.](woff|woff2|eot){1}$", 10, true)
                {
                }
                /// <summary>
                /// 加载文件
                /// </summary>
                /// <param name="file"></param>
                protected override bool LoadFile(FileInfo file)
                {
                        this.Response.SetCache(file.LastWriteTime, 600);
                        return true;
                }
        }
}
