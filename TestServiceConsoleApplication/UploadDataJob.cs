using Elite.LMS.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TestServiceConsoleApplication
{
    [DisallowConcurrentExecution]
    class UploadDataJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.AutoFlush = true;
            Debug.Indent();
            Debug.WriteLine("Entering Main"); //显示在DebugView的信息
            Debug.WriteLine("Exiting Main");
            Debug.Unindent();

            var jsonString = "{\"Name\":\"jsonstring\"}";
            var jObject = (JObject)JsonConvert.DeserializeObject(jsonString);
            var name = jObject.GetValue("Name").ToString();

            DataTable table = SqlDbHelper.GetDataSet("select * from UploadForODIStoreFGPickInsert").Tables[0];
            var timeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine(timeString);
            Trace.WriteLine(timeString);
            Trace.WriteLine("Trace");

            //System.Threading.Thread.Sleep(5000);
        }
    }
}
