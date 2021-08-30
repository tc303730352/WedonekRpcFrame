using System;
using System.Collections.Generic;
using System.Text;

using HttpService.Interface;

namespace HttpService
{
        /// <summary>
        /// 解析RFC1867协议
        /// </summary>
        internal class HttpPostFileHelper
        {
                public HttpPostFileHelper(IBasicHandler obj)
                {
                        this._Encoding = Encoding.UTF8;
                        this._StreamLen = obj.Request.ContentLength;
                        if (this._StreamLen == 0)
                        {
                                return;
                        }
                        string contentType = obj.Request.ContentType;
                        if (string.IsNullOrEmpty(contentType))
                        {
                                return;
                        }
                        this._IsPostFile = contentType.StartsWith("multipart/form-data;");
                        if (this._IsPostFile)
                        {
                                this._PostStream = obj.Request.Stream;
                                string str = contentType.Substring(30).Trim();
                                this._boundary = this._Encoding.GetBytes("--" + str);
                                this._boundaryLen = this._boundary.Length + 2;
                                this._SboundaryLen = this._boundary.Length;
                                this._boundaryEndLen = this._boundaryLen + 2;
                        }
                }
                private readonly Encoding _Encoding;

                private readonly byte[] _boundary = null;

                private readonly byte[] _PostStream = null;

                private readonly int _boundaryLen = 0;
                private readonly int _SboundaryLen = 0;
                private readonly int _boundaryEndLen = 0;

                private readonly bool _IsPostFile = false;

                public bool IsPostFile => this._IsPostFile;

                private readonly long _StreamLen = 0;

                private int _BeginIndex = 0;

                private int _EndIndex = 0;

                private int _DataLen = 0;

                public List<MultiPartInfo> GetPostFileList()
                {
                        if (this._PostStream == null)
                        {
                                return new List<MultiPartInfo>();
                        }
                        List<MultiPartInfo> list = new List<MultiPartInfo>();
                        int type = 0;
                        while (this._GetLine())
                        {
                                type = this._GetBoundaryType();
                                if (type == 1)
                                {
                                        this._GetMultiPart(ref type, ref list);
                                        if (type == 2)
                                        {
                                                break;
                                        }
                                }
                                else
                                {
                                        break;
                                }
                        }
                        return list;
                }
                private void _GetMultiPart(ref int lastType, ref List<MultiPartInfo> list)
                {
                        MultiPartInfo obj = null;
                        while (this._GetLine())
                        {
                                if (obj != null)
                                {
                                        lastType = this._GetBoundaryType();
                                        if (lastType != 0)
                                        {
                                                obj.DataLen -= 2;
                                                list.Add(obj);
                                                if (lastType == 1)
                                                {
                                                        this._GetMultiPart(ref lastType, ref list);
                                                }
                                                break;
                                        }
                                        else
                                        {
                                                if (obj.Name != "description" && obj.ContentType == null)
                                                {
                                                        string res = this._Encoding.GetString(this._PostStream, this._BeginIndex, this._DataLen).Trim();
                                                        if (res.StartsWith("Content-Type"))
                                                        {
                                                                obj.ContentType = res.Remove(0, 13).Replace("\"", "");
                                                                continue;
                                                        }
                                                }
                                                if (obj.BeginIndex == 0)
                                                {
                                                        obj.BeginIndex = this._BeginIndex;
                                                }
                                                int sLen = obj.DataLen;
                                                obj.DataLen += this._DataLen;
                                        }
                                }
                                else
                                {
                                        string res = this._Encoding.GetString(this._PostStream, this._BeginIndex, this._DataLen);
                                        if (!res.StartsWith("Content-Disposition"))
                                        {
                                                continue;
                                        }
                                        obj = new MultiPartInfo();
                                        string[] t = res.Split(';');
                                        int i = 0;
                                        Array.ForEach(t, a =>
                                        {
                                                a = a.Trim();
                                                i = a.IndexOf('=');
                                                if (a.StartsWith("Content-Disposition"))
                                                {
                                                        obj.Disposition = a.Remove(0, i + 1);
                                                }
                                                else if (a.StartsWith("name"))
                                                {
                                                        obj.Name = a.Remove(0, i + 1).Replace("\"", "");
                                                }
                                                else if (a.StartsWith("filename"))
                                                {
                                                        obj.FileName = a.Remove(0, i + 1).Replace("\"", "");
                                                }
                                                else if (a.StartsWith("Content-Type"))
                                                {
                                                        obj.ContentType = a.Remove(0, i + 1).Replace("\"", "");
                                                }
                                        });
                                }
                        }
                }
                private short _GetBoundaryType()
                {
                        if (this._DataLen != this._boundaryLen && this._DataLen != this._boundaryEndLen)
                        {
                                return 0;
                        }
                        for (int i = 0; i < this._SboundaryLen; i++)
                        {
                                if (this._boundary[i] != this._PostStream[i + this._BeginIndex])
                                {
                                        return 0;
                                }
                        }
                        if (this._DataLen == this._boundaryEndLen)
                        {
                                int index = this._BeginIndex + 22;
                                if (index >= this._StreamLen)
                                {
                                        index = this._PostStream.Length - 1;
                                }
                                if (this._PostStream[index - 1] == 45 && this._PostStream[index] == 45)
                                {
                                        return 2;
                                }
                        }
                        return 1;
                }
                private bool _GetLine()
                {
                        if (this._EndIndex >= this._StreamLen)
                        {
                                return false;
                        }
                        this._BeginIndex = this._EndIndex;
                        while (this._EndIndex < this._StreamLen)
                        {
                                if (this._PostStream[this._EndIndex++] == 10 && this._PostStream[this._EndIndex - 2] == 13)
                                {
                                        this._DataLen = this._EndIndex - this._BeginIndex;
                                        if (this._DataLen == 2)
                                        {
                                                this._BeginIndex = this._EndIndex;
                                                continue;
                                        }
                                        else if (this._DataLen == 0)
                                        {
                                                continue;
                                        }
                                        return true;
                                }
                        }
                        return false;
                }
        }
}
