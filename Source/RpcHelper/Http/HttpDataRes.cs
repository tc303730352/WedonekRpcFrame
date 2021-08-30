using System.Net;
using System.Text;

namespace RpcHelper
{
        public class HttpDataRes
        {
                private readonly string _Uri = null;
                /// <summary>
                /// URI
                /// </summary>
                public string Uri => this._Uri;
                private readonly string _Post = null;
                /// <summary>
                /// PostJson
                /// </summary>
                public string Post => this._Post;
                internal void SetErrorMsg(string error)
                {
                        this.IsSystemError = true;
                        this.ErrorMsg = error;
                }
                public HttpDataRes(string uri, string post)
                {
                        this._Uri = uri;
                        this._Post = post;
                        this.IsSystemError = false;
                }
                public bool IsSystemError { get; set; }

                public string ErrorMsg { get; set; }
                public HttpStatusCode StatusCode { get; set; }
                public byte[] Content
                {
                        get;
                        set;
                }
                public object Arg { get; set; }

                public string GetStr()
                {
                        return this.Content == null ? null : Encoding.UTF8.GetString(this.Content).Replace("\0", string.Empty);
                }
        }
}
