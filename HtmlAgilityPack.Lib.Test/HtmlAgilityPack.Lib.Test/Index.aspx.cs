using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HtmlAgilityPack.Lib.Test
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDoc = htmlWeb.Load("http://localhost:6673/Test.aspx");
            HtmlDocument htmlDoc1 = htmlWeb.Load("http://mil.news.baidu.com/");
            HtmlNodeCollection coll = htmlDoc1.DocumentNode.SelectNodes("//div[@id='instant-news']");
            Response.Write(coll[0].OuterHtml);

        }
    }
}