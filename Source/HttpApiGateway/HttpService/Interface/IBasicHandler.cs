using System;
using System.Net;

namespace HttpService.Interface
{
        internal interface IBasicHandler : IHttpHandler, IDisposable, ICloneable
        {
                /// <summary>
                /// 请求路径
                /// </summary>
                string RequestPath { get; }


                /// <summary>
                /// 是否符合
                /// </summary>
                /// <param name="path"></param>
                /// <returns></returns>
                bool IsUsable(string path);
                /// <summary>
                /// 排序位
                /// </summary>
                string SortNum { get; }
                /// <summary>
                /// 响应文本
                /// </summary>
                string ResponseTxt { get; }
                bool IsGenerateMd5 { get; }

                /// <summary>
                /// 初始化请求
                /// </summary>
                /// <param name="context"></param>
                /// <returns></returns>
                void Init(HttpListenerContext context);

                /// <summary>
                /// 检查上传文件
                /// </summary>
                /// <param name="param">参数</param>
                /// <param name="index">编号</param>
                /// <returns></returns>
                bool CheckUpFile(UpFileParam param, int index);

                /// <summary>
                /// 验证文件
                /// </summary>
                /// <param name="upParam"></param>
                /// <param name="length"></param>
                /// <returns></returns>
                bool VerificationFile(UpFileParam upParam, long length);
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
