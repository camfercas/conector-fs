using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConectorFS
{
    class ConsumirRESTWS
    {
        public static string GetPOSTResponse(string url, string param)
        {
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(url);

            wrGETURL.Method = "POST";
            wrGETURL.ContentType = @"application/json; charset=utf-8";
            using (var streamWriter = new StreamWriter(wrGETURL.GetRequestStream()))
            {
                streamWriter.Write(param);
                streamWriter.Flush();
                streamWriter.Close();
            }
            HttpWebResponse webresponse = wrGETURL.GetResponse() as HttpWebResponse;

            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // read response stream from response object
            StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            // read string from stream data
            string strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();

            return strResult;
        }
    }
}
