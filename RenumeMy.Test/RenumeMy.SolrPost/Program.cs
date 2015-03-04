using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Basement.Framework.Net;
using Basement.Framework.Serialization;

using Basement.Framework.Excels;
using System.Data;
using Basement.Framework.MSMQ;
using System.Configuration;
using Basement.Framework.Logging;
using Basement.Framework.Logging.LogProvider;
using Basement.Framework.Utility;
using Basement.Framework.Configuration;


namespace RenumeMy.SolrPost
{
    class Program
    {
        static string line = "===================={0}=========================";
        static List<Thread> threadList = new List<Thread>();
        static string QueueName = ConfigurationManager.AppSettings["Msmq"];
        static int ThreadCount = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadCount"]);
        static ILogger loger = LogFactoryProvider.GetLogProvider(EnumLogType.Log4Net, "ErrorLogAdapter_RollingFile");
        static IList<Resume> listResume = new List<Resume>();
        static bool isOpen = false;
        static object lockobj = new object();
        static Msmq msmq;
        static void Main(string[] args)
        {
            try
            {
                //初始化log4net配置
                Log4NetConfig.InitLog4NetConfig();

                msmq = new Msmq(QueueName);

                //第一步读取excel数据导入msmq
                InitMsmqQueue();

                //第二步开启多个线程执行solr导入
                StartThreads();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();

        }
        /// <summary>
        /// 开启线程
        /// </summary>
        public static void StartThreads()
        {
            for (int i = 0; i < ThreadCount; i++)
            {
                Thread th = new Thread(new ThreadStart(AddResumeToSolr));
                th.Name = "线程" + i;
                threadList.Add(th);
            }
            foreach (var thread in threadList)
            {
                thread.Start();
                Console.WriteLine("" + thread.Name + " 已启动");
            }
        }
        /// <summary>
        /// 导入solr
        /// </summary>
        public static void AddResumeToSolr()
        {
            while (true)
            {
                Resume model = null;
                try
                {

                    string json = string.Empty;
                    model = msmq.Recive<Resume>();
                    List<Resume> list = new List<Resume>();
                    if (model != null)
                        list.Add(model);
                    if (list.Count > 0)
                        json = JsonSerializer.ConvertToJosnString(list);

                    //向api发送请求导入solr
                    PostToIAPI(json, model);

                }
                catch (Exception ex)
                {
                    string str = string.Empty;
                    if (model != null)
                        str = ex.Message + "异常[" + model.UserMasterId + "|" + model.RootCompanyId + "]";
                    else
                        str = ex.Message + "";
                    Console.WriteLine(str);
                    loger.Log(EnumLogLevel.Info, str);
                }

                lock (lockobj)
                {
                    long mcount = 0;
                    try
                    {
                        mcount = msmq.GetQueueAllMessages();
                    }
                    catch (Exception){ }
                    if (mcount == 0)
                    {
                        Console.WriteLine("消息队列无数据 " + Thread.CurrentThread.Name + " 终止 ");
                        threadList.Remove(Thread.CurrentThread);
                        if (threadList.Count == 0 && isOpen == false)//如果线程数小于1时开启新线程执行 汇总操作
                        {
                            Thread th = new Thread(new ThreadStart(FindSolrResume));
                            th.Start();
                        }
                        Thread.CurrentThread.Abort();
                    }
                }

            }

        }
        /// <summary>
        /// 将excel数据添加到消息队列中
        /// </summary>
        public static void InitMsmqQueue()
        {
            string wls = string.Format(line, "第一步将数据导入消息队列");
            Console.WriteLine(wls);
            loger.Log(EnumLogLevel.Info, wls);
            string msg = string.Empty;
            Resume model = null;
            try
            {
                listResume = new ExcelRd().GetResumeList();
                foreach (var itm in listResume)
                {
                    model = itm;
                    msmq.Send<Resume>(itm);
                    Console.WriteLine(itm.UserMasterId + " == " + itm.RootCompanyId + "成功");
                }
            }
            catch (Exception ex)
            {
                msg = model.UserMasterId + " == " + model.RootCompanyId + "导入队列异常：" + ex.Message;
                loger.Log(EnumLogLevel.Info, msg);
            }
            msg = "导入队列消息：" + msmq.GetQueueAllMessages();
            string lgs = string.Format(line, "第二步导入solr");
            loger.Log(EnumLogLevel.Info, msg);
            loger.Log(EnumLogLevel.Info, lgs);
            Console.WriteLine(msg);
            Console.WriteLine(lgs);
        }
        /// <summary>
        /// 查询Solr简历数据
        /// </summary>
        public static void FindSolrResume()
        {
            string lgr = string.Format(line, "第三步查询数据是否存在");
            Console.WriteLine(lgr);
            loger.Log(EnumLogLevel.Info, lgr);
            isOpen = true;
            var list = new ExcelRd().GetResumeList();
            int total = 0;
            int ytotal = 0;
            int i = 0;
            foreach (var itm in listResume)
            {
                string result = HttpWeb.Get(string.Format(ConfigurationManager.AppSettings["SolrAdr"] + "?q={0}&wt=json&indent=true&_=" + DateTimeTool.GetTimeStamp(DateTime.Now), itm.UserMasterId));
                if (!result.Contains(string.Format("\"root_company_id\":{0},", itm.RootCompanyId)))
                {
                    string msg = string.Format("{0} == {1}匹配失败", itm.UserMasterId, itm.RootCompanyId);
                    Console.WriteLine(msg);
                    loger.Log(EnumLogLevel.Info, msg + "");
                    total++;
                }
                else
                {
                    ytotal++;
                }
                i++;
            }
            string str = "\r\n统计：总条数" + list.Count() + " 失败" + total + " 成功" + ytotal + "";
            loger.Log(EnumLogLevel.Info, str);
            Console.WriteLine(str);
        }
        /// <summary>
        /// 向api发送请求导入solr
        /// </summary>
        /// <param name="json"></param>
        /// <param name="model"></param>
        public static void PostToIAPI(string json, Resume model)
        {
            string result = string.Empty;
            Dictionary<string, object> dd = null;
            if (!string.IsNullOrWhiteSpace(json) && model != null)
            {
                var url = ConfigurationManager.AppSettings["CampusrdiApi"];
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("_appid", "rdiapi");
                dic.Add("list", json);
                result = HttpUtility.SendPost(url, dic, Encoding.UTF8);
                dd = JsonSerializer.JsonToDictionary(result);
                if (dd["message"].ToString() != "成功")
                {
                    loger.Log(EnumLogLevel.Info, "失败[" + model.UserMasterId + "|" + model.RootCompanyId + "]" + dd["message"]);
                }
                Console.WriteLine(model.UserMasterId + " == " + model.RootCompanyId + dd["message"]);
            }
        }

    }

    [Serializable]
    public class Resume
    {
        //[{"UserMasterId":"103020602","RootCompanyId":"10584"}]
        public long UserMasterId { set; get; }
        public long RootCompanyId { set; get; }
    }

    public class SolrDocument
    {
        public long post_id { set; get; }
        public long campus_id { set; get; }
        public int language_id { set; get; }
        public long root_company_id { set; get; }
        public long positionid { set; get; }
        public string GetXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<add>");
            sb.Append("<doc>");
            sb.AppendFormat("<field name=\"post_id\">{0}</field>", post_id);
            sb.AppendFormat("<field name=\"campus_id\">{0}</field>", campus_id);
            sb.AppendFormat("<field name=\"language_id\">{0}</field>", language_id);
            sb.AppendFormat("<field name=\"root_company_id\">{0}</field>", root_company_id);
            sb.AppendFormat("<field name=\"positionid\">{0}</field>", positionid);
            sb.Append("</doc>");
            sb.Append("</add>");
            return sb.ToString();
        }
    }
}
