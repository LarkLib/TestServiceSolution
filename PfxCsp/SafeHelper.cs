using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
 
 
namespace PfxCsp
{
    public class SafeHelper
    {
        /// <summary>
        /// 单据文件解密
        /// </summary>
        /// <param name="pfxFilePath">pfx文件路径</param>
        /// <param name="pfxPassword">pfx密码</param>
        /// <param name="decryptSourceFile">待解密文件路径</param>
        /// <returns></returns>
        public static string FileDecrypt(string pfxFilePath, string pfxPassword, string decryptSourceFile)
        {
            if (string.IsNullOrEmpty(pfxFilePath) || string.IsNullOrEmpty(pfxPassword) || string.IsNullOrEmpty(decryptSourceFile))
                return "";
            try
            {
                string data = "";
                using (StreamReader reader = new StreamReader(decryptSourceFile))
                {
                    string key = RSADecrypt(pfxFilePath, pfxPassword, reader.ReadLine());
                    string iv = RSADecrypt(pfxFilePath, pfxPassword, reader.ReadLine());
                    data = AESDecode(reader.ReadLine(), key, iv);
                };
                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="pfxFilePath">pfx文件路径</param>
        /// <param name="pfxPassword">pfx密码</param>
        /// <param name="decryptStr">待解密字符串</param>
        /// <returns></returns>
        private static string RSADecrypt(string pfxFilePath, string pfxPassword, string decryptStr)
        {
            X509Certificate2 prvcrt = new X509Certificate2(pfxFilePath, pfxPassword, X509KeyStorageFlags.Exportable);
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(prvcrt.PrivateKey.ToXmlString(true));
            byte[] rgb = ConvertHexToBytes(decryptStr);
            byte[] bytes = provider.Decrypt(rgb, false);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <param name="decryptIV"></param>
        /// <returns></returns>
        private static string AESDecode(string decryptString, string decryptKey, string decryptIV)
        {
            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Padding = PaddingMode.PKCS7;
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
            rijndaelProvider.IV = Encoding.UTF8.GetBytes(decryptIV); ;
            ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

            byte[] inputData = ConvertHexToBytes(decryptString);
            byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);
            return Encoding.UTF8.GetString(decryptedData);
        }

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] ConvertHexToBytes(string value)
        {
            int len = value.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[i] = (byte)(Convert.ToInt32(value.Substring(i * 2, 2), 16));
            return ret;
        }

        /// <summary>
        /// 16进制字符转字符
        /// </summary>
        /// <param name="hexstr"></param>
        /// <returns></returns>
        private static string Hex2ToString(string hexstr)
        {
            StringBuilder sb = new StringBuilder();
            string s0 = hexstr;
            for (int i = 0; i < hexstr.Length / 2; i++)
            {
                string s1 = s0.Substring(i * 2, 2);
                int value = Convert.ToInt32(s1, 16);
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                sb.Append(charValue);
            }
            return sb.ToString();
        }
    }
}
