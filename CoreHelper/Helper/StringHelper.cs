using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Omipay.Core
{
    public static class StringHelper
    {
        public static string FromBase64String(string base64str , Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            string str = encoding.GetString(Convert.FromBase64String(base64str));
            return str;
        }


        public static byte[] FromBase64StringToBytes(string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        public static Stream FromBase64StringToStream(string base64Str)
        {
            return new MemoryStream(FromBase64StringToBytes(base64Str));
        }

        public static string ToBase64String(string source, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return ToBase64String(encoding.GetBytes(source));
        }

        public static string ToBase64String(byte[] bytes)
        {
            string str = Convert.ToBase64String(bytes);
            return str;
        }

        public static bool IsMatch(string s, string reg)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("则正参数必填", "s");
            }
            if (string.IsNullOrWhiteSpace(reg))
            {
                throw new ArgumentException("则正参数必填", "reg");
            }
            Regex regex = new Regex(reg);
            return regex.IsMatch(s);
        }


    }
}
