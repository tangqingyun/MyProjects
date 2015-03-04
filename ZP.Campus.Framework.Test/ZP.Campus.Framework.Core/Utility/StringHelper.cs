using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.Utility
{
    public static class StringHelper
    {
        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        ///
        /// The string returned includes outer quotes
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;

                    case '\\':
                        sb.Append("\\\\");
                        break;

                    case '\b':
                        sb.Append("\\b");
                        break;

                    case '\f':
                        sb.Append("\\f");
                        break;

                    case '\n':
                        sb.Append("\\n");
                        break;

                    case '\r':
                        sb.Append("\\r");
                        break;

                    case '\t':
                        sb.Append("\\t");
                        break;

                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        /// <summary>
        /// Check String is null or empty (length is zero or all chars in this string are space)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim().Length == 0;
        }

        /// <summary>
        /// Check String is not null or  not empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string input)
        {
            return !input.IsEmpty();
        }

        /// <summary>
        /// Gets the left string.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="leftLength">Length of the left.</param>
        /// <returns></returns>
        public static string GetLeftString(this string description, int leftLength)
        {
            if (IsEmpty(description) || description.Length <= leftLength)
            {
                return description;
            }
            return description.Substring(0, leftLength);
        }

        /// <summary>
        /// Gets the right string.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="rightLength">Length of the right.</param>
        /// <returns></returns>
        public static string GetRightString(this string description, int rightLength)
        {
            if (IsEmpty(description) || description.Length <= rightLength)
            {
                return description;
            }
            return description.Substring(description.Length - rightLength);
        }

        public static string GetRandomStr(int strLength)
        {
            return GetRandomStr(strLength, 0);
        }

        public static string GetRandomStr(int strLength, int randomSeed)
        {
            string strBaseStr = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] baseStrList = strBaseStr.Split(',');

            Random randomObject = null;
            if (randomSeed > 0)
            {
                randomObject = new Random(randomSeed);
            }
            else
            {
                randomObject = new Random();
            }

            string strResult = "";
            while (strResult.Length < strLength)
            {
                strResult = strResult + baseStrList[randomObject.Next(baseStrList.Length)];
            }

            return strResult;
        }

        public static string GetRandomSixthString()
        {
            return HashHelper.MD5(Guid.NewGuid().ToString()).HexUInt64;
        }

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string[] BubbleSort(this string[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (string.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        string str = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = str;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static bool IsInt(string beCheckedStr)
        {
            if (string.IsNullOrEmpty(beCheckedStr)) return false;

            int tempInt;
            return int.TryParse(beCheckedStr, out tempInt);
        }

        public static bool IsDecimal(string beCheckedStr)
        {
            if (string.IsNullOrEmpty(beCheckedStr)) return false;

            decimal tempDecimal;
            return decimal.TryParse(beCheckedStr, out tempDecimal);
        }

        public static bool IsDouble(string beCheckedStr)
        {
            if (string.IsNullOrEmpty(beCheckedStr)) return false;

            double tempDouble;
            return double.TryParse(beCheckedStr, out tempDouble);
        }

        public static bool IsDateTime(string beCheckedStr)
        {
            if (string.IsNullOrEmpty(beCheckedStr)) return false;

            DateTime tempDateTime;
            return DateTime.TryParse(beCheckedStr, out tempDateTime);
        }

        public static string GetStringValue(object obj)
        {
            if (obj == null) return string.Empty;

            return obj.ToString();
        }

        public static int GetIntValue(object obj)
        {
            if (obj == null) return 0;

            return GetIntValue(obj.ToString());
        }

        public static int GetIntValue(string str)
        {
            if (IsInt(str))
            {
                return int.Parse(str);
            }
            else
            {
                return 0;
            }
        }

        public static decimal GetDecimalValue(object obj)
        {
            if (obj == null) return 0;

            return GetDecimalValue(obj.ToString());
        }

        public static decimal GetDecimalValue(string str)
        {
            if (IsDecimal(str))
            {
                return decimal.Parse(str);
            }
            else
            {
                return 0;
            }
        }

        public static DateTime GetDateTimeValue(string str)
        {
            if (IsDateTime(str))
            {
                return DateTime.Parse(str);
            }
            else
            {
                return DateTime.MinValue;//DateTimeHelper.MinValue;
            }
        }

        public static string[] ChangeIntArrayToStringArray(int[] beChangeArray)
        {
            if (beChangeArray == null) return new string[0];

            string[] strList = new string[beChangeArray.Length];
            int I = 0;

            foreach (int curInt in beChangeArray)
            {
                strList[I] = curInt.ToString();

                I++;
            }

            return strList;
        }

        public static int[] ChangeStringArrayToIntArray(string[] beChangeArray)
        {
            if (beChangeArray == null) return new int[0];

            int[] intList = new int[beChangeArray.Length];
            int I = 0;
            int tempInt = 0;

            foreach (string curStr in beChangeArray)
            {
                intList[I] = 0;
                tempInt = 0;
                if (int.TryParse(curStr, out tempInt))
                {
                    intList[I] = tempInt;
                }

                I++;
            }

            return intList;
        }

        public static decimal[] ChangeStringArrayToDecimalArray(string[] beChangeArray)
        {
            if (beChangeArray == null) return new decimal[0];

            decimal[] decimalList = new decimal[beChangeArray.Length];
            int I = 0;
            decimal tempDecimal = 0;

            foreach (string curStr in beChangeArray)
            {
                decimalList[I] = 0;
                tempDecimal = 0;
                if (decimal.TryParse(curStr, out tempDecimal))
                {
                    decimalList[I] = tempDecimal;
                }

                I++;
            }

            return decimalList;
        }

        public static string ChangeStringArrayToStr(string[] beChangeArray, string linkStr)
        {
            return ChangeStringListToStr(beChangeArray.ToList(), linkStr);
        }

        public static string ChangeStringListToStr(List<string> beChangeList, string linkStr)
        {
            StringBuilder tempResult = new StringBuilder();

            int I = 1;
            foreach (string curStr in beChangeList)
            {
                tempResult.Append(curStr);
                if (I < beChangeList.Count)
                {
                    tempResult.Append(linkStr);
                }

                I++;
            }

            return tempResult.ToString();
        }

        public static List<string> ChangeSearchStrToStrList(string searchStr)
        {
            string tempStr = searchStr;
            if (tempStr == null) tempStr = string.Empty;
            tempStr = tempStr.Replace(",", " ");
            tempStr = tempStr.Replace(";", " ");
            tempStr = tempStr.Replace("，", " ");
            tempStr = tempStr.Replace("；", " ");

            while (tempStr.IndexOf("  ") >= 0)
            {
                tempStr = tempStr.Replace("  ", " ");
            }

            List<string> tempResult = tempStr.Split(' ').ToList();

            int I = 0;
            while (I < tempResult.Count)
            {
                tempResult[I] = tempResult[I].Trim();

                I++;
            }

            return tempResult;
        }

        public static List<int> ChangeSearchStrToIntList(string searchStr)
        {
            List<string> tempStrList = ChangeSearchStrToStrList(searchStr);
            List<int> tempResult = new List<int>();

            foreach (string cutStr in tempStrList)
            {
                if (IsInt(cutStr))
                {
                    tempResult.Add(int.Parse(cutStr));
                }
            }

            return tempResult;
        }

        public static List<Int64> ChangeSearchStrToInt64List(string searchStr)
        {
            List<string> tempStrList = ChangeSearchStrToStrList(searchStr);
            List<Int64> tempResult = new List<Int64>();

            foreach (string cutStr in tempStrList)
            {
                if (IsInt(cutStr))
                {
                    tempResult.Add(Int64.Parse(cutStr));
                }
            }

            return tempResult;
        }

        public static List<string> ChangeIntListToStringList(List<int> beChangeList)
        {
            if (beChangeList == null || beChangeList.Count <= 0) return new List<string>();

            List<string> strList = new List<string>();

            foreach (int curInt in beChangeList)
            {
                strList.Add(curInt.ToString());
            }

            return strList;
        }

        public static List<int> ChangeStringListToIntList(List<string> beChangeList)
        {
            if (beChangeList == null || beChangeList.Count <= 0) return new List<int>();

            List<int> intList = new List<int>();

            foreach (string curStr in beChangeList)
            {
                if (IsInt(curStr))
                {
                    intList.Add(int.Parse(curStr));
                }
            }

            return intList;
        }

        /// <summary>
        /// 字符串格式化。
        /// </summary>
        /// <param name="target">要格式化的字符串。</param>
        /// <param name="args">参数。</param>
        /// <returns>格式化的字符串。</returns>
        public static string FormatWith(string target, params object[] args)
        {
            return IsEmpty(target) ? string.Empty : string.Format(CultureInfo.CurrentCulture, target, args == null ? new object[] { } : args);
        }
    }
}
