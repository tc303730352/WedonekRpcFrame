using System;
using System.Text.RegularExpressions;

using WeDonekRpc.Helper.Validate;

namespace WeDonekRpc.Helper
{
    public class ValidateHelper
    {
        private static readonly Regex _CheckIsHtmlColor = new Regex(@"^[#]\w{6}$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckIsName = new Regex(@"^([\u4E00-\u9FBF]|[a-z]|[ \\])+$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckIsLetterNumPoint = new Regex(@"^[0-9a-z\.]+$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckIsLetterPoint = new Regex(@"^[a-z\.]+$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckIcon = new Regex(@"^[0-9a-z -_]+$", RegexOptions.IgnoreCase);
        private static readonly Regex _NumLetterRegex = new Regex(@"^[0-9a-z]+$", RegexOptions.IgnoreCase);
        private static readonly Regex _ContainNumRegex = new Regex(@"\d+");

        private static readonly Regex _ContainLetterRegex = new Regex("[a-zA-Z]+");

        private static readonly Regex _MacRegex = new Regex(@"^(((\w{2}[:-]){5}\w{2})|((\w{4}[-]){2}\w{4})|(\w{12})){1}$", RegexOptions.IgnoreCase);


        private static readonly Regex _NumberRegex = new Regex(@"^[-]{0,1}\d+([.]\d+){0,1}$");
        private static readonly Regex _NumRegex = new Regex(@"^\d+$");
        private static readonly Regex _LetterRegex = new Regex(@"^[a-z]+$", RegexOptions.IgnoreCase);

        private static readonly Regex _Ip6Regex = new Regex(@"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$", RegexOptions.IgnoreCase);

        private static readonly Regex _IpRegex = new Regex(@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$", RegexOptions.IgnoreCase);

        private static readonly Regex _IpPortRegex = new Regex(@"^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}([:]\d{1,5}){0,1}$", RegexOptions.IgnoreCase);

        private static readonly Regex _UriRegex = new Regex(@"^(http:|https:){1}\/\/(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?.*$");

        private static readonly Regex _UriHostRegex = new Regex(@"^(\w+[.]){0,}\w+([:]\d+){0,1}$");

        private static readonly Regex _RelativePathRegex = new Regex(@"^[.]{0,2}[/]{0,1}\w+([/]\w+)*[/]{0,1}$", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static readonly Regex _ImgUriRegex = new Regex(@"^(http:|https:){1}\/\/(\w+(-\w+)*)(\.(\w+(-\w+)*))*.*[.](jpg|png|bmp|jpeg|gif)(\?\S*)?.*$", RegexOptions.IgnoreCase);

        private static readonly Regex _PhoneRegex = new Regex(@"^(0\d{2,4}[-]{0,1}){0,1}(([1]\d{10})|([9]\d{4,5})|([1-9]\d{5,7})|((\d+[-]){2}\d+)){1}$");

        private static readonly Regex _CardIdRegex = new Regex(@"^((\d{8}((0[1-9])|(1[0-2])){1}\d{5})|(\d{10}((0[1-9])|(1[0-2])){1}\d{5}[0-9x]{1})){1}$", RegexOptions.IgnoreCase);

        private static readonly Regex _EmailRegex = new Regex(@"^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$");

        private static readonly Regex _QueryParamRegex = new Regex(@"^(.+[=].+[&]{0,1})+$");


        private static readonly Regex _ConstStrRegex = new Regex(@"([\u4E00-\u9FBF]|\w|[()])+", RegexOptions.IgnoreCase);

        private static readonly Regex _CheckChina = new Regex("[\\u4E00-\\u9FBF]+");
        private static readonly Regex _CheckChinaNumLetter = new Regex(@"^([\u4E00-\u9FBF]|\w)+$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckChinaLetter = new Regex(@"^([\u4E00-\u9FBF]|[a-z])+$", RegexOptions.IgnoreCase);
        private static readonly Regex _CheckIsChina = new Regex("^[\\u4E00-\\u9FBF]+$");

        private static readonly Regex _MobileRegex = new Regex(@"^(\d{1,4}){0,1}1[3-9]\d{9}$");

        private static readonly Regex _IsPcRegex = new Regex(@"((windows NT)|(windows 98)|(Linux x86_64)|(linux i686)|(macintosh)){1}", RegexOptions.IgnoreCase);

        private static readonly Regex _IsAndroidRegex = new Regex(@"(android)+", RegexOptions.IgnoreCase);

        private static readonly Regex _IsSerialNoRegex = new Regex(@"^[a-z]{2}\w{2}\d{28}$", RegexOptions.IgnoreCase);
        private static readonly Regex _ExcelRegex = new Regex(@"^.*[.](xls|xlsx|xlt|xlsm|csv|xltx){1}$");

        private static readonly Regex _CheckIsBasic64 = new Regex("^(?:[A-ZA-Z0-9+/]{4})*(?:[A-ZA-Z0-9+/]{2}==|[A-ZA-Z0-9+/]{3}=)$");

        private static readonly Regex _CheckDate = new Regex(@"^20\d{2}[-/]((1[0-2])|([0]{0,1}[1-9])){1}[-/](([0][1-9])|([1-2][0-9])|([3][0-1])){1}$");

        private static readonly Regex _CheckHourMin = new Regex(@"^((2[0-3])|(1[0-9])|([0]{0,1}[0-9])){1}[:](([1-5][0-9])|([0]{0,1}[0-9])){1}$");

        private static readonly Regex _CheckTime = new Regex(@"^20\d{2}[-/]((1[0-2])|([0]{0,1}[1-9])){1}[-/](([0][1-9])|([1-2][0-9])|([3][0-1])){1}([ ]((2[0-3])|(1[0-9])|([0]{0,1}[0-9])){1}[:](([1-5][0-9])|([0]{0,1}[0-9])){1}([:](([1-5][0-9])|([0]{0,1}[0-9])){1}([.]\d{1,3}){0,1}){0,1}){0,1}$");

        private static readonly Regex _CheckIsGuid = new Regex(@"^((\w{32}|)|(\w{8}[-]\w{4}[-]\w{4}[-]\w{4}[-]\w{12})){1}$", RegexOptions.IgnoreCase);
        /// <summary>
        /// 检查是否为GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsGuid (string str)
        {
            return _CheckIsGuid.IsMatch(str);
        }
        public static bool CheckUriHost (string str)
        {
            return _UriHostRegex.IsMatch(str);
        }
        public static bool CheckAlipayAccount (string account)
        {
            if (_MobileRegex.IsMatch(account))
            {
                return true;
            }
            else if (_EmailRegex.IsMatch(account))
            {
                return true;
            }
            return false;
        }
        public static bool CheckIsBankAccount (string cardId)
        {
            if (cardId.Length != 16 || cardId.Length != 18 || !_NumRegex.IsMatch(cardId))
            {
                return false;
            }
            else
            {
                return _CheckBankAccount(cardId);
            }
        }
        private static bool _CheckBankAccount (string cardId)
        {
            char[] chs = cardId.ToCharArray();
            int index = chs.Length - 1;
            int num = 0;
            for (int i = index - 1, j = 0; i >= 0; i--, j++)
            {
                int k = chs[i] - '0';
                if (j % 2 == 0)
                {
                    k *= 2;
                    k = ( k / 10 ) + ( k % 10 );
                }
                num += k;
            }
            char val = ( num % 10 == 0 ) ? '0' : (char)( 10 - ( num % 10 ) + '0' );
            return val == chs[index];
        }

        public static bool CheckIsHtmlColor (string str)
        {
            return _CheckIsHtmlColor.IsMatch(str);
        }
        /// <summary>
        /// 检查是否为正数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsNum (string str)
        {
            return _NumRegex.IsMatch(str);
        }
        public static bool CheckIsNumLetter (string str)
        {
            return _NumLetterRegex.IsMatch(str);
        }
        /// <summary>
        /// 检查是否为BASIC64格式
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool CheckIsBasic64 (string txt)
        {
            return _CheckIsBasic64.IsMatch(txt);
        }

        /// <summary>
        /// 检查是否为数字(带小数位)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool CheckIsNumber (string number)
        {
            return _NumberRegex.IsMatch(number);
        }
        /// <summary>
        /// 检查是否是Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckIsEmail (string email)
        {
            return _EmailRegex.IsMatch(email);
        }
        /// <summary>
        /// 检查是否是GET参数格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool CheckIsQueryStr (string str)
        {
            return _QueryParamRegex.IsMatch(str);
        }
        /// <summary>
        /// 检查身份证
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static bool CheckCardID (string cardId)
        {
            return _CardIdRegex.IsMatch(cardId);
        }
        /// <summary>
        /// 检查是否包含中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsChina (string str)
        {
            return _CheckChina.IsMatch(str);
        }
        /// <summary>
        /// 检查是否由数字字母中文组成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckChinaNumLetter (string str)
        {
            return _CheckChinaNumLetter.IsMatch(str);
        }
        /// <summary>
        /// 检查是否为中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckWholeChina (string str)
        {
            return _CheckIsChina.IsMatch(str);
        }
        /// <summary>
        /// 检查Mac格式
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static bool CheckMac (string mac)
        {
            if (string.IsNullOrEmpty(mac))
            {
                return false;
            }
            return _MacRegex.IsMatch(mac);
        }
        /// <summary>
        /// 检查Ip格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIp (string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            return _IpRegex.IsMatch(ip);
        }
        /// <summary>
        /// 检查Ip6
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIp6 (string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            return _Ip6Regex.IsMatch(ip);
        }
        /// <summary>
        /// 检查Ip加端口格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIpPort (string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }
            return _IpPortRegex.IsMatch(ip);
        }
        /// <summary>
        /// 检查是否为PC
        /// </summary>
        /// <param name="uagent"></param>
        /// <returns></returns>
        public static bool CheckIsPc (string uagent)
        {
            if (uagent != null && !_IsAndroidRegex.IsMatch(uagent) && _IsPcRegex.IsMatch(uagent))
            {
                if (uagent.IndexOf("UCBrowser") != -1 && uagent.IndexOf("Windows NT 5.2") != -1)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查是否为URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool CheckIsUri (string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return false;
            }
            return _UriRegex.IsMatch(uri);
        }
        /// <summary>
        /// 检查是否为图片URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool CheckIsImgUri (string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return false;
            }
            return _ImgUriRegex.IsMatch(uri);
        }
        /// <summary>
        /// 检查是否为联系号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool CheckIsPhone (string phone)
        {
            return _PhoneRegex.IsMatch(phone);
        }
        /// <summary>
        /// 检查是否为手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool CheckIsMobilePhone (string phone)
        {
            return _MobileRegex.IsMatch(phone);
        }
        /// <summary>
        /// 检查文件名是否为Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsExcelFile (string fileName)
        {
            return _ExcelRegex.IsMatch(fileName);
        }

        public static bool CheckIsSerialNo (string str)
        {
            return _IsSerialNoRegex.IsMatch(str);
        }

        /// <summary>
        /// 检查时间格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsTime (string str)
        {
            return _CheckTime.IsMatch(str);
        }
        /// <summary>
        /// 检查时间格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsHourMinute (string str)
        {
            return _CheckHourMin.IsMatch(str);
        }
        /// <summary>
        /// 检查日期格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsDate (string str)
        {
            return _CheckDate.IsMatch(str);
        }
        /// <summary>
        /// 检查是否为质数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool CheckIsPrime (int n)
        {
            int m = (int)Math.Sqrt(n);
            for (int i = 2; i <= m; i++)
            {
                if (n % i == 0 && i != n)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查是否包含字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsLetter (string str)
        {
            return _ContainLetterRegex.IsMatch(str);
        }
        /// <summary>
        /// 检查是否包含数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsInt (string str)
        {
            return _ContainNumRegex.IsMatch(str);
        }
        /// <summary>
        /// 检查是否为偶数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsEvenNumber (long num)
        {
            return num % 2 == 0;
        }
        /// <summary>
        /// 检查是否为偶数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsEvenNumber (int num)
        {
            return num % 2 == 0;
        }
        /// <summary>
        /// 检查是否为偶数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsEvenNumber (short num)
        {
            return num % 2 == 0;
        }
        /// <summary>
        /// 检查是否为偶数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsEvenNumber (decimal num)
        {
            return num % 2 == 0;
        }
        /// <summary>
        /// 检查是否为奇数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsOddNumber (long num)
        {
            return num % 2 != 0;
        }
        /// <summary>
        /// 检查是否为奇数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsOddNumber (int num)
        {
            return num % 2 != 0;
        }
        /// <summary>
        /// 检查是否为奇数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool CheckIsOddNumber (decimal num)
        {
            return num % 2 != 0;
        }
        public static bool CheckIsIconClass (string str)
        {
            return _CheckIcon.IsMatch(str);
        }
        public static bool CheckIsLetterNumPoint (string str)
        {
            return _CheckIsLetterNumPoint.IsMatch(str);
        }
        public static bool CheckIsLetterPoint (string str)
        {
            return _CheckIsLetterPoint.IsMatch(str);
        }
        public static bool CheckIsName (string str)
        {
            return _CheckIsName.IsMatch(str);
        }

        public static string GetFormatRegexStr (ValidateFormat format, string containStr, out string show)
        {
            if (format == ValidateFormat.Email)
            {
                show = null;
                return _EmailRegex.ToString();
            }
            else if (format == ValidateFormat.图标样式)
            {
                show = null;
                return _CheckIcon.ToString();
            }
            else if (format == ValidateFormat.数字字母点)
            {
                show = null;
                return _CheckIsLetterNumPoint.ToString();
            }
            else if (format == ValidateFormat.颜色值)
            {
                show = "#ffffff";
                return _CheckIsHtmlColor.ToString();
            }
            else if (format == ValidateFormat.银行卡号)
            {
                show = "长度16或19位全数字,卡号最后一位为校验位，取基偶校验!";
                return null;
            }
            else if (format == ValidateFormat.支付宝)
            {
                show = "邮箱或手机号";
                return string.Format("邮箱：{0} 或 {1} ", _EmailRegex.ToString(), _MobileRegex.ToString());
            }
            else if (format == ValidateFormat.中文字母数字括号)
            {
                show = "中文，字母，数字和括号";
                return _ConstStrRegex.ToString();
            }
            else if (format == ValidateFormat.相对路径)
            {
                show = "相对路径格式 pages/index/index";
                return _RelativePathRegex.ToString();
            }
            else if (format == ValidateFormat.姓名)
            {
                show = "中文名";
                return _CheckIsName.ToString();
            }
            else if (format == ValidateFormat.纯数字)
            {
                show = "纯数字";
                return _NumRegex.ToString();
            }
            else if (format == ValidateFormat.HOST)
            {
                show = "域名地址";
                return _UriHostRegex.ToString();
            }
            else if (format == ValidateFormat.纯字母)
            {
                show = "A-Z,a-z";
                return _LetterRegex.ToString();
            }
            else if (format == ValidateFormat.数字字母)
            {
                show = null;
                return _NumLetterRegex.ToString();
            }
            else if (format == ValidateFormat.手机号)
            {
                show = null;
                return _MobileRegex.ToString();
            }
            else if (format == ValidateFormat.联系电话)
            {
                show = null;
                return _PhoneRegex.ToString();
            }
            else if (format == ValidateFormat.URL)
            {
                show = null;
                return _UriRegex.ToString();
            }
            else if (format == ValidateFormat.文件URI)
            {
                show = string.Format("检查为URI格式并匹配：{0} 文件后缀!", containStr);
                return _UriRegex.ToString();
            }
            else if (format == ValidateFormat.图片URI)
            {
                show = null;
                return _ImgUriRegex.ToString();
            }
            else if (format == ValidateFormat.MAC)
            {
                show = null;
                return _MacRegex.ToString();
            }
            else if (format == ValidateFormat.IP)
            {
                show = null;
                return _IpRegex.ToString();
            }
            else if (format == ValidateFormat.IP6)
            {
                show = null;
                return _Ip6Regex.ToString();
            }
            else if (format == ValidateFormat.IP含端口)
            {
                show = null;
                return _IpPortRegex.ToString();
            }
            else if (format == ValidateFormat.包含数字)
            {
                show = null;
                return _ContainNumRegex.ToString();
            }
            else if (format == ValidateFormat.包含字母)
            {
                show = null;
                return _ContainLetterRegex.ToString();
            }
            else if (format == ValidateFormat.Guid)
            {
                show = null;
                return _CheckIsGuid.ToString();
            }
            else if (format == ValidateFormat.身份证号)
            {
                show = null;
                return _CardIdRegex.ToString();
            }
            else if (format == ValidateFormat.日期)
            {
                show = null;
                return _CheckDate.ToString();
            }
            else if (format == ValidateFormat.时间)
            {
                show = null;
                return _CheckTime.ToString();
            }
            else if (format == ValidateFormat.小时分)
            {
                show = null;
                return _CheckHourMin.ToString();
            }
            else if (format == ValidateFormat.纯中文)
            {
                show = null;
                return _CheckIsChina.ToString();
            }
            else if (format == ValidateFormat.包含中文)
            {
                show = null;
                return _CheckChina.ToString();
            }
            else if (format == ValidateFormat.SerialNo)
            {
                show = null;
                return _IsSerialNoRegex.ToString();
            }
            else if (format == ValidateFormat.中文字母数字)
            {
                show = null;
                return _CheckChinaNumLetter.ToString();
            }
            else if (format == ValidateFormat.中文字母)
            {
                show = null;
                return _CheckChinaLetter.ToString();
            }
            else if (format == ValidateFormat.字母点)
            {
                show = null;
                return _CheckIsLetterPoint.ToString();
            }
            else
            {
                show = null;
                return null;
            }
        }
        public static bool CheckData (string str, ValidateFormat format, string containStr)
        {
            if (format == ValidateFormat.已指定字符开头)
            {
                return str.StartsWith(containStr);
            }
            else if (format == ValidateFormat.已指定字符结尾)
            {
                return str.EndsWith(containStr);
            }
            else if (format == ValidateFormat.GET参数)
            {
                return CheckIsQueryStr(str);
            }
            else if (format == ValidateFormat.Email)
            {
                return CheckIsEmail(str);
            }
            else if (format == ValidateFormat.图标样式)
            {
                return CheckIsIconClass(str);
            }
            else if (format == ValidateFormat.数字字母点)
            {
                return CheckIsLetterNumPoint(str);
            }
            else if (format == ValidateFormat.颜色值)
            {
                return ValidateHelper.CheckIsHtmlColor(str);
            }
            else if (format == ValidateFormat.中文字母数字括号)
            {
                return ValidateHelper.CheckIsLetterNumBrackets(str);
            }
            else if (format == ValidateFormat.银行卡号)
            {
                return ValidateHelper.CheckIsBankAccount(str);
            }
            else if (format == ValidateFormat.支付宝)
            {
                return ValidateHelper.CheckAlipayAccount(str);
            }
            else if (format == ValidateFormat.HOST)
            {
                return ValidateHelper.CheckUriHost(str);
            }
            else if (format == ValidateFormat.姓名)
            {
                return CheckIsName(str);
            }
            else if (format == ValidateFormat.纯数字)
            {
                return CheckIsNum(str);
            }
            else if (format == ValidateFormat.数字字母)
            {
                return CheckIsNumLetter(str);
            }
            else if (format == ValidateFormat.手机号)
            {
                return CheckIsMobilePhone(str);
            }
            else if (format == ValidateFormat.联系电话)
            {
                return CheckIsPhone(str);
            }
            else if (format == ValidateFormat.URL)
            {
                return CheckIsUri(str);
            }
            else if (format == ValidateFormat.文件URI)
            {
                return CheckIsFileUri(str, containStr);
            }
            else if (format == ValidateFormat.图片URI)
            {
                return CheckIsImgUri(str);
            }
            else if (format == ValidateFormat.MAC)
            {
                return CheckMac(str);
            }
            else if (format == ValidateFormat.IP)
            {
                return CheckIp(str);
            }
            else if (format == ValidateFormat.IP6)
            {
                return CheckIp6(str);
            }
            else if (format == ValidateFormat.IP含端口)
            {
                return CheckIpPort(str);
            }
            else if (format == ValidateFormat.包含特定字符)
            {
                return str.IndexOf(containStr) != -1;
            }
            else if (format == ValidateFormat.包含数字)
            {
                return CheckIsInt(str);
            }
            else if (format == ValidateFormat.包含字母)
            {
                return CheckIsLetter(str);
            }
            else if (format == ValidateFormat.Guid)
            {
                return CheckIsGuid(str);
            }
            else if (format == ValidateFormat.身份证号)
            {
                return CheckCardID(str);
            }
            else if (format == ValidateFormat.日期)
            {
                return CheckIsDate(str);
            }
            else if (format == ValidateFormat.时间)
            {
                return CheckIsTime(str);
            }
            else if (format == ValidateFormat.小时分)
            {
                return CheckIsHourMinute(str);
            }
            else if (format == ValidateFormat.纯字母)
            {
                return _LetterRegex.IsMatch(str);
            }
            else if (format == ValidateFormat.纯中文)
            {
                return CheckWholeChina(str);
            }
            else if (format == ValidateFormat.相对路径)
            {
                return _RelativePathRegex.IsMatch(str);
            }
            else if (format == ValidateFormat.包含中文)
            {
                return CheckIsChina(str);
            }
            else if (format == ValidateFormat.SerialNo)
            {
                return CheckIsSerialNo(str);
            }
            else if (format == ValidateFormat.中文字母数字)
            {
                return CheckChinaNumLetter(str);
            }
            else if (format == ValidateFormat.中文字母)
            {
                return _CheckChinaLetter.IsMatch(str);
            }
            else if (format == ValidateFormat.字母点)
            {
                return _CheckIsLetterPoint.IsMatch(str);
            }
            return false;
        }

        public static bool CheckIsLetterNumBrackets (string str)
        {
            return _ConstStrRegex.IsMatch(str);
        }

        public static bool CheckIsFileUri (string uri, string containStr)
        {
            if (!CheckIsUri(uri))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(containStr))
            {
                return true;
            }
            else if (!containStr.Contains(','))
            {
                containStr = containStr.ToLower();
                Uri newUri = new Uri(uri);
                string str = newUri.AbsoluteUri.ToLower();
                return str.EndsWith(containStr);
            }
            else
            {
                string[] str = containStr.ToLower().Split(',');
                Uri newUri = new Uri(uri);
                string us = newUri.AbsoluteUri.ToLower();
                return Array.FindIndex(str, a =>
                {
                    return us.EndsWith(a);
                }) != -1;
            }
        }
    }
}
