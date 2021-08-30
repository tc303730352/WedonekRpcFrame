using System.Collections.Generic;
using System.Text;

using HttpService.Interface;

namespace HttpService
{
        internal class UpFileHelper
        {
                private readonly IBasicHandler _Handler = null;
                private readonly IUpFileRequest _Request = null;
                public UpFileHelper(IBasicHandler handler)
                {
                        this._Request = (IUpFileRequest)handler.Request;
                        this._Handler = handler;
                        if (this._Handler.Request.HttpMethod == "OPTIONS")
                        {
                                return;
                        }
                        this._InitFileList();
                }

                private bool _IsPostFile = false;

                public bool IsPostFile => this._IsPostFile;

                private HttpPostFileHelper _FileHelper = null;

                private string _PostStr = null;

                public string PostString => this._PostStr;

                private void _InitFileList()
                {
                        this._FileHelper = new HttpPostFileHelper(this._Handler);
                        if (this._FileHelper.IsPostFile)
                        {
                                this._IsPostFile = true;
                                List<MultiPartInfo> files = this._FileHelper.GetPostFileList();
                                if (files.Count != 0)
                                {
                                        files.ForEach(a =>
                                        {
                                                if (a.DataLen != 0)
                                                {
                                                        if (a.Name == "description")
                                                        {
                                                                this._PostStr = Encoding.UTF8.GetString(this._Handler.Request.Stream, a.BeginIndex, a.DataLen);
                                                                this._Request.SetForm(this._PostStr);
                                                        }
                                                        else if (a.Name == "files[]" || a.Name == "file" || a.Name == "media" || !string.IsNullOrEmpty(a.FileName))
                                                        {
                                                                this._Files.Add(new PostFile(this._Handler.Request.Stream, a));
                                                        }
                                                }
                                        });
                                }
                        }
                }
                private readonly List<PostFile> _Files = new List<PostFile>();
                public List<PostFile> Files => this._Files;
        }
}
