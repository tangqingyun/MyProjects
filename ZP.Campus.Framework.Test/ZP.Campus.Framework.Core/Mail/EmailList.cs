using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.Mail
{
    public class EmailList
    {
        public Dictionary<string, string> Dic;
        public string _strMailUrl;

        public EmailList()
        {
            Dic = new Dictionary<string, string>();
            Dic.Add("163.com", "http://mail.163.com");
            Dic.Add("126.com", "http://www.126.com");
            Dic.Add("qq.com", "http://mail.qq.com");
            Dic.Add("google.com", "http://mail.google.com");
            Dic.Add("yahoo.cn", "http://mail.cn.yahoo.com");
            Dic.Add("yahoo.com.cn", "http://mail.cn.yahoo.com");
            Dic.Add("hotmai.com", "http://www.hotmail.com");
            Dic.Add("sina.com", "http://mail.sina.com.cn");
            Dic.Add("sina.cn", "http://mail.sina.com.cn");
            Dic.Add("2008.sina.com", "http://mail.sina.com.cn");
            Dic.Add("139.com", "http://mail.139.com");
            Dic.Add("tom.com", "http://mail.tom.com");
            Dic.Add("vip.tom.com", "http://mail.tom.com");
            Dic.Add("163.net", "http://mail.tom.com");
            Dic.Add("21cn.com", "http://mail.21cn.com");
            Dic.Add("sogou.com", "http://mail.sogou.com");
            Dic.Add("189.cn", "http://webmail3.189.cn/webmail");
            Dic.Add("eyou.com", "http://www.eyou.com");
            Dic.Add("yeah.net", "http://www.yeah.net");
            Dic.Add("sohu.com", "http://mail.sohu.com");
            Dic.Add("263.net", "http://mail.263.net");
            Dic.Add("263.net.cn", "http://mail.263.net");
        }

        public void CheckMailList(string value)
        {
            if (Dic.Keys.Contains(value))
            {
                _strMailUrl = Dic[value];
            }
            else
            {
                _strMailUrl = "http://" + value;
            }
        }
    }
}
