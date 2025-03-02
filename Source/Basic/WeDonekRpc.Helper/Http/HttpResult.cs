using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Http
{
    /// <summary>
    /// 请求结果
    /// </summary>
    public class HttpResult
    {
        private byte[] _data;

        private HttpContentHeaders _Header;
        private Encoding _Encoding;
        internal HttpResult ( byte[] datas,
          HttpContentHeaders headers ,
          Encoding encoding)
        {
            _Encoding = encoding;
            _data = datas;
            _Header = headers;
        }
        internal HttpResult (HttpContentHeaders headers)
        {
            _Header = headers;
        }
        /// <summary>
        /// 响应头类型
        /// </summary>
        public string ContentType
        {
            get => _Header.ContentType.ToString();
        }
        /// <summary>
        /// 源数据流
        /// </summary>
        public byte[] Content
        {
            get => _data;
        }

        /// <summary>
        /// 返回的Cookie
        /// </summary>
        public string Cookies
        {
            get => _Header.GetValues("Set-Cookie").FirstOrDefault();
        }
        /// <summary>
        /// 请求头
        /// </summary>
        public HttpContentHeaders Header { get => this._Header; }

        /// <summary>
        /// 获取字符串(默认UTF-8)
        /// </summary>
        /// <returns></returns>
        public string GetString ()
        {
            if ( _data.IsNull() )
            {
                return string.Empty;
            }
            return _Encoding.GetString(_data);
        }
       
        /// <summary>
        /// 获取JSON对象
        /// </summary>
        /// <returns></returns>
        public T GetObject<T> () where T : class
        {
            string json = this.GetString();
            if ( json == string.Empty )
            {
                return default(T);
            }
            return JsonTools.Json<T>(json);
        }
     
    }
}
