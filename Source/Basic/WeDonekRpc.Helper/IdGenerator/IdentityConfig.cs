using System;

namespace WeDonekRpc.Helper.IdGenerator
{
    public class IdentityConfig
    {
        /// <summary>
        /// 机器码位长，决定 WorkerId 的最大值，默认值6
        /// </summary>
        public byte WorkerIdBitLength { get; set; } = 8;
        /// <summary>
        /// 算法模式
        /// </summary>
        public short Method { get; set; } = 1;
        /// <summary>
        /// 机器码，最重要参数，无默认值，必须 全局唯一
        /// </summary>
        public ushort WorkId
        {
            get;
            set;
        }
        /// <summary>
        /// 序列数位长
        /// </summary>
        public byte SeqBitLength
        {
            get;
            set;
        } = 6;
        /// <summary>
        /// 最小序列数，默认值5
        /// </summary>
        public ushort MinSeqNumber
        {
            get;
            set;
        } = 5;
        /// <summary>
        /// 最大序列数，默认 0
        /// </summary>
        public int MaxSeqNumber
        {
            get;
            set;
        } = 0;
        /// <summary>
        /// 时间戳类型（0-毫秒，1-秒），默认0。
        /// </summary>
        public byte TimestampType
        {
            get;
            set;
        } = 0;
        /// <summary>
        /// 数据中心ID（机房ID，默认0），请确保全局唯一。
        /// </summary>
        public uint DataCenterId
        {
            get;
            set;
        } = 1;
        /// <summary>
        /// 数据中心ID长度（默认0）。
        /// </summary>
        public byte DataCenterIdBitLength
        {
            get;
            set;
        } = 2;
        /// <summary>
        /// 基础时间（也称：基点时间、原点时间、纪元时间），有默认值（2020年），是毫秒时间戳（是整数，.NET是DatetTime类型）
        /// </summary>
        public DateTime BaseTime { get; set; } = new DateTime(2024, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);

    }
}
