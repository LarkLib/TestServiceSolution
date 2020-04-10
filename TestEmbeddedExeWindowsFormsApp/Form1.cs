using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEmbeddedExeWindowsFormsApp
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fexePath = @"C:\Windows\system32\calc.exe"; // 外部exe位置
            //fexePath = @"""C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"""; // 外部exe位置
            //fexePath = @"chrome.exe"; // 外部exe位置

            Process p = new Process();
            p.StartInfo.FileName = fexePath;
            //p.StartInfo.Arguments = "http://www.bing.com";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.Start();
            while (p.MainWindowHandle.ToInt32() == 0)
            {
                System.Threading.Thread.Sleep(200);
            }
            SetParent(p.MainWindowHandle, this.Handle);
            ShowWindow(p.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
        }
    }
}
