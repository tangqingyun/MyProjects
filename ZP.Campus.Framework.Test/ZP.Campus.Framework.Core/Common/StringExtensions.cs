using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// 从右取字符串
        /// </summary>
        /// <param name="s">原串</param>
        /// <param name="len">长度</param>
        /// <returns>从右取出的字符串</returns>
        public static string Right(this string s, int len)
        {
            return s.Substring(len > s.Length ? 0 : s.Length - len);
        }

        /// <summary>
        /// 从左取字符串
        /// </summary>
        /// <param name="s">原串</param>
        /// <param name="len">长度</param>
        /// <returns>从左取出的字符串</returns>
        public static string Left(this string s, int len)
        {
            return s.Substring(0, len > s.Length ? s.Length : len);
        }
    }
}
