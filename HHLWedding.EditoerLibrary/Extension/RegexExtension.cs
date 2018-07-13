using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HHLWedding.EditoerLibrary.Extension
{
    public class RegexExtension
    {
        //验证Email地址
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([w-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([w-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$");
        }


        //dd-mm-yy 的日期形式代替 mm/dd/yy 的日期形式。
        //string MDYToDMY(String input)
        //{
        //    return Regex.Replace(input, "\b(?^\d{1,2})/(?\d{1,2})/(?\d{2,4})\b", "${day}-${month}-${year}");
        //}


        //验证是否为小数
        public static bool IsValidDecimal(string strIn)
        {
            return Regex.IsMatch(strIn, @"[0].d{1,2}|[1]");
        }


        //验证是否为电话号码
        public static bool IsValidPhone(string strIn)
        {
            return Regex.IsMatch(strIn, @"^1[34578][0-9]{9}$");
        }


        //验证年月日
        public static bool IsValidDate(string strIn)
        {
            return Regex.IsMatch(strIn, @"^2d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]d|3[0-1])(?:0?[1-9]|1d|2[0-3]):(?:0?[1-9]|[1-5]d):(?:0?[1-9]|[1-5]d)$");
        }


        //验证后缀名
        public static bool IsValidPostfix(string strIn)
        {
            return Regex.IsMatch(strIn, @".(?i:gif|jpg)$");
        }


        //验证字符是否在4至12之间
        public static bool IsValidByte(string strIn)
        {
            return Regex.IsMatch(strIn, @"^[a-z]{4,12}$");
        }


        //验证IP
        public static bool IsValidIp(string strIn)
        {
            return Regex.IsMatch(strIn, @"^(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])$");
        }
    }
}
