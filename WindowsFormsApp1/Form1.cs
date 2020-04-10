using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace AGVManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartWebApp()
        {
            try
            {
                string url = String.Format("http://{0}:{1}", ConfigurationManager.AppSettings["HostName"], ConfigurationManager.AppSettings["HostPort"]);
                WebApp.Start<Startup>(url);
                textBox1.Text = "启动成功\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (MessageBox.Show("宿主服务启动失败，是否继续启动应用程序？", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartWebApp();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Type"] = "GET";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
                var result = client.DownloadString("http://127.0.0.1:9000/API/MES/Test");
                textBox1.AppendText(string.Format("结果:{0}\r\n", result));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Type"] = "GET";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
                var result = client.DownloadString("http://127.0.0.1:9000/API/MES/GetMaterialInventory?distributionId=100&materialCode=abcd");
                textBox1.AppendText(string.Format("结果:{0}\r\n", result));
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Type"] = "POST";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
                var result = client.UploadString("http://127.0.0.1:9000/API/MES/PostOneTask?TaskID=100&TaskContent=LiaoNing","");
                textBox1.AppendText(string.Format("结果:{0}\r\n", result));
            }

        }
    }

}
