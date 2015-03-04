using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ObjectExtension.Test
{
    public class BinarySerializer<T> : ISerialize<T>
    {
        public BinarySerializer()
        {

        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="resume"></param>
        /// <returns></returns>
        public virtual string Serialize(T source)
        {
            //序列化对象
            using (MemoryStream stream = new MemoryStream()) //创建内存流对象
            {
                BinaryFormatter bf = new BinaryFormatter(); //创建二进制格式化对象
                bf.Serialize(stream, source); //序列化类结构信息到内存流中
                Byte[] content = stream.ToArray();//转化流信息为字节数组
                string result = Convert.ToBase64String(content);//转化字节数组信息为base64字符串

                return result;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual T Deserialize(string result)
        {
            //将base64字符串转化为字节数组
            byte[] buffer = Convert.FromBase64String(result);
            using (MemoryStream ms = new MemoryStream(buffer))//创建内存流对象
            {
                BinaryFormatter bf = new BinaryFormatter();//创建二进制格式对象
                T obj = (T)bf.Deserialize(ms);//反序列化数据
                return obj;
            }
        }
    }
}
