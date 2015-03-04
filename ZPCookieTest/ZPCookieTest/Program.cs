using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zhaopin.Universal.ZPCookie;

namespace ZPCookieTest
{
    class Program
    {
        /// <summary>
        /// 默认cookie加密密钥
        /// </summary>
        private static string[] DEFAULT_COOKIE_KEYS = {
                "sdfdsgweg",
                "asdfssgjweh",
                "asdfssgjweh",
                "asdsdfsdfssgjweh",
                "sadf",
                "asdfiulssgjweh",
                "dfmasrtdfsdfssgjweh",
                "gm765",
                "m,yuliuy4",
                "a,56i6k",
                "457hjk"};
        static IZpCookie cookie = CookieHelper.GetInstance();
        static void Main(string[] args)
        {
            string str = "382C3D7551694479046D1F79456C5B754D345A2C487559694E79786D6279486C1D751834002C177515693579476D43791D6C1C7557340E2C167501694E79716D6179486C1D751834002C177515693579476D43791D6C1C7557340E2C167501694E79676D7879486C587542342C2C2D7551694479046D0179446C597559345C2C4B75566946790D6D1779346C257544345C2C427539693479096D6679276C59754A345D2C4B7558694279056D1979456C52752C34392C447526691A79566D7F79106C0C751234082C0B75576926797D6D1179466C5A7542348"; //cookie.getCookieValue("JSpUserInfo");
            try
            {
                string cookieValue = DecryptData(str);
                string[] arr = cookieValue.Split(';');
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine(arr[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
            
        }


        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="cookieValue"></param>
        /// <returns></returns>
        public static string DecryptData(string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieValue))
                return string.Empty;
            int intRnd = Convert.ToInt32(cookieValue.Substring(cookieValue.Length - 1));

            byte[] data = new byte[(cookieValue.Length - 1) / 2];
            int k = 0;
            for (int i = 0; i < cookieValue.Length - 1; i = i + 2)
            {
                data[k++] = Convert.ToByte(Convert.ToInt64(cookieValue.Substring(i, 2), 16));
            }

            string strData = Encoding.Unicode.GetString(data);
            byte[] ret = Encrypt(strData, DEFAULT_COOKIE_KEYS[intRnd]);
            string strRet = Encoding.Unicode.GetString(ret);
            return strRet;
        }

        /// <summary>
        /// 根据cookie密钥加密cookie数据
        /// </summary>
        /// <param name="cookieValue">Cookie值明文</param>
        /// <param name="cookieKey">cookie加密密钥</param>
        /// <returns>加密的cookie数据</returns>
        private static byte[] Encrypt(string cookieValue, string cookieKey)
        {
            byte[] arrValues = Encoding.Unicode.GetBytes(cookieValue);
            if (string.IsNullOrEmpty(cookieKey.Trim()))
                return arrValues;

            byte[] arrKeys = Encoding.UTF8.GetBytes(cookieKey);
            List<byte> ret = new List<byte>();
            byte c;
            int k = 0;

            for (int i = 0; i < arrValues.Length; i++)
            {
                c = (byte)(arrValues[i] ^ arrKeys[k]);
                ret.Add(c);
                k++;
                if (k >= cookieKey.Length)
                    k = 0;
            }
            return ret.ToArray<byte>();
        }

    }
}
