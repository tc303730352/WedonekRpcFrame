using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogInfo : Dictionary<string, dynamic>
    {
        public LogInfo ()
        {

        }
        public LogInfo ( string title, string group, LogGrade grade ) : this(group, grade)
        {
            this.LogTitle = title;
            this.LogType = LogType.信息日志;
        }
        public LogInfo ( string title, string content, string group, LogGrade grade ) : this(group, grade)
        {
            this.LogTitle = title;
            this.LogContent = content;
            this.LogType = LogType.信息日志;
        }
        protected LogInfo ( string group, LogGrade grade )
        {
            this.LogGrade = grade;
            this.Type = group;
            this.AddTime = DateTime.Now;
        }
        public LogInfo ( string error, LogGrade grade, string group ) : this(group, grade)
        {
            this.ErrorCode = error;
            this.LogType = LogType.错误日志;
        }
        public LogInfo ( LogGrade grade, string group ) : this(group, grade)
        {
            this.LogType = LogType.信息日志;
        }
        public LogInfo ( Exception e, string group, LogGrade grade ) : this(group, grade)
        {
            this.Exception = e;
            this.ErrorCode = "public.system.error";
            this.LogType = LogType.错误日志;
        }
        public LogInfo ( ErrorException e, string group ) : this(group, e.LogGrade)
        {
            this.LogTitle = e.Message;
            this.LogContent = e.Remark;
            this.Exception = e;
            this.ErrorCode = e.ToString();
            this.LogType = LogType.错误日志;
            if ( e.Args != null )
            {
                foreach ( KeyValuePair<string, string> i in e.Args )
                {
                    this.Add(i.Key, i.Value);
                }
            }
        }
        /// <summary>
        /// 日志标题
        /// </summary>
        [JsonIgnore]
        public string LogTitle;
        /// <summary>
        /// 日志内容
        /// </summary>
        [JsonIgnore]
        public string LogContent;
        /// <summary>
        /// 类型
        /// </summary>
        [JsonIgnore]
        public string Type;
        /// <summary>
        /// 日志类型
        /// </summary>
        [JsonIgnore]
        public LogType LogType { get; private set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonIgnore]
        public DateTime AddTime { get; private set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonIgnore]
        public Exception Exception { get; private set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        [JsonIgnore]
        public LogGrade LogGrade { get; protected set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonIgnore]
        public string ErrorCode { get; private set; }
        [JsonIgnore]
        public bool IsLocal { get; set; }


        public new dynamic this[string key]
        {
            get
            {
                if ( TryGetValue(key, out dynamic val) )
                {
                    return val;
                }
                return null;
            }
            set
            {
                if ( ContainsKey(key) )
                {
                    base[key] = value;
                    return;
                }
                this.Add(key, value);
            }
        }

        public void Save ()
        {
            LogSystem.AddLog(this);
        }
        public void Save ( bool isLocal )
        {
            this.IsLocal = isLocal;
            LogSystem.AddLog(this);
        }

        public override string ToString ()
        {
            StringBuilder txt = new StringBuilder();
            _ = txt.AppendLine("---------------------------------------------");
            _ = txt.Append("标题:");
            _ = txt.AppendLine(this.LogTitle);
            if ( this.LogContent != null )
            {
                _ = txt.Append("描述:");
                _ = txt.AppendLine(this.LogContent);
            }
            _ = txt.Append("日志等级:");
            _ = txt.AppendLine(this.LogGrade.ToString());
            _ = txt.Append("产生时间:");
            _ = txt.AppendLine(this.AddTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if ( this.LogType == LogType.错误日志 )
            {
                _ = txt.AppendLine("Error:");
                _ = txt.AppendLine(string.Concat("错误码:", this.ErrorCode));
                if ( this.Exception != null )
                {
                    _ = txt.Append("错误类型:");
                    _ = txt.AppendLine(this.Exception.GetType().Name);
                    _ = txt.Append("错误描述:");
                    _ = txt.AppendLine(this.Exception.Message);
                    _ = txt.Append("错误源:");
                    _ = txt.AppendLine(this.Exception.Source);
                    _ = txt.Append("错误堆栈:");
                    _ = txt.AppendLine(this.Exception.StackTrace);
                    if ( this.Exception.InnerException != null )
                    {
                        Exception e = this.Exception.InnerException;
                        do
                        {
                            _ = txt.AppendLine("InnerException:");
                            _ = txt.Append("错误类型:");
                            _ = txt.AppendLine(e.GetType().Name);
                            _ = txt.Append("错误描述:");
                            _ = txt.AppendLine(e.Message);
                            _ = txt.Append("错误源:");
                            _ = txt.AppendLine(e.Source);
                            _ = txt.Append("错误堆栈:");
                            _ = txt.AppendLine(e.StackTrace);
                            e = e.InnerException;
                        } while ( e != null );
                    }
                }
            }
            if ( this.Count > 0 )
            {
                _ = txt.AppendLine("扩展:");
                foreach ( KeyValuePair<string, dynamic> i in this )
                {
                    if ( i.Value != null )
                    {
                        txt.AppendLine(string.Concat(i.Key, ": ", _ToString(i.Value)));
                    }
                }
            }
            _ = txt.AppendLine("-------------------------------------------------------------------");
            return txt.ToString();
        }
        internal void Save ( StreamWriter stream )
        {
            stream.WriteLine(string.Empty);
            stream.WriteLine("---------------------------------------------");
            stream.Write("标题:");
            stream.WriteLine(this.LogTitle);
            if ( this.LogContent != null )
            {
                stream.Write("描述:");
                stream.WriteLine(this.LogContent);
            }
            stream.Write("日志等级:");
            stream.WriteLine(this.LogGrade.ToString());
            stream.Write("产生时间:");
            stream.WriteLine(this.AddTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if ( this.LogType == LogType.错误日志 )
            {
                stream.WriteLine("Error:");
                stream.WriteLine(string.Concat("错误码:", this.ErrorCode));
                if ( this.Exception != null )
                {
                    stream.Write("错误类型:");
                    stream.WriteLine(this.Exception.GetType().Name);
                    stream.Write("错误描述:");
                    stream.WriteLine(this.Exception.Message);
                    stream.Write("错误源:");
                    stream.WriteLine(this.Exception.Source);
                    stream.Write("错误堆栈:");
                    stream.WriteLine(this.Exception.StackTrace);
                    if ( this.Exception.InnerException != null )
                    {
                        Exception e = this.Exception.InnerException;
                        do
                        {
                            stream.Write("Inner错误类型:");
                            stream.WriteLine(e.GetType().Name);
                            stream.Write("错误描述:");
                            stream.WriteLine(e.Message);
                            stream.Write("错误源:");
                            stream.WriteLine(e.Source);
                            stream.Write("错误堆栈:");
                            stream.WriteLine(e.StackTrace);
                            e = e.InnerException;
                        } while ( e != null );
                    }
                    stream.WriteLine(string.Empty);
                }
            }
            if ( this.Count > 0 )
            {
                stream.WriteLine("扩展:");
                foreach ( KeyValuePair<string, dynamic> i in this )
                {
                    if ( i.Value != null )
                    {
                        stream.WriteLine(string.Concat(i.Key, ": ", _ToString(i.Value)));
                    }
                }
            }
            stream.WriteLine("-------------------------------------------------------------------");
        }
        private static string _ToString ( dynamic val )
        {
            Type type = val.GetType();
            if ( type == PublicDataDic.StrType )
            {
                return val;
            }
            else if ( type.IsEnum )
            {
                return val.ToString();
            }
            //else if (type == PublicDataDic.IPAddressType || type == PublicDataDic.IPEndPoint)
            //{
            //        return val.ToString ();
            //}
            else if ( type == PublicDataDic.UriType || type == PublicDataDic.GuidType )
            {
                return val.ToString();
            }
            else if ( type.IsGenericType || type.IsArray || type.IsClass )
            {
                return JsonTools.Json(val);
            }
            else if ( type.IsPrimitive )
            {
                return val.ToString();
            }
            else
            {
                return JsonTools.Json(val);
            }
        }

        public void AddOrSet ( IDictionary<string, dynamic> dic )
        {
            foreach ( KeyValuePair<string, dynamic> i in dic )
            {
                if ( this.ContainsKey(i.Key) )
                {
                    this[i.Key] = i.Value;
                }
                else
                {
                    this.Add(i.Key, i.Value);
                }
            }
        }

    }
}
