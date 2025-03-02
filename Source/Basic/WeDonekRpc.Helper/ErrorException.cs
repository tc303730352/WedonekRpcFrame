using System;
using System.Collections.Generic;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper
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
        public ErrorException ( string code, LogGrade grade = LogGrade.Information )
        {
            this.ErrorCode = LocalErrorManage.Format(code, out this._Param);
            this.LogGrade = grade;
        }
        public ErrorException ( string code, Dictionary<string, string> args )
        {
            this.ErrorCode = LocalErrorManage.Format(code, out this._Param);
            this.LogGrade = LogGrade.Information;
            this.Args = args;
        }
        public ErrorException ( string code, string remark, Dictionary<string, string> args )
        {
            this.Remark = remark;
            this.LogGrade = LogGrade.Information;
            this.ErrorCode = LocalErrorManage.Format(code, out this._Param);
            this.Args = args;
        }
        public ErrorException ( Exception e, Dictionary<string, string> args ) : base(e.Message, e)
        {
            this.Args = args;
            if ( e is ErrorException i )
            {
                this.IsSystemError = i.IsSystemError;
                this.ErrorCode = i.ErrorCode;
                this.LogGrade = i.LogGrade;
                if ( i.Args != null )
                {
                    this._InitArgs(i.Args);
                }
            }
            else
            {
                this.LogGrade = LogGrade.ERROR;
                this.ErrorCode = "public.system.error";
                this.IsSystemError = true;
            }
        }
        public ErrorException ( Exception e, string code, Dictionary<string, string> args ) : this(e, code)
        {
            this.Args = args;
        }
        public ErrorException ( Exception e, string code ) : base(e.Message, e)
        {
            if ( e is ErrorException i )
            {
                this.IsSystemError = i.IsSystemError;
                this.ErrorCode = i.ErrorCode;
                this.LogGrade = i.LogGrade;
                if ( i.Args != null )
                {
                    this._InitArgs(i.Args);
                }
            }
            else
            {
                this.LogGrade = LogGrade.ERROR;
                this.ErrorCode = code;
                this.IsSystemError = true;
            }
        }

        public ErrorException ( Exception e, LogGrade grade = LogGrade.ERROR ) : base(e.Message, e.InnerException ?? e)
        {
            this.LogGrade = grade;
            if ( e is ErrorException i )
            {
                this.IsSystemError = i.IsSystemError;
                this.ErrorCode = i.ErrorCode;
                if ( i.Args != null )
                {
                    this._InitArgs(i.Args);
                }
            }
            else
            {
                this.ErrorCode = "public.system.error";
                this.IsSystemError = true;
            }
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="param"></param>
        public ErrorException ( string code, string param, LogGrade grade = LogGrade.Information ) : this(code, grade)
        {
            this._Param = param;
        }
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 向附带参数中追加成员
        /// </summary>
        /// <param name="args"></param>
        public void AppendArg ( Dictionary<string, string> args )
        {
            if ( this.Args == null )
            {
                this.Args = args;
                return;
            }
            foreach ( KeyValuePair<string, string> i in args )
            {
                if ( !this.Args.ContainsKey(i.Key) )
                {
                    this.Args.Add(i.Key, i.Value);
                }
            }
        }
        private void _InitArgs ( Dictionary<string, string> args )
        {
            if ( this.Args == null )
            {
                this.Args = args;
                return;
            }
            foreach ( KeyValuePair<string, string> i in args )
            {
                this.Args.Add(i.Key, i.Value);
            }

        }
        private Dictionary<string, string> _Args = null;
        /// <summary>
        /// 错误关联参数
        /// </summary>
        public Dictionary<string, string> Args
        {
            get
            {
                this._Args ??= [];
                return this._Args;
            }
            set
            {
                if ( this._Args == null )
                {
                    this._Args = value;
                }
                else
                {
                    this.AppendArg(value);
                }
            }
        }
        private readonly string _Param = null;

        private string _Msg = null;
        /// <summary>
        /// 是否是系统错误(ErrorException 以外的异常称为系统错误)
        /// </summary>
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
                this._Msg ??= LocalErrorManage.GetText(this.ErrorCode, this.ErrorCode);
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
        public LogGrade LogGrade { get; } = LogGrade.ERROR;
        /// <summary>
        /// 参数值
        /// </summary>
        public string Param => this._Param;
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public ErrorMsg GetError ()
        {
            ErrorMsg msg = LocalErrorManage.Get(this.ErrorCode);
            if ( msg == null )
            {
                return new ErrorMsg
                {
                    ErrorId = -1,
                    ErrorCode = this.ErrorCode,
                    Text = this.ErrorCode,
                    Lang = LocalErrorManage.DefLang
                };
            }
            return msg;
        }

        public override string ToString ()
        {
            if ( this._Param == null )
            {
                return this.ErrorCode;
            }
            return string.Format("{0}[{1}]", this.ErrorCode, this._Param);
        }
        /// <summary>
        /// 将异常信息封装
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ErrorException FormatError ( Exception e )
        {
            if ( e.InnerException != null )
            {
                e = e.InnerException;
            }
            if ( e is ErrorException k )
            {
                return k;
            }
            return new ErrorException(e);
        }
        /// <summary>
        /// 将异常信息封装
        /// </summary>
        /// <param name="e"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static ErrorException FormatError ( Exception e, LogGrade grade )
        {
            if ( e.InnerException != null )
            {
                e = e.InnerException;
            }
            if ( e is ErrorException k )
            {
                return k;
            }
            return new ErrorException(e, grade);
        }
        /// <summary>
        /// 将当前错误保存成日志
        /// </summary>
        /// <param name="group"></param>
        public void SaveLog ( string group = "def" )
        {
            if ( LogSystem.CheckIsRecord(this.LogGrade) )
            {
                new ErrorLog(this, group).Save();
            }
        }
        /// <summary>
        /// 将当前错误保存成日志
        /// </summary>
        /// <param name="group"></param>
        public void Save ( string group = "def" )
        {
            new ErrorLog(this, group).Save();
        }
        /// <summary>
        /// 将当前错误保存成日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="group"></param>
        public void SaveLog ( string title, string group = "def" )
        {
            if ( LogSystem.CheckIsRecord(this.LogGrade) )
            {
                return;
            }
            new ErrorLog(this, title, group).Save();
        }
    }
}
