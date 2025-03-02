namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// ORM 转换方案
    /// </summary>
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    public interface ISchemeMapper
    {
        void InitConfig (IMapperConfig config);
    }
}
