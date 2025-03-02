namespace WeDonekRpc.Helper.Error
{
    public interface IErrorManage
    {
        string Lang { get; }

        bool TryGet ( string code, out ErrorMsg error );
    }
}