using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZP.Campus.Framework.Core.Common;

namespace ZP.Campus.Framework.Core.Mail
{
    public class SendMail : Mail
    {
        private List<string> _emailPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SendMail()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="emailtype"></param>
        public SendMail(List<string> emailPath)
        {
            _emailPath = emailPath;
        }

        /// <summary>
        /// 邮件优先级属性(值:高－a,中－b,低－c)
        /// </summary>
        public string EmailType
        {
            set
            {
                int RndNumber;
                string[] emailPath = _emailPath.ToArray();
                RndNumber = int.Parse(DateTime.Now.ToString("yyyyMMddHHmmss").Right(1)) % (emailPath.GetUpperBound(0) + 1);
                base.Root_path_folder = new string[1] { emailPath[RndNumber] };
            }
        }
    }
}
