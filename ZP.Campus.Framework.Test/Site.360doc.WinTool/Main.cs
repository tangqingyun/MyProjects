using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZP.Campus.Framework.Core.Web;

namespace Site._360doc.WinTool
{
    public partial class Main : Form
    {
        public static CookieCollection cookies1 = null;
        public static CookieCollection loginCookies = null;
        public Main()
        {
            InitializeComponent();
        }

        public static string Uname
        {
            get
            {
                return ConfigurationManager.AppSettings["uname"];
            }
        }

        public static string Upwd
        {
            get
            {
                return ConfigurationManager.AppSettings["upwd"];
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            int pageNum = Convert.ToInt32(TbPageNum.Text);
            string toUrl = TbUrl.Text;

            string tbcontent = TbContent.Text;
            //第一步登陆站点
            bool bol = Login360doc(out loginCookies);
            if (!bol)
            {
                return;
            }


            if (!string.IsNullOrWhiteSpace(tbArtID.Text))
            {
                string result = UpdateArt(tbArtID.Text, toUrl);
                TbContent.Text = tbcontent + result;
                return;
            }

            for (int i = 0; i < pageNum; i++)
            {
                MatchCollection matchs = GetMyCategoryArtPager(i + 1);
                if (matchs.Count == 0)
                    return;
                foreach (Match itm in matchs)
                {
                    string articleid = GetArtID(itm.Value);
                    string result = UpdateArt(articleid, toUrl);
                    TbContent.Text = tbcontent + result;
                }
            }


        }

        /// <summary>
        /// 登陆360doc站点
        /// </summary>
        /// <returns></returns>
        public static bool Login360doc(out CookieCollection cookies)
        {
            string loginUrl = string.Format("http://www.360doc.com/userLogin.aspx?email={0}&pws={1}&isr=0&login=1", Uname, Upwd);
            string loginResult = HttpUtils.SendPost(loginUrl, null, "tqyitweb", out cookies, null, null, Encoding.UTF8, null, null, null);
            return true;
        }

        /// <summary>
        /// 获取我的馆藏收藏的文章
        /// </summary>
        /// <returns></returns>
        public static MatchCollection GetMyCategoryArtPager(int curnum = 1)
        {
            //第二步获取我的馆藏收藏的文章
            string getmycategoryArt = string.Format("http://www.360doc.com/ajax/getmycategoryArt.ashx?pagenum=10&curnum={0}&icid=-1&ishowabstract=1&word=&_=1413515984360", curnum);
            HttpUtils.Referer = "http://www.360doc.com/myfiles.aspx?app=1&type=3";
            string mycategoryArt = HttpUtils.Get(getmycategoryArt, null, null, loginCookies, null, null);
            string pattern = string.Format("<div class=\"list listwz1 font14\">{0}</div>", @"[\s\S]*?");
            MatchCollection matchs = Regex.Matches(mycategoryArt, pattern);
            return matchs;
        }

        /// <summary>
        /// 获取文章ID
        /// </summary>
        /// <param name="alink"></param>
        /// <returns></returns>
        public static string GetArtID(string alink)
        {
            Match match = Regex.Match(alink, string.Format("<a href=\"{0}\"", @"[\s\S]*?"));
            //<a href="http://www.360doc.com/content/13/0727/16/3635381_302841579.shtml"
            string url = match.Value.Replace("\"", "").Replace("<a href=", "");
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            int a1 = url.LastIndexOf('/');
            int a2 = url.LastIndexOf('.');
            int len = a2 - a1;
            string str = url.Substring(a1 + 1, len - 1);
            string[] arrs = str.Split('_');
            string articleid = "";
            if (arrs.Length > 1)
            {
                articleid = str.Split('_')[1];
            }
            else
            {
                articleid = str.Split('_')[0];
            }
            //http://www.360doc.com/editartnew.aspx?articleid=302841579
            //bpu http://www.360doc.com/editartnew.aspx?articleid=302841579
            //进入文章编辑页面
            string editartnew = "http://www.360doc.com/editartnew.aspx?articleid=" + articleid;
            return articleid;
        }

        /// <summary>
        /// 获取要提交的表单及内容
        /// </summary>
        /// <param name="articleid"></param>
        /// <returns></returns>
        public static IList<ParamKeyValue> GetPostFormDatas(string articleid, string tourl = "")
        {
            string editartnew = "http://www.360doc.com/editartnew.aspx?articleid=" + articleid;
            string newsContent = HttpUtils.Get(editartnew, null, null, loginCookies, null, null);
            MatchCollection matchs1 = Regex.Matches(newsContent, string.Format("<input {0}/>", @"[\s\S]*?"));
            MatchCollection matchs2 = Regex.Matches(newsContent, string.Format("<textarea{0}</textarea>", @"[\s\S]*?"));

            IList<ParamKeyValue> paramlist = new List<ParamKeyValue>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (Match m in matchs1)
            {
                Match matchsName = Regex.Match(m.Value, string.Format("name=\"{0}\"", @"[\s\S]*?"));
                Match matchsValue = Regex.Match(m.Value, string.Format("value=\"{0}\"", @"[\s\S]*?"));
                string inputName = matchsName.Value.Replace("\"", "").Replace("name=", "");
                string inputValue = matchsValue.Value.Replace("\"", "").Replace("value=", "");
                if (inputName == "hidtitle")
                {
                    inputValue = inputValue + "";
                }

                if (!string.IsNullOrWhiteSpace(inputName))
                {
                    if (inputName != "Button1" && inputName != "Button4")
                    {
                        paramlist.Add(new ParamKeyValue(inputName, inputValue));
                        dict.Add(inputName, inputValue);
                    }
                }
            }

            foreach (Match m in matchs2)
            {
                Match matchName = Regex.Match(m.Value, string.Format("name=\"{0}\"", @"[\s\S]*?"));
                Match matchTextarea = Regex.Match(m.Value, string.Format("<textarea{0}>", @"[\s\S]*?"));
                string textarea = matchTextarea.Value;
                string name = matchName.Value.Replace("\"", "").Replace("name=", "");
                string value = m.Value.Replace(textarea, "").Replace("</textarea>", "");
                if (name == "txtAbstract")
                {
                    value = "添加摘要";
                }

                if (name == "content")
                {
                    value = HtmlHelper.HtmlDecode(value);
                }
                if (!string.IsNullOrWhiteSpace(name))
                {
                    dict.Add(name, value);
                    paramlist.Add(new ParamKeyValue(name, value));
                }

            }
            var HiddenResave = paramlist.Where(m => m.Key == "HiddenResave").FirstOrDefault();
            var Content = paramlist.Where(m => m.Key == "content").FirstOrDefault();
            if (HiddenResave != null)
            {
                HiddenResave.Value = Content.Value;
            }

            #region 修改文章正文章内容
            string tohref = "<a href=\"{0}\" target=\"_blank\" flag=\"####%\">";
            if (Content != null)
            {
                Match flag = Regex.Match(Content.Value.ToLower(), string.Format(tohref, @"[\s\S]*?"));
                if (flag != null && !string.IsNullOrWhiteSpace(flag.Value))
                {
                    int flen = flag.Value.Length;
                    string substr = Content.Value.Substring(flen);
                    Content.Value = string.Format(tohref, tourl) + substr;
                }
                else
                {
                    Content.Value = string.Format("<a href=\"{0}\" target=\"_blank\" flag=\"####%\">{1}</a>", tourl, Content.Value);
                }
            }
            paramlist.Add(new ParamKeyValue("Button4.x", "0"));
            paramlist.Add(new ParamKeyValue("Button4.y", "0"));
            #endregion

            return paramlist;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="articleid"></param>
        /// <param name="toUrl"></param>
        /// <returns></returns>
        public static string UpdateArt(string articleid, string toUrl)
        {
            IList<ParamKeyValue> paramlist = GetPostFormDatas(articleid, toUrl);
            string editartnew = "http://www.360doc.com/editartnew.aspx?articleid=" + articleid;
            bool isSuccess = false;
            //保存文章内容
            HttpUtils.Referer = editartnew;
            string result = HttpUtils.SendPost(editartnew, paramlist, null, out cookies1, null, null, Encoding.UTF8, loginCookies, null, null);
            var titleMatch = Regex.Match(result, string.Format("<title>{0}</title>", @"[\s\S]*?"));
            if (titleMatch != null)
            {
                if (titleMatch.Value.Contains("修改文章"))
                    isSuccess = false;
                else
                    isSuccess = true;
            }
            return string.Format("文章：{0}更新{1}！\r\n", articleid, isSuccess ? "成功" : "失败");

        }
    }
}
