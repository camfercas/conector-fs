using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConectorFS
{
    class ArmarXML
    {
        public static string ConsultaBarcode(string RequesterNumber, string Country, string Code, string Barcode)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("BCmd");
            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;
            XmlAttribute CountryAttribute = doc.CreateAttribute("Country");
            CountryAttribute.Value = Country;
            XmlAttribute CodeAttribute = doc.CreateAttribute("Code");
            CodeAttribute.Value = Code;
            XmlAttribute BarcodeAttribute = doc.CreateAttribute("BarCode");
            BarcodeAttribute.Value = Barcode;

            bNode.Attributes.Append(RequesterNumberAttribute);
            bNode.Attributes.Append(CodeAttribute);
            bNode.Attributes.Append(CountryAttribute);
            bNode.Attributes.Append(BarcodeAttribute);
            rootNode.AppendChild(bNode);

            return doc.OuterXml;
        }

        public static string ConsultaStock(string OrderNumber, string RequesterNumber, string BarCode, string BatchNumber, string ExternalIdCode)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            // Add the new node to the document.
            XmlElement root = doc.DocumentElement;
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("VCmd");
            XmlAttribute OrderNumberAttribute = doc.CreateAttribute("OrderNumber");
            OrderNumberAttribute.Value = OrderNumber;
            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;
            XmlAttribute BarCodeAttribute = doc.CreateAttribute("BarCode");
            BarCodeAttribute.Value = BarCode;
            XmlAttribute BatchNumberAttribute = doc.CreateAttribute("BatchNumber");
            BatchNumberAttribute.Value = BatchNumber;
            XmlAttribute ExternalIdCodeAttribute = doc.CreateAttribute("ExternalIdCode");
            ExternalIdCodeAttribute.Value = ExternalIdCode;

            bNode.Attributes.Append(OrderNumberAttribute);
            bNode.Attributes.Append(RequesterNumberAttribute);
            if (!string.IsNullOrEmpty(BarCode))
            {
                bNode.Attributes.Append(BarCodeAttribute);
                bNode.Attributes.Append(BatchNumberAttribute);
                bNode.Attributes.Append(ExternalIdCodeAttribute);
            }

            rootNode.AppendChild(bNode);

            return doc.OuterXml;
        }

        public static string DispensarArticulo(string RequesterNumber, string OrderNumber, string OutputNumber, string Priority, string Country, string Code, List<ConectorFS.WSDispensarArticuloItem> Productos, string BatchNumber = "")
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("ACmd");

            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;
            XmlAttribute OrderNumberAttribute = doc.CreateAttribute("OrderNumber");
            OrderNumberAttribute.Value = OrderNumber;
            XmlAttribute OutputNumberAttribute = doc.CreateAttribute("OutputNumber");
            OutputNumberAttribute.Value = OutputNumber;
            XmlAttribute PriorityAttribute = doc.CreateAttribute("Priority");
            PriorityAttribute.Value = Priority;

            // Tag Record

            foreach (var producto in Productos)
            {
                XmlNode bNodeItem = doc.CreateElement("Record");

                XmlAttribute CountryAttribute = doc.CreateAttribute("Country");
                CountryAttribute.Value = Country;
                XmlAttribute CodeAttribute = doc.CreateAttribute("Code");
                CodeAttribute.Value = Code;
                XmlAttribute BarCodeAttribute = doc.CreateAttribute("BarCode");
                XmlAttribute QuantityAttribute = doc.CreateAttribute("Quantity");
                QuantityAttribute.Value = producto.ProductoCantidad.ToString().Trim();
                XmlAttribute BatchNumberAttribute = doc.CreateAttribute("BatchNumber");
                BatchNumberAttribute.Value = BatchNumber;
                XmlAttribute ExternalIdCodeAttribute = doc.CreateAttribute("ExternalIdCode");
                BarCodeAttribute.Value = producto.ProductoId;
                ExternalIdCodeAttribute.Value = "";

                //string tipoalta = Configuration.GetConfiguration("conexionConfig", "dispense");

                //if (tipoalta == "cb")
                //{
                //    BarCodeAttribute.Value = producto.ProductoId;
                //    ExternalIdCodeAttribute.Value = "";
                //}
                //else
                //{
                //    BarCodeAttribute.Value = "";
                //    ExternalIdCodeAttribute.Value = producto.ProductoId;
                //}

                bNodeItem.Attributes.Append(CountryAttribute);
                bNodeItem.Attributes.Append(CodeAttribute);
                bNodeItem.Attributes.Append(BarCodeAttribute);
                bNodeItem.Attributes.Append(QuantityAttribute);
                bNodeItem.Attributes.Append(BatchNumberAttribute);
                bNodeItem.Attributes.Append(ExternalIdCodeAttribute);
                bNode.AppendChild(bNodeItem);
            }

            bNode.Attributes.Append(RequesterNumberAttribute);
            bNode.Attributes.Append(OrderNumberAttribute);
            bNode.Attributes.Append(OutputNumberAttribute);
            bNode.Attributes.Append(PriorityAttribute);
            rootNode.AppendChild(bNode);
            return doc.OuterXml;
        }

        // O Dialog
        public static string EstadoOrden(string RequesterNumber, string OrderNumber)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("OCmd");
            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;
            XmlAttribute OrderNumberAttribute = doc.CreateAttribute("OrderNumber");
            OrderNumberAttribute.Value = OrderNumber;

            bNode.Attributes.Append(RequesterNumberAttribute);
            bNode.Attributes.Append(OrderNumberAttribute);
            rootNode.AppendChild(bNode);

            return doc.OuterXml;
        }

        public static string IniciarDialogo(string RequesterNumber, string Protocol, string Dialogs)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("RCmd");
            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;
            XmlAttribute ProtocolAttribute = doc.CreateAttribute("Protocol");
            ProtocolAttribute.Value = Protocol;
            XmlAttribute DialogsAttribute = doc.CreateAttribute("Dialogs");
            DialogsAttribute.Value = Dialogs;

            bNode.Attributes.Append(RequesterNumberAttribute);
            bNode.Attributes.Append(ProtocolAttribute);
            bNode.Attributes.Append(DialogsAttribute);
            rootNode.AppendChild(bNode);

            return doc.OuterXml;
        }

        public static string StatusRobot(string RequesterNumber)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("WaWi");
            doc.AppendChild(rootNode);

            XmlNode bNode = doc.CreateElement("SCmd");
            XmlAttribute RequesterNumberAttribute = doc.CreateAttribute("RequesterNumber");
            RequesterNumberAttribute.Value = RequesterNumber;

            bNode.Attributes.Append(RequesterNumberAttribute);
            rootNode.AppendChild(bNode);

            return doc.OuterXml;
        }

    }
}
