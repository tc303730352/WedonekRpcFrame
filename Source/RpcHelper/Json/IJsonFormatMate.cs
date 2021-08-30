namespace RpcHelper.Json
{
        public interface IJsonFormatMate
        {
                /// <summary>
                /// 匹配格式
                /// </summary>
                /// <param name="data"></param>
                /// <returns></returns>
                bool FormatMate(dynamic data);
        }
}
