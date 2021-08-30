using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

using RpcHelper.Config;

namespace RpcHelper
{

        public class LogSystem : IDisposable
        {
                private static readonly LogType[] _LogType = new LogType[]
                {
                        LogType.信息日志,
                        LogType.错误日志
                };

                private static event Action<LogInfo[]> _BindEvent = null;

                private static readonly string _LogPath = null;
                /// <summary>
                /// 日志级别
                /// </summary>
                private static LogGrade _LogGrade = LogGrade.DEBUG;

                private static readonly Thread _Thread = new Thread(new ThreadStart(_WriteLog));

                private static volatile bool _IsStop = false;
                public static void CloseLog()
                {
                        if (!_IsStop)
                        {
                                _IsStop = true;
                                _WriteLog();
                        }
                }
                /// <summary>
                /// 设置日志在控制台输出
                /// </summary>
                public static bool IsConsole
                {
                        get;
                        set;
                } = false;

                static LogSystem()
                {
                        _LogPath = Config.LocalConfig.Local.GetValue("Log:SaveDir", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
                        if (!Directory.Exists(_LogPath))
                        {
                                Directory.CreateDirectory(_LogPath);
                        }
                        AppDomain.CurrentDomain.UnhandledException += _CurrentDomain_UnhandledException;

                        if (_Thread.ThreadState == System.Threading.ThreadState.Unstarted)
                        {
                                _Thread.Start();
                                Process pro = Process.GetCurrentProcess();
                                pro.EnableRaisingEvents = true;
                                pro.Exited += new EventHandler(_Pro_Exited);
                        }
                        Config.LocalConfig.Local.RefreshEvent += Local_RefreshEvent;
                }

                private static void Local_RefreshEvent(IConfigCollect config, string name)
                {
                        if (name.StartsWith("log:") || name == string.Empty)
                        {
                                IsConsole = config.GetValue("Log:IsConsole", false);
                                _LogGrade = config.GetValue("Log:LogGrade", LogGrade.DEBUG);
                        }
                }

                public static bool CheckIsRecord(LogGrade grade)
                {
                        return LogSystem._LogGrade <= grade;
                }
                public static bool CheckIsRecord(ErrorException e)
                {
                        return LogSystem._LogGrade <= e.LogGrade;
                }
                private static StreamWriter _GetFileStream(string type, LogType logType)
                {
                        FileInfo file = new FileInfo(string.Format(@"{0}\{1}\{2}_{3}_{4}.log", _LogPath, type, logType == LogType.信息日志 ? "Log" : "Error", HeartbeatTimeHelper.CurrentDate.ToString("yyyy-MM-dd"), HeartbeatTimeHelper.Hour));
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        return new StreamWriter(file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete | FileShare.Read), Encoding.UTF8, 4096);
                }

                private static void _CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
                {
                        Exception ex = (Exception)e.ExceptionObject;
                        if (e.IsTerminating)
                        {
                                new CriticalLog(ex, "发生严重错误!").Save();
                        }
                        else
                        {
                                new ErrorLog(ex, "发生未经处理的异常!").Save();
                        }
                        _WriteLog();
                }

                public static void AddErrorEvent(Action<LogInfo[]> errorEvent)
                {
                        if (_BindEvent == null)
                        {
                                _BindEvent += errorEvent;
                        }
                }
                private static void _Pro_Exited(object sender, EventArgs e)
                {
                        CloseLog();
                }

                private static readonly List<LogInfo> _LogList = new List<LogInfo>();

                private static readonly LockHelper _Lock = new LockHelper();
                /// <summary>
                /// 当前待写入日志队列总数
                /// </summary>
                public static long LogCount => _LogList.Count;

                internal static void AddLog(LogInfo log)
                {
                        if (_Lock.GetLock())
                        {
                                _LogList.Add(log);
                                _Lock.Exit();
                        }
                }
                public static LogInfo CreateErrorLog(Exception e, string title, string group)
                {
                        return new ErrorLog(e, title, group);
                }
                private static void _WriteConsole(LogInfo log)
                {
                        if (IsConsole)
                        {
                                Console.WriteLine(log.ToString());
                        }
                }
                private static void _WriteLog(string type, LogType logType, LogInfo[] logs)
                {
                        LogInfo[] list = logs.FindAll(a => a.LogType == logType && a.Type == type);
                        if (list.Length > 0)
                        {
                                using (StreamWriter stream = _GetFileStream(type, logType))
                                {
                                        stream.BaseStream.Position = stream.BaseStream.Length;
                                        list.ForEach(a =>
                                        {
                                                a.Save(stream);
                                                _WriteConsole(a);
                                        });
                                        stream.Flush();
                                }
                        }

                }

                private static void _WriteLog(LogInfo[] logs)
                {
                        string[] type = logs.Distinct(a => a.Type);
                        foreach (LogType i in _LogType)
                        {
                                foreach (string k in type)
                                {
                                        _WriteLog(k, i, logs);
                                }
                        }
                }
                private static void _WriteLog()
                {
                        if (!Directory.Exists(_LogPath))
                        {
                                Directory.CreateDirectory(_LogPath);
                        }
                        do
                        {
                                try
                                {
                                        LogInfo[] logs = null;
                                        if (_Lock.GetLock())
                                        {
                                                if (_LogList.Count > 0)
                                                {
                                                        logs = new LogInfo[_LogList.Count];
                                                        _LogList.CopyTo(logs);
                                                        _LogList.Clear();
                                                }
                                                _Lock.Exit();
                                        }
                                        if (logs != null && logs.Length > 0)
                                        {
                                                _WriteLog(logs);
                                                _BindEvent?.Invoke(logs);
                                        }
                                }
                                catch
                                {
                                }
                                if (_IsStop)
                                {
                                        if (_LogList.Count == 0)
                                        {
                                                break;
                                        }
                                }
                                else
                                {
                                        Thread.Sleep(5000);
                                }
                        } while (true);
                }

                public void Dispose()
                {
                        CloseLog();
                }


        }
}
