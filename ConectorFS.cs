using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConectorFS
{
    public class ConectorFS
    {
        static string ip = Configuration.GetConfiguration("conexionConfig", "ip");
        static int port = Int32.Parse(Configuration.GetConfiguration("conexionConfig", "port"));
        
        public class WSConsultarStockProductoItem
        {
            public string ProductoId { get; set; }
            public int ProductoCantidad { get; set; }
        }

        public class WSDispensarArticuloItem
        {
            public string ProductoId { get; set; }
            public int ProductoCantidad { get; set; }
            public DateTime ProductoFechaVenc { get; set; }
            public IList<string> ProductoCodigoBarraId { get; set; }
        }

        public class WSDispensarArticulo
        {
            public int Ticket { get; set; }
            public string POS { get; set; }
            public int NroOrden { get; set; }
            public int NroPedido { get; set; }
            public string NroProceso { get; set; }
            public int NroBandejaSalida { get; set; }
            public List<WSDispensarArticuloItem> Productos { get; set; }
        }

        private static Socket ConnectSocket(string ip,int port)
        {           
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s.NoDelay = true;
            //only as example to terminate the application
            //s.ReceiveTimeout = 20000;
            s.Connect(ipe);
            if (s.Connected)
                return (s);
            return (null);
        }

        // This method requests the home page content for the specified server.
        private static string SocketSendReceive(string server, int port,string xml)
        {
            string request = getStx() + xml + getEtx();

            string configvalue = "";

            Byte[] bytesToSend = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new Byte[1228800];

            // Create a socket connection with the specified server and port.
            using (Socket s = ConnectSocket(server, port))
            {

                if (s == null)
                {
                    Console.Error.WriteLine("Error opening socket");
                    return configvalue;
                }

                // Send request to the server.
                s.Send(bytesToSend, bytesToSend.Length, 0);


                StringBuilder builder = new StringBuilder();

                int countBytesReceved=0;

                try
                {
                    countBytesReceved = s.Receive(bytesReceived, bytesReceived.Length, 0);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.Message);
                }
                builder.Append(Encoding.ASCII.GetString(bytesReceived, 0, countBytesReceved));
                foreach (string msg in getMessages(builder))
                {
                    configvalue = msg;
                }

            }
            return configvalue;
        }

        public static int IndexOf(StringBuilder sb, string value)
        {
            int index;
            int length = value.Length;
            int maxSearchLength = (sb.Length - length) + 1;

            for (int i = 0; i < maxSearchLength; ++i)
            {
                if (sb[i] == value[0])
                {
                    index = 1;
                    while ((index < length) && (sb[i + index] == value[index]))
                        ++index;

                    if (index == length)     
                        return i;
                }
            }

            return -1;
        }

        private static List<string> getMessages(StringBuilder builder)
        {
            List<string> toReturn = new List<string>();
            while (true)
            {
                int messageStart = IndexOf(builder, getStx());
                if (messageStart < 0)
                {
                    builder.Remove(0, builder.Length);
                    break;
                }
                builder.Remove(0, messageStart);
                int messageEnd = IndexOf(builder, getEtx());
                if (messageEnd < 0)
                {
                    break;
                }

                toReturn.Add(builder.ToString(0, messageEnd + getStx().Length));
                builder.Remove(0, messageEnd + getEtx().Length);
            }
            return (toReturn);
        }

        static string stx = null;
        static string etx = null;

        private static String getStx()
        {
            if (stx == null)
            {
                int unicode = 2;
                char character = (char)unicode;
                stx = character.ToString();
            }
            return (stx);
        }

        private static String getEtx()
        {
            if (etx == null)
            {
                int unicode = 3;
                char character = (char)unicode;
                etx = character.ToString();
            }
            return (etx);
        }

        public string ConsultarStock(string CodigoBarra)
        {
            string xml = ArmarXML.ConsultaStock("12", "1", CodigoBarra, "", "");
            string msg = SocketSendReceive(ip, port, xml);
            GraboLog.GrabarLog(xml);
            List<WSConsultarStockProductoItem> productos = new List<WSConsultarStockProductoItem>();

            WSConsultarStockProductoItem wsconsultarstockproductoitem = new WSConsultarStockProductoItem();

            List<Record> vmsg = XMLParse.GetListElement(msg, "VMsg", "Record", "BarCode", "");
            foreach (var vmsgitem in vmsg)
            {
                wsconsultarstockproductoitem.ProductoId = vmsgitem.BarCode;
                wsconsultarstockproductoitem.ProductoCantidad = 1;
                productos.Add(wsconsultarstockproductoitem);
            }
            string json = JsonConvert.SerializeObject(productos, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        public int ConsultarStockInterno(string CodigoBarra)
        {
            string xml = ArmarXML.ConsultaStock("12", "1", CodigoBarra, "", "");
            string msg = SocketSendReceive(ip, port, xml);
            
            List<WSConsultarStockProductoItem> productos = new List<WSConsultarStockProductoItem>();

            WSConsultarStockProductoItem wsconsultarstockproductoitem = new WSConsultarStockProductoItem();

            List<Record> vmsg = XMLParse.GetListElement(msg, "VMsg", "Record", "BarCode", "");
            int stock = 0;
            foreach (var vmsgitem in vmsg)
            {
                stock += 1;
            }
            
            return stock;
        }

        public bool DispensarArticulos(string DispensarJson)
        {
            GraboLog.GrabarLog(DispensarJson);
            DispensarJson = DispensarJson.Replace("0000-00-00T00:00:00", "1001-01-01T00:00:00");
            WSDispensarArticulo wsdispensararticulo = JsonConvert.DeserializeObject<WSDispensarArticulo>(DispensarJson);
            //WSDispensarArticulo wsdispensararticuloaux = new WSDispensarArticulo();
            //WSDispensarArticuloItem wsdispensararticuloitem = new WSDispensarArticuloItem();
            //foreach (var producto in wsdispensararticulo.Productos)
            //{
            //    int stockCB = 0;
            //    int productoCantidad = producto.ProductoCantidad;
            //    foreach (var codigobarra in producto.ProductoCodigoBarraId)
            //    {
            //        // Consulto stock de cada cb
            //        stockCB = ConsultarStockInterno(codigobarra);
            //        if (stockCB > 0)
            //        {
            //            int diferencia = stockCB - productoCantidad;
            //            if (diferencia >= 0)
            //            {
            //                // Cumplo con el stock
            //                wsdispensararticuloitem.ProductoId = codigobarra;
            //                wsdispensararticuloitem.ProductoCantidad = productoCantidad;
            //                wsdispensararticuloaux.Productos.Add(wsdispensararticuloitem);
            //                break;
            //            }
            //            else
            //            {
            //                wsdispensararticuloitem.ProductoId = codigobarra;
            //                wsdispensararticuloitem.ProductoCantidad = stockCB;
            //                wsdispensararticuloaux.Productos.Add(wsdispensararticuloitem);
            //                // Todavia no cumplo con el stock
            //                productoCantidad = productoCantidad - stockCB;
            //            }
            //        }

            //    }
            //}
            string request = getStx() + ArmarXML.DispensarArticulo("999", wsdispensararticulo.NroOrden.ToString().Trim(), "1", "3", "0", "0", wsdispensararticulo.Productos) + getEtx();
            GraboLog.GrabarLog(request);
            string msg = SocketSendReceive(ip, port, request);
            string orderState = XMLParse.GetSingleElement(msg, "AMsg", "", "OrderState");

            List<Record> amsg = XMLParse.GetListElement(msg, "AMsg", "Record", "BarCode", "Quantity");
            foreach (var amsgitem in amsg)
            {
                string texto = "Estado: " + orderState + " " + amsgitem.BarCode + " - " + amsgitem.Quantity;
                GraboLog.GrabarLog(texto);
            }
            return true;
        }

        public string ConsultarOrden(string Orden)
        {

            string request = getStx() + ArmarXML.EstadoOrden("1",Orden) + getEtx();
            GraboLog.GrabarLog(request);
            string msg = SocketSendReceive(ip, port, request);
            string orderState = XMLParse.GetSingleElement(msg, "OMsg", "", "State");
            string orderText  = XMLParse.GetSingleElement(msg, "OMsg", "", "Text");

            string response = "Estado: "  + orderState + "-" + orderText ;
            return response;
        }

    }
}
