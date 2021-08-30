namespace ApiGateway.Model
{
        public class ApiUpSet
        {
                /// <summary>
                /// 限定一次请求上传文件数量（0 无限制）
                /// </summary>
                public int LimitFileNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 文件扩展
                /// </summary>
                public string[] Extension
                {
                        get;
                        set;
                }

                /// <summary>
                /// 最大文件大小
                /// </summary>
                public int MaxSize
                {
                        get;
                        set;
                }
        }
}
