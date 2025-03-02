using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GatewayBuildCli.Model
{
    internal class XmlParamDic
    {
        private readonly Dictionary<string, string> _XmlParam = [];

        public void LoadXml (string path)
        {
            string xml = null;
            using (StreamReader read = new StreamReader(path, Encoding.UTF8))
            {
                xml = read.ReadToEnd();
            }
            XmlDataDocument doc = new XmlDataDocument();
            doc.LoadXml(xml);
            XmlNodeList list = doc.DocumentElement.GetElementsByTagName("member");
            if (list.Count == 0)
            {
                return;
            }
            foreach (XmlNode node in list)
            {
                XmlAttribute attr = node.Attributes["name"];
                if (attr != null && node.HasChildNodes)
                {
                    bool isFun = attr.InnerText.StartsWith("M:");
                    string name = attr.InnerText.Remove(0, 2).Replace("System.", string.Empty);
                    string val = node.ChildNodes[0].InnerText.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                    if (!string.IsNullOrEmpty(val))
                    {
                        this._XmlParam.Add(name, val);
                    }
                    if (isFun && node.ChildNodes.Count > 1)
                    {
                        foreach (XmlNode i in node.ChildNodes)
                        {
                            if (string.IsNullOrEmpty(i.InnerText))
                            {
                                continue;
                            }
                            else if (i.Name == "param")
                            {
                                val = i.InnerText;
                                string n = string.Concat(name, ".", i.Attributes["name"].InnerText.ToLower());
                                this._XmlParam.Add(n, val);
                            }
                            else if (i.Name == "returns")
                            {
                                val = i.InnerText;
                                string n = string.Concat(name, ".returns");
                                this._XmlParam.Add(n, val);
                            }
                        }
                    }
                }
            }
        }

        internal string FindParamShow (Type type)
        {
            string key = string.Concat(type.Namespace, ".", type.Name);
            return this._XmlParam.TryGetValue(key, out string val) ? val : null;
        }
        internal string FindParamShow (Type type, MethodInfo method)
        {
            string name = this._GetMethodStr(method);
            string key = string.Join(".", type.Namespace, type.Name, name);
            return this._XmlParam.TryGetValue(key, out string val) ? val : null;
        }
        private string _GetMethodStr (MethodInfo method)
        {
            if (method.GetParameters().Length == 0)
            {
                return method.Name;
            }
            string name = method.ToString().Replace("System.", string.Empty);
            StringBuilder str = new StringBuilder(name);
            _ = str.Remove(0, name.IndexOf(" ") + 1);
            _ = str.Replace(" ByRef", "@");
            _ = str.Replace(" ", string.Empty);
            name = str.ToString();
            int index = name.IndexOf("`");
            if (index != -1)
            {
                int end = name.IndexOf("[", index);
                if (end != -1)
                {
                    _ = str.Remove(index, end - index + 1);
                    _ = str.Insert(index, "{");
                    _ = str.Replace("]", "}");
                    name = str.ToString();
                }
            }
            return name;
        }
        internal string FindParamShow (Type type, MethodInfo method, string paramName)
        {
            string name = this._GetMethodStr(method);
            string key = string.Join(".", type.Namespace, type.Name, name, paramName);
            return this._XmlParam.TryGetValue(key, out string val) ? val : null;
        }

        internal string FindParamShow (Type type, string name)
        {
            string key = string.Join(".", type.Namespace, type.Name, name);
            return this._XmlParam.TryGetValue(key, out string val) ? val : null;
        }
    }
}
