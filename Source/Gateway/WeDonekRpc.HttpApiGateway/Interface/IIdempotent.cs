using System;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Idempotent;
namespace WeDonekRpc.HttpApiGateway.Interface
{
    [IgnoreIoc]
    public interface IIdempotent : IDisposable
    {
        StatusSaveType SaveType { get; }

        bool SubmitToken (string tokenId);
    }
}