using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TestEasWindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Test4()
        {
            string postString = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
   <soapenv:Header>
        <Esb xmlns:ns=""http://esb.services.hnjzt.com"">
            <Route> 
               <Sender>WMS</Sender>
                <Time>2018-11-09 17:30:23</Time>
                <ServCode>LIMS_N3</ServCode>
                <MsgId>MES_20190402173023_1541755823</MsgId>
                <TestFlag>0</TestFlag>
                <UserId>wms_user</UserId>
                <AuthCode>4lwkqwopjtxnrg9x</AuthCode>
                <MsgType/>
                <Field1/>
                <Field2/>
            </Route>
        </Esb>
    <tem:Route xmlns:tem=""http://tempuri.org/""><tem:Sender>WMS</tem:Sender><tem:Time>2018-11-09 17:30:23</tem:Time><tem:ServCode>LIMS_N1</tem:ServCode><tem:MsgId>MES_20190402173023_1541755823</tem:MsgId><tem:TestFlag>0</tem:TestFlag><tem:UserId>mes_user</tem:UserId><tem:AuthCode>eyJ1c2VyTmFtZSI6Inp5eSIsInJpZ2h0TGlzdCI6WyJnZXRWZXJzaW9uMjIiLCJkc2ZzIl19</tem:AuthCode></tem:Route></soapenv:Header>
    <soapenv:Body>
        <ns:RunAction xmlns:ns=""http://tempuri.org/"">
            <ns:sActionName>SunwayWebServices.ReceivedParameters_Return</ns:sActionName>
         <!--Optional:-->
         <ns:Json> {""ORDNO"":"""",""BATCHNO"":""2019051701"",""MATCODE"":""WN001 WN001A""}</ns:Json>
        </ns:RunAction>
    </soapenv:Body>
</soapenv:Envelope>
    ";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            var url = "http://192.168.0.46/LimsWebService/CommonService.asmx";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.CookieContainer = cookies;
            //request.Proxy = webProxy;
            request.Method = "POST";
            request.ContentType = "text/xml;charset=UTF-8";
            //request.Accept = "text/html, application/xhtml+xml, */*";
            //request.UserAgent = userAgent;
            request.Referer = string.Empty;
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            // request.Headers.Add("Cookie", $"ASP.NET_SessionId={sessionId}");
            //request.ServerCertificateValidationCallback +=
            //        (sender, cert, chain, error) =>
            //        {
            //            return true;
            //        };

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
                    //var info = $"Post:keymc={keymc},sssvalue={sssvalue},questionReqult={questionResult},sessionId={sessionId}";
                    //LogInfo(info);
                    //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},{info}");
                }
            }
            request = null;
            response.Close();
            txtResult.Text = responseContent;
        }

        private void Test3()
        {
            var typeobj = Type.GetType("System.String");
            //var list = new List<typeobj>();
            var client = new EliteCommonWebService();
            var table = client.ODIStoreFGCheckInsertQuery("1", "UTF-8");
            txtResult.Text = table.Rows.Count.ToString();
            var baseName = TestClassLibrary.Class1.GetBaseName();
        }

        private void Test2()
        {
            txtResult.Text = string.Empty;
            string result = string.Empty;
            try
            {
                var login = new EASLoginProxyService();
                login.Url = "http://192.168.0.24:7888/ormrpc/services/EASLogin";
                var lrt = login.login("user", "13579", "eas", "test", "L2", 2);
                //var lrt = login.login("wms_user", "123456", "eas", "EAStest", "L2", 2);
                var client = new WSBaseResServerFacadeSrvProxyService();
                client.Url = "http://192.168.0.24:7888/ormrpc/services/WSBaseResServerFacade";
                //var container = new CookieContainer();
                //container.Add(new Cookie("sessionId", lrt.sessionId));
                //client.CookieContainer = container;
                result = client.queryBaseRes(cmbCategory.Text, string.IsNullOrWhiteSpace(txtQueryParams.Text) ? null : txtQueryParams.Text);
                //result = client.queryBaseRes("PurOrder", "{\"method\": \"query\"}");

                txtResult.Text = result;
                File.WriteAllText(".\\" + cmbCategory.SelectedItem.ToString() + ".txt", result);
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
        }

        void Test()
        {
            ////创建HttpWebRequest 实例，使用WebRequest.Create
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("服务地址");
            ////发送请求
            //webRequest.Method = "POST";
            ////编码
            //webRequest.ContentType = "text/xml";
            //string soap = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            //soap += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            //soap += " <soap:Header>";
            //soap += " <SoapHeader xmlns=\"http://www.52taiqiu.com/\">";
            //soap += " <Login>" + txtUser.Text + "</Login>";
            //soap += " <Password>" + txtPwd.Text + "</Password>";
            //soap += " </SoapHeader>";
            //soap += " </soap:Header>";

            //soap += "<soap:Body>";

            //soap += " <Request xmlns=\"http://www.52taiqiu.com/\">";
            //soap += " <requestXml><![CDATA[" + txtRequest.Text + "]]></requestXml>";
            //soap += " </Request>";
            //soap += "</soap:Body>";
            //soap += "</soap:Envelope>";

            //webRequest.Headers["SoapAction"] = "http://www.52taiqiu.com/Request";

            ////字符转字节
            //byte[] bytes = Encoding.UTF8.GetBytes(soap);
            //Stream writer = webRequest.GetRequestStream();
            //writer.Write(bytes, 0, bytes.Length);
            //writer.Flush();
            //writer.Close();
            //string result = "";
            ////返回 HttpWebResponse
            //try
            //{
            //    HttpWebResponse hwRes = webRequest.GetResponse() as HttpWebResponse;
            //    if (hwRes.StatusCode == System.Net.HttpStatusCode.OK)
            //    {//是否返回成功
            //        Stream rStream = hwRes.GetResponseStream();
            //        //流读取
            //        StreamReader sr = new StreamReader(rStream, Encoding.UTF8);
            //        result = sr.ReadToEnd();
            //        sr.Close();
            //        rStream.Close();
            //    }
            //    else
            //    {
            //        result = "连接错误";
            //    }
            //    //关闭
            //    hwRes.Close();
            //    txtResponse.Text = result;
            //}
            //catch (System.Net.WebException ex)
            //{
            //    String responseFromServer = ex.Message.ToString() + " ";
            //    if (ex.Response != null)
            //    {
            //        using (WebResponse response = ex.Response)
            //        {
            //            Stream data = response.GetResponseStream();
            //            using (StreamReader reader = new StreamReader(data))
            //            {
            //                responseFromServer += reader.ReadToEnd();
            //            }
            //        }
            //    }
            //    txtResponse.Text = responseFromServer;
            //}
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Test2();
            //Test4();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new IncServiceReference.WebServicePortTypeClient("WebServiceHttpSoap11Endpoint");
            var result = client.exportCompanysData();
            txtResult.Text = result;
            //var client = new WebService();
            //var result = client.exportEmployeesData();
            //txtResult.Text = result.@return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var sub = "string123".Substring(0, 100);//throw error
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            String filePath1 = isoStore.GetType().GetField("m_RootDir", BindingFlags.Instance).GetValue(isoStore).ToString();

            if (isoStore.FileExists("TestStore.txt"))
            {
                Console.WriteLine("The file already exists!");
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
                {
                    String filePath2 = isoStream.GetType().GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(isoStream).ToString();
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        Console.WriteLine("Reading contents:");
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.CreateNew, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine("Hello Isolated Storage");
                        Console.WriteLine("You have written to the file.");
                    }
                }
            }
        }
    }
}
