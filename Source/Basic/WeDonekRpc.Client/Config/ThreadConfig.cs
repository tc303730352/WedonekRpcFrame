using System.Threading;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Config
{
    internal class ThreadConfig
    {
        public ThreadConfig ()
        {
            RpcClient.Config.GetSection("rpc:threadPool").AddRefreshEvent(this._InitConfig);
        }

        public int MinWorker { get; private set; }

        public int MinCompletionPort { get; private set; }

        public int MaxWorker { get; private set; }

        public int MaxCompletionPort { get; private set; }

        private void _InitConfig (IConfigSection section, string name)
        {
            if (section.GetValue<bool>("IsEnable", true))
            {
                this.MinWorker = section.GetValue("MinWorker", 15);
                this.MinCompletionPort = section.GetValue("MinCompletionPort", 15);
                this.MaxWorker = section.GetValue("MaxWorker", 300);
                this.MaxCompletionPort = section.GetValue("MaxCompletionPort", 300);
                _ = ThreadPool.SetMinThreads(this.MinWorker, this.MinCompletionPort);
                _ = ThreadPool.SetMaxThreads(this.MaxWorker, this.MaxCompletionPort);
            }
        }
    }
}
