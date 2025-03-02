namespace WeDonekRpc.HttpService.Rewrite
{
    internal class PathParam
    {
        private readonly string _RegexStr;
        public PathParam(string name,int i)
        {
            this.Index = i;
            if (name.StartsWith('{') && name.EndsWith('}'))
            {
                this.IsPath = false;
                int index = name.IndexOf(':');
                if (index == -1)
                {
                    this.Name = name.Substring(1, name.Length - 2).Trim();
                    this._RegexStr = @"\w+";
                }
                else
                {
                    this.Name = name.Substring(1, index - 1).Trim();
                    string type = name.Substring(index + 1, name.Length - index - 2).Trim();
                    if (type.StartsWith("regex("))
                    {
                        this._RegexStr = type.Substring(6, type.Length - 7);
                    }
                    else
                    {
                        this._RegexStr = Helper.Helper.GetRegexStr(type);
                    }
                }
            }
            else
            {
                this.IsPath = true;
                this.Name = name;
            }
        }
        public int Index
        {
            get;
        }
        public bool IsPath
        {
            get;
        }

        public string Name { get; }

        public override string ToString()
        {
            if (this.IsPath)
            {
                return this.Name;
            }
            return this._RegexStr;
        }
    }
}
