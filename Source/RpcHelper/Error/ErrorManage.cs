using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RpcHelper
{
        public class ErrorManage
        {
                #region 静态属性

                private static readonly Dictionary<string, ErrorManage> _ErrorList = new Dictionary<string, ErrorManage>();

                private static readonly Dictionary<long, string> _ErrorCodeDic = new Dictionary<long, string>();


                private static IErrorEvent _ErrorEvent = null;

                private static readonly string _defLang = "zh";

                public static string DefLang => _defLang;

                private static ErrorManage _DefError = null;


                public static bool DropError(long errorId)
                {
                        if (_ErrorCodeDic.TryGetValue(errorId, out string code))
                        {
                                ErrorManage[] errors = _ErrorList.Values.ToArray();
                                errors.ForEach(a =>
                                {
                                        a.DropError(code);
                                });
                                return true;
                        }
                        return _ErrorEvent.DropError(errorId);
                }
                public static long MaxErrorId
                {
                        get;
                        private set;
                }
                #endregion
                static ErrorManage()
                {
                        _LoadError();
                }
                #region 实例区域

                private volatile bool _IsLock = false;
                private readonly LockHelper _Lock = new LockHelper();

                public ErrorManage(string path)
                {
                        this._LoadErrorXml(path);
                }
                private void _LoadErrorXml(string path)
                {
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                                string xml = null;
                                using (StreamReader ready = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete), Encoding.UTF8))
                                {
                                        xml = ready.ReadToEnd();
                                        ready.Close();
                                }
                                if (string.IsNullOrEmpty(xml))
                                {
                                        return;
                                }
                                this._LoadXml(xml);
                        }
                        else
                        {
                                new InfoLog("加载本地错误信息文件,未找到!", path).Save();
                        }
                }
                private void _LoadXml(string xml)
                {
                        XmlDataDocument doc = new XmlDataDocument();
                        doc.LoadXml(xml);
                        this._Lang = doc.DocumentElement.GetAttribute("lang");
                        if (string.IsNullOrEmpty(this._Lang))
                        {
                                return;
                        }
                        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                        {
                                if (node.HasChildNodes)
                                {
                                        string name = node.Name;
                                        foreach (XmlNode i in node.ChildNodes)
                                        {
                                                ErrorMsg msg = new ErrorMsg()
                                                {
                                                        Lang = _Lang
                                                };
                                                msg.InitErrorMsg(name, i);
                                                if (!this._ErrorMsg.ContainsKey(msg.ErrorCode))
                                                {
                                                        this._ErrorMsg.Add(msg.ErrorCode, msg);
                                                }
                                                if (!_ErrorCodeDic.ContainsKey(msg.ErrorId))
                                                {
                                                        _ErrorCodeDic.Add(msg.ErrorId, msg.ErrorCode);
                                                }
                                        }
                                }
                        }
                }
                private string _Lang = "zh";

                public string Lang
                {
                        get;
                }
                private readonly Dictionary<string, ErrorMsg> _ErrorMsg = new Dictionary<string, ErrorMsg>();

                public bool GetError(string code, out ErrorMsg error)
                {
                        if (this._IsLock)
                        {
                                if (this._Lock.GetLock())
                                {
                                        if (this._ErrorMsg.TryGetValue(code, out error))
                                        {
                                                this._Lock.Exit();
                                                return true;
                                        }
                                        this._Lock.Exit();
                                }
                                return ErrorManage.FindRemoteError(code, this._Lang, out error);
                        }
                        return this._ErrorMsg.TryGetValue(code, out error) || ErrorManage.FindRemoteError(code, this._Lang, out error);
                }
                public void DropError(string code)
                {
                        this._IsLock = true;
                        if (this._Lock.GetLock())
                        {
                                this._ErrorMsg.Remove(code);
                                this._Lock.Exit();
                        }
                        this._IsLock = false;
                }
                #endregion
                #region 静态方法
                private static void _LoadError()
                {
                        string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ErrorMsg_zh.xml");
                        _DefError = new ErrorManage(path);
                        if (_ErrorList.ContainsKey(_defLang))
                        {
                                _ErrorList[_defLang] = _DefError;
                        }
                        else
                        {
                                _ErrorList.Add(_defLang, _DefError);
                        }
                        if (_ErrorCodeDic.Count > 0)
                        {
                                ErrorManage.MaxErrorId = _ErrorCodeDic.Keys.Max();
                        }
                }
                public static void SetAction(IErrorEvent error)
                {
                        ErrorManage._ErrorEvent = error;
                }
                public static string FormatError(string error, out string param)
                {
                        return _FormatError(error, out param);
                }
                private static string _FormatError(string error, out string param)
                {
                        int i = error.Length - 1;
                        if (error[i] != ']')
                        {
                                param = null;
                                return error;
                        }
                        int b = error.LastIndexOf('[') + 1;
                        param = error.Substring(b, i - b);
                        return error.Substring(0, b - 1);
                }
                public static ErrorMsg GetError(string code)
                {
                        if (_DefError.GetError(code, out ErrorMsg error))
                        {
                                return error;
                        }
                        return null;
                }
                public static bool GetErrorMsg(string code, out ErrorMsg error, out string param)
                {
                        if (string.IsNullOrEmpty(code) || code.IndexOf('.') == -1)
                        {
                                param = null;
                                error = null;
                                return false;
                        }
                        code = _FormatError(code, out param);
                        return _DefError.GetError(code, out error);
                }
                public static bool GetErrorMsg(string code, out ErrorMsg error)
                {
                        if (string.IsNullOrEmpty(code) || code.IndexOf('.') == -1)
                        {
                                error = null;
                                return false;
                        }
                        return _DefError.GetError(code, out error);
                }
                internal static string FindRemoteError(long errorId)
                {
                        return _ErrorEvent?.FindErrorCode(errorId);
                }
                internal static bool FindRemoteError(string code, string lang, out ErrorMsg msg)
                {
                        if (_ErrorEvent == null)
                        {
                                msg = null;
                                return false;
                        }
                        return _ErrorEvent.FindError(code, lang, out msg);
                }
                public static bool GetErrorMsg(string code, string lang, out ErrorMsg error)
                {
                        if (string.IsNullOrEmpty(code))
                        {
                                error = null;
                                return false;
                        }
                        else if (string.IsNullOrEmpty(lang))
                        {
                                lang = _defLang;
                        }
                        return _ErrorList.TryGetValue(lang, out ErrorManage obj) ? obj.GetError(code, out error) : FindRemoteError(code, lang, out error);
                }
                public static string GetErrorMsg(string code, string lang)
                {
                        return GetErrorMsg(code, lang, out ErrorMsg error) ? error.Msg : code;
                }
                public static long GetErrorCode(string code, string lang)
                {
                        return GetErrorMsg(code, lang, out ErrorMsg error) ? error.ErrorId : 0;
                }
                public static string QueryCode(long errorId)
                {
                        return errorId == 0 ? null : _ErrorCodeDic.TryGetValue(errorId, out string code) ? code : ErrorManage.FindRemoteError(errorId);
                }
                public static long GetErrorCode(string code)
                {
                        return GetErrorMsg(code, out ErrorMsg error) ? error.ErrorId : 0;
                }
                public static string GetErrorMsg(string code)
                {
                        return GetErrorMsg(code, out ErrorMsg error) ? error.Msg : code;
                }

                public static ErrorMsg[] GetAllErrorMsg(string lang)
                {
                        if (_defLang == lang)
                        {
                                return _DefError._ErrorMsg.Values.ToArray();
                        }
                        else if (_ErrorList.TryGetValue(lang, out ErrorManage error))
                        {
                                return error._ErrorMsg.Values.ToArray();
                        }
                        return null;
                }
                public static void SaveError()
                {
                        ErrorMsg[] errors = GetAllErrorMsg(_defLang);
                        if (_ErrorEvent != null)
                        {
                                errors = errors.Join(_ErrorEvent.LoadError(_defLang));
                        }
                        if (!errors.IsNull())
                        {
                                errors = errors.Distinct().ToArray();
                                _SaveFile(errors, _defLang);
                                _LoadError();
                        }
                }
                private static void _SaveFile(ErrorMsg[] msgs, string lang)
                {
                        string[] groups = msgs.ConvertAll<ErrorMsg, string>(a =>
                        {
                                return a.ErrorCode.Split('.')[0];
                        });
                        groups = groups.Distinct().ToArray();
                        StringBuilder xml = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                        xml.AppendLine("<errorlist lang=\"zh\">");
                        groups.ForEach(a =>
                        {
                                xml.AppendFormat("<{0}>\r\n", a);
                                string str = string.Format("{0}.", a);
                                ErrorMsg[] t = msgs.FindAll(b => b.ErrorCode.StartsWith(str));
                                t.ForEach(b =>
                                {
                                        xml.AppendFormat("<error><id>{0}</id><code>{1}</code><msg>{2}</msg></error>\r\n", b.ErrorId, b.ErrorCode.Remove(0, str.Length), b.Msg);
                                });
                                xml.AppendFormat("</{0}>\r\n", a);
                        });
                        xml.AppendLine("</errorlist>");
                        string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("ErrorMsg_{0}.xml", lang));
                        if (File.Exists(path))
                        {
                                File.Delete(path);
                        }
                        using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)))
                        {
                                writer.Write(xml);
                                writer.Flush();
                                writer.Close();
                        }

                }
                #endregion

        }
}
