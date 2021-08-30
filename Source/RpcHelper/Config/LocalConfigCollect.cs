using System;
using System.Text;

using RpcHelper.TaskTools;
namespace RpcHelper.Config
{
        internal class LocalConfigCollect : ConfigCollect
        {
                private readonly string _Path = null;

                public LocalConfigCollect(string path)
                {
                        this._Path = path;
                        string json = Tools.ReadText(path, Encoding.UTF8, true);
                        if (!string.IsNullOrEmpty(json))
                        {
                                this._Init(json, 10);
                        }
                        bool isRefresh = this.GetValue<bool>("IsRefresh", false);
                        if (isRefresh)
                        {
                                TaskManage.AddTask(new TaskHelper("刷新配置", new TimeSpan(0, 1, 0), this._Refresh));
                        }
                }



                private void _Refresh()
                {
                        string json = Tools.ReadText(this._Path, Encoding.UTF8, true);
                        if (string.IsNullOrEmpty(json))
                        {
                                return;
                        }
                        this._Init(json, 1);
                }
        }
}
