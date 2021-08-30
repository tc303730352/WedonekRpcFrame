using System;
using System.IO;
using System.Net;

namespace HttpService.Interface
{
        public interface IHttpResponse : IDisposable
        {
                /// <summary>
                /// 响应文本
                /// </summary>
                string ResponseTxt { get; }

                /// <summary>
                /// 响应状态码
                /// </summary>
                int StatusCode { get; }
                /// <summary>
                /// 是否截止
                /// </summary>
                bool IsEnd { get; }
                /// <summary>
                /// 内容类型
                /// </summary>
                string ContentType { get; set; }

                /// <summary>
                /// 追加Cookie
                /// </summary>
                /// <param name="cookie"></param>
                void AppendCookie(Cookie cookie);
                /// <summary>
                /// 移除Cookie
                /// </summary>
                /// <param name="name"></param>
                void RemoveCookie(string name);
                /// <summary>
                /// 设置Cookie
                /// </summary>
                /// <param name="toUpdateTime"></param>
                /// <param name="cacheTime"></param>
                void SetCache(DateTime toUpdateTime, int cacheTime);

                /// <summary>
                /// 设置缓存
                /// </summary>
                /// <param name="cacheType">缓存类型</param>
                void SetCacheType(string cacheType);
                /// <summary>
                /// 设置响应缓存
                /// </summary>
                /// <param name="cacheTime"></param>
                void SetCache(int cacheTime);
                /// <summary>
                /// 设置头部
                /// </summary>
                /// <param name="name"></param>
                /// <param name="value"></param>
                void SetHead(string name, string value);

                /// <summary>
                /// 设置缓存
                /// </summary>
                /// <param name="etag"></param>
                void SetCache(string etag);
                /// <summary>
                /// 设置Cookie
                /// </summary>
                /// <param name="cookie"></param>
                void SetCookie(Cookie cookie);

                /// <summary>
                /// 写入文件
                /// </summary>
                /// <param name="file"></param>
                void WriteFile(FileInfo file);

                /// <summary>
                /// 写入文本
                /// </summary>
                /// <param name="text"></param>
                void Write(string text);

                /// <summary>
                /// 写入流
                /// </summary>
                /// <param name="stream"></param>
                void WriteStream(Stream stream);
                /// <summary>
                /// 设置响应状态
                /// </summary>
                /// <param name="status"></param>
                void SetHttpStatus(HttpStatusCode status);

                /// <summary>
                /// 跳转
                /// </summary>
                /// <param name="url"></param>
                void Redirect(Uri url);

                /// <summary>
                /// 跳转链接
                /// </summary>
                /// <param name="url"></param>
                void Redirect(string url);

                /// <summary>
                /// 结束响应
                /// </summary>
                void End();
        }
}