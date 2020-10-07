using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConectorFS
{
    class GraboLog
    {
        public class WSGraboLogItem
        {
            public string Mensaje { get; set; }
        }

        public static void GrabarLog(string texto)
        {
            // Chequeo si el logger esta prendido (1)
            //bool logger = bool.Parse(Configuration.GetConfiguration("conexionConfig", "logger"));
            bool logger = true;
            if (logger)
            {
                WSGraboLogItem wsgrabologitem = new WSGraboLogItem();
                string uris = Configuration.GetConfiguration("ws", "WSGraboLog");
                wsgrabologitem.Mensaje = "DLL: " + texto;
                string paramsa = JsonConvert.SerializeObject(wsgrabologitem);
                ConsumirRESTWS.GetPOSTResponse(uris, paramsa);
            }
        }
    }
}
