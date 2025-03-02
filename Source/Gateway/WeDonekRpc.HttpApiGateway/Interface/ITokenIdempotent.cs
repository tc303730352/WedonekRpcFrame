namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface ITokenIdempotent : IIdempotent
    {
        string ApplyToken();

    }
}