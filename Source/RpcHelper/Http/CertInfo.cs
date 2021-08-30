namespace RpcHelper
{
        public class CertInfo
        {
                public CertInfo()
                {

                }
                public CertInfo(string path, string pwd)
                {
                        this.CertPath = path;
                        this.CertPwd = pwd;
                }
                public string CertPath
                {
                        get;
                        set;
                }
                public string CertPwd
                {
                        get;
                        set;
                }
        }
}
