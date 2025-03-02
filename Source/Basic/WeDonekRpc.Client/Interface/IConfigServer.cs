using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Client.Interface
{
    /// <summary>
    /// 配置系统数据来源服务
    /// </summary>
    [Attr.IgnoreIoc]
    public interface IConfigServer
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        void LoadConfig();
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name">配置路径</param>
        /// <returns></returns>
        string GetConfigVal(string name);
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="name">配置路径</param>
        /// <returns>配置值</returns>
        T GetConfigVal<T>(string name);
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="name">配置路径</param>
        /// <param name="defValue">无配置项时返回的默认值</param>
        /// <returns>配置值</returns>
        T GetConfigVal<T>(string name, T defValue);
        /// <summary>
        /// 获取配置集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConfigSection GetSection(string name);
    }
}
