using System;

namespace RpcHelper
{
        /// <summary>
        /// 错误异常
        /// </summary>
        public class ErrorException : Exception
        {
                /// <summary>
                /// 异常信息
                /// </summary>
                /// <param name="message">消息</param>
                public ErrorException(string code, LogGrade grade = LogGrade.DEBUG)
                {
                        this.ErrorCode = ErrorManage.FormatError(code, out this._Param);
                        this.LogGrade = grade;
                }
                public ErrorException(Exception e, LogGrade grade = LogGrade.ERROR) : base(string.Empty, e)
                {
                        this.ErrorCode = "public.system.error";
                        this.LogGrade = grade;
                        this.IsSystemError = true;
                }
                /// <summary>
                /// 异常信息
                /// </summary>
                /// <param name="code"></param>
                /// <param name="param"></param>
                public ErrorException(string code, string param, LogGrade grade = LogGrade.DEBUG) : this(code, grade)
                {
                        this._Param = param;
                }
                private readonly string _Param = null;

                private string _Msg = null;

                public bool IsSystemError
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 错误信息
                /// </summary>
                public override string Message
                {
                        get
                        {
                                if (this._Msg == null)
                                {
                                        this._Msg = ErrorManage.GetErrorMsg(this.ErrorCode);
                                }
                                return this._Msg;
                        }
                }
                /// <summary>
                /// 是否结束
                /// </summary>
                public bool IsEnd
                {
                        get;
                        set;
                }
                /// <summary>
                /// 错误码
                /// </summary>
                public string ErrorCode
                {
                        get;
                }
                /// <summary>
                /// 日志级别
                /// </summary>
                public LogGrade LogGrade { get; }
                /// <summary>
                /// 参数值
                /// </summary>
                public string Param => this._Param;
                public ErrorMsg GetError()
                {
                        ErrorMsg msg= ErrorManage.GetError(this.ErrorCode);
                        if(msg == null)
                        {
                                return new ErrorMsg
                                {
                                        ErrorId=-1,
                                        ErrorCode=this.ErrorCode,
                                        Msg=this.ErrorCode,
                                        Lang=ErrorManage.DefLang
                                };
                        }
                        return msg;
                }

                public override string ToString()
                {
                        if (this._Param == null)
                        {
                                return this.ErrorCode;
                        }
                        return string.Format("{0}[{1}]", this.ErrorCode, this._Param);
                }
                public static ErrorException FormatError(Exception e)
                {
                        if (e.InnerException != null)
                        {
                                e = e.InnerException;
                        }
                        if (e is ErrorException k)
                        {
                                return k;
                        }
                        return new ErrorException(e);
                }
                public static ErrorException FormatError(Exception e, LogGrade grade)
                {
                        if (e.InnerException != null)
                        {
                                e = e.InnerException;
                        }
                        if (e is ErrorException k)
                        {
                                return k;
                        }
                        return new ErrorException(e, grade);
                }
                public void SaveLog(string group = "def")
                {
                        new ErrorLog(this, group).Save();
                }
                public void SaveLog(string show, string group = "def")
                {
                        ErrorLog log = new ErrorLog(this, group)
                        {
                                LogContent = show
                        };
                        log.Save();
                }
        }
}
