namespace WeDonekRpc.Client.FileUp
{
    public interface IUpFileInfo
    {
        string FileName { get; }
        long FileSize { get; }

        byte[] GetByte();
        T GetData<T>();
        string GetString();
        T GetValue<T>() where T : struct;
    }
}