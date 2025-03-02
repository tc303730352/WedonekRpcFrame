using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace Wedonek.Gateway.Modular.Oom
{
    /// <summary>
    /// 实体转换方案 继承：IMapper
    /// </summary>
    internal class OomScheme : ISchemeMapper
    {
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Scheme => "SchemeDemo";

        /// <summary>
        /// 初始化转换配置
        /// </summary>
        /// <param name="config"></param>
        public void InitConfig (IMapperConfig config)
        {
            _ = config.ConvertUsing<string, int>((a) =>
              {
                  if (a.IsNull())
                  {
                      return 0;
                  }
                  return int.Parse(a.Remove(a.Length - 1, 1));
              });
        }
    }
}
