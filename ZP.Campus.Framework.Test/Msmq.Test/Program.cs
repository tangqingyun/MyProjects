using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using ZP.Campus.Framework.Core.MSMQ;

namespace Msmq.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建名为myQueueTest消息队列
            string path = @".\private$\myQueueTest";
            //if (!MessageQueue.Exists(path))
            //{
            //    MessageQueue mq = MessageQueue.Create(path);
            //}
            //MessageQueue myQueue = new MessageQueue(path);
            //Message myMessage = new Message();
            //myMessage.Body = "Hello World";
            //myMessage.Formatter = new XmlMessageFormatter();
            //myQueue.Send(myMessage);
            //MessageQueue.Delete(path);

            ZP.Campus.Framework.Core.MSMQ.Msmq msmq = new ZP.Campus.Framework.Core.MSMQ.Msmq(path);
            //msmq.Send<Person>(new Person { Name = "tang", Sex = "1" });

            Person p = msmq.Recive<Person>();

            Console.WriteLine(string.Format("name:{0},sex:{1}",p.Name,p.Sex));
            Console.ReadKey();

        }
    }

    [Serializable]
    public class Person {
        public string Name { set; get; }
        public string Sex { set; get; }
    }

}
