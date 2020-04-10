using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestInvockWebServiceConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = ExecuteServiceMethodByWebRequest();
            result = ExecuteServiceMethodByReference();
            result = ExecuteServiceMethodByProxyClass();
        }
        public static string ExecuteServiceMethodByReference()
        {
            var client = new WcsWebServiceReference.WcsWebServiceSoapClient();
            return client.XMZ_WCS_COMMAND("1", "CA0");
        }
        public static string ExecuteServiceMethodByProxyClass()
        {
            var client = new WcsWebService();
            //client.Url = "https://localhost:44315/WcsWebService.asmx";
            return client.XMZ_WCS_COMMAND("1", "CA0");
        }
        public static string ExecuteServiceMethodByWebRequest()
        {
            string postString = @"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://webServiceI.ewms.zjepe.com"">
   <soapenv:Header/>
   <soapenv:Body>
      <web:XMZ_WCS_COMMAND>
         <!--Optional:-->
         <AUTO_TYPE>2</AUTO_TYPE>
         <!--Optional:-->
         <WHSE_ID>2</WHSE_ID>
      </web:XMZ_WCS_COMMAND>
   </soapenv:Body>
</soapenv:Envelope>
";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            var url = "https://localhost:44315/WcsWebService.asmx";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml;charset=UTF-8";
            request.Referer = string.Empty;
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            //request.Headers.Add("SOAPAction", "urn:mediate");
            //request.Headers.Add("SOAPAction", string.IsNullOrWhiteSpace(methodInfo.SOAPAction) ? "" : methodInfo.SOAPAction);
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.ServerCertificateValidationCallback +=
                    (sender, cert, chain, error) =>
                    {
                        return true;
                    };

            string responseContent = null;
            using (var writer = request.GetRequestStream())
            {
                writer.Write(postData, 0, postData.Length);
                writer.Close();
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    responseContent = responseContent = reader.ReadToEnd();
                }
            }
            request = null;
            response.Close();

            //StringReader Reader = new StringReader(responseContent);
            //XmlDocument xml = new XmlDocument();
            //xml.Load(Reader);
            //responseContent = xml.InnerText.ToString();

            return responseContent;
        }

    }
}
