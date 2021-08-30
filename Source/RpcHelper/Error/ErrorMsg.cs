using System.Xml;

namespace RpcHelper
{
        [System.Serializable]
        public class ErrorMsg
        {
                public long ErrorId
                {
                        get;
                        set;
                }
                public string Lang
                {
                        get;
                        set;
                }
                public string Msg
                {
                        get;
                        set;
                }
                public string ErrorCode
                {
                        get;
                        set;
                }
                public override bool Equals(object obj)
                {
                        return obj is ErrorMsg i && i.ErrorId == this.ErrorId;
                }
                public override int GetHashCode()
                {
                        return this.ErrorId.GetHashCode();
                }
                internal void InitErrorMsg(string group, XmlNode node)
                {
                        this.ErrorId = long.Parse(node["id"].InnerText);
                        this.ErrorCode = string.Format("{0}.{1}", group, node["code"].InnerText);
                        this.Msg = node["msg"].InnerText;
                }
        }
}
