using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Rewrite
{

    internal class RewriteRoute : IRewriteRoute
    {
        private readonly int[] _PathIndex;
        private readonly string[] _Names;
        private readonly string _EndPath;
        private readonly bool _IsRegex = false;
        private readonly Regex _Regex;
        private readonly string _Path;
        public RewriteRoute (Regex regex, string endPath)
        {
            this._Path = regex.ToString();
            this._IsRegex = true;
            this._EndPath = endPath;
            this._Regex = regex;
        }
        public RewriteRoute (string path, string endPath)
        {
            this._Path = path;
            this._EndPath = endPath;
            string[] paths = path.Split('/');
            PathParam[] list = paths.ConvertAll((a, i) => new PathParam(a, i));
            if (list.IsExists(c => !c.IsPath))
            {
                this._IsRegex = true;
                string regex = string.Concat("^", list.Join("/", a => a.ToString()), "$");
                this._Regex = new Regex(regex, RegexOptions.IgnoreCase);
                list = list.FindAll(c => !c.IsPath);
                this._PathIndex = new int[list.Length];
                this._Names = new string[list.Length];
                list.ForEach((c, i) =>
                {
                    this._PathIndex[i] = c.Index;
                    this._Names[i] = c.Name;
                });
            }
            else
            {
                this.IsFullPath = !path.EndsWith("/");
            }
        }
        public bool IsRegex => this._IsRegex;

        public string Path => this._Path;

        public bool IsFullPath
        {
            get;
        } = false;

        public string EndPath => this._EndPath;

        public RouteParam GetRouteParam (Uri uri)
        {
            Dictionary<string, string> args = this._GetRouteArgs(uri);
            return new RouteParam(args, this._EndPath);
        }
        private Dictionary<string, string> _GetRouteArgs (Uri uri)
        {
            if (this._PathIndex != null)
            {
                string[] t = uri.AbsolutePath.Split('/');
                Dictionary<string, string> dic = [];
                foreach (int i in this._PathIndex)
                {
                    dic.Add(this._Names[i], t[i]);
                }
                return dic;
            }
            return null;
        }
        /// <summary>
        /// 是否符合
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsUsable (string path)
        {
            if (this._IsRegex)
            {
                return this._Regex.IsMatch(path);
            }
            return path.StartsWith(this._Path);
        }


    }
}
