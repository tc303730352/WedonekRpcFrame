using System;
using System.Collections.Generic;
using System.Net;

namespace WeDonekRpc.HttpService.Interface
{
    public interface IBasicHandler : IHttpHandler, IDisposable, ICloneable, IUpFileRequest
    {
        /// <summary>
        /// 是否全路径
        /// </summary>
        bool IsFullPath { get; }

        /// <summary>
        /// 请求路径
        /// </summary>
        string RequestPath { get; }

        /// <summary>
        /// 是否为正则表达式
        /// </summary>
        bool IsRegex { get; }

        /// <summary>
        /// 是否符合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsUsable(string path);

        /// <summary>
        /// 排序位
        /// </summary>
        int SortNum { get; }

        /// <summary>
        /// 响应文本
        /// </summary>
        string ResponseTxt { get; }

        /// <summary>
        /// 初始化请求
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        void Init(HttpListenerContext context, Dictionary<string, string> args);

        /// <summary>
        /// 执行错误
        /// </summary>
        /// <param name="error"></param>
        void ExecError(Exception error);

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        bool Verification();

        /// <summary>
        /// 执行
        /// </summary>
        void Execute();

        /// <summary>
        /// 结束
        /// </summary>
        void End();
    
    }
}
