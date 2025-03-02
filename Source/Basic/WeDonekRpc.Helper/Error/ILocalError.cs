namespace WeDonekRpc.Helper.Error
{
    internal delegate void RegErrorCode (long errorId, string code);
    internal interface ILocalError : IErrorManage
    {
        bool IsChange { get; }
        ErrorMsg[] Errors { get; }
        bool Add (ErrorMsg msg);
        void Drop (string code);
        void Save (string dir);
    }
}
