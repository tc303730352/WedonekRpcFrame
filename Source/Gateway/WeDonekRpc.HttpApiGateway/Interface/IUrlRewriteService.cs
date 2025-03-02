using System;
using System.Text.RegularExpressions;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IUrlRewriteService
    {
        void Init ();
        /// <summary>
        /// 添加正则表达式的URL重写
        /// </summary>
        /// <param name="regex">请求地址的正则表达式</param>
        /// <param name="filter">重写的委托返回终结点</param>
        void Add (Regex regex, Func<Uri, string> filter);
        /// <summary>
        /// 添加正则表达式的URL重写
        /// </summary>
        /// <param name="regex">请求地址的正则表达式</param>
        /// <param name="endPoint">对应的终结点</param>
        void Add (Regex regex, string endPoint);
        void Add (string path, Func<Uri, string> filter);
        void Add (string path, string endPoint);
        void Remove (Regex regex);
        void Remove (string path);
    }
}