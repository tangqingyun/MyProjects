using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Easyui.Framework.Test.ashx
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : IHttpHandler
    {
        HttpRequest Request;
        HttpResponse Response;
        public void ProcessRequest(HttpContext context)
        {
            Request = context.Request;
            Response = context.Response;

            //page=2&rows=10
            int pageindex = Convert.ToInt32(Request.Form["page"]);//第几页
            int pagesize = Convert.ToInt32(Request.Form["rows"]);//每页显示条数
            List<Result> list = new List<Result>();
            for (int i = 0; i < 10; i++)
            {
                Result model = new Result();
                model.ID = i;
                model.DepartName = "部门" + i;
                model.SiteName = "网站" + i;
                model.AdminName = "管理员" + i;
                model.Mobile = "15901473139";
                model.Name = "yj";
                list.Add(model);
            }
            var newlist = (from c in list select c).Skip(pageindex).Take(pagesize).ToList();
            JavaScriptSerializer json = new JavaScriptSerializer();
            string content = json.Serialize(new Pager<Result> { total = 100, rows = newlist });
            context.Response.Write(content);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    public class Pager<T>
    {
        public int total { set; get; }
        public List<T> rows { set; get; }

    }

    public class Result
    {
        public int ID { set; get; }
        public string DepartName { set; get; }
        public string SiteName { set; get; }
        public string Name { set; get; }
        public string AdminName { set; get; }
        public string Mobile { set; get; }
    }
}