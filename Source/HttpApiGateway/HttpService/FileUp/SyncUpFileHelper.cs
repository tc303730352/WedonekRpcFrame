using System.IO;
using System.Text;

using HttpService.Interface;

namespace HttpService
{
        internal class SyncUpFileHelper : ISyncUpFileHelper
        {
                public SyncUpFileHelper(IHttpRequest request)
                {
                        this._StreamLen = request.ContentLength;
                        this._boundary = Encoding.UTF8.GetBytes("--" + request.ContentType.Substring(30).Trim());
                }

                private readonly byte[] _boundary = null;

                private bool _IsLoadHead = true;

                private readonly long _StreamLen = 0;

                public bool LoadFile(Stream stream, IUpFileRequest handler)
                {
                        byte[] myByte = new byte[2048];
                        int len = 0;
                        UpFilePage page = null;
                        do
                        {
                                int num = stream.Read(myByte, 0, 2048);
                                if (num > 0)
                                {
                                        len += num;
                                        if (!this._SplictPage(handler, myByte, num, ref page))
                                        {
                                                return false;
                                        }
                                }
                        } while (len != this._StreamLen);
                        return true;

                }
                private bool _SplictPage(IUpFileRequest handler, byte[] myByte, int len, ref UpFilePage page)
                {
                        int index = 0;
                        return this._SplictPage(handler, myByte, len, ref index, ref page);
                }
                private bool _SplictPage(IUpFileRequest handler, byte[] myByte, int len, ref int index, ref UpFilePage page)
                {
                        if (page == null)
                        {
                                page = new UpFilePage(this._boundary, handler, this._IsLoadHead);
                                this._IsLoadHead = false;
                        }
                        if (page.LoadData(myByte, len, ref index))
                        {
                                page.Dispose();
                                page = null;
                                return index == len || this._SplictPage(handler, myByte, len, ref index, ref page);
                        }
                        else if (page.LoadProgress == UpFileProgress.加载错误)
                        {
                                page.Delete();
                                page = null;
                                return false;
                        }
                        return true;
                }


        }
}
