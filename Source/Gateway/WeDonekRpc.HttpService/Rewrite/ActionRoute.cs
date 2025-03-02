using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Rewrite
{
    internal class ActionRoute : IRewriteRoute
    {
        private readonly int[] _PathIndex;
        private readonly string[] _Names;
        private readonly Func<Uri, string> _EndPoint;
        private readonly bool _IsRegex = false;
        private readonly Regex _Regex;
        private readonly string _Path;

        public string Path => this._Path;

        public bool IsFullPath { get; }

        public string EndPath => "Func";

        public bool IsRegex => this._IsRegex;

        public ActionRoute (Regex regex, Func<Uri, string> endPoint)
        {
            this._Path = regex.ToString();
            this.IsFullPath = false;
            this._IsRegex = true;
            this._Regex = regex;
            this._EndPoint = endPoint;
        }
        public ActionRoute (string path, Func<Uri, string> endPoint)
        {
            this._Path = path;
            this._EndPoint = endPoint;
            string[] paths = path.Split('/');
            PathParam[] list = paths.ConvertAll((a, i) => new PathParam(a, i));
            if (list.IsExists(c => !c.IsPath))
            {
                this.IsFullPath = false;
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
                this.IsFullPath = path.EndsWith("/");
                this._Path = path;
            }
        }



        public RouteParam GetRouteParam (Uri uri)
        {
            string path = this._EndPoint(uri);
            if (path == null)
            {
                return null;
            }
            Dictionary<string, string> args = this._GetRouteArgs(uri);
            return new RouteParam(args, path);
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
