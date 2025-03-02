using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ExtendModel.SysError
{
    public class ExceptionMsg
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public ExceptionMsg ()
        {

        }
        public ExceptionMsg (Exception e, int depth = 0)
        {
            this.Message = e.Message;
            this.StackTrace = e.StackTrace;
            this.Source = e.Source;
            this.HelpLink = e.HelpLink;
            this.HResult = e.HResult;
            if (e.TargetSite != null)
            {
                this.Method = new Dictionary<string, string>
                                {
                                        { "方法名", e.TargetSite.Name },
                                        { "声明此方法的类", e.TargetSite.DeclaringType?.FullName },
                                        { "方法体", e.TargetSite.GetMethodBody()?.ToString() }
                                };
            }
            if (e.InnerException != null && depth < 10)
            {
                this.InnerException = new ExceptionMsg(e.InnerException, ++depth);
            }
            if (e.Data != null && e.Data.Count != 0)
            {
                this.Data = [];
                foreach (string i in e.Data.Keys)
                {
                    this.Data.Add(i, _GetValue(e.Data[i]));
                }
            }
        }
        private static string _GetValue (object obj)
        {
            Type type = obj.GetType();
            if (type.IsClass && type.Name != PublicDataDic.StringTypeName)
            {
                return obj.ToJson();
            }
            return obj.ToString();
        }
        /// <summary>
        /// 获取描述当前异常的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 获取调用堆栈上的即时框架字符串表示形式
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 获取或设置导致错误的应用程序或对象的名称
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 获取或设置指向与此异常关联的帮助文件链接
        /// </summary>
        public string HelpLink { get; set; }

        /// <summary>
        /// 获取或设置 HRESULT（一个分配给特定异常的编码数字值）。
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// 应发异常的方法
        /// </summary>
        public Dictionary<string, string> Method { get; set; }

        /// <summary>
        /// 获取提供有关异常的其他用户定义信息的键/值对集合。
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 下级错误
        /// </summary>
        public ExceptionMsg InnerException { get; set; }
    }
}
