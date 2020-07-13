using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Core.ConvertHelper
{
   public   class XmlConvert
    {
        #region Object对象序列化成XML文件
        /// <summary>
        /// Object对象序列化成XML文件
        /// </summary>
        /// <param name="o">Object对象</param>
        /// <param name="xmlFileName">xml文件</param>
        public static void ObjectToXMLFile(object o, string xmlFileName)
        {
            Debug.Assert(o != null);
            XmlSerializer ser = new XmlSerializer(o.GetType());
            try
            {
                string s = "";
                XmlTextWriter writer = null;
                try
                {
                    writer = new XmlTextWriter(xmlFileName, Encoding.Default);
                    ser.Serialize(writer, o);
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 对象序列化成 XML字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="isDeclaration">是否申明头</param>
        /// <returns>XML字符串</returns>
        public static string ObjectToXmlString(object o, bool isDeclaration = false)
        {
            Debug.Assert(o != null);
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            try
            {
                XmlWriter writer = null;
                try
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = Encoding.UTF8;
                    if (!isDeclaration)
                    {
                        settings.OmitXmlDeclaration = true;  // 不生成声明头  
                    }

                    StringBuilder builder = new StringBuilder();
                    writer = XmlWriter.Create(builder, settings);
                    serializer.Serialize(writer, o, ns);
                    return builder.ToString();
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// xml文件序列化成对象
        /// </summary>
        /// <param name="xmlData">xml文件</param>
        /// <param name="type">对象类别</param>
        /// <return>对象</return>
        public static T XmlStringToObject<T>(string xmlData)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                return default(T);
            }
            StringReader xmlReader = new StringReader(xmlData);
            try
            {
                var type = typeof(T);
                XmlSerializer xs = new XmlSerializer(type);

                return (T)xs.Deserialize(xmlReader);
            }
            finally
            {
                xmlReader.Close();
            }
        }

        #region xml文件序列化成对象
        /// <summary>
        /// xml文件序列化成对象
        /// </summary>
        /// <param name="xmlFileName">xml文件</param>
        /// <param name="type">对象类别</param>
        /// <return>对象</return>
        public static object XmlFileToObject<T>(string xmlFileName)
        {
            if (!File.Exists(xmlFileName))
            {
                throw new FileNotFoundException(xmlFileName);
            }
            XmlTextReader xmlReader = new XmlTextReader(xmlFileName);
            try
            {
                var type = typeof(T);
                XmlSerializer xs = new XmlSerializer(type);
                if (xs.CanDeserialize(xmlReader))
                {
                    return xs.Deserialize(xmlReader);
                }
            }
            finally
            {
                xmlReader.Close();
            }
            return null;
        }
        #endregion
    }
}
