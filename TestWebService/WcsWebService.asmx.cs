using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TestWebService
{
    /// <summary>
    /// Summary description for WcsWebService
    /// </summary>
    [WebService(Namespace = "http://webServiceI.ewms.zjepe.com")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Services.WebServiceBindingAttribute(Name = "webWcsSoapBinding", Namespace = "http://webServiceI.ewms.zjepe.com/")]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WcsWebService
    {

        //        [WebMethod]
        //        public string HelloWorld()
        //        {
        //            return "Hello World";
        //        }
        //        [WebMethod]
        //        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //        public string XMZ_WCS_COMMAND(string AUTO_TYP, string WHSE_ID)
        //        {
        //            return string.Format(string.Format(@"
        //<![CDATA[<?xml version=""1.0"" encoding=""utf - 8""?>
        //<FLAG>1</FLAG>//成功失败标识0：失败；1：成功
        //<AUTO_ID>100</AUTO_ID>//自动化编号-主键
        //<CELL_ID>200</CELL_ID>//货位ID-目的地
        //<AUTO_TYPE>1</AUTO_TYPE>出入库标识 0：入库；1：出库
        //<ZTIME1>{0}</ZTIME1>//传输时间
        //]]>", DateTime.Now.ToString("yyyyMMddHHmmss")));
        //        }

        //        [WebMethod]
        //        public string XMZ_WCS_SUCCESS(string AUTO_ID, string IS_SUCCESS, string AUTO_TYPE, string IS_FINISH, string WHSE_ID)
        //        {
        //            return string.Format(@"
        //<?xml version=""1.0"" encoding=""utf - 8""?>
        //<FLAG>1</FLAG>成功标识 0：失败；1：成功
        //<AUTO_ID>{0}</AUTO_ID>//自动化编号-主键
        //<ZTIME1>{1}</ZTIME1>//传输时间", AUTO_ID, DateTime.Now.ToString("yyyyMMddHHmmss"));
        //        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string XMZ_WCS_SUCCESS([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string AUTO_ID, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string IS_SUCCESS, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string AUTO_TYPE, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string IS_FINISH, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string WHSE_ID)
        {
            return string.Format(@"
            <?xml version=""1.0"" encoding=""utf - 8""?>
            <FLAG>1</FLAG>成功标识 0：失败；1：成功
            <AUTO_ID>{0}</AUTO_ID>//自动化编号-主键
            <ZTIME1>{1}</ZTIME1>//传输时间", AUTO_ID, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute(Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string XMZ_WCS_COMMAND([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string AUTO_TYPE, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string WHSE_ID)
        {
            return string.Format(string.Format(@"
            <![CDATA[<?xml version=""1.0"" encoding=""utf - 8""?>
            <FLAG>1</FLAG>//成功失败标识0：失败；1：成功
            <AUTO_ID>100</AUTO_ID>//自动化编号-主键
            <CELL_ID>200</CELL_ID>//货位ID-目的地
            <AUTO_TYPE>{1}</AUTO_TYPE>出入库标识 0：入库；1：出库
            <ZTIME1>{0}</ZTIME1>//传输时间
            ]]>", DateTime.Now.ToString("yyyyMMddHHmmss"), AUTO_TYPE));
        }
    }
}
