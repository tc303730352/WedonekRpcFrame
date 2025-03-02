using System;

namespace WeDonekRpc.Client.Interface
{
        internal interface IRemoteVerGroup:IRemoteGroup,IDisposable
        {
                int VerNum { get; }
        }
}
