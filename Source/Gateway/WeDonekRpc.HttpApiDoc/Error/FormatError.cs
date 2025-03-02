using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.HttpApiDoc.Error
{
    internal class FormatError : ProErrorMsg
    {
        public FormatError () { }
        public FormatError (RegexValidate regex) : base(regex.ErrorMsg)
        {
            this.FormatTitle = "正则表达式验证";
            this.FormatRegex = regex.Regex.ToString();
        }
        public FormatError (TimeValidate time) : base(time.ErrorMsg)
        {
            this.FormatTitle = "时间格式验证";
            this.FormatShow = time.Show;
        }
        public FormatError (YearValidate year) : base(year.ErrorMsg)
        {
            this.FormatTitle = "年份格式验证";
            this.FormatShow = year.MaxYear == 0
                    ? string.Format("年份应大于{0}年前", year.MinYear == 0 ? "今" : year.MinYear.ToString())
                    : string.Format("年份应大于{0}年前 且 小于{1}年前", year.MinYear == 0 ? "今" : year.MinYear.ToString(), year.MaxYear);
        }
        public FormatError (AreaValidate area) : base(area.ErrorMsg)
        {
            this.FormatTitle = "区域数据验证";
            this.FormatShow = string.Format("验证是否为：{0} 数据ID", area.AreaType.ToString());
        }
        public FormatError (FormatValidate regex) : base(regex.ErrorMsg)
        {
            StringBuilder showStr = new StringBuilder();
            StringBuilder titleStr = new StringBuilder();
            StringBuilder regexStr = new StringBuilder();
            regex.FormatType.ForEach(c =>
            {
                _ = titleStr.Append("," + c.ToString());
                if (c == ValidateFormat.包含特定字符)
                {
                    _ = showStr.AppendFormat(",需包含”{0}“字符", regex.ContainStr);
                }
                else
                {
                    _ = regexStr.Append("," + string.Concat(ValidateHelper.GetFormatRegexStr(c, regex.ContainStr, out string show), "/g"));
                    _ = showStr.Append("," + show);
                }
            });
            _ = titleStr.Remove(0, 1);
            _ = showStr.Remove(0, 1);
            if (regexStr.Length > 0)
            {
                _ = regexStr.Remove(0, 1);
                this.FormatRegex = regexStr.ToString();
            }
            this.FormatTitle = titleStr.ToString();
            this.FormatShow = showStr.ToString();
        }
        public string FormatTitle
        {
            get;
            set;
        }
        public string FormatShow
        {
            get;
        }
        public string FormatRegex
        {
            get;
        }
    }
}
