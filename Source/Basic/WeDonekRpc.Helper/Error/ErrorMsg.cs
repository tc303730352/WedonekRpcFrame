using System;
using System.Xml;

namespace WeDonekRpc.Helper.Error
{
    public class ErrorMsg : IEquatable<ErrorMsg>
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
        public string Text
        {
            get;
            set;
        }
        public string ErrorCode
        {
            get;
            set;
        }
        public override bool Equals ( object obj )
        {
            return obj is ErrorMsg i && i.ErrorId == this.ErrorId;
        }

        public bool Equals ( ErrorMsg other )
        {
            if ( other == null )
            {
                return false;
            }
            return other.ErrorId == this.ErrorId;
        }

        public override int GetHashCode ()
        {
            return this.ErrorId.GetHashCode();
        }
        internal void InitErrorMsg ( string group, XmlNode node )
        {
            this.ErrorId = long.Parse(node["id"].InnerText);
            this.ErrorCode = string.Format("{0}.{1}", group, node["code"].InnerText);
            this.Text = node["msg"].InnerText;
        }
    }
}
