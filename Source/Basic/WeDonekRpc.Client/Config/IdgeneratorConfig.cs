using System;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Config
{
    /// <summary>
    /// 雪花ID生成配置
    /// </summary>
    internal class IdgeneratorConfig
    {
        private readonly Action<IdgeneratorConfig> _refresh;
        public IdgeneratorConfig (Action<IdgeneratorConfig> refresh)
        {
            this._refresh = refresh;
            LocalConfig.Local.GetSection("rpc:Idgenerator").AddRefreshEvent(this._Refresh);
        }
        private void _Refresh (IConfigSection section, string name)
        {
            this.WorkerIdBitLength = section.GetValue<byte>("WorkerIdBitLength", 8);
            this.Method = section.GetValue<short>("Method", 1);
            this.SeqBitLength = section.GetValue<byte>("SeqBitLength", 6);
            this.MinSeqNumber = section.GetValue<ushort>("MinSeqNumber", 5);
            this.MaxSeqNumber = section.GetValue<ushort>("MaxSeqNumber", 0);
            this.TimestampType = section.GetValue<byte>("TimestampType", 0);
            this.EnableDataCenter = section.GetValue<bool>("EnableDataCenter", true);
            this.DataCenterIdBitLength = section.GetValue<byte>("DataCenterIdBitLength", 4);
            this.BaseTime = section.GetValue<DateTime>("BaseTime", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            this._refresh(this);
        }
        /// <summary>
        /// 机器码位长，决定 WorkerId 的最大值，默认值6
        /// </summary>
        public byte WorkerIdBitLength { get; private set; }

        /// <summary>
        /// 算法模式
        /// </summary>
        public short Method { get; private set; }
        /// <summary>
        /// 序列数位长
        /// </summary>
        public byte SeqBitLength
        {
            get;
            private set;
        }
        /// <summary>
        /// 最小序列数，默认值5
        /// </summary>
        public ushort MinSeqNumber
        {
            get;
            private set;
        }
        /// <summary>
        /// 最大序列数，默认 0
        /// </summary>
        public int MaxSeqNumber
        {
            get;
            private set;
        }
        /// <summary>
        /// 时间戳类型（0-毫秒，1-秒），默认0。
        /// </summary>
        public byte TimestampType
        {
            get;
            private set;
        }
        /// <summary>
        /// 启用数据中心
        /// </summary>
        public bool EnableDataCenter
        {
            get;
            private set;
        }
        /// <summary>
        /// 数据中心的ID最大位数
        /// </summary>
        public byte DataCenterIdBitLength
        {
            get;
            private set;
        }
        /// <summary>
        /// 基础时间（也称：基点时间、原点时间、纪元时间），有默认值（2020年），是毫秒时间戳（是整数，.NET是DatetTime类型）
        /// </summary>
        public DateTime BaseTime { get; private set; }
    }
}
