using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PfxCsp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string filetype = "*.*";
            string filePath = OpenFileDialog(filetype);
            if (!string.IsNullOrEmpty(filePath))
                txtFile.Text = filePath;
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="_filetype"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public string OpenFileDialog(string _filetype)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.RestoreDirectory = true;
            op.Title = "请选择加密文件";
            //op.Filter = _filetype;
            if ((bool)op.ShowDialog().GetValueOrDefault())//打开
                return op.FileName;
            return "";
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFile.Text.Trim()))
                return;
            try
            {
                string data = SafeHelper.FileDecrypt(AppDomain.CurrentDomain.BaseDirectory + "JZT.pfx", txtPassword.Text.Trim(), txtFile.Text.Trim());
                string decryptFilePath = AppDomain.CurrentDomain.BaseDirectory + Guid.NewGuid().ToString() + ".xml";
                System.IO.File.AppendAllText(decryptFilePath, data);
                MessageBox.Show("文件成功解密到：" + decryptFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件成功解密失败。错误信息：" + ex.Message);
            }
            
        }
    }
}
