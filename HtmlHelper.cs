using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace uni2uni.com.Framework.Common
{
    public delegate string StringProcessEvaluator(string text);

    public class HtmlHelper
    {
        #region = Html =
        private string _html = string.Empty;
        public string Html
        {
            get { return _html; }
            set { _html = value; }
        }
        #endregion

        #region = Instance =
        /// <summary>
        /// Instances the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static HtmlHelper Instance(string html)
        {
            return new HtmlHelper(html);
        }
        #endregion

        #region = Constructor =
        public HtmlHelper(string html)
        {
            this.Html = html;
        }
        #endregion

        #region = ReplaceText =
        /// <summary>
        /// 替换Html中的文本,而忽略html标签
        /// </summary>
        /// <remarks>
        /// 将html标签装入队列
        /// 调用客户端的委托
        /// 将队伍写回
        /// </remarks>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns></returns>
        public static string ReplaceText(string html, StringProcessEvaluator evaluator)
        {
            //替换html标签为标识
            html = html.Replace("@", "&#64;");
            const string strre = @"\<.+?\>";

            MatchCollection mc = Regex.Matches(html, strre);

            StringBuilder strHtml = new StringBuilder();

            int start = 0; //文本开头
            int end = 0; //文本结尾

            Queue<string> HtmlTag = new Queue<string>(); //html标签
            foreach (Match m in mc)
            {
                end = m.Index - start;
                strHtml.Append(html.Substring(start, end)); //加入文本

                HtmlTag.Enqueue(m.Value); //加入队列中

                strHtml.Append(CharHelper.Replicate("@", m.Length));

                start = m.Index + m.Length;
            }

            strHtml.Append(html.Substring(start));

            string replacehtml = strHtml.ToString();

            if (evaluator != null)
            {
                replacehtml = evaluator(replacehtml);
            }

            //分解
            mc = Regex.Matches(replacehtml, "@{2,}");
            start = 0; //html标签开头
            end = 0; //html标签结尾

            strHtml = new StringBuilder();
            foreach (Match m in mc)
            {
                end = m.Index - start;

                strHtml.Append(replacehtml.Substring(start, end)); //加入文本

                string s = HtmlTag.Dequeue();
                while (s.Length < m.Length) //两个标签在一起时,累加
                {
                    s += HtmlTag.Dequeue();
                }
                strHtml.Append(s);

                start = m.Index + m.Length;
            }

            strHtml.Append(replacehtml.Substring(start));

            return strHtml.ToString();
        }
        public string ReplaceText(StringProcessEvaluator evaluator)
        {
            return ReplaceText(this.Html, evaluator);
        }

        public static string ReplaceHtmlCode(string strText)
        {
            strText = Regex.Replace(strText, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            return strText;
        }
        #endregion

        #region = RemoveTag =
        /// <summary>
        /// Removes the tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static string RemoveTag(string html, string tag)
        {
            string returnStr;
            string regexstr1 = string.Format(@"<{0}([^>])*>", tag);
            string regexstr2 = string.Format(@"</{0}([^>])*>", tag);
            returnStr = Regex.Replace(html, regexstr1, string.Empty, RegexOptions.IgnoreCase);
            returnStr = Regex.Replace(returnStr, regexstr2, string.Empty, RegexOptions.IgnoreCase);
            return returnStr;
        }

        public string RemoveTag(string tag)
        {
            return RemoveTag(this.Html, tag);
        }

        public static string RemoveTag(string html, string[] tags)
        {
            string content = html;
            string regexstr1, regexstr2;
            foreach (string tag in tags)
            {
                if (string.IsNullOrEmpty(tag) == false)
                {
                    regexstr1 = string.Format(@"<{0}([^>])*>", tag);
                    regexstr2 = string.Format(@"</{0}([^>])*>", tag);
                    content = Regex.Replace(content, regexstr1, string.Empty, RegexOptions.IgnoreCase);
                    content = Regex.Replace(content, regexstr2, string.Empty, RegexOptions.IgnoreCase);
                }
            }
            return content;
        }

        public string RemoveTag(string[] tags)
        {
            return RemoveTag(this.Html, tags);
        }
        #endregion

        #region = Encode =
        /// <summary>
        /// 将特殊字元转成 HTML 格式。
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  HtmlEncode(string)
        /// 本函式将特殊字元转成 HTML 的字串格式 ( &....; )。最常用到的场合可能就是处理客户留言的留言版了。
        /// & (和) 转成 &amp; 
        /// " (双引号) 转成 &quot; 
        /// < (小于) 转成 &lt; 
        /// > (大于) 转成 &gt; 
        /// 此函式只转换上面的特殊字元，并不会全部转换成 HTML 所定的 ASCII 转换。 
        /// </remarks>
        /// <param name="str">字符串表达式，所有 string 的字元都转成 HTML 的特殊字集字串。如果 string 参数中包含 Null，则返回 ""</param>
        /// <returns>字串</returns>
        public static string Encode(string html)
        {
            if (html == null)
            {
                return null;
            }
            if (html.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder("", html.Length * 2);
            foreach (char ch1 in html)
            {
                if ((((ch1 > '`') && (ch1 < '{')) || ((ch1 > '@') && (ch1 < '['))) || (((ch1 == ' ') || ((ch1 > '/') && (ch1 < ':'))) || (((ch1 == '.') || (ch1 == ',')) || ((ch1 == '-') || (ch1 == '_')))))
                {
                    builder1.Append(ch1);
                }
                else
                {
                    int i = (int)ch1;
                    builder1.Append("&#" + i.ToString() + ";");
                }
            }
            return builder1.ToString();

            //if (string.IsNullOrEmpty(html) == true)
            //{
            //    return string.Empty;
            //}

            //StringBuilder str = new StringBuilder();
            //foreach (char c in html)
            //{
            //    int i = (int)c;
            //    switch (i)
            //    {
            //        case 13:
            //            str.Append("<br/>");
            //            break;
            //        case 10:
            //            break;
            //        case 34:
            //            str.Append("&quot;");
            //            break;
            //        case 32:
            //            str.Append("&nbsp;");
            //            break;
            //        default:
            //            str.AppendFormat("&#{0};", i);
            //            break;
            //    }

            //}

            //return str.ToString();
        }

        public string Encode()
        {
            return Encode(this.Html);
        }
        #endregion

        #region = Decode =
        public static string Decode(string html)
        {
            return Regex.Replace(html, @"&#\d+;", Decode);
        }

        private static string Decode(Match m)
        {
            string s = Regex.Replace(m.Value, @"^&#(\d+);$", "$1");

            int i;

            if (int.TryParse(s, out i) == true)
            {
                return ((char)i).ToString();
            }

            return string.Empty;
        }

        #endregion

        #region = EncodeAttribute =
        public static string EncodeAttribute(string strInput)
        {
            if (strInput == null)
            {
                return null;
            }
            if (strInput.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder("", strInput.Length * 2);
            foreach (char ch1 in strInput)
            {
                if ((((ch1 > '`') && (ch1 < '{')) || ((ch1 > '@') && (ch1 < '['))) || (((ch1 > '/') && (ch1 < ':')) || (((ch1 == '.') || (ch1 == ',')) || ((ch1 == '-') || (ch1 == '_')))))
                {
                    builder1.Append(ch1);
                }
                else
                {
                    builder1.Append("&#" + ch1.ToString() + ";");
                }
            }
            return builder1.ToString();
        }
        #endregion

        #region = Compress =
        /// <summary>
        /// 压缩html代码
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string Compress(string text)
        {
            text = Regex.Replace(text, @"<!--\S*?-->", "");

            text = Regex.Replace(text, @"", "");

            text = Regex.Replace(text, " {2,}", " ");
            text = Regex.Replace(text, @"\s{2,}", "\n");
            text = Regex.Replace(text, @">\s+?<", "><");
            text = Regex.Replace(text, @"^\s+?", string.Empty);

            return text;
        }

        public string Compress()
        {
            return Compress(this.Html);
        }
        #endregion

        #region = 提取HTML控件的指定属性 =
        /// <summary>
        /// Pickups the HTML control attribute.
        /// </summary>
        /// <param name="HTML">The HTML.</param>
        /// <param name="TagName">Name of the tag.</param>
        /// <param name="AttributeName">Name of the attribute.</param>
        /// <param name="RemoteUri">The remote URI.</param>
        /// <returns></returns>
        public static string[] PickupAttribute(string html, string TagName, string AttributeName, Uri RemoteUri, string ExtensionName)
        {
            Regex re;
            if (string.IsNullOrEmpty(ExtensionName) == true)
            {
                re = new Regex("<" + TagName + " .*?" + AttributeName + "=\"(?<" + AttributeName + ">.+?)\".*?/>", RegexOptions.IgnoreCase);
            }
            else
            {
                re = new Regex("<" + TagName + " .*?" + AttributeName + "=\"(?<" + AttributeName + ">.+?\\." + ExtensionName + ")\".*?/>", RegexOptions.IgnoreCase);
            }


            MatchCollection matches = re.Matches(html);

            string[] string_array = new string[matches.Count];

            int i = 0;
            foreach (Match match in matches)
            {
                string word = match.Groups[AttributeName].Value.Replace("\"", "");

                if (word.StartsWith("http") == true)
                {

                }
                else if (word.StartsWith("/") == true) //root 处理
                {
                    if (RemoteUri != null)
                    {
                        word = RemoteUri.ToString().Replace(RemoteUri.AbsolutePath, string.Empty) + word;
                    }
                }
                else
                {
                }

                string_array[i] = word;
                i++;
            }

            return string_array;
        }

        public static string[] PickupAttribute(string html, string TagName, string AttributeName)
        {
            return PickupAttribute(html, TagName, AttributeName, null, null);
        }

        public string[] PickupAttribute(string TagName, string AttributeName)
        {
            return PickupAttribute(TagName, AttributeName, null, null);
        }

        public string[] PickupAttribute(string TagName, string AttributeName, Uri RemoteUri, string ExtensionName)
        {
            return PickupAttribute(this.Html, TagName, AttributeName, RemoteUri, ExtensionName);
        }
        #endregion

        #region = ConvertToText =
        /// <summary>
        /// 转换为文本格式
        /// </summary>
        /// <returns></returns>
        public string ConvertToText()
        {
            return ConvertToText(this.Html);
        }

        public static string ConvertToText(string strHTML)
        {
            strHTML = RemoveTagContent(strHTML, "head");
            strHTML = RemoveTagContent(strHTML, "script");
            strHTML = Regex.Replace(strHTML, @"<br.+>", "\n");
            strHTML = Regex.Replace(strHTML, @"&\s+;", string.Empty);
            //过滤HTML
            strHTML = RemoveHtml(strHTML);

            strHTML = Decode(strHTML);

            return strHTML;
        }
        #endregion

        #region = RemoveTagContent =
        public static string RemoveTagContent(string html, string tagname)
        {
            string regexstr = string.Format(@"(?i)<{0}([^>])*>(\w|\W)*?</{0}([^>])*>", tagname);//@"<script.*</script>";
            return Regex.Replace(html, regexstr, " ", RegexOptions.IgnoreCase);
        }
        #endregion

        #region = RemoveHtml =
        /// <summary>
        /// Removes the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            html = Regex.Replace(html, "<br>", " ");
            html = Regex.Replace(html, @"<(?:\.|\s)*?>", string.Empty);
            html = Regex.Replace(html, @"<[\s|\S]+?>", string.Empty);
            html = Regex.Replace(html, "&nbsp;", " ");
            html = Regex.Replace(html, " {2,}", " ");
            html = Regex.Replace(html, @"[\f\n\r\t\v]", string.Empty);
            return html;
        }

        public string RemoveHtml()
        {
            return RemoveHtml(this.Html);
        }
        #endregion
    }
}
