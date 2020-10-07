using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConectorFS
{
    class XMLParse
    {
        public class RecordItem : Record
        {
        }

        private static string xmlCleaner(string xml)
        {
            string xmlaux = xml.Substring(1);
            xmlaux = xmlaux.Remove(xmlaux.Length - 1);
            return xmlaux;
        }

        public static string GetSingleElement(string xml, string type, string name, string attr)
        {
            string attrVal = "";
            string xmlaux = xmlCleaner(xml);
            // Leo el archivo XML
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlaux);
            XmlNodeList elemList = xDoc.GetElementsByTagName(type);
            for (int i = 0; i < elemList.Count; i++)
            {
                attrVal = elemList[i].Attributes[attr].Value;
            }

            return attrVal;
        }

        public static List<Record> GetListElement(string xml, string type, string name, string attr1, string attr2)
        {
            string xmlaux = xmlCleaner(xml);
            // Leo el archivo XML
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlaux);

            XmlNodeList elemList = xDoc.GetElementsByTagName("WaWi");
            XmlNodeList elemListItems = ((XmlElement)elemList[0]).GetElementsByTagName(type);
            List<Record> kmsg = new List<Record>();
            foreach (XmlElement node in elemListItems)
            {
                XmlNodeList configValueNode = node.GetElementsByTagName(name);
                foreach (XmlElement nodeItem in configValueNode)
                {
                    var kmsgitem = new RecordItem();
                    kmsgitem.BarCode = nodeItem.GetAttribute(attr1);
                    if (!string.IsNullOrEmpty(attr2))
                    {
                        kmsgitem.Quantity = nodeItem.GetAttribute(attr2);
                    }
                    kmsg.Add(kmsgitem);
                }
            }
            return kmsg;
        }
    }
}
