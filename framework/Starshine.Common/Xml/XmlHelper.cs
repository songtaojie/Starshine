using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hx.Common.Xml
{
    /// <summary>
    /// Xml的帮助类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 向指定的Xml文档的置顶元素中插入属性
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="element">要设置属性的元素</param>
        /// <param name="attributeName">属性的名称</param>
        /// <param name="attributeValue">属性值</param>
        /// <returns>设置的属性元素对象</returns>
        public static XmlAttribute AppendAttribute(XmlDocument xml, XmlElement element, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = xml.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            element.Attributes.Append(attribute);
            return attribute;
        }
        /// <summary>
        /// 向Xml文档中追加节点
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="parentElement">要追加的元素节点的父元素</param>
        /// <param name="elementName">要追加的元素节点的名称</param>
        /// <returns></returns>
        public static XmlElement AppendElement(XmlDocument xml, XmlElement parentElement, string elementName)
        {
            XmlElement element = xml.CreateElement(elementName);
            parentElement.AppendChild(element);
            return element;
        }
        /// <summary>
        /// 向Xml文档中追加元素节点，并设置元素节点的值
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="parentElement">要追加的元素节点的父元素</param>
        /// <param name="elementName">要追加的元素节点的名称</param>
        /// <param name="elementValue">要追加的元素节点中的值</param>
        /// <returns></returns>
        public static XmlElement AppendElement(XmlDocument xml, XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement element = xml.CreateElement(elementName);
            element.InnerText = elementValue;
            parentElement.AppendChild(element);
            return element;
        }
        /// <summary>
        /// 创建一个Xml元素节点
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="elementName">要创建的元素的名称</param>
        /// <returns></returns>
        public static XmlElement CreateElement(XmlDocument xml, string elementName)
        {
            return xml.CreateElement(elementName);
        }
        /// <summary>
        /// 创建一个Xml元素节点，并设置元素节点中的值
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="elementName">要创建的元素的名称</param>
        /// <param name="elementValue">要创建的元素节点中的值</param>
        /// <returns></returns>
        public static XmlElement CreateElement(XmlDocument xml, string elementName, string elementValue)
        {
            XmlElement element = xml.CreateElement(elementName);
            element.InnerText = elementValue;
            return element;
        }
        /// <summary>
        /// 创建一个XMl的声明
        /// </summary>
        /// <param name="xml"></param>
        public static void AppendXmlDeclaration(XmlDocument xml)
        {
            XmlNode xmlNode = xml.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
            xml.AppendChild(xmlNode);
        }
        /// <summary>
        /// 追加到Xml文档的末尾
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static XmlElement AppendMainElement(XmlDocument xml, string elementName)
        {
            XmlElement mainElement = xml.CreateElement(elementName);
            xml.AppendChild(mainElement);
            return mainElement;
        }
        /// <summary>
        /// 把Xml文档转换成字符串
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ToString(XmlDocument xml)
        {
            return XmlHelper.ToString(xml, Formatting.Indented);
        }
        /// <summary>
        /// 使用指定的缩进方式返回Xml的字符串
        /// </summary>
        /// <param name="xml">Xml文档</param>
        /// <param name="format">缩进方式</param>
        /// <returns></returns>
        public static string ToString(XmlDocument xml, Formatting format)
        {
            if (xml == null) return null;

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8)
            {
                Formatting = format
            };
            xml.Save(writer);

            System.IO.StreamReader sr = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return xmlString;
        }
        /// <summary>
        /// 把Xml格式的字符串转换成Xml对象
        /// </summary>
        /// <param name="xmlSource"></param>
        /// <returns></returns>
        public static XmlDocument ToXml(string xmlSource)
        {
            if (string.IsNullOrEmpty(xmlSource)) return null;

            XmlDocument xml = new XmlDocument();
            xml.Load(new System.IO.StringReader(xmlSource));

            return xml;
        }

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    object obj = xmldes.Deserialize(sr);
                    if (obj == null) return null;

                    return obj as T;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream">反序列化的流</param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream) where T : class
        {
            try
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                object obj = xmldes.Deserialize(stream);
                if (obj == null) return null;

                return obj as T;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add("", ""); //not ot output the default namespace
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj, nameSpace);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
    }
}
