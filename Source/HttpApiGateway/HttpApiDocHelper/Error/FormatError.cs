using RpcHelper;
using RpcHelper.Validate;
namespace HttpApiDocHelper.Error
{
        internal class FormatError : ProErrorMsg
        {
                public FormatError(RegexValidate regex) : base(regex.ErrorMsg)
                {
                        this.FormatTitle = "正则表达式验证";
                        this.FormatRegex = regex.Regex.ToString();
                }
                public FormatError(TimeValidate time) : base(time.ErrorMsg)
                {
                        this.FormatTitle = "时间格式验证";
                        this.FormatShow = time.Show;
                }
                public FormatError(YearValidate year) : base(year.ErrorMsg)
                {
                        this.FormatTitle = "年份格式验证";
                        this.FormatShow = year.MaxYear == 0
                                ? string.Format("年份应大于{0}年前", year.MinYear == 0 ? "今" : year.MinYear.ToString())
                                : string.Format("年份应大于{0}年前 且 小于{1}年前", year.MinYear == 0 ? "今" : year.MinYear.ToString(), year.MaxYear);
                }
                public FormatError(AreaValidate area) : base(area.ErrorMsg)
                {
                        this.FormatTitle = "区域数据验证";
                        this.FormatShow = string.Format("验证是否为：{0} 数据ID", area.AreaType.ToString());
                }
                public FormatError(FormatValidate regex) : base(regex.ErrorMsg)
                {
                        this.FormatTitle = regex.FormatType.ToString();
                        if (regex.FormatType == ValidateFormat.包含特定字符)
                        {
                                this.FormatShow = string.Format("需包含”{0}“字符", regex.ContainStr);
                        }
                        else
                        {
                                this.FormatRegex = string.Concat(ValidateHelper.GetFormatRegexStr(regex.FormatType, regex.ContainStr, out string show), "/g");
                                this.FormatShow = show;
                        }
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
