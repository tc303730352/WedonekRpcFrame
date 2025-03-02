namespace WeDonekRpc.Model
{
    public static class LinqHelper
    {
        public static RemoteSet ToRemoteSet (this IRemoteConfig config)
        {
            return new RemoteSet
            {
                IsEnableLock = config.IsEnableLock,
                AppIdentity = config.AppIdentity,
                IdentityColumn = config.IdentityColumn,
                IsAllowRetry = config.IsAllowRetry,
                LockColumn = config.LockColumn,
                LockType = config.LockType,
                RetryNum = config.RetryNum,
                TimeOut = config.TimeOut,
                Transmit = config.Transmit,
                TransmitType = config.TransmitType,
                ZIndexBit = config.ZIndexBit
            };
        }
    }
}
