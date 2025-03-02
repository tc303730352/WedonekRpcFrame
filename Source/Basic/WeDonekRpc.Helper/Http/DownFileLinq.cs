using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace WeDonekRpc.Helper.Http
{
    public enum DownProgress
    {
        开始 = 0,
        下载中 = 1,
        结束 = 2
    }
    public delegate void DownEvent ( byte[] stream, int size, DownProgress progress, HttpResponseMessage response );
    public static class DownFileLinq
    {
        public static void DownFile<T> ( this HttpClient client, Uri uri, T data, DownEvent downEvent, RequestSet set ) where T : class
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                msg.Content = new StringContent(data.ToJson(), set.RequestEncoding, "application/json");
                DownFile(client, msg, downEvent, set);
            }
        }
        public static void DownFile<T> ( this HttpClient client, Uri uri, T data, DownEvent downEvent ) where T : class
        {
            DownFile(client, uri, data, downEvent, HttpHelper.DefConfig);
        }
        public static void DownFile ( this HttpClient client, Uri uri, DownEvent downEvent )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, uri) )
            {
                DownFile(client, msg, downEvent, HttpHelper.DefConfig);
            }
        }
        public static void DownFile ( this HttpClient client, Uri uri, DownEvent downEvent, RequestSet set )
        {
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, uri) )
            {
                DownFile(client, msg, downEvent, set);
            }
        }
        public static void DownFile ( this HttpClient client, HttpRequestMessage msg, DownEvent downEvent, RequestSet set )
        {
            set.Init(client, msg.Content, msg.RequestUri);
            using ( HttpResponseMessage response = client.Send(msg, HttpCompletionOption.ResponseContentRead) )
            {
                _SaveStream(response, downEvent, set);
            }
        }
        public static void DownFile ( this HttpClient client, HttpRequestMessage msg, FileInfo saveFile, RequestSet set, HttpCompletionOption option = HttpCompletionOption.ResponseContentRead )
        {
            if ( saveFile.Exists )
            {
                throw new ErrorException("http.down.file.exists");
            }
            if ( !saveFile.Directory.Exists )
            {
                saveFile.Directory.Create();
            }
            set.Init(client, msg.Content, msg.RequestUri);
            using ( HttpResponseMessage response = client.Send(msg, HttpCompletionOption.ResponseContentRead) )
            {
                using ( Stream fileStream = saveFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite) )
                {
                    _SaveStream(response, fileStream, set);
                }
            }
        }
        public static void DownFile ( this HttpClient client, Uri uri, FileInfo saveFile )
        {
            DownFile(client, uri, saveFile, HttpHelper.DefConfig);
        }
        public static void DownFile ( this HttpClient client, Uri uri, FileInfo saveFile, RequestSet set )
        {
            if ( saveFile.Exists )
            {
                throw new ErrorException("http.down.file.exists");
            }
            if ( !saveFile.Directory.Exists )
            {
                saveFile.Directory.Create();
            }
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, uri) )
            {
                set.Init(client, msg.Content, uri);
                using ( HttpResponseMessage response = client.Send(msg, HttpCompletionOption.ResponseContentRead) )
                {
                    using ( Stream fileStream = saveFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite) )
                    {
                        _SaveStream(response, fileStream, set);
                    }
                }
            }
        }
        public static void DownFile<T> ( this HttpClient client, Uri uri, T data, FileInfo saveFile, RequestSet set ) where T : class
        {
            if ( saveFile.Exists )
            {
                throw new ErrorException("http.down.file.exists");
            }
            if ( !saveFile.Directory.Exists )
            {
                saveFile.Directory.Create();
            }
            using ( HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, uri) )
            {
                msg.Content = new StringContent(data.ToJson(), set.RequestEncoding, "application/json");
                set.Init(client, msg.Content, uri);
                using ( HttpResponseMessage response = client.Send(msg, HttpCompletionOption.ResponseContentRead) )
                {
                    using ( Stream fileStream = saveFile.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite) )
                    {
                        _SaveStream(response, fileStream, set);
                    }
                }
            }
        }
        private static void _SaveStream ( HttpResponseMessage response, DownEvent action, RequestSet set )
        {
            if ( response.StatusCode != HttpStatusCode.OK )
            {
                HttpRequestMessage request = response.RequestMessage;
                throw new ErrorException("http.error." + (int)response.StatusCode);
            }
            else
            {
                action(null, 0, DownProgress.开始, response);
                using ( Stream stream = response.Content.ReadAsStream() )
                {
                    _ReadStream(stream, response, action, response.Content.Headers.ContentLength.GetValueOrDefault(-1), set.ReadBufferSize);
                }
            }
        }
        private static void _ReadStream ( Stream stream, HttpResponseMessage response, DownEvent action, int size )
        {
            using MemoryStream ms = new(size);
            int read = 0;
            while ( true )
            {
                read = stream.ReadByte();
                if ( read == -1 )
                {
                    action(ms.ToArray(), (int)ms.Length, DownProgress.结束, response);
                    return;
                }
                ms.WriteByte((byte)read);
                if ( ms.Length == size )
                {
                    action(ms.ToArray(), size, DownProgress.下载中, response);
                    ms.SetLength(0);
                }
            }
        }
        private static void _ReadStream ( Stream source, HttpResponseMessage response, DownEvent action, long len, int size )
        {
            if ( len == -1 )
            {
                _ReadStream(source, response, action, size);
                return;
            }
            if ( size < len )
            {
                size = (int)len;
            }
            long read = 0;
            long max = len - size;
            byte[] myByte = new byte[size];
            int num;
            do
            {
                num = source.Read(myByte, 0, size);
                if ( num > 0 )
                {
                    read += num;
                    if ( read == len )
                    {
                        action(myByte, num, DownProgress.结束, response);
                        break;
                    }
                    action(myByte, num, DownProgress.下载中, response);
                    if ( read > max )
                    {
                        size = (int)( len - read );
                        myByte = new byte[size];
                    }
                }
            } while ( true );
        }
        private static void _SaveStream ( HttpResponseMessage response, Stream fileStream, RequestSet set )
        {
            if ( response.StatusCode != HttpStatusCode.OK )
            {
                HttpRequestMessage request = response.RequestMessage;
                throw new ErrorException("http.error." + (int)response.StatusCode);
            }
            else
            {
                using ( Stream stream = response.Content.ReadAsStream() )
                {
                    _SaveStream(stream, fileStream, response.Content.Headers.ContentLength.Value, set.ReadBufferSize);
                }
            }
        }
        private static void _SaveStream ( Stream stream, Stream fileStream, long len, int size )
        {
            if ( size < len )
            {
                size = (int)len;
            }
            long index = 0;
            long max = len - size;
            byte[] myByte = new byte[size];
            fileStream.Position = 0;
            do
            {
                int num = stream.Read(myByte, 0, size);
                if ( num > 0 )
                {
                    fileStream.Write(myByte, 0, num);
                    index += num;
                    if ( index > max && index != len )
                    {
                        size = (int)( len - index );
                        myByte = new byte[size];
                    }
                }
            } while ( index != len );
            fileStream.Flush();
        }
    }
}
