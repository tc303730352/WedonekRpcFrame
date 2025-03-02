using System;
using System.Diagnostics;
using System.Threading;

namespace WeDonekRpc.Client.Helper
{
    public class CurProcessHelper
    {
        private static readonly Process _Process = null;
        private static double _LastCpuTime = 0;
        private static readonly int _cpuBase = 10000;
        private static readonly Timer _RefreshCpuRate;
        private static readonly int _Interval = 1000;
        static CurProcessHelper ()
        {
            _Process = Process.GetCurrentProcess();
            ProcessorCount = Environment.ProcessorCount;
            _cpuBase = _Interval * Environment.ProcessorCount;
            if (_Process != null)
            {
                _RefreshCpuRate = new Timer(new TimerCallback(_Refresh), null, _Interval, _Interval);
                _Refresh(null);
            }
        }
        public static ProcessModuleCollection Modules
        {
            get => _Process.Modules;
        }
        /// <summary>
        /// 主模块
        /// </summary>
        public static ProcessModule MainModule
        {
            get => _Process.MainModule;
        }
        /// <summary>
        /// 进程ID
        /// </summary>
        public static int ProcessId => _Process.Id;
        /// <summary>
        /// CPU使用率
        /// </summary>
        public static short CpuRate
        {
            get;
            private set;
        }
        /// <summary>
        /// 逻辑处理器数
        /// </summary>
        public static int ProcessorCount
        {
            get;
        }
        /// <summary>
        /// 工作内存
        /// </summary>
        public static long WorkingSet64
        {
            get;
            private set;
        }
        /// <summary>
        /// 线程数
        /// </summary>
        public static int ThreadNum
        {
            get;
            private set;
        }
        public static DateTime StartTime => _Process.StartTime;
        public static string ProcessName => _Process.ProcessName;
        public static string MachineName => _Process.MachineName;

        public static long CpuTime => (long)_Process.TotalProcessorTime.TotalMilliseconds;

        internal static void Dispose ()
        {
            _Process.Dispose();
        }

        private static void _Refresh (object state)
        {
            _Process.Refresh();
            double ms = _Process.TotalProcessorTime.TotalMilliseconds;
            CpuRate = (short)( Math.Round(( ms - _LastCpuTime ) / _cpuBase * 100, 2) * 100 );
            _LastCpuTime = ms;
            WorkingSet64 = _Process.WorkingSet64;
            ThreadNum = _Process.Threads.Count;
        }
    }

}
