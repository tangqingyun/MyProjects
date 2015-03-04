using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace _360DOC.Tool
{
    public static class HttpHelper
    {
        private const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public delegate void DelegateGetComplated(string url, string statusCode, string result, int usedTime);
        public delegate void DelegatePostComplated(string url, string paramStr, string statusCode, string result, int usedTime);

        public static DelegateGetComplated GetComplated;
        public static DelegatePostComplated PostComplated;

        /// <summary>  
        /// 通过GET方式获取文本
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static string Get(string url, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, IList<ParamKeyValue> heards = null)
        {
            HttpWebResponse httpResponse = null;
            var timeBegin = DateTime.Now;
            DateTime timeEnd;
            try
            {
                httpResponse = CreateGetHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
                var result = GetTxtByHttpWebResponse(httpResponse);
                timeEnd = DateTime.Now;
                if (GetComplated != null)
                {
                    GetComplated(url, httpResponse.StatusCode.ToString(), result,
                                 Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                return result;
            }
            catch (Exception ex)
            {
                timeEnd = DateTime.Now;
                if (GetComplated != null)
                {
                    GetComplated(url, httpResponse == null ? "" : httpResponse.StatusCode.ToString(), ex.Message, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                throw;
            }
        }

        /// <summary>  
        /// 通过POST方式获取文本 
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="parameterStr"></param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static string Post(string url, IList<ParamKeyValue> parameters, string parameterStr, int? timeout = null, string userAgent = null, Encoding requestEncoding = null, CookieCollection cookies = null, string contentType = null, IList<ParamKeyValue> heards = null)
        {
            HttpWebResponse httpResponse = null;
            var timeBegin = DateTime.Now;
            DateTime timeEnd;
            try
            {
                ServicePointManager.Expect100Continue = false;
                httpResponse = CreatePostHttpResponse(url, parameters, parameterStr, timeout, userAgent, requestEncoding, cookies, contentType, heards);
                var result = GetTxtByHttpWebResponse(httpResponse);

                timeEnd = DateTime.Now;

                if (PostComplated != null)
                {
                    PostComplated(url, ParamList2Str(parameters), httpResponse.StatusCode.ToString(), result, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                return result;
            }
            catch (Exception ex)
            {
                timeEnd = DateTime.Now;
                if (PostComplated != null)
                {
                    PostComplated(url, ParamList2Str(parameters), httpResponse == null ? "" : httpResponse.StatusCode.ToString(), ex.Message, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                throw;
            }
        }


        /// <summary>  
        /// 通过POST方式获取文本 
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="parameterStr"></param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static string SendPost(string url, IList<ParamKeyValue> parameters, string parameterStr, out CookieCollection hwrCookies, int? timeout = null, string userAgent = null, Encoding requestEncoding = null, CookieCollection cookies = null, string contentType = null, IList<ParamKeyValue> heards = null)
        {
            hwrCookies = null;
            HttpWebResponse httpResponse = null;
            var timeBegin = DateTime.Now;
            DateTime timeEnd;
            try
            {
                ServicePointManager.Expect100Continue = false;
                httpResponse = CreatePostResponse(url, parameters, parameterStr, timeout, userAgent, requestEncoding, cookies, contentType, heards);
                var result = GetTxtByHttpWebResponse(httpResponse);

                timeEnd = DateTime.Now;

                if (PostComplated != null)
                {
                    PostComplated(url, ParamList2Str(parameters), httpResponse.StatusCode.ToString(), result, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                hwrCookies = httpResponse.Cookies;
                return result;

            }
            catch (Exception ex)
            {
                timeEnd = DateTime.Now;
                if (PostComplated != null)
                {
                    PostComplated(url, ParamList2Str(parameters), httpResponse == null ? "" : httpResponse.StatusCode.ToString(), ex.Message, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
                }
                throw;
            }
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies, string contentType, IList<ParamKeyValue> heards)
        {
            var request = CreateHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
            request.Method = "GET";
            return request.GetResponse() as HttpWebResponse;
        }

        public static HttpWebRequest CreateHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies, string contentType, IList<ParamKeyValue> heards)
        {

            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.ContentType = !string.IsNullOrWhiteSpace(contentType) ? contentType : "application/x-www-form-urlencoded";

            if (!String.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();
            }
            if (heards != null)
            {
                foreach (var heard in heards)
                {
                    request.Headers.Add(heard.Key, heard.Value);
                }
            }
            return request;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="parameterStr"></param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IList<ParamKeyValue> parameters, string parameterStr, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies, string contentType, IList<ParamKeyValue> heards)
        {
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            var request = CreateHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
            request.Method = "POST";
            //如果需要POST数据  
            var postDataStr = "";
            if (!(parameters == null || parameters.Count == 0))
            {
                postDataStr += ParamList2Str(parameters);
            }
            if (!string.IsNullOrEmpty(parameterStr))
            {
                if (!string.IsNullOrWhiteSpace(postDataStr))
                {
                    postDataStr += "&";
                }
                postDataStr += parameterStr;
            }
            if (!string.IsNullOrWhiteSpace(postDataStr))
            {
                byte[] data = requestEncoding.GetBytes(postDataStr);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }



        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="parameterStr"></param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <param name="contentType"></param>
        /// <param name="heards"></param>
        /// <returns></returns>  
        public static HttpWebResponse CreatePostResponse(string url, IList<ParamKeyValue> parameters, string parameterStr, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies, string contentType, IList<ParamKeyValue> heards)
        {
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            var request = CreateHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            //如果需要POST数据  
            var postDataStr = "";
            if (!(parameters == null || parameters.Count == 0))
            {
                postDataStr += ParamList2Str(parameters, requestEncoding);
            }
            if (!string.IsNullOrEmpty(parameterStr))
            {
                if (!string.IsNullOrWhiteSpace(postDataStr))
                {
                    postDataStr += "&";
                }
                postDataStr += parameterStr;
            }
            if (!string.IsNullOrWhiteSpace(postDataStr))
            {
                byte[] data = requestEncoding.GetBytes(postDataStr);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;
        }


        public static string ParamList2Str(ICollection<ParamKeyValue> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return "";
            }
            var buffer = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (buffer.Length > 0)
                {
                    buffer.AppendFormat("&");
                }
                //buffer.AppendFormat("{0}={1}", parameter.Key, parameter.Value);
                buffer.AppendFormat("{0}={1}", HttpUtility.UrlEncode(parameter.Key), HttpUtility.UrlEncode(parameter.Value));
            }
            return buffer.ToString();
        }

        public static string ParamList2Str(ICollection<ParamKeyValue> parameters, Encoding encoding=null)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return "";
            }
            var buffer = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (buffer.Length > 0)
                {
                    buffer.AppendFormat("&");
                }
                if (encoding != null)
                {
                    buffer.AppendFormat("{0}={1}", HttpUtility.UrlEncode(parameter.Key), HttpUtility.UrlEncode(parameter.Value, encoding));
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", HttpUtility.UrlEncode(parameter.Key), HttpUtility.UrlEncode(parameter.Value));
                }
             
            }
            return buffer.ToString();
        }


        /// <summary>
        /// 通过HTTP请求获取HTML
        /// </summary>
        /// <param name="hwr"></param>
        /// <param name="closeHttpWebResponse">是否关闭HttpWebResponse</param>
        /// <returns></returns>
        public static string GetTxtByHttpWebResponse(HttpWebResponse hwr, bool closeHttpWebResponse = false)
        {
            var stream = hwr.GetResponseStream();
            var reader = new StreamReader(stream, Encoding.GetEncoding(hwr.CharacterSet));

            var result = reader.ReadToEnd();

            reader.Close();
            if (stream != null) stream.Close();
            if (closeHttpWebResponse)
            {
                hwr.Close();
            }

            return result;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        /// <summary>
        /// 参数键值对信息
        /// </summary>
       
    }

    public class ParamKeyValue
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        public ParamKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

}
