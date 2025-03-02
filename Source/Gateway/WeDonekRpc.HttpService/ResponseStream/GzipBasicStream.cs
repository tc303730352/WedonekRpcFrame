using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using WeDonekRpc.HttpService.Collect;

namespace WeDonekRpc.HttpService.ResponseStream
{
    internal class GzipBasicStream : BasicStream
    {
        private bool _IsZip = false;
        public GzipBasicStream(HttpListenerResponse response, string range) : base(response, range)
        {
        }

        protected override void _SetLength(long len)
        {
            if (!this._IsZip)
            {
                base._Response.ContentLength64 = len;
            }
        }
        private void _EnableZip()
        {
            this._IsZip = true;
            base._Response.AddHeader("Content-Encoding", "gzip");
            base._SetOutputStream(new GZipStream(this._Response.OutputStream, CompressionMode.Compress, true));
        }
        public override void WriteText(string text, Encoding encoding)
        {
            if (GzipCollect.CheckTextIsZip(text.Length))
            {
                this._EnableZip();
            }
            base.WriteText(text, encoding);
        }
        public override void WriteStream(Stream stream, string extension)
        {
            if (GzipCollect.CheckIsZip(stream, extension))
            {
                this._EnableZip();
            }
            base.WriteStream(stream, extension);
        }
        public override void WriteFile(FileInfo file)
        {
            if (GzipCollect.CheckIsZip(file, out FileInfo cacheFile))
            {
                this._Response.AddHeader("Content-Encoding", "gzip");
                if (cacheFile == null)
                {
                    this._IsZip = true;
                    base._SetOutputStream(new GZipStream(this._Response.OutputStream, CompressionMode.Compress, true));
                    base.WriteFile(file);
                    GzipCollect.CacheFile(file);
                }
                else
                {
                    base._WriteFile(cacheFile,file.Extension);
                }
            }
            else
            {
                base.WriteFile(file);
            }
        }
    }
}
