using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Helper.Error
{
    public class ErrorManage : ILocalError
    {
        private readonly RegErrorCode _RegCode;
        private readonly ILocalErrorManage _ErrorManage;
        private bool _IsChange = false;
        private string _Lang = "zh";
        public string Lang
        {
            get => this._Lang;
        }
        public bool IsChange { get => this._IsChange; }

        private readonly ConcurrentDictionary<string, ErrorMsg> _ErrorMsg = new ConcurrentDictionary<string, ErrorMsg>();
        public ErrorMsg[] Errors { get => this._ErrorMsg.Values.ToArray(); }
        internal ErrorManage ( string path, ILocalErrorManage localError, RegErrorCode reg )
        {
            this._RegCode = reg;
            this._ErrorManage = localError;
            this._LoadErrorXml(path);
        }

        private void _LoadErrorXml ( string path )
        {
            FileInfo file = new FileInfo(path);
            if ( file.Exists )
            {
                string xml = null;
                using ( StreamReader ready = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Delete), Encoding.UTF8) )
                {
                    xml = ready.ReadToEnd();
                    ready.Close();
                }
                if ( string.IsNullOrEmpty(xml) )
                {
                    return;
                }
                this._LoadXml(xml);
                this._IsChange = false;
            }
        }
        public void Save ( string dir )
        {
            this._IsChange = false;
            ErrorMsg[] msgs = this._ErrorMsg.Values.ToArray();
            string[] groups = msgs.Select(a => a.ErrorCode.Substring(0, a.ErrorCode.IndexOf('.'))).Distinct().ToArray();
            StringBuilder xml = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            _ = xml.AppendLine("<errorlist lang=\"" + this.Lang + "\">");
            groups.ForEach(a =>
            {
                _ = xml.AppendFormat("<{0}>\r\n", a);
                string str = string.Format("{0}.", a);
                ErrorMsg[] t = msgs.FindAll(b => b.ErrorCode.StartsWith(str));
                t.ForEach(b =>
                {
                    _ = xml.AppendFormat("<error><id>{0}</id><code>{1}</code><msg>{2}</msg></error>\r\n", b.ErrorId, b.ErrorCode.Remove(0, str.Length), b.Text);
                });
                _ = xml.AppendFormat("</{0}>\r\n", a);
            });
            _ = xml.AppendLine("</errorlist>");
            string path = Path.Combine(dir, string.Format("ErrorMsg_{0}.xml", this.Lang));
            if ( File.Exists(path) )
            {
                File.Delete(path);
            }
            using ( StreamWriter writer = new StreamWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)) )
            {
                writer.Write(xml);
                writer.Flush();
                writer.Close();
            }
        }
        private void _LoadXml ( string xml )
        {
            XmlDataDocument doc = new XmlDataDocument();
            doc.LoadXml(xml);
            this._Lang = doc.DocumentElement.GetAttribute("lang");
            if ( string.IsNullOrEmpty(this._Lang) )
            {
                return;
            }
            foreach ( XmlNode node in doc.DocumentElement.ChildNodes )
            {
                if ( node.HasChildNodes )
                {
                    string name = node.Name;
                    foreach ( XmlNode i in node.ChildNodes )
                    {
                        ErrorMsg msg = new ErrorMsg()
                        {
                            Lang = this._Lang
                        };
                        msg.InitErrorMsg(name, i);
                        if ( !this._ErrorMsg.ContainsKey(msg.ErrorCode) )
                        {
                            if ( this._ErrorMsg.TryAdd(msg.ErrorCode, msg) )
                            {
                                this._RegCode(msg.ErrorId, msg.ErrorCode);
                            }
                        }
                    }
                }
            }
        }

        public bool TryGet ( string code, out ErrorMsg error )
        {
            if ( this._ErrorMsg.TryGetValue(code, out error) )
            {
                return true;
            }
            else if ( this._ErrorManage.RemoteGet(this, code, out error) )
            {
                _ = this.Add(error);
                return true;
            }
            return false;
        }
        public void Drop ( string code )
        {
            if ( this._ErrorMsg.TryRemove(code, out ErrorMsg msg) )
            {
                this._IsChange = true;
                this._ErrorManage.TriggerDrop(this, msg);
            }
        }
        public bool Add ( ErrorMsg msg )
        {
            if ( !this._ErrorMsg.ContainsKey(msg.ErrorCode) )
            {
                if ( this._ErrorMsg.TryAdd(msg.ErrorCode, msg) )
                {
                    this._IsChange = true;
                    return true;
                }
            }
            return false;
        }
    }
}
