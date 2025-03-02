namespace RpcCentral.Service
{
    internal class Helper
    {
        public static long FormatApiVer (string ver)
        {
            string[] str = ver.Split('.');
            return ( long.Parse(str[0]) * 10000 ) + ( int.Parse(str[1]) * 100 ) + int.Parse(str[2]);
        }
    }
}
