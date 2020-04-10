using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TestServiceConsoleApplication
{
    class UploadDataService
    {
        public void Start()
        {
            var jobsBuilder = new JobsBuilder();
            jobsBuilder.CreateDataJobs();
        }
        public void Stop()
        {
        }
    }
}
