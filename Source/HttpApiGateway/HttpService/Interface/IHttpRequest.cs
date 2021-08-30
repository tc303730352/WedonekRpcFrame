using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace HttpService.Interface
{
        public interface IHttpRequest
        {
                /// <summary>
                /// 请求地址
                /// </summary>
                Uri Url { get; }
                /// <summary>
                /// 初始化请求
                /// </summary>
                bool InitRequest();
                /// <summary>
                /// 是否为文件上传请求
                /// </summary>
                bool IsPostFile { get; }
                List<IUpFile> Files { get; }
                /// <summary>
                /// 请求头
                /// </summary>
                NameValueCollection Headers { get; }
                /// <summary>
                /// GET请求
                /// </summary>
                NameValueCollection QueryString { get; }

                /// <summary>
                /// 字节流
                /// </summary>
                byte[] Stream { get; }
                /// <summary>
                /// 请求的字符串
                /// </summary>
                string PostString { get; }

                /// <summary>
                /// 客户端Ip
                /// </summary>
                string ClientIp { get; }
                /// <summary>
                /// 请求来源
                /// </summary>
                Uri UrlReferrer { get; }

                /// <summary>
                /// 表单
                /// </summary>
                NameValueCollection Form { get; }
                /// <summary>
                /// 请求内容类型
                /// </summary>
                string ContentType { get; }
                /// <summary>
                /// 内容长度
                /// </summary>
                long ContentLength { get; }
                /// <summary>
                /// 请求方式
                /// </summary>
                string HttpMethod { get; }
                /// <summary>
                /// 获取请求的本地路径
                /// </summary>
                /// <returns></returns>
                string GetLocalPath();
                /// <summary>
                /// 获取Post的数据对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <returns></returns>
                T GetPostObject<T>() where T : class;

                /// <summary>
                /// 获取Cookie值
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                string GetCookieValue(string name);

                /// <summary>
                /// 获取Cookie
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                Cookie GetCookie(string name);

                /// <summary>
                /// 将路径转换为本地URI地址
                /// </summary>
                /// <param name="path"></param>
                /// <returns></returns>
                string GetLocalUri(string path);

        }
}