using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WeDonekRpc.Helper.Http
{
    public class HttpHelper
    {
        public static RequestSet DefConfig
        {
            get;
        }
        static HttpHelper ()
        {
            DefConfig = new RequestSet();
        }
        internal static HttpResult GetResponse ( HttpResponseMessage response, RequestSet set, HttpCompletionOption option )
        {
            if ( response.StatusCode == HttpStatusCode.NoContent )
            {
                return new HttpResult(response.Content.Headers);
            }
            else if ( response.StatusCode != HttpStatusCode.OK )
            {
                HttpRequestMessage request = response.RequestMessage;
                throw new ErrorException("http.error." + (int)response.StatusCode);
            }
            else if ( option == HttpCompletionOption.ResponseHeadersRead )
            {
                return new HttpResult(response.Content.Headers);
            }
            else
            {
                using ( Stream stream = response.Content.ReadAsStream() )
                {
                    byte[] buffer = _ReadStream(stream, response.Content.Headers.ContentLength, set.ReadBufferSize);
                    return new HttpResult(buffer, response.Content.Headers, set.ResponseEncoding);
                }
            }
        }

        private static byte[] _ReadStream ( Stream SourceStream, long? slen, int size )
        {
            if ( !slen.HasValue )
            {
                return _ReadStream(SourceStream);
            }
            int len = (int)slen;
            if ( size < len )
            {
                size = len;
            }
            int index = 0;
            int max = len - size;
            byte[] myByte = new byte[size];
            using MemoryStream stream = new(len);
            stream.Position = 0;
            do
            {
                int num = SourceStream.Read(myByte, 0, size);
                if ( num > 0 )
                {
                    stream.Write(myByte, 0, num);
                    index += num;
                    if ( index > max && index != len )
                    {
                        size = len - index;
                        myByte = new byte[size];
                    }
                }
            } while ( index != len );
            return stream.ToArray();
        }
        private static byte[] _ReadStream ( Stream stream )
        {
            using ( MemoryStream ms = new MemoryStream() )
            {
                int read = 0;
                while ( true )
                {
                    read = stream.ReadByte();
                    if ( read == -1 )
                    {
                        return ms.GetBuffer();
                    }
                    ms.WriteByte((byte)read);
                }
            }
        }
    }
}
