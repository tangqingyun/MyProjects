using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZP.Campus.Framework.Core.Xml
{
    /// <summary>
    /// XML转换器（序列化/反序列化）
    /// </summary>
    /// <typeparam name="T">需要转换的类型</typeparam>
    public class XmlConverter<T>
    {
        /// <summary>
        /// 构造XML转换器（使用UTF8编码进行序列化操作，无BOM）
        /// </summary>
        public XmlConverter()
        {
        }

        /// <summary>
        /// 构造XML转换器（使用指定编码进行序列化操作）
        /// </summary>
        /// <param name="serializerEncoding">序列化编码</param>
        public XmlConverter(Encoding serializerEncoding)
        {
            _SerializerEncoding = serializerEncoding;
        }

        private Encoding _SerializerEncoding;
        Encoding SerializerEncoding
        {
            get
            {
                if (_SerializerEncoding == null)
                {
                    _SerializerEncoding = new UTF8Encoding(false);
                }
                return _SerializerEncoding;
            }
        }

        private XmlSerializer _Serializer;
        XmlSerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                {
                    _Serializer = new XmlSerializer(typeof(T));
                }
                return _Serializer;
            }
        }

        #region Load

        public List<T> LoadListFromFile(string path)
        {
            XmlConverter<T> xmlConverter = new XmlConverter<T>(Encoding.UTF8);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //XmlNodeList nodeList = doc.ChildNodes;
            //List<T> list = new List<T>();
            //foreach (XmlNode node in nodeList)
            //{
            //    list.Add(LoadFromString(node.OuterXml));

            //}



            return new List<T>();
        }

        /// <summary>
        /// 从XML文本加载对象
        /// </summary>
        /// <param name="xmlString">XML内容</param>
        public T LoadFromString(string xmlString)
        {
            using (MemoryStream ms = new MemoryStream(SerializerEncoding.GetBytes(xmlString)))
            {
                return (T)Serializer.Deserialize(ms);
            }
        }

        /// <summary>
        /// 从文件流加载对象
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns>目标对象</returns>
        public T LoadFromStream(FileStream fileStream)
        {
            return LoadFromStream((Stream)fileStream);
        }

        /// <summary>
        /// 从流加载对象
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>目标对象</returns>
        public T LoadFromStream(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return (T)Serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 从文件加载对象
        /// </summary>
        /// <param name="filename">XML文件名</param>
        public T LoadFromFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return LoadFromStream(fs);
            }
        }
        #endregion

        #region Save / Get Content
        /// <summary>
        /// 获取XML命名空间
        /// </summary>
        /// <param name="source">对象</param>
        /// <returns>命名空间</returns>
        private XmlSerializerNamespaces GetXmlSerializerNamespaces(T source)
        {
            Dictionary<string, string> namespaceAlias = null;
            if (source is IMultiNamespaceObject)
            {
                namespaceAlias = (source as IMultiNamespaceObject).NamespaceAlias;
            }

            XmlSerializerNamespaces ns = null;
            if (namespaceAlias != null)
            {
                ns = new XmlSerializerNamespaces();

                foreach (KeyValuePair<string, string> kvp in namespaceAlias)
                {
                    ns.Add(kvp.Key, kvp.Value);
                }
            }

            return ns;
        }

        /// <summary>
        /// 将对象保存至文件
        /// </summary>
        /// <param name="source">对象</param>
        /// <param name="filename">XML文件名</param>
        public void SaveToFile(T source, string filename)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(filename, false, SerializerEncoding))
            {
                Serializer.Serialize(writer, source, GetXmlSerializerNamespaces(source));

            }
        }

        /// <summary>
        /// 获取XML内容
        /// </summary>
        /// <param name="source">对象</param>
        /// <returns>XML内容</returns>
        public string GetString(T source)
        {
            using (Stream stream = GetStream(source))
            {
                string constructedString = SerializerEncoding.GetString(((MemoryStream)stream).ToArray());
                return constructedString;
            }
        }

        /// <summary>
        /// 获取对象流
        /// 注意：使用后请关闭流！！！
        /// </summary>
        /// <param name="source">对象</param>
        /// <returns>对象流</returns>
        public Stream GetStream(T source)
        {
            Stream stream = new MemoryStream();

            #region 指定编码的实现
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = SerializerEncoding;
            setting.Indent = false;
            //setting.NamespaceHandling = NamespaceHandling.Default;
            using (XmlWriter writer = XmlWriter.Create(stream, setting))
            {
                // writer.Settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
                Serializer.Serialize(writer, source, GetXmlSerializerNamespaces(source));
            }
            #endregion

            #region 未指定编码的实现
            //Serializer.Serialize(stream, source);
            #endregion

            return stream;
        }
        #endregion


    }
}
