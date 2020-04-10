using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace TestAliHealthConsoleApp
{
    class Program
    {
        // TOP服务地址，正式环境需要设置为http://gw.api.taobao.com/router/rest
        //private static string serverUrl = "http://gw.api.tbsandbox.com/router/rest";
        private static string serverUrl = "http://gw.api.taobao.com/router/rest";
        private static string appKey = "28162238"; // 可替换为您的沙箱环境应用的AppKey
        private static string appSecret = "e95adf0db591ca0feec5fb21499a332e"; // 可替换为您的沙箱环境应用的AppSecret
        static void Main(string[] args)
        {
            //Test0();
            //testGetCustomerList();
            //GetFileByUrl();
            TestDecompress();
        }

        public static void Test0()
        {
            //// TOP服务地址，正式环境需要设置为http://gw.api.taobao.com/router/rest
            //string serverUrl = "http://gw.api.tbsandbox.com/router/rest";
            ////string serverUrl = "http://gw.api.taobao.com/router/rest";
            //string appKey = "28162238"; // 可替换为您的沙箱环境应用的AppKey
            //string appSecret = "e95adf0db591ca0feec5fb21499a332e"; // 可替换为您的沙箱环境应用的AppSecret

            string sessionKey = "test"; // 必须替换为沙箱账号授权得到的真实有效SessionKey

            ITopClient client = new DefaultTopClient(serverUrl, appKey, appSecret, "json");

            //ITopClient client = new DefaultTopClient(url, appkey, secret);
            AlibabaAlihealthDrugCodeKytQuerycodeRequest req = new AlibabaAlihealthDrugCodeKytQuerycodeRequest();
            req.RefEntId = "aefferw123445";
            req.Codes = "81472160000100364995";
            AlibabaAlihealthDrugCodeKytQuerycodeResponse rsp = client.Execute(req);
            Console.WriteLine(rsp.Body);
        }
        public static void Test()
        {
            //// TOP服务地址，正式环境需要设置为http://gw.api.taobao.com/router/rest
            //string serverUrl = "http://gw.api.tbsandbox.com/router/rest";
            ////string serverUrl = "http://gw.api.taobao.com/router/rest";
            //string appKey = "28162238"; // 可替换为您的沙箱环境应用的AppKey
            //string appSecret = "e95adf0db591ca0feec5fb21499a332e"; // 可替换为您的沙箱环境应用的AppSecret

            string sessionKey = "test"; // 必须替换为沙箱账号授权得到的真实有效SessionKey

            ITopClient client = new DefaultTopClient(serverUrl, appKey, appSecret, "json");

            ////ITopClient client = new DefaultTopClient(url, appkey, secret);
            //AlibabaAlihealthDrugCodeKytQuerycodeRequest req = new AlibabaAlihealthDrugCodeKytQuerycodeRequest();
            //req.RefEntId = "aefferw123445";
            //req.Codes = "81472160000100364995";
            //AlibabaAlihealthDrugCodeKytQuerycodeResponse rsp = client.Execute(req);
            //Console.WriteLine(rsp.Body);

            AlibabaAlihealthDrugKytUploadinoutbillRequest req = new AlibabaAlihealthDrugKytUploadinoutbillRequest();
            req.BillCode = "BC1_112001";
            req.BillTime = DateTime.Parse("2018-02-02 02:21:21");
            req.BillType = 201L; //企业上传出入库信息，包括采购入库（102），退货入库（103），供应入库（107）,销售出库（201）,退货出库（202），销毁出库（205），抽检出库（206）， 供应出库（209）, 不包括对个人的零售出库，疫苗接种，领药出库。
            req.PhysicType = 3L;//药品类型【3普药2特药】
            req.RefUserId = "320000000000127971"; //上传企业的单位维一编码【注意：该入参是ref_ent_id，不是ent_id】
            req.AgentRefUserId = "320000000000127971";//可选,代理企业REF标识
            req.FromUserId = "5069452c34b94a778abaa26c2b40b305";//发货企业entId【注意：该入参是ent_id,并不是ref_ent_id】
            req.ToUserId = "5069452c34b94a778abaa26c2b40b305";//收货企业entId【注意：该入参是ent_id,并不是ref_ent_id】
            req.DestUserId = "5069452c34b94a778abaa26c2b40b305";//可选,直调企业标识【注意：该入参是ent_id,并不是ref_ent_id】
            req.OperIcCode = "210000234"; //单据提交者（appkey编号）
            req.OperIcName = "张三";//单据提交者姓名
            req.ClientType = "2";//客户端类型[必须填2]
            req.ReturnReasonCode = "1";//可选,退货原因代码[退货入出库时填写]
            req.ReturnReasonDes = "退货原因描述";//可选
            req.CancelReasonCode = "1";//可选,注销原因
            req.CancelReasonDes = "注销原因描述";//可选
            req.ExecuterName = "执行人";//可选
            req.ExecuterCode = "11034564321";//可选,执行人证件号【销毁出库时填写】
            req.SuperviserName = "监督人";//可选
            req.SuperviserCode = "11276789342";//可选,监督人证件号【销毁出库时填写】
            req.WarehouseId = "W001";//可选,仓号
            req.DrugId = "D001";//可选,药品ID[企业自已系统的药品ID]
            req.TraceCodes = "81012350000000157474";//追溯码[多个时用逗号分开]
            AlibabaAlihealthDrugKytUploadinoutbillResponse rsp = client.Execute(req);
            Console.WriteLine(rsp.Body);
        }

        public static void testGetCustomerList()
        {
            ITopClient client = new DefaultTopClient(serverUrl, appKey, appSecret, "json");
            AlibabaAlihealthDrugKytListpartsRequest req = new AlibabaAlihealthDrugKytListpartsRequest();
            req.RefEntId = "320000000000016884";
            //req.EntName = "名称";
            //req.RefPartnerId = "123001129";
            req.PageSize = 20L;
            req.Page = 1L;
            //req.BeginDate = "2018-12-26 13:55:00";
            //req.EndDate = "2018-12-26 13:55:00";
            bool isFinished = false;
            var isFirst = true;
            while (!isFinished)
            {
                AlibabaAlihealthDrugKytListpartsResponse rsp = client.Execute(req);
                var result = rsp.Body;
                //Console.WriteLine(result);
                var jObject = JsonConvert.DeserializeObject<Rootobject>(result);
                var list = jObject.alibaba_alihealth_drug_kyt_listparts_response.result.model.result_list.p_ent_par_dto;
                var total_num = jObject.alibaba_alihealth_drug_kyt_listparts_response.result.model.total_num;
                var sb = new StringBuilder();
                foreach (var item in list)
                {
                    var properties = item.GetType().GetProperties();
                    if (isFirst)
                    {
                        foreach (var property in properties)
                        {
                            sb.Append(property.Name);
                            sb.Append(",");
                        }
                        isFirst = false;
                    }

                    foreach (var property in properties)
                    {
                        sb.Append(item.GetType().GetProperty(property.Name).GetValue(item));
                        sb.Append(",");
                    }
                    sb.Append(Environment.NewLine);
                }

                File.AppendAllText("001.txt", sb.ToString());

                req.Page += 1L;
                Console.WriteLine("page:{0}/{1}", req.Page, total_num);
                if (req.Page * 20 >= total_num)
                {
                    break;
                }
            }
        }
        public static void GetFileByUrl()
        {
            WebClient client = new WebClient();

            //var stockContent =
            client.DownloadFile("http://drugadminoss.oss.aliyuncs.com/DATAEXTRACTION/CODE_RELATION/320000000000016884/41f9b85806a84dc7ad88f63ee16d8ac5?Expires=1576031493&OSSAccessKeyId=LTAI3p6XD6JjU1Q3&Signature=Ror8K6PmRGzZ4JQnkV5Octv01i8%3D", "zz.zip");
            client.Dispose();
        }

        public static void TestDecompress()
        {
            File.Delete(@"D:\Temp\TestAli\test.zip");
            using (ZipArchive zip = ZipFile.Open(@"D:\Temp\TestAli\test.zip", ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(@"D:\Temp\TestAli\something.txt", "data/path/something.txt");
            }
            //if (Directory.Exists(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5")) Directory.Delete(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5", true);            
            if (Directory.Exists(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5")) Directory.Delete(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5", true);
            ZipFile.ExtractToDirectory(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5.zip", @"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5");
            var files = Directory.GetFiles(@"D:\Temp\TestAli\41f9b85806a84dc7ad88f63ee16d8ac5");
        }
    }
}
