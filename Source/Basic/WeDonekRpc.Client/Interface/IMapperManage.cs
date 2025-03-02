namespace WeDonekRpc.Client.Interface
{
    internal interface IMapperManage
    {
        void SetMapper(IMapperHandler mapper);

        void Reset();
    }
}
