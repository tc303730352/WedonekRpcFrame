using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
namespace RpcHelper
{
        /// <summary>
        /// 日志信息
        /// </summary>
        public class LogInfo : Dictionary<string, dynamic>
        {
                public LogInfo()
                {

                }
                public LogInfo(string title, string group, LogGrade grade) : this(group, grade)
                {
                        this.LogTitle = title;
                        this.LogType = LogType.信息日志;
                }
                public LogInfo(string title, string content, string group,LogGrade grade) : this(group, grade)
                {
                        this.LogTitle = title;
                        this.LogContent = content;
                        this.LogType = LogType.信息日志;
                }
                protected LogInfo(string group, LogGrade grade)
                {
                        this.LogGrade = grade;
                        this.Type = group;
                        this.AddTime = DateTime.Now;
                }
                public LogInfo(string error, LogGrade grade, string group) : this(group, grade)
                {
                        this.ErrorCode = error;
                        this.LogType = LogType.错误日志;
                }
                public LogInfo(LogGrade grade, string group) : this(group, grade)
                {
                        this.LogType = LogType.信息日志;
                }
                public LogInfo(Exception e, string group, LogGrade grade) : this(group, grade)
                {
                        this.Exception = e.InnerException ?? e;
                        this.ErrorCode = "public.system.error";
                        this.LogType = LogType.错误日志;
                }
                public LogInfo(ErrorException e, string group) : this(group, e.LogGrade)
                {
                        this.Exception = e.InnerException;
                        this.ErrorCode = e.ToString();
                        this.LogType = LogType.错误日志;
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
                public bool IsLocal { get; private set; }


                public new dynamic this[string key]
                {
                        get
                        {
                                if (base.TryGetValue(key, out dynamic val))
                                {
                                        return val;
                                }
                                return null;
                        }
                        set
                        {
                                if (base.ContainsKey(key))
                                {
                                        base[key] = value;
                                        return;
                                }
                                this.Add(key, value);
                        }
                }

                public void Save()
                {
                        LogSystem.AddLog(this);
                }
                public void Save(bool isLocal)
                {
                        this.IsLocal = isLocal;
                        LogSystem.AddLog(this);
                }

                public override string ToString()
                {
                        StringBuilder txt = new StringBuilder();
                        txt.AppendLine("---------------------------------------------");
                        txt.Append("标题:");
                        txt.AppendLine(this.LogTitle);
                        if (this.LogContent != null)
                        {
                                txt.Append("描述:");
                                txt.AppendLine(this.LogContent);
                        }
                        txt.Append("日志等级:");
                        txt.AppendLine(this.LogGrade.ToString());
                        txt.Append("产生时间:");
                        txt.AppendLine(this.AddTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        if (this.LogType == LogType.错误日志)
                        {
                                txt.AppendLine("Error:");
                                txt.AppendLine(string.Concat("错误码:", this.ErrorCode));
                                if (this.Exception != null)
                                {
                                        txt.Append("错误描述:");
                                        txt.AppendLine(this.Exception.Message);
                                        txt.Append("错误源:");
                                        txt.AppendLine(this.Exception.Source);
                                        txt.Append("错误堆栈:");
                                        txt.AppendLine(this.Exception.StackTrace);
                                }
                        }
                        if (this.Count > 0)
                        {
                                txt.AppendLine("扩展:");
                                foreach (KeyValuePair<string, dynamic> i in this)
                                {
                                        if (i.Value != null)
                                        {
                                                txt.AppendLine(string.Concat(i.Key, ": ", _ToString(i.Value)));
                                        }
                                }
                        }
                        txt.AppendLine("-------------------------------------------------------------------");
                        return txt.ToString();
                }
                internal void Save(StreamWriter stream)
                {
                        stream.WriteLine(string.Empty);
                        stream.WriteLine("---------------------------------------------");
                        stream.Write("标题:");
                        stream.WriteLine(this.LogTitle);
                        if (this.LogContent != null)
                        {
                                stream.Write("描述:");
                                stream.WriteLine(this.LogContent);
                        }
                        stream.Write("日志等级:");
                        stream.WriteLine(this.LogGrade.ToString());
                        stream.Write("产生时间:");
                        stream.WriteLine(this.AddTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        if (this.LogType == LogType.错误日志)
                        {
                                stream.WriteLine("Error:");
                                stream.WriteLine(string.Concat("错误码:", this.ErrorCode));
                                if (this.Exception != null)
                                {
                                        stream.Write("错误描述:");
                                        stream.WriteLine(this.Exception.Message);
                                        stream.Write("错误源:");
                                        stream.WriteLine(this.Exception.Source);
                                        stream.Write("错误堆栈:");
                                        stream.WriteLine(this.Exception.StackTrace);
                                }
                        }
                        if (this.Count > 0)
                        {
                                stream.WriteLine("扩展:");
                                foreach (KeyValuePair<string, dynamic> i in this)
                                {
                                        if (i.Value != null)
                                        {
                                                stream.WriteLine(string.Concat(i.Key, ": ", _ToString(i.Value)));
                                        }
                                }
                        }
                        stream.WriteLine("-------------------------------------------------------------------");
                }
                private static string _ToString(dynamic val)
                {
                        Type type = val.GetType();
                        if (type == PublicDataDic.StrType)
                        {
                                return val;
                        }
                        else if (type.IsEnum)
                        {
                                return val.ToString();
                        }
                        else if (type == PublicDataDic.UriType || type == PublicDataDic.GuidType)
                        {
                                return val.ToString();
                        }
                        else if (type.IsGenericType || type.IsArray || type.IsClass)
                        {
                                return Tools.Json(val);
                        }
                        else if (type.IsPrimitive)
                        {
                                return val.ToString();
                        }
                        else
                        {
                                return Tools.Json(val);
                        }
                }

                public void AddOrSet(IDictionary<string, dynamic> dic)
                {
                        foreach (KeyValuePair<string, dynamic> i in dic)
                        {
                                if (this.ContainsKey(i.Key))
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
