using System;
using System.IO;
using System.Net;

namespace RpcHelper
{
        public class HttpStreamRes
        {
                public HttpStatusCode StatusCode
                {
                        get;
                        set;
                }
                public string ContentType
                {
                        get;
                        set;
                }
                public byte[] Stream
                {
                        get;
                        set;
                }
                public Exception Error
                {
                        get;
                        set;
                }
                public KeValue[] Cookies { get; set; }

                public KeValue[] HeadList { get; set; }

                public void SaveStream(string savePath)
                {
                        using (FileStream stream = File.Open(savePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete))
                        {
                                stream.Write(this.Stream, 0, this.Stream.Length);
                                stream.Flush();
                        }
                }
        }
}
