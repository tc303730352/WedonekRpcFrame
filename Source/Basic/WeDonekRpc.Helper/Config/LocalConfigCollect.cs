using System;
using System.IO;
using System.Text;
using System.Threading;
namespace WeDonekRpc.Helper.Config
{
    /// <summary>
    /// 本地配置项
    /// </summary>
    internal class LocalConfigCollect : ConfigCollect
    {
        private readonly FileInfo _File = null;
        private Timer _SaveTimer;
        private Timer _RefreshTimer;
        private DateTime _LastWriteTime;
        private ConfigSet _Config;
        private int _curState = 0;
        public LocalConfigCollect (string path) : base()
        {
            this.RefreshEvent += this._RefreshEvent;
            this._File = new FileInfo(path);
            this._Load();
        }

        public LocalConfigCollect () : base()
        {
            this.RefreshEvent += this._RefreshEvent;
        }
        private void _RefreshEvent (IConfigCollect collect, string name)
        {
            if (name.IsNotNull())
            {
                _ = Interlocked.CompareExchange(ref this._curState, 2, 0);
            }
        }

        private void _Load ()
        {
            if (this._File.Exists)
            {
                this._LastWriteTime = this._File.LastWriteTime;
                string json = Tools.ReadText(this._File, Encoding.UTF8, false);
                if (!string.IsNullOrEmpty(json))
                {
                    this._Init(Tools.CompressJson(json), 10);
                }
            }
            IConfigSection section = this.GetSection("local:config");
            this._Config = new ConfigSet(section, this._RefreshConfig);
        }
        private void _RefreshConfig (ConfigSet config, string name)
        {
            if (name == string.Empty || name == "AutoLoad" || name == "CheckTime")
            {
                this._RefreshTimer?.Dispose();
                if (config.AutoLoad)
                {
                    int time = config.CheckTime * 1000;
                    this._RefreshTimer = new Timer(this._Refresh, null, time, time);
                }
            }
            if (name == string.Empty || name == "AutoSave" || name == "SaveTime")
            {
                this._SaveTimer?.Dispose();
                if (config.AutoSave)
                {
                    int time = config.SaveTime * 1000;
                    this._SaveTimer = new Timer(this._SaveFile, null, time, time);
                }
            }
        }
        private void _SaveFile (object _)
        {
            if (Interlocked.CompareExchange(ref this._curState, 1, 2) == 2)
            {
                base.SaveFile(this._File);
                _ = Interlocked.Exchange(ref this._curState, 0);
            }
        }
        public void SaveFile ()
        {
            base.SaveFile(this._File);
        }
        public override string ToString ()
        {
            return base.ToString();
        }

        private void _Refresh (object _)
        {
            this._File.Refresh();
            if (!this._File.Exists || this._File.LastWriteTime <= this._LastWriteTime)
            {
                return;
            }
            this._LastWriteTime = this._File.LastWriteTime;
            string json = Tools.ReadText(this._File, Encoding.UTF8, true);
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            json = Tools.CompressJson(json);
            this._Set(json, this._Config.Prower);
        }
    }
}
