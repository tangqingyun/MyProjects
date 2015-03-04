using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

namespace XmlToXsltTest
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string xsltPath = Server.MapPath("/11819.xslt");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("/11819.xml"));

            XsltArgumentList xslArgList = new XsltArgumentList();
            xslArgList.AddParam("param1", "","参数1");
            xslArgList.AddParam("param2", "", "参数2");
            string htmlContent = this.Trans(xmlDoc.OuterXml, xsltPath, xslArgList);
            Response.Write(htmlContent);
        }

        private string Trans(string xml, string xslPath, XsltArgumentList arg)
        {
            XmlDocument objXml = new XmlDocument();
            objXml.LoadXml(xml);

            XslCompiledTransform objXSL = new XslCompiledTransform();
            XsltSettings xsltSet = new XsltSettings();
            xsltSet.EnableScript = true;
            objXSL.Load(xslPath, xsltSet, new XmlUrlResolver());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = new UTF8Encoding(false);
            settings.ConformanceLevel = ConformanceLevel.Auto;

            StringBuilder str = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(str, settings);
            objXSL.Transform(objXml, arg, writer);
            return str.ToString();
        }


    }
}