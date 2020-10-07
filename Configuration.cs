using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConectorFS
{
    class Configuration
    {
        public static string GetConfiguration(string type, string name)
        {
            string configValue = "";
            // Leo el archivo XML
            XmlDocument xDoc = new XmlDocument();
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string xmlFileName = Path.Combine(assemblyFolder, "Configuration.xml");
            xDoc.Load(xmlFileName);
            XmlNodeList configurations = xDoc.GetElementsByTagName("configurations");
            XmlNodeList configurationsItems = ((XmlElement)configurations[0]).GetElementsByTagName(type);
            foreach (XmlElement node in configurationsItems)
            {
                XmlNodeList configValueNode = node.GetElementsByTagName(name);
                configValue = configValueNode[0].InnerText;
            }

            return configValue;
        }
    }
}
