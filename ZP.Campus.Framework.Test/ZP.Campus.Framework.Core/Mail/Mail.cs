using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZP.Campus.Framework.Core.Mail
{
    public class Mail
    {
        //发送人的名字
        public string FromName { get; set; }

        //发送人的EMAIL
        public string FromAddress { get; set; }

        //主题
        public string Subject { get; set; }

        //内容
        public string BodyText { get; set; }

        //收信人姓名
        private string ToName { get; set; }

        //收信人邮箱
        private string ToAddress { get; set; }

        public int Adid { get; set; }

        private string ExpertDate
        {
            get { return DateTime.Now.ToString("yyyyMMdd"); }
        }

        private string Adsite
        {
            get { return "edm_" + DateTime.Now.ToString("yyyyMMdd"); }
        }

        private string ContentType
        {
            get { return "text/html; charset=\"gb2312\""; }
        }


        //从配置文件中读取的邮件信体存放目录，如：D:\disk\m\mail_a
        private string[] _root_path_fold;
        protected string[] Root_path_folder
        {
            get
            {
                //#if DEBUG
                //List<string> al=new List<string>();
                //al.Add(@"d:\Campus2012\m\mail_a1");
                //return al.ToArray();
                //#endif//DEBUG

                return this._root_path_fold;

            }
            set
            {
                this._root_path_fold = value;
                //检查路径是否存在，不存在则创建
                for (int i = 0; i <= this._root_path_fold.GetUpperBound(0); i++)
                {
                    if (!string.IsNullOrEmpty(this._root_path_fold[i]) && !Directory.Exists(this._root_path_fold[i]))
                    {
                        Directory.CreateDirectory(this._root_path_fold[i]);
                    }
                }
            }
        }

        //是否需要将内容作为附件(是y/否不填)
        public string is_Attachment { get; set; }

        //发件人邮件地址信息
        public string Replyto { get; set; }

        //发件人邮件地址信息 
        public string ReplytoName { get; set; }

        //附件
        public string AttachFile { get; set; }

        private string[] ArrGetGuidPath { get; set; }

        private string CustomerHeader { get; set; }

        private string StrAttachFile { get; set; }

        /// <summary>
        /// 添加收件人
        /// </summary>
        /// <param name="name">收件人姓名</param>
        /// <param name="address">收件人EMAIL地址</param>
        public void AddRecipient(string name, string address)
        {
            if (string.IsNullOrEmpty(name)) { name = address; }

            this.ToName += "__4@4__" + name;
            this.ToAddress += "__4@4__" + address;
        }

        /// <summary>
        /// 添加邮件头
        /// </summary>
        /// <param name="headername">邮件头名称</param>
        /// <param name="headervalue">邮件头名称内容</param>
        /// <remarks>邮件头格式：邮件头1:邮件头名称内容1__4@4__邮件头2:邮件头名称内容2</remarks>
        public void AddCustomerHeader(string headername, string headervalue)
        {
            if (string.IsNullOrEmpty(this.CustomerHeader))
            {
                this.CustomerHeader = headername + ":" + headervalue;
            }
            else
            {
                this.CustomerHeader += "__4@4__" + headername + ":" + headervalue;
            }
        }

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="filepath">附件路径</param>
        /// <remarks>格式：附件路径1_4@4_附件路径2</remarks>
        public void AddAttachMent(string filepath)
        {
            if (string.IsNullOrEmpty(this.StrAttachFile))
            {
                this.StrAttachFile = filepath;
            }
            else
            {
                this.StrAttachFile += "_4@4_" + filepath;
            }
        }

        /// <summary>
        /// 取guid目录
        /// </summary>
        private void GetGuidPath()
        {
            string fileTime = DateTime.Now.ToString("yyyyMMddhhmmss");

            this.ArrGetGuidPath = new string[this.Root_path_folder.GetUpperBound(0) + 1];

            for (int i = 0; i <= this.Root_path_folder.GetUpperBound(0); i++)
            {
                this.ArrGetGuidPath[i] = this.Root_path_folder[i] + @"\" + fileTime + "_{" + System.Guid.NewGuid().ToString().ToUpper() + "}";
            }
        }

        /// <summary>
        /// 发送EMAIL
        /// </summary>
        /// <remarks>如果产生路径访问错误之类的问题，由上层调用捕错判断。</remarks>
        public void Send()
        {
            int i;
            string strResult;

            strResult = "";
            strResult += "FromAddress:" + this.FromAddress + "\r\n";
            strResult += "FromName:" + FromName + "\r\n";
            strResult += "ToName:" + this.ToName.Substring(7, this.ToName.Length - 7) + "\r\n";
            strResult += "ToAddress:" + this.ToAddress.Substring(7, this.ToAddress.Length - 7) + "\r\n";
            strResult += "Subject:" + this.Subject + "\r\n";
            strResult += "Attachment:" + this.is_Attachment + "\r\n";
            //为了监控添加的代码
            strResult += "expertDate:" + this.ExpertDate + "\r\n";
            strResult += "adid:" + this.Adid + "\r\n";
            strResult += "adsite:" + this.Adsite + "\r\n";
            strResult += "Content-Type:" + this.ContentType + "\r\n";

            if (string.IsNullOrEmpty(this.Replyto)) { this.Replyto = this.FromAddress; }
            strResult += "Replyto:" + this.Replyto + "\r\n";

            if (string.IsNullOrEmpty(this.ReplytoName)) { this.ReplytoName = this.Replyto; }
            strResult += "ReplytoName:" + this.ReplytoName + "\r\n";

            if (!string.IsNullOrEmpty(this.StrAttachFile))
            {
                if (!string.IsNullOrEmpty(this.AttachFile))
                {
                    strResult += "AttachFile:" + this.AttachFile + "_4@4_" + this.StrAttachFile + "\r\n";
                }
                else
                {
                    strResult += "AttachFile:" + this.StrAttachFile + "\r\n";
                }
            }
            else
            {
                strResult += "AttachFile:" + this.AttachFile + "\r\n";
            }

            strResult += "CustomerHeader:" + this.CustomerHeader + "\r\n";
            strResult += "\r\n";

            strResult += "\r\n";
            strResult += this.BodyText;

            GetGuidPath();
            string file;
            for (i = 0; i <= this.ArrGetGuidPath.GetUpperBound(0); i++)
            {
                file = this.ArrGetGuidPath[i] + ".txt";
                if (!System.IO.File.Exists(file))
                {
                    StreamWriter sw = System.IO.File.CreateText(file);
                    sw.Close();
                    sw.Dispose();
                }

                using (FileStream fs = System.IO.File.Open(file, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(Encoding.GetEncoding("gb2312").GetBytes(strResult), 0, Encoding.GetEncoding("gb2312").GetByteCount(strResult));
                }
            }

        }
    }
}
