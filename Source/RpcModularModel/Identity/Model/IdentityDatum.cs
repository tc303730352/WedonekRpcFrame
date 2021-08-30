namespace RpcModularModel.Identity.Model
{
        public class IdentityDatum
        {
                /// <summary>
                /// 应用名称
                /// </summary>
                public string AppName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否有效
                /// </summary>
                public bool IsValid
                {
                        get;
                        set;
                }
                /// <summary>
                /// 资源路径
                /// </summary>
                public string[] Path
                {
                        get;
                        set;
                }
                /// <summary>
                /// 路由列表
                /// </summary>
                public string[] Routes
                {
                        get;
                        set;
                }
        }
}
