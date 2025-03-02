using System.IO;
using System.Net;
using System.Text;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;

namespace WeDonekRpc.HttpService.ResponseStream
{
    internal class BasicStream : IResponseStream
    {
        private Stream _Output;

        protected HttpListenerResponse _Response;
        private string _Range;
        public BasicStream (HttpListenerResponse response, string range)
        {
            this._Response = response;
            this._Range = range;
            this._Output = response.OutputStream;
        }

        public virtual void WriteText (string text, Encoding encoding)
        {
            this._Response.ContentEncoding = encoding;
            byte[] myByte = encoding.GetBytes(text);
            this._SetLength(myByte.Length);
            this._Output.Write(myByte, 0, myByte.Length);
        }
        public virtual void WriteStream (Stream stream, string extension)
        {
            this._Response.ContentEncoding = Encoding.UTF8;
            this._Response.ContentType = HttpHeaderHelper.GetContentType(extension);
            this._Write(stream);
        }
        public virtual void WriteFile (FileInfo file)
        {
            this._Response.ContentEncoding = Encoding.UTF8;
            this._Response.ContentType = HttpHeaderHelper.GetContentType(file.Extension);
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this._Write(stream);
            }
        }
        protected void _WriteFile (FileInfo file, string extension)
        {
            this._Response.ContentEncoding = Encoding.UTF8;
            this._Response.ContentType = HttpHeaderHelper.GetContentType(extension);
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this._Write(stream);
            }
        }
        protected virtual void _SetLength (long len)
        {
            this._Response.ContentLength64 = len;
        }
        private void _Write (Stream stream)
        {
            if (this._Range != null && this._Range.StartsWith("bytes="))
            {
                string range = this._Range.Remove(0, 6);
                if (!range.Contains(','))
                {
                    this._WriteStream(stream, range);
                }
                else
                {
                    this._WriteStream(stream, range.Split(','));
                }
            }
            else
            {
                this._SetLength(stream.Length);
                stream.CopyTo(this._Output);
            }
        }

        private void _WriteStream (Stream stream, string range)
        {
            long end = stream.Length;
            string[] i = range.Split('-');
            if (i[1] != string.Empty)
            {
                end = long.Parse(i[1]) + 1;
            }
            else
            {
                i[1] = ( stream.Length - 1 ).ToString();
            }
            long begin = i[0] == string.Empty ? stream.Length - end : long.Parse(i[0]);
            this._Response.StatusCode = 206;
            this._Response.Headers.Add("Accept-Ranges", "bytes");
            this._Response.Headers.Add("Content-Range", string.Format("bytes {0}-{1}/{2}", i[0], i[1], stream.Length));
            long len = end - begin;
            this._SetLength(len);
            stream.Position = begin;
            FileHelper.WriteStream(this._Output, stream, len);
        }
        private void _WriteStream (Stream stream, string[] ranges)
        {
            FilePage page = FileHelper.GetFilePage(stream, this._Response.ContentType, ranges);
            this._Response.StatusCode = 206;
            this._Response.Headers.Add("Accept-Ranges", "bytes");
            this._Response.Headers.Add("Content-Type", string.Concat("multipart/byteranges; boundary=", page.boundary));
            this._Response.ContentEncoding = Encoding.UTF8;
            this._SetLength(page.Size);
            FileHelper.WriteStream(this._Output, stream, page);
        }
        protected void _SetOutputStream (Stream stream)
        {
            this._Output = stream;
        }
        public void Dispose ()
        {
            this._Output.Flush();
            this._Output.Dispose();
        }
        public virtual void InitResponse (HttpListenerResponse response, string range)
        {
            this._Response = response;
            this._Range = range;
            this._Output = response.OutputStream;
        }
    }
}
