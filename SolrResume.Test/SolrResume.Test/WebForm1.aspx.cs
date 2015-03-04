using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;



namespace SolrResume.Test
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var s = new Zhaopin.Campus.IAPI.Test.SolrDocumentTest();
           // string path = @"D:\postForTest.xml";
           // SolrDocument doc = GetSolrDocumentFromXml(path);
           // string xmlStr=doc.GetXml();
        }

        //public void GetSolrDocumentFromXml(string path)
        //{
        //    path = @"D:\postForTest.xml";
        //    SolrDocument solrDoc = new SolrDocument("Resume");
        //    var xmlDoc = new XmlDocument();
        //    xmlDoc.Load(path);
        //    XmlNode root = xmlDoc.SelectSingleNode("doc");
        //    foreach (XmlNode node in root.ChildNodes)
        //    {
        //        var name = node.Attributes["name"].Value;
        //        var value = node.InnerText;
        //        solrDoc.Add(name, value);
        //    }
        //    solrDoc.GetXml();
        //}

    }
}