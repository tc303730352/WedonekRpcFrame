using System;
using System.Text;
using System.Xml;

namespace WeDonekRpc.Helper
{
    public class XmlToJsonHelper
    {
        public static string XmlToJson(string xml, bool showNodeName = false, params string[] rColName)
        {
            xml = xml.Replace(" ", string.Empty).Trim();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return XmlToJson(doc.FirstChild, showNodeName, rColName);
        }
        private static void _ToXmlNodeVal(string name, string txt, StringBuilder json, string[] rColName)
        {
            if (rColName != null && Array.FindIndex(rColName, a => a == name) != -1)
            {
                return;
            }
            if (ValidateHelper.CheckIsNumber(txt))
            {
                json.AppendFormat("\"{0}\":{1},", name, txt);
            }
            else
            {
                json.AppendFormat("\"{0}\":\"{1}\",", name, txt);
            }
        }
        private static string _ToXmlNode(XmlNode node, params string[] rColName)
        {
            if (node == null)
            {
                return string.Empty;
            }
            StringBuilder json = new StringBuilder("{");
            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (rColName != null && rColName.FindIndex(a => a == attr.Name) == -1)
                    {
                        json.AppendFormat("\"{0}\":\"{1}\",", attr.Name, attr.Value);
                    }
                }
            }
            if (node.HasChildNodes)
            {
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    if (cnode.NodeType == XmlNodeType.Element)
                    {
                        if (rColName != null && rColName.FindIndex(a => a == cnode.Name) == -1)
                        {
                            if (cnode.FirstChild == null)
                            {
                                json.AppendFormat("\"{0}\":[", cnode.Name);
                                if (cnode.ChildNodes.Count > 0)
                                {
                                    foreach (XmlNode i in cnode.ChildNodes)
                                    {
                                        json.Append(_ToXmlNode(i, rColName));
                                        json.Append(",");
                                    }
                                    json.Remove(json.Length - 1, 1);
                                }
                                json.Append("]");
                            }
                            else if (cnode.FirstChild.NodeType != XmlNodeType.Element)
                            {
                                _ToXmlNodeVal(cnode.Name, cnode.FirstChild.InnerText, json, rColName);
                            }
                            else
                            {
                                json.AppendFormat("\"{0}\":{1},", cnode.Name, _ToXmlNode(cnode, rColName));
                            }
                        }
                    }
                    else
                    {
                        _ToXmlNodeVal(cnode.Name, cnode.InnerText, json, rColName);
                    }
                }
            }
            if (json.Length > 1)
            {
                json.Remove(json.Length - 1, 1);
            }
            json.Append("}");
            return json.ToString();
        }
        public static string XmlToJson(XmlNode node, bool showNodeName = false, params string[] rColName)
        {
            if (showNodeName)
            {
                StringBuilder json = new StringBuilder("{");
                if (rColName != null && rColName.FindIndex(a => a == node.Name) == -1)
                {
                    json.AppendFormat("\"{0}\":{1}", node.Name, _ToXmlNode(node, rColName));
                }
                json.Append("}");
                return json.ToString();
            }
            return _ToXmlNode(node, rColName);
        }

    }
}
