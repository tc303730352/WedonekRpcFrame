using Yitter.IdGenerator;

namespace WeDonekRpc.Helper.IdGenerator
{
    public class IdentityHelper
    {
        private static bool _IsInit = false;
        private static long _tempId = 1;

        public static bool IsInit { get => _IsInit; }

        public static void Init (IdentityConfig config)
        {
            IdGeneratorOptions options = new IdGeneratorOptions(config.WorkId)
            {
                WorkerIdBitLength = config.WorkerIdBitLength,
                SeqBitLength = config.SeqBitLength,
                BaseTime = config.BaseTime,
                Method = config.Method,
                MaxSeqNumber = config.MaxSeqNumber,
                MinSeqNumber = config.MinSeqNumber,
                DataCenterId = config.DataCenterId,
                TimestampType = config.TimestampType,
                DataCenterIdBitLength = config.DataCenterIdBitLength
            };
            YitIdHelper.SetIdGenerator(options);
            _IsInit = true;
        }
        public static long CreateIdOrTempId ()
        {
            if (!_IsInit)
            {
                return System.Threading.Interlocked.Increment(ref _tempId);
            }
            return YitIdHelper.NextId();
        }
        public static long CreateId ()
        {
            return YitIdHelper.NextId();
        }
    }
}
