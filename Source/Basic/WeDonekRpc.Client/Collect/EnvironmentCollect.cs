using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Collect
{
    internal class EnvironmentCollect : IDisposable
    {
        private static EnvironmentCollect _Cur;
        static EnvironmentCollect ()
        {
            _Cur = new EnvironmentCollect();
        }
        public EnvironmentCollect ()
        {
            this._Config = new EnvironmentUploadConfig();
        }
        private Timer _CheckTimer;
        private int _ModuleNum;
        private readonly EnvironmentUploadConfig _Config;
        private int _sendNum = 0;
        public static void Init ()
        {
            _Cur._SendEnvironment();
        }

        private void _Check (object state)
        {
            ProcessModuleCollection list = CurProcessHelper.Modules;
            if (list.Count != this._ModuleNum)
            {
                UploadLoadModule send = new UploadLoadModule
                {
                    Modules = new ProcModule[list.Count]
                };
                int index = 0;
                foreach (ProcessModule i in list)
                {
                    using (i)
                    {
                        ProcModule md = new ProcModule
                        {
                            FileName = i.FileName,
                            ModuleName = i.ModuleName
                        };
                        string path = md.FileName;
                        if (!Path.IsPathFullyQualified(path))
                        {
                            path = Path.GetFullPath(path);
                        }
                        if (File.Exists(path))
                        {
                            md.FileVer = i.FileVersionInfo.ToString();
                        }
                        send.Modules[index++] = md;
                    }
                }
                if (!RemoteCollect.Send(send, out string error))
                {
                    RpcLogSystem.AddFatalError("服务节点模块信息发送失败!", string.Empty, error);
                    return;
                }
            }
            int num = Interlocked.Increment(ref this._sendNum);
            if (num > this._Config.CheckTime)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 发送环境变量
        /// </summary>
        private void _SendEnvironment ()
        {
            EnvironmentConfig env = new EnvironmentConfig();
            env.Init();
            ProcessModule main = CurProcessHelper.MainModule;
            if (main != null)
            {
                using (main)
                {
                    env.MainModule = new ProcModule
                    {
                        FileName = main.FileName,
                        FileVer = main.FileVersionInfo.ToString(),
                        ModuleName = main.ModuleName,
                    };
                }
            }
            ProcessModuleCollection list = CurProcessHelper.Modules;
            env.Modules = new ProcModule[list.Count];
            this._ModuleNum = list.Count;
            int index = 0;
            foreach (ProcessModule i in list)
            {
                using (i)
                {
                    ProcModule md = new ProcModule
                    {
                        FileName = i.FileName,
                        ModuleName = i.ModuleName
                    };
                    string path = md.FileName;
                    if (!Path.IsPathFullyQualified(path))
                    {
                        path = Path.GetFullPath(path);
                    }
                    if (File.Exists(path))
                    {
                        md.FileVer = i.FileVersionInfo.ToString();
                    }
                    env.Modules[index++] = md;
                }
            }
            UploadEnvironment send = new UploadEnvironment
            {
                Environment = env
            };
            if (!RemoteCollect.Send(send, out string error))
            {
                RpcLogSystem.AddFatalError("服务节点环境变量发送失败!", string.Empty, error);
            }
            if (this._Config.CheckTime > 0)
            {
                this._CheckTimer = new Timer(this._Check, null, this._Config.DelayInterval, this._Config.DelayInterval);
            }
            else
            {
                this.Dispose();
            }
        }

        public void Dispose ()
        {
            this._CheckTimer?.Dispose();
            _Cur = null;
        }
    }
}
