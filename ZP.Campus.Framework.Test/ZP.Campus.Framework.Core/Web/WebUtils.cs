using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ZP.Campus.Framework.Core.Web
{
    public class WebUtils
    {
        /// <summary>
        /// 判断字符串中是否只支持中文、数字和字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsSinogramOrCharacterOrNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) return true;

            var regChina = new Regex("^([\u4e00-\u9fbb]|[0-9]|[a-zA-Z])+$");

            return regChina.IsMatch(str);
        }

        public static string HttpRequest(string url, Encoding encoding, string WebProxy)
        {

            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";


            if (!string.IsNullOrEmpty(WebProxy))
            {
                wReq.Proxy = new WebProxy(WebProxy);
            }


            // Get the response instance.
            using (WebResponse wResp = wReq.GetResponse())
            {
                using (Stream respStream = wResp.GetResponseStream())
                {
                    // Dim reader As StreamReader = New StreamReader(respStream)
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

        }


        /// <summary>
        /// 获得Rquest参数
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paraName">参数名称</param>
        /// <returns></returns>
        public static T GetRequest<T>(string paraName)
        {
            return GetRequest<T>(paraName, null);
        }

        /// <summary>
        /// 获得Rquest参数
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="paraName">参数名称</param>
        /// <returns></returns>
        public static T GetRequest<T>(string paraName, object defaultValue)
        {
            if (HttpContext.Current.Request[paraName] == null)
            {
                if (defaultValue != null)
                {
                    return (T)defaultValue;
                }
                else
                {
                   // throw new CampusExceptionBase(string.Format("参数{0}为空", paraName), new Exception(string.Format("参数{0}为空", paraName))) { ErrorCode = 101, MessageType = "ParameterException" };
                }
            }

            try
            {
                return (T)Convert.ChangeType(HttpContext.Current.Request[paraName], typeof(T));
            }
            catch
            {
                return (T)defaultValue;
                //throw new CampusExceptionBase(string.Format("参数{0}格式不正确", paraName), new Exception("参数{0}格式不正确")) { ErrorCode = 101, MessageType = "ParameterException" };
            }
        }

        #region 获取IP

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            string ip = string.Empty;
            try
            {
                OperationContext context = OperationContext.Current;
                if (context != null)
                {
                    MessageProperties properties = context.IncomingMessageProperties;
                    HttpRequestMessageProperty httpRequest = properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                    if (httpRequest != null)
                    {
                        var header = httpRequest.Headers;
                        if (header != null)
                        {
                            string clientIPxff = string.Empty;
                            string[] xff = header.GetValues("X-Forwarded-For");
                            if (xff != null)
                            {
                                foreach (var x in xff)
                                {
                                    clientIPxff += x;
                                }
                                if (clientIPxff != string.Empty)
                                {
                                    return clientIPxff;
                                }
                            }
                        }
                    }

                    RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    ip = endpoint.Address;
                }
                else if (HttpContext.Current != null)
                {
                    var httpReq = HttpContext.Current.Request;
                    if (httpReq != null)
                    {
                        // 经过F5转发的请求，源客户端IP会记录在X-Forwarded-For头中
                        ip = httpReq.Headers.Get("X-Forwarded-For");
                        if (string.IsNullOrEmpty(ip))
                        {
                            ip = httpReq.UserHostAddress;
                        }
                    }
                }
                else
                {
                    ip = GetWebServerIp();
                }
            }
            catch
            {
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            if (ip == string.Empty)
            {
                ip = GetVisitorIPAddress(IPAddressType.WAN);
            }
            return ip;
        }

        /// <summary>
        /// 获取IP地址的方式
        /// </summary>
        public enum IPAddressType
        {
            LAST,
            WAN,
            FULL
        }

        /// <summary>
        /// 获取访问者的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetVisitorIPAddress(IPAddressType type)
        {
            string result = string.Empty;

            if (HttpContext.Current != null)
            {
                var sv = HttpContext.Current.Request.ServerVariables;
                result = sv["HTTP_X_FORWARDED_FOR1"];
                if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                {
                    result = sv["HTTP_XOR_FWARDED_FOR"];
                    if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                    {
                        result = sv["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                        {
                            result = sv["HTTP_FOX_RWARDED_FOR"];
                            if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                            {
                                result = sv["HTTP_XOR_FWARDED_FOR"];
                                if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                                {
                                    result = sv["REMOTE_ADDR"];
                                    if (string.IsNullOrEmpty(result) || result.ToLower().IndexOf("unknown") > 0)
                                    {
                                        result = HttpContext.Current.Request.UserHostAddress;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理
                if (result.IndexOf(".") == -1) //没有“.”肯定是非IPv4格式
                {
                    result = string.Empty;
                }
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。
                        result = result.Replace(" ", "").Replace("'", "");

                        string[] temparyip = result.Split(",;".ToCharArray());

                        switch (type)
                        {
                            case IPAddressType.FULL:
                                return result;
                            case IPAddressType.LAST:
                                return temparyip[temparyip.Length - 1];
                            case IPAddressType.WAN:
                                for (int i = 0; i < temparyip.Length; i++)
                                {
                                    if (temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                                    {
                                        return temparyip[i]; //找到不是内网的地址
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取Web服务器IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetWebServerIp()
        {
            string ip = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    ip = HttpContext.Current.Request.ServerVariables["Local_Addr"];
                    //return ip;
                }
                IPAddress[] ips = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;

                int len = ips.Length;
                if (len > 0)
                {
                    foreach (var item in ips)
                    {
                        if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ip = item.ToString();
                            break;
                        }
                    }

                    //IP V6
                    //foreach (var item in ips)
                    //{
                    //    if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    //    {
                    //        ip += item.ToString() + " | ";
                    //    }
                    //}
                }

                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
                //return ip;
            }
            catch
            {
            }
            return ip;
        }

        #endregion


        #region 获取汉子首字母

        /// <summary>
        /// 获取一组汉字的首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }
        public static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }

        #endregion
    }
}
