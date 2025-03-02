using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Helper.Error
{
    public class LocalErrorManage : ILocalErrorManage
    {
        private static readonly ILocalErrorManage _Manage = new LocalErrorManage();
        private static ErrorConfig _Config;
        private static Timer _AutoSave;
        private static readonly ConcurrentDictionary<string, ILocalError> _ErrorList = new ConcurrentDictionary<string, ILocalError>();

        private static readonly ConcurrentDictionary<long, string> _ErrorCodeDic = new ConcurrentDictionary<long, string>();
        private static readonly ConcurrentDictionary<string, long> _ErrorIdDic = new ConcurrentDictionary<string, long>();

        private static Func<IErrorEvent> _ErrorEvent = null;

        public static string DefLang { get; private set; } = "zh";

        private static ILocalError _DefError = null;
        static LocalErrorManage ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("local");
            section.AddRefreshEvent(_Local_RefreshEvent);
            _Config = section.GetValue<ErrorConfig>("error", new ErrorConfig());
            _Load();
        }

        private static void _Local_RefreshEvent (IConfigSection config, string name)
        {
            if (name == "error" || name.StartsWith("error:"))
            {
                _Config = config.GetValue<ErrorConfig>("error", new ErrorConfig());
                _Load();
            }
        }

        #region 对外的方法
        public static void Drop (long errorId)
        {
            if (_ErrorCodeDic.TryRemove(errorId, out string code))
            {
                _ = _ErrorIdDic.TryRemove(code, out errorId);
                ILocalError[] errors = _ErrorList.Values.ToArray();
                errors.ForEach(a =>
                {
                    a.Drop(code);
                });
                if (_ErrorEvent != null)
                {
                    _ErrorEvent().DropError(errorId, code);
                }
            }
        }

        private static void _Load ()
        {
            DefLang = _Config.Lang[0];
            _Config.Lang.ForEach(c =>
            {
                _Load(c);
            });
            if (!_Config.IsAutoSave && _AutoSave != null)
            {
                _AutoSave.Dispose();
                _AutoSave = null;
            }
            else if (_Config.IsAutoSave && _AutoSave == null)
            {
                _AutoSave = new Timer((a) => SaveFile(), null, _Config.IntervalTime * 1000, _Config.IntervalTime * 1000);
            }
        }
        private static void _Load (string lang)
        {
            if (_ErrorList.ContainsKey(lang))
            {
                return;
            }
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Concat("ErrorMsg_", lang, ".xml"));
            ILocalError error = new ErrorManage(path, _Manage, _Reg);
            if (LocalErrorManage._ErrorList.TryAdd(lang, error) && lang == DefLang)
            {
                _DefError = error;
            }

        }
        internal static bool GetError (string lang, out ILocalError error)
        {
            if (LocalErrorManage._DefError != null && LocalErrorManage._DefError.Lang == lang)
            {
                error = LocalErrorManage._DefError;
                return true;
            }
            return _ErrorList.TryGetValue(lang, out error);
        }
        public static void SetAction (IErrorEvent error)
        {
            SetAction(() => error);
        }
        public static void SetAction (Func<IErrorEvent> error)
        {
            LocalErrorManage._ErrorEvent = error;
        }
        public static string Format (string error, out string param)
        {
            if (error == null)
            {
                param = null;
                return error;
            }
            return ErrorHelper.FormatError(error, out param);
        }
        public static ErrorMsg Get (string code)
        {
            if (LocalErrorManage._DefError.TryGet(code, out ErrorMsg error))
            {
                return error;
            }
            return null;
        }
        public static bool TryGet (string code, out ErrorMsg error, out string param)
        {
            if (code.IsNull() || !code.Contains('.'))
            {
                param = null;
                error = null;
                return false;
            }
            code = ErrorHelper.FormatError(code, out param);
            return LocalErrorManage._DefError.TryGet(code, out error);
        }
        public static bool GetErrorMsg (string code, out ErrorMsg msg)
        {
            if (code.IsNull() || !code.Contains('.'))
            {
                msg = null;
                return false;
            }
            return GetErrorMsg(code, DefLang, out msg);
        }
        public static bool GetErrorMsg (string code, string lang, out ErrorMsg msg)
        {
            if (GetError(lang, out ILocalError error))
            {
                return error.TryGet(code, out msg);
            }
            msg = null;
            return false;
        }
        public static bool GetErrorMsg (string code, out ErrorMsg msg, out string param)
        {
            if (code.IsNull() || !code.Contains('.'))
            {
                param = null;
                msg = null;
                return false;
            }
            code = Format(code, out param);
            return GetErrorMsg(code, DefLang, out msg);
        }
        public static bool TryGet (string code, string lang, out ErrorMsg error)
        {
            if (code.IsNull())
            {
                error = null;
                return false;
            }
            else if (string.IsNullOrEmpty(lang))
            {
                lang = DefLang;
            }
            if (GetError(lang, out ILocalError er))
            {
                return er.TryGet(code, out error);
            }
            error = null;
            return false;
        }
        public static string GetText (string code, string lang, string def)
        {
            return TryGet(code, lang, out ErrorMsg error) ? error.Text : def;
        }
        public static string GetText (string code, string def)
        {
            return TryGet(code, DefLang, out ErrorMsg error) ? error.Text : def;
        }
        public static string GetCode (long errorId)
        {
            if (errorId == 0)
            {
                return null;
            }
            else if (_ErrorCodeDic.TryGetValue(errorId, out string code))
            {
                return code;
            }
            else
            {
                code = _ErrorEvent.TryGetCode(errorId);
                if (!code.IsNull())
                {
                    _Reg(errorId, code);
                }
                return code;
            }
        }
        private static void _Reg (long errorId, string code)
        {
            if (!_ErrorCodeDic.ContainsKey(errorId))
            {
                _ = _ErrorCodeDic.TryAdd(errorId, code);
            }
            if (!_ErrorIdDic.ContainsKey(code))
            {
                _ = _ErrorIdDic.TryAdd(code, errorId);
            }
        }
        public static long GetErrorId (string code)
        {
            code = ErrorHelper.FormatError(code);
            if (_ErrorIdDic.TryGetValue(code, out long errorId))
            {
                return errorId;
            }
            long id = _ErrorEvent.TryGetId(code);
            if (id > 0)
            {
                _Reg(id, code);
                return id;
            }
            return -1;
        }

        public static void AddError (ErrorMsg msg)
        {
            if (!GetError(msg.Lang, out ILocalError error) || !error.Add(msg))
            {
                return;
            }
            _Reg(msg.ErrorId, msg.ErrorCode);
        }

        public static ErrorMsg[] GetAllError (string lang)
        {
            if (GetError(lang, out ILocalError error))
            {
                return error.Errors;
            }
            return null;
        }
        public static void SaveFile ()
        {
            string[] keys = _ErrorList.Keys.ToArray();
            keys.ForEach(c =>
            {
                SaveFile(c);
            });
        }
        public static void SaveFile (string lang)
        {
            if (GetError(lang, out ILocalError error) && error.IsChange)
            {
                error.Save(System.AppDomain.CurrentDomain.BaseDirectory);
            }
        }
        #endregion

        public ErrorConfig Config => _Config;
        public void TriggerDrop (IErrorManage error, ErrorMsg msg)
        {
            _ErrorEvent.TriggerDrop(msg);
        }

        public bool RemoteGet (IErrorManage error, string code, out ErrorMsg msg)
        {
            msg = _ErrorEvent.GetError(code, error.Lang);
            if (msg == null)
            {
                return false;
            }
            else if (msg.Text.IsNull())
            {
                msg.Text = _Config.DefErrorText.GetValueOrDefault(error.Lang, code);
            }
            _Reg(msg.ErrorId, msg.ErrorCode);
            return true;
        }
    }
}
