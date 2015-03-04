using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ZP.Campus.Framework.Core.Xml
{
    public static class XmlSerializer<T>
    {
        public static T DeserializeFromFile(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                T t = Deserialize(stream);
                stream.Close();
                return t;
            }
        }

        public static T Deserialize(Stream stream)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(stream);
        }

        public static T Deserialize(TextReader textReader)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(textReader);
        }

        public static T Deserialize(XmlReader xmlReader)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(xmlReader);
        }

        public static T Deserialize(XmlReader xmlReader, string encodingStyle)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(xmlReader, encodingStyle);
        }


        public static void SerializeToFile(string fileName, T t)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                Serialize(stream, t);
                stream.Close();
            }
        }

        public static void Serialize(Stream stream, T t)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));
            xser.Serialize(stream, t);
        }

        public static void Serialize(TextWriter writer, T t)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));
            xser.Serialize(writer, t);
        }


        public static void Serialize(XmlWriter writer, T t)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));
            xser.Serialize(writer, t);
        }

        public static string ToString(T t)
        {
            using (StringWriter writer = new StringWriter())
            {
                Serialize(writer, t);
                return writer.ToString();
            }
        }

        public static T FromString(string str)
        {
            using (StringReader reader = new StringReader(str))
            {
                return Deserialize(reader);
            }
        }
    }
}
