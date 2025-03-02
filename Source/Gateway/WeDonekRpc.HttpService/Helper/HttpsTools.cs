using System;
using System.Collections.Generic;

using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService.Helper
{
    internal class HttpsTools
    {
        public string HashVal { get; }

        public HttpsTools (string hash)
        {
            this.HashVal = hash;
        }
        public bool BindUri (Uri uri)
        {
            Guid guid = Tools.GetProcedureGuid();
            if (guid == Guid.Empty || string.IsNullOrEmpty(this.HashVal))
            {
                return false;
            }
            string ports = string.Concat(":", uri.Port.ToString());
            List<_netsh_sslcert> certs = this._GetNetshSslcert();
            return certs.FindIndex(a => a.ip.EndsWith(ports) && a.certhash == this.HashVal && a.appid == guid) == -1
                    ? this._SetNetshSslCert(guid, this.HashVal, uri)
                    : this._CheckIsBindUri(uri) || this._AddUrlBind(uri);
        }
        private bool _AddUrlBind (Uri uri)
        {
            string cmd = uri.Port == 443
                    ? string.Format("netsh http add urlacl url=https://{0}:443/ user=system", uri.Authority)
                    : string.Format("netsh http add urlacl url=https://{0}/ user=system", uri.Authority);
            return Tools.RunCmd(new string[] { cmd }, out string res) && !res.Contains("错误", StringComparison.CurrentCulture) && !res.Contains("失败", StringComparison.CurrentCulture);
        }
        private bool _CheckIsBindUri (Uri uri)
        {
            string cmd = uri.Port == 443
                    ? string.Format("netsh http show urlacl url=https://{0}:443/", uri.Authority)
                    : string.Format("netsh http show urlacl url=https://{0}/", uri.Authority);
            return Tools.RunCmd(cmd, out _) && cmd.IndexOf(uri.Authority) != -1;
        }
        private bool _SetNetshSslCert (Guid guid, string certhash, Uri uri)
        {
            string host = uri.Authority;
            if (!host.Contains(':'))
            {
                host = string.Concat(uri.Authority, ":443");
            }
            string cmd = string.Format("netsh http add sslcert hostnameport={2} appid={0} certhash={1} clientcertnegotiation=enable certstorename=my", guid.ToString("B"), certhash, host);
            return Tools.RunCmd(cmd, out string res)
            && !res.Contains("错误", StringComparison.CurrentCulture)
            && !res.Contains("失败", StringComparison.CurrentCulture);
        }
        private List<_netsh_sslcert> _GetNetshSslcert ()
        {
            if (Tools.RunCmd("netsh http show sslcert", out string res))
            {
                string[] t = res.Replace("\r\n", "\n").Replace("\t", "").Replace(" ", "").Split('\n');
                int index = Array.FindIndex(t, a => a == "-------------------------");
                _netsh_sslcert temp = null;
                string str = null;
                int i = index + 2;
                List<_netsh_sslcert> list = [];
                do
                {
                    str = t[i].Trim();
                    if (str.StartsWith("IP:端口:"))
                    {
                        temp = new _netsh_sslcert
                        {
                            ip = str.Remove(0, 6),
                            certhash = t[++i].Remove(0, 5),
                            appid = new Guid(t[++i].Remove(0, 8).Replace("}", ""))
                        };
                        list.Add(temp);
                        i += 15;
                    }
                    else
                    {
                        i++;
                    }
                } while (i < t.Length);
                return list;
            }
            return null;
        }

        #region 私有类
        public class _netsh_sslcert
        {
            public string ip
            {
                get;
                set;
            }
            public string certhash
            {
                get;
                set;
            }
            public Guid appid
            {
                get;
                set;
            }
        }
        #endregion
    }
}
