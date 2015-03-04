using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ZP.Campus.Framework.Core.Utility
{
    /// <summary>
    /// 校验工具集。
    /// </summary>
    public static class Validator
    {
        private const string DigitsPattern = @"^[0-9]+$";
        private const string EmailPattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private const string ChineseCharacterPattern = @"[\u4e00-\u9fa5]";
        private const string TelephonePattern = @"^\d{3,4}-\d{7,8}(-\d{3,4})?$";
        private const string MobilePattern = @"^1\d{10}$";
        private const string UrlPattern = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
        private const string ArmCardPattern = @"^\d{7}$";
        private const string ZipCodePattern = @"^\d{6}$";
        private const string MoneyPattern = @"(^[1-9][0-9]{0,8}$)|(^[1-9][0-9]{0,8}\.{1}[0-9]{1}[0-9]?$)|(^[0-9]{1}\.{1}[0-9]{1}[0-9]?$)";

        /// <summary>
        /// 判断一个值是否能够转换成一个有效的数字。
        /// </summary>
        /// <param name="value">要判断的值。</param>
        /// <returns>如果 <paramref name="value"/> 是一个有效的数字，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsMoney(string value)
        {
            Regex regex = new Regex(MoneyPattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 判断一个值是否能够转换成一个有效的数字。
        /// </summary>
        /// <param name="value">要判断的值。</param>
        /// <returns>如果 <paramref name="value"/> 是一个有效的数字，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsNumeric(object value)
        {
            if (value == null)
            {
                return false;
            }
            double dbl;
            return double.TryParse(value.ToString(), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out dbl);
        }

        /// <summary>
        /// 判断一个值是否能够转换成一个整形数字。
        /// </summary>
        /// <param name="value">要判断的值。</param>
        /// <returns>如果 <paramref name="value"/> 是一个有效的整数，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsInteger(object value)
        {
            if (value == null)
            {
                return false;
            }
            int i;
            return int.TryParse(value.ToString(), out i);
        }

        /// <summary>
        /// 判断一个值是否能够转换成一个Decimal。
        /// </summary>
        /// <param name="value">要判断的值。</param>
        /// <returns>如果 <paramref name="value"/> 是一个有效的Decimal，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsDecimal(object value)
        {
            if (value == null)
            {
                return false;
            }
            decimal dec;
            return decimal.TryParse(value.ToString(), out dec);
        }

        /// <summary>
        /// 判断一个值是否能够转换成一个Double。
        /// </summary>
        /// <param name="value">要判断的值。</param>
        /// <returns>如果 <paramref name="value"/> 是一个有效的Decimal，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsDouble(object value)
        {
            if (value == null)
            {
                return false;
            }
            double dec;
            return double.TryParse(value.ToString(), out dec);
        }

        /// <summary>
        /// 判断一个字符串是否仅由0-9的数字组成。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 字符串是否仅由0-9的数字组成，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsDigits(string value)
        {
            Regex regex = new Regex(DigitsPattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 判断字符串是否有中文字符。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 字符串包含中文，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool HasChineseCharacter(string value)
        {
            Regex regex = new Regex(ChineseCharacterPattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 判断字符串是否是Email地址。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 字符串是Email地址，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsEmail(string value)
        {
            Regex regex = new Regex(EmailPattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 判断字符串是否是ZipCode。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 字符串是ZipCode，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsZipCode(string value)
        {
            Regex regex = new Regex(ZipCodePattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 判断字符串是否是日期。
        /// </summary>
        /// <param name="value">要判断的字符串。</param>
        /// <returns>如果 <paramref name="value"/> 字符串是日期，则返回 <b>true</b>; 反之返回 <b>false</b>。</returns>
        public static bool IsDate(string value)
        {
            DateTime dt;
            return DateTime.TryParse(value, out dt);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTelephone(string value)
        {
            Regex regex = new Regex(TelephonePattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMobile(string value)
        {
            Regex regex = new Regex(MobilePattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(string value)
        {
            Regex regex = new Regex(UrlPattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIdentityCardNumber(string IDCard)
        {
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            string[] Checker = ("1,9,8,7,6,5,4,3,2,1,1").Split(',');
            int intLength = IDCard.Length;

            int i = 0, TotalmulAiWi = 0;
            int modValue = 0;
            string strVerifyCode = "";
            string Ai = "";
            string BirthDay = "";
            int intYear = 0;
            int intMonth = 0;
            int intDay = 0;

            if (intLength != 18)
            {
                return false;
            }
            if (intLength == 18)
            {
                Ai = IDCard.Substring(0, 17);
            }
            else if (intLength == 15)
            {
                Ai = IDCard;
                Ai = Ai.Substring(0, 6) + "19" + Ai.Substring(6, 9);
            }
            if (!IsNumeric(Ai)) return false;

            intYear = Convert.ToInt32(Ai.Substring(6, 4));
            intMonth = Convert.ToInt32(Ai.Substring(10, 2));
            intDay = Convert.ToInt32(Ai.Substring(12, 2));

            BirthDay = intYear.ToString() + "-" + intMonth.ToString() + "-" + intDay.ToString();
            if (IsDate(BirthDay))
            {
                DateTime DateBirthDay = DateTime.Parse(BirthDay);
                if (DateBirthDay > DateTime.Now)
                {
                    return false;
                }

                int intYearLength = DateBirthDay.Year - DateBirthDay.Year;
                if (intYearLength < -140)
                {
                    return false;
                }
            }

            if (intMonth > 12 || intDay > 31)
            {
                return false;
            }

            for (i = 0; i < 17; i++)
            {
                TotalmulAiWi = TotalmulAiWi + (Convert.ToInt32(Ai.Substring(i, 1)) * Convert.ToInt32(Wi[i].ToString()));
            }
            modValue = TotalmulAiWi % 11;

            strVerifyCode = arrVarifyCode[modValue].ToString();
            Ai = Ai + strVerifyCode;
            if (intLength == 18 && !Ai.Equals(IDCard, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsArmyCardNumber(string value)
        {
            Regex regex = new Regex(ArmCardPattern);
            return regex.IsMatch(value);
        }
    }
}
