using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace AGVManagement
{
    [RoutePrefix("API/MES")]
    public class MesController : ApiController
    {
        private delegate void TaskActuator(Dictionary<string, dynamic> userData);

        [HttpGet]
        [HttpPost]
        [Route("Test")]
        [Route("Test123")]
        public HttpResponseMessage Test()
        {
            MesWebResponse result = new MesWebResponse();

            try
            {
                //TODO:查询库存
                Dictionary<string, int> userData = new Dictionary<string, int>();
                userData.Add("materialOutAccount", 10);
                result.data = userData;
                result.code = 0;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, Encoding.UTF8, "application/json") };
        }

        [HttpGet]
        [Route("GetMaterialInventory")]
        public HttpResponseMessage GetMaterialInventory(int distributionId, string materialCode)
        {
            MesWebResponse result = new MesWebResponse();

            try
            {
                //TODO:查询库存
                Dictionary<string, int> userData = new Dictionary<string, int>();
                userData.Add("materialOutAccount", 10);
                userData.Add("distributionId", distributionId);
                result.data = userData;
                result.code = 0;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, Encoding.UTF8, "application/json") };
        }

        [HttpPost]
        [Route("PostOneTask")]
        public HttpResponseMessage PostOneTask([FromUri] Int32 TaskID, [FromUri] String TaskContent)
        {
            MesWebResponse result = new MesWebResponse();

            try
            {
                //TODO:查询库存
                Dictionary<string, int> userData = new Dictionary<string, int>();
                userData.Add(TaskContent, TaskID);
                userData.Add("等待执行", TaskID + 1);
                result.data = userData;
                result.code = 0;
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, Encoding.UTF8, "application/json") };
        }



        private HttpResponseMessage ProcessMesTask(JObject data, TaskActuator taskActuator)
        {
            MesWebResponse result = new MesWebResponse();
            try
            {
                Dictionary<string, dynamic> userData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data.ToString());
                if (userData != null)
                {
                    taskActuator(userData);
                    result.code = 0;
                }
                else
                {
                    result.code = -1;
                    result.msg = "数据格式错误！";
                }
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = ex.Message;
            }

            string jsonResult = JsonConvert.SerializeObject(result);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, Encoding.UTF8, "application/json") };
        }

        //[HttpPost]
        //[Route("IssueLogisticsTask")]
        //public HttpResponseMessage IssueLogisticsTask([FromBody]JObject data)
        //{
        //    return ProcessMesTask(data, new TaskActuator(FormMain.FormInstance.ProcessLogisticsTask));
        //}

        //[HttpPost]
        //[Route("IssueDeleteTask")]
        //public HttpResponseMessage IssueDeleteTask([FromBody]JObject data)
        //{
        //    return ProcessMesTask(data, new TaskActuator(FormMain.FormInstance.ProcessDeleteTask));
        //}

        //[HttpPost]
        //[Route("IssueStorageTask")]
        //public HttpResponseMessage IssueStorageTask([FromBody]JObject data)
        //{
        //    return ProcessMesTask(data, new TaskActuator(FormMain.FormInstance.ProcessStorageTask));
        //}

        //[HttpPost]
        //[Route("IssueOutboundTask")]
        //public HttpResponseMessage IssueOutboundTask([FromBody]JObject data)
        //{
        //    return ProcessMesTask(data, new TaskActuator(FormMain.FormInstance.ProcessOutboundTask));
        //}
    }



    /// <summary>
    /// api返回的json数据模型
    /// </summary>
    public class MesWebResponse
    {
        /// <summary>
        /// 结果
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string msg { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object data { get; set; }
    }

}
